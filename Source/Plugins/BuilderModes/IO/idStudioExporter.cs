#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using CodeImp.DoomBuilder.Data;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;
using CodeImp.DoomBuilder.VisualModes;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.BuilderModes.Interface;
using System.Windows.Forms;
using System.Linq;
using System.Security.Permissions;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes.IO
{
	internal struct idStudioExportSettings
	{
		public string modPath;
		public float xyDownscale;
		public float zDownscale;
		public float xShift;
		public float yShift;
		public bool exportTextures;

		public idStudioExportSettings(idStudioExporterForm form)
		{
			modPath = form.ModPath;
			xyDownscale = form.xyDownscale;
			zDownscale = form.zDownscale;
			xShift = form.xShift;
			yShift = form.yShift;
			exportTextures = form.ExportTextures;
		}
	}

	internal class idStudioExporter
	{
		private idStudioExportSettings cfg;
		private idStudioExporterForm form;
		private idStudioMapWriter writer;

		public void Export(idStudioExporterForm p_form)
		{
			form = p_form;
			cfg = new idStudioExportSettings(form);

			if (cfg.exportTextures)
				idStudioTextureExporter.ExportTextures();

			ExportLevel();
		}

		private void ExportLevel()
		{
			string mapPath = Path.Combine(cfg.modPath, "base/maps/");
			Directory.CreateDirectory(mapPath);
			writer = new idStudioMapWriter(cfg);

			// STEP 1: BUILD FLOOR/CEILING BRUSHES
			//General.ErrorLogger.Add(ErrorType.Warning, "We have " + General.Map.Map.Sectors.Count + " sectors");
			foreach(Sector s in General.Map.Map.Sectors)
			{
				List<idVertex> verts = new List<idVertex>();
				verts.Capacity = s.Triangles.Vertices.Count;
				foreach(Vector2D dv in s.Triangles.Vertices)
				{
					idVertex fv = new idVertex();
					fv.x = ((float)dv.x + cfg.xShift) / cfg.xyDownscale;
					fv.y = ((float)dv.y + cfg.yShift) / cfg.xyDownscale;
					verts.Add(fv);
				}
				float floorHeight = s.FloorHeight / cfg.zDownscale;
				float ceilingHeight = s.CeilHeight / cfg.zDownscale;

				// Given in clockwise winding order
				//General.ErrorLogger.Add(ErrorType.Warning, "HAs " + verts.Count + " verts");
				for (int i = 0; i < verts.Count;)
				{
					idVertex c = verts[i++];
					idVertex b = verts[i++];
					idVertex a = verts[i++];
					writer.WriteFloorBrush(a, b, c, floorHeight, false, s.FloorTexture);
					if(!s.CeilTexture.Equals("F_SKY1"))
						writer.WriteFloorBrush(a, b, c, ceilingHeight, true, s.CeilTexture);
				}
			}

			/*
			* STEP TWO: DRAW WALLS 
			* 
			* Draw Height Rules:
			*
			* One Sided: Ceiling (Default) / Floor (Lower Unpegged)
			* Lower Textures: Highest Floor (Default) / Ceiling the side is facing (Lower Unpegged) 
				- WIKI IS INCORRECT: Falsely asserts Lower Unpegged draws it from the higher ceiling downward
			* Upper Textures: Lowest Ceiling (Default) / Highest Ceiling (Upper Unpegged)
			* Middle Textures:
			*	- Do not repeat vertically - we must modify the brush bounds to account for this
			*		- TODO: THIS QUIRK IS NOT YET IMPLEMENTED
			*	- Highest Ceiling (Default) / Highest Floor (Lower Unpegged)
			*
			* No need for any crazy vector projection when calculating drawheight, so we can simply add in the
			* vertical offset right now
			*/
			foreach(Linedef line in General.Map.Map.Linedefs)
			{
				if (line.Front == null)
					continue;
				bool upperUnpegged = (line.RawFlags & 0x8) > 0;
				bool lowerUnpegged = (line.RawFlags & 0x10) > 0;
				idVertex v0 = new idVertex();
				idVertex v1 = new idVertex();
				{
					Vector2D vec = line.Start.Position;
					v0.x = ((float)vec.x + cfg.xShift) / cfg.xyDownscale;
					v0.y = ((float)vec.y + cfg.yShift) / cfg.xyDownscale;
					vec = line.End.Position;
					v1.x = ((float)vec.x + cfg.xShift) / cfg.xyDownscale;
					v1.y = ((float)vec.y + cfg.yShift) / cfg.xyDownscale;
				}

				Sidedef front = line.Front;
				float frontOffsetX = front.OffsetX / cfg.xyDownscale;
				float frontOffsetY = front.OffsetY / cfg.zDownscale;
				float frontFloor = front.Sector.FloorHeight / cfg.zDownscale;
				float frontCeil = front.Sector.CeilHeight / cfg.zDownscale;

				// If true, this is a one-sided linedef
				if(front.MiddleRequired())
				{
					// level.minHeight, level.maxHeight
					float drawHeight = frontOffsetY + (lowerUnpegged ? frontFloor : frontCeil);
					writer.WriteWallBrush(v0, v1, frontFloor, frontCeil, drawHeight, front.MiddleTexture, frontOffsetX);
					continue;
				}

				Sidedef back = line.Back;
				float backOffsetX = back.OffsetX / cfg.xyDownscale;
				float backOffsetY = back.OffsetY / cfg.zDownscale;
				float backFloor = back.Sector.FloorHeight / cfg.zDownscale;
				float backCeil = back.Sector.CeilHeight / cfg.zDownscale;

				// Texture pegging is based on the lowest/highest floor/ceiling - so we must distinguish
				// which values are smaller / larger - no way around this ugly chain of if statements unfortunately
				float lowerFloor, lowerCeiling, higherFloor, higherCeiling;
				if (frontCeil < backCeil) {
					lowerCeiling = frontCeil;
					higherCeiling = backCeil;
				}
				else {
					lowerCeiling = backCeil;
					higherCeiling = frontCeil;
				}
				if (frontFloor < backFloor) {
					lowerFloor = frontFloor;
					higherFloor = backFloor;
				}
				else {
					lowerFloor = backFloor;
					higherFloor = frontFloor;
				}

				// Brush the front sidedefs in relation to the back sector heights
				if (front.LowRequired()) // This function checks a LOT more than whether the texture exists
				{
					// level.minHeight, backSector.floorHeight
					float drawHeight = frontOffsetY + (lowerUnpegged ? frontCeil : higherFloor);
					writer.WriteWallBrush(v0, v1, frontFloor, backFloor, drawHeight, front.LowTexture, frontOffsetX);
				}
				if (!front.MiddleTexture.Equals("-"))
				{
					float drawHeight = frontOffsetY + (lowerUnpegged ? higherFloor : higherCeiling);
					writer.WriteWallBrush(v0, v1, backFloor, backCeil, drawHeight, front.MiddleTexture, frontOffsetX);
				}
				if (front.HighRequired())
				{
					// backSector.ceilHeight, level.maxHeight
					float drawHeight = frontOffsetY + (upperUnpegged ? higherCeiling : lowerCeiling);
					writer.WriteWallBrush(v0, v1, backCeil, frontCeil, drawHeight, front.HighTexture, frontOffsetX);
				}

				// Brush the back sidedefs in relation to the front sector heights
				// This approach results in two overlapping brushes if both sides have a middle texture
				// BUG FIXED: Must swap start/end vertices to ensure texture is drawn on correct face
				// and begins at correct position
				if (back.LowRequired())
				{
					// level.minHeight, frontSector.floorHeight
					float drawHeight = backOffsetY + (lowerUnpegged ? backCeil : higherFloor);
					writer.WriteWallBrush(v1, v0, backFloor, frontFloor, drawHeight, back.LowTexture, backOffsetX);
				}
				if (!back.MiddleTexture.Equals("-"))
				{
					float drawHeight = backOffsetY + (lowerUnpegged ? higherFloor : higherCeiling);
					writer.WriteWallBrush(v1, v0, frontFloor, frontCeil, drawHeight, back.MiddleTexture, backOffsetX);
				}
				if (back.HighRequired())
				{
					float drawHeight = backOffsetY + (upperUnpegged ? higherCeiling : lowerCeiling);
					// frontSector.ceilHeight, level.maxHeight
					writer.WriteWallBrush(v1, v0, frontCeil, backCeil, drawHeight, back.HighTexture, backOffsetX);
				}
			}

			writer.SaveFile(mapPath + "test.map");
		}
	}

	#region 3D Math

	internal struct idVertex
	{
		public float x;
		public float y;

		// Default zero-constructor can be inferred

		public idVertex(float p_x, float p_y)
		{
			x = p_x; 
			y = p_y;
		}
	}

	internal struct idVector
	{
		public float x;
		public float y;
		public float z;

		// Default zero-constructor can be inferred

		public idVector(float p_x, float p_y, float p_z)
		{
			x = p_x; y = p_y; z = p_z;
		}

		public idVector(idVertex v0, idVertex v1)
		{
			x = v1.x - v0.x;
			y = v1.y - v0.y;
			z = 0.0f;
		}

		public void Normalize()
		{
			float magnitude = Magnitude();
			if(magnitude != 0)
			{
				x /= magnitude;
				y /= magnitude;
				z /= magnitude;
			}
		}

		public float Magnitude()
		{
			return (float)Math.Sqrt(x * x + y * y + z * z);
		}
	}

	internal struct idPlane
	{
		public idVector n;
		public float d;

		public void SetFrom(idVector p_normal, idVertex point)
		{
			n = p_normal;
			n.Normalize();
			d = n.x * point.x + n.y * point.y;
		}
	}

	#endregion


	#region Map Writer
	internal class idStudioMapWriter
	{
		private const string rootMap =
@"Version 7
HierarchyVersion 1
entity {
	entityDef world {
		inherit = ""worldspawn"";
		edit = {
		}
	}
";
		private idStudioExportSettings cfg;
		private StringBuilder writer;
		private int brushHandle = 100000000; // Must increment with every written brush
		private float flatScale;
		private float flatXShift;
		private float flatYShift;

		public idStudioMapWriter(idStudioExportSettings p_cfg)
		{
			cfg = p_cfg;
			writer = new StringBuilder(rootMap);

			//Flats are always 64x64, allowing us to use 1 / 64 as a constant
			// Full formula is xShift / xyDownscale * xyDownscale * 0.015625f * -1
			// Sign is flipped for right/left shift, but not for up/down shift
			// (Obviously we'll need to refactor this once we begin considering non-vanilla formats)
			flatScale = 0.015625f * cfg.xyDownscale;
			flatXShift = -0.015625f * cfg.xShift;
			flatYShift = 0.015625f * cfg.yShift;
		}

		public void SaveFile(string outputPath)
		{
			// Close Entity
			writer.Append("\n}");

			using (StreamWriter file = new StreamWriter(outputPath, false))
			{
				file.Write(writer.ToString());
			}
		}

		private void BeginBrushDef()
		{
			writer.Append("{\n\thandle = " + brushHandle++ + "\n\tbrushDef3 {");
		}

		private void WritePlane(idPlane p)
		{
			writer.Append("( " + p.n.x + " " + p.n.y + " " + p.n.z + " " + -p.d
				+ " ) ( ( 1 0 0 ) ( 0 1 0 ) ) \"art/tile/common/shadow_caster\" 0 0 0");
		}

		private void EndBrushDef()
		{
			writer.Append("\n\t}\n}\n");
		}

		public void WriteWallBrush(idVertex v0, idVertex v1, float minHeight, float maxHeight, float drawHeight, string texture, float offsetX)
		{
			idPlane[] bounds = new idPlane[5]; // Untextured surfaces
			idPlane surface = new idPlane();   // Texture surface
			idVector horizontal = new idVector(v0, v1);

			// PART 1 - CONSTRUCT THE PLANES
			// Crossing horizontal X <0, 0, 1>
			surface.SetFrom(new idVector(horizontal.y, -horizontal.x, 0), v1);

			// Plane 0 - The "Back" SideDef to the LineDef's left
			bounds[0].n.x = -surface.n.x;
			bounds[0].n.y = -surface.n.y;
			bounds[0].n.z = 0;

			//idVertex d0 = new idVertex(bounds[0].n.x* 0.0075f + v0.x, bounds[0].n.y * 0.0075f + v0.y);
			idVertex d1 = new idVertex(bounds[0].n.x* 0.0075f + v1.x, bounds[0].n.y * 0.0075f + v1.y);
			bounds[0].d = bounds[0].n.x * d1.x + bounds[0].n.y * d1.y;

			// Plane 1: Forward Border Sliver: d1 - v1
			idVector deltaVector = new idVector(v1, d1);
			bounds[1].SetFrom(new idVector(deltaVector.y, -deltaVector.x, 0), d1);

			// Plane 2: Rear Border Sliver: v0 - d0
			bounds[2].n.x = -bounds[1].n.x;
			bounds[2].n.y = -bounds[1].n.y;
			bounds[2].n.z = 0;
			bounds[2].d = bounds[2].n.x * v0.x + bounds[2].n.y * v0.y;

			// Plane 3: Upper Bound:
			bounds[3].n = new idVector(0, 0, 1);
			bounds[3].d = maxHeight;

			// Plane 4: Lower Bound
			bounds[4].n = new idVector(0, 0, -1);
			bounds[4].d = minHeight * -1;


			// PART 2: DRAW THE SURFACE
			BeginBrushDef();

			// Write untextured bounds
			for (int i = 0; i < 5; i++)
			{
				writer.Append("\n\t\t");
				WritePlane(bounds[i]);
			}

			// Write Textured surface
			// POSSIBLE TODO: TEST IF TEXTURE DOES NOT EXIST, draw as regular plane if it doesn't
			writer.Append("\n\t\t");

			ImageData dimensions = General.Map.Data.GetTextureImage(texture);
			float xScale = 1.0f / dimensions.Width * cfg.xyDownscale;
			float yScale = 1.0f / dimensions.Height * cfg.zDownscale;

			/*
			* We must shift the texture grid such that the origin is centered on
			* the wall's left vertex. To do this accurately, we calculate the magnitude
			* of the projection of the shift vector onto the horizontal wall vector.
			* We finalize this by adding the texture X offset to this value.
			* The math works out such that the XY downscale cancels in both terms when
			* the texture's X scale is multiplied in at the end.
			*/
			float projection = ((horizontal.x * v0.x + horizontal.y * v0.y) / horizontal.Magnitude() - offsetX) * xScale * -1;

			writer.Append("( " + surface.n.x + " " + surface.n.y + " " + surface.n.z + " " + -surface.d + " ) ");
			writer.Append("( ( " + xScale + " 0 " + projection + " ) ( 0 " + yScale + " " + drawHeight * yScale + " ) ) \"art/wadtobrush/walls/" + texture + "\" 0 0 0");
			EndBrushDef();
		}

		public void WriteFloorBrush(idVertex a, idVertex b, idVertex c, float height, bool isCeiling, string texture)
		{
			General.ErrorLogger.Add(ErrorType.Warning, "In WriteFloorBrush");
			idPlane[] bounds = new idPlane[4]; // Untextured surfaces
			idPlane surface = new idPlane(); // Texture surface

			// PART 1 - CONSTRUCT PLANE OBJECTS
			// We assume the points are given in a COUNTER-CLOCKWISE order
			// Hence, we cross horizontal X <0, 0, 1> to get our normal

			// Plane 0 - First Wall
			idVector h = new idVector(a, b);
			bounds[0].SetFrom(new idVector(h.y, -h.x, 0.0f), a);

			// Plane 1 - Second Wall
			h = new idVector(b, c);
			bounds[1].SetFrom(new idVector(h.y, -h.x, 0.0f), b);

			// Plane 2 - Last Wall
			h = new idVector(c, a);
			bounds[2].SetFrom(new idVector(h.y, -h.x, 0.0f), c);

			if (isCeiling) {
				bounds[3].n = new idVector(0, 0, 1);
				bounds[3].d = height + 0.0075f;
				surface.n = new idVector(0, 0, -1);
				surface.d = -height;
			}
			else {
				surface.n = new idVector(0, 0, 1);
				surface.d = height;
				bounds[3].n = new idVector(0, 0, -1);
				bounds[3].d = 0.0075f - height;
			}

			// PART 2: DRAW THE SURFACE
			BeginBrushDef();
			for(int i = 0; i < 4; i++) {
				writer.Append("\n\t\t");
				WritePlane(bounds[i]);
			}
			writer.Append("\n\t\t");

			// horizontal: (0, -1) Vertical (1, 0) - Ensures proper rotation of textures (for floors)
			writer.Append("( " + surface.n.x + " " + surface.n.y + " " + surface.n.z + " " + -surface.d + " ) ");
			writer.Append("( ( 0 " + (isCeiling ? -flatScale : flatScale) + " " + flatXShift + " ) ( " + -flatScale + " 0 " + flatYShift
				+ " ) ) \"art/wadtobrush/flats/" + texture + "\" 0 0 0");

			EndBrushDef();
		}
	}
	#endregion

	#region Texture Exports

	/*
	 * IMMEDIATE TODO:
	 * ENSURE OUTPUT DIRECTORY IS CORRECT
	 * INVESTIGATE TEXTURES BEING BRIGHTER COMPARED TO WHAT WADTOBRUSH GENERATES
	 */
	internal class idStudioTextureExporter
	{
		private const string mat2_static =
@"declType( material2 ) {{
	inherit = ""template/pbr"";
	edit = {{
		RenderLayers = {{
			item[0] = {{
				parms = {{
					smoothness = {{
						filePath = ""art/wadtobrush/black.tga"";
					}}
					specular = {{
						filePath = ""art/wadtobrush/black.tga"";
					}}
					albedo = {{
						filePath = ""art/wadtobrush/{0}{1}.tga"";
					}}
				}}
			}}
		}}
	}}
}}";

		private const string mat2_staticAlpha =
@"declType( material2 ) {{
	inherit = ""template/pbr_alphatest"";
	edit = {{
		RenderLayers = {{
			item[0] = {{
				parms = {{
					cover = {{
						filePath = ""art/wadtobrush/{0}{1}.tga"";
					}}
					smoothness = {{
						filePath = ""art/wadtobrush/black.tga"";
					}}
					specular = {{
						filePath = ""art/wadtobrush/black.tga"";
					}}
					albedo = {{
						filePath = ""art/wadtobrush/{0}{1}.tga"";
					}}
				}}
			}}
		}}
	}}
}}";

		private const string dir_flats_art = "base/art/wadtobrush/flats/";
		private const string dir_flats_mat = "base/declTree/material2/art/wadtobrush/flats/";
		private const string dir_walls_art = "base/art/wadtobrush/walls/";
		private const string dir_walls_mat = "base/declTree/material2/art/wadtobrush/walls/";

		private const string path_black = "base/art/wadtobrush/black.tga";
		
		// Unable to export patches at this time
		//private const string dir_patches = "base/art/wadtobrush/patches/";
		//private const string dir_patches_mat = "base/declTree/material2/art/wadtobrush/patches/";

		/*
		 * Credits: This function is a modified port of https://gist.github.com/maluoi/ade07688e741ab188841223b8ffeed22
		 */
		private static void WriteTGA(in string filename, in Bitmap data)
		{
			byte[] header = { 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				(byte)(data.Width % 256), (byte)(data.Width / 256), 
				(byte)(data.Height % 256), (byte)(data.Height / 256), 
				32, 0x20 };

			using(FileStream file = File.OpenWrite(filename))
			{
				lock(data)
				{
					file.Write(header, 0, header.Length);

					for (int h = 0; h < data.Height; h++)
					{
						for (int w = 0; w < data.Width; w++)
						{
							Color c = data.GetPixel(w, h);
							byte[] pixel = { c.B, c.G, c.R, c.A };
							file.Write(pixel, 0, pixel.Length);
						}
					}
				}
			} 
		}

		private static void WriteArtAsset(in string subFolder, ImageData img)
		{
			// PART ONE - Write the art file
			string artPath = "base/art/wadtobrush/" + subFolder + img.Name + ".tga";
			WriteTGA(artPath, img.ExportBitmap());


			// PART 2 - Write the material2 decl
			bool useAlpha = img.IsTranslucent || img.IsMasked;

			string matPath = "base/declTree/material2/art/wadtobrush/" + subFolder + img.Name + ".decl";

			string format;

			if (useAlpha)
				format = String.Format(mat2_staticAlpha, subFolder, img.Name);
			else format = String.Format(mat2_static, subFolder, img.Name);

			File.WriteAllText(matPath, format);
		}

		public static void ExportTextures()
		{
			Directory.CreateDirectory(dir_flats_art);
			Directory.CreateDirectory(dir_flats_mat);
			Directory.CreateDirectory(dir_walls_art);
			Directory.CreateDirectory(dir_walls_mat);

			// Generate black texture
			{
				Color pixel = Color.FromArgb(255, 0, 0, 0);
				int blackWidth = 64, blackHeight = 64;
				Bitmap black = new Bitmap(blackWidth, blackHeight);
				

				for(int w = 0; w < blackWidth; w++)
					for(int h = 0; h < blackHeight; h++)
						black.SetPixel(w, h, pixel);

				WriteTGA(path_black, black);
			}

			foreach(ImageData img in General.Map.Data.Textures)
				WriteArtAsset("walls/", img);

			foreach (ImageData img in General.Map.Data.Flats)
				WriteArtAsset("flats/", img);
		}
	}

	#endregion
}