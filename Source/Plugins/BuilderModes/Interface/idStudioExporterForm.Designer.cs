﻿namespace CodeImp.DoomBuilder.BuilderModes.Interface
{
	partial class idStudioExporterForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gui_ModPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gui_FolderBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gui_yShift = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gui_xShift = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.gui_zDownscale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gui_xyDownscale = new System.Windows.Forms.NumericUpDown();
            this.gbTextureControls = new System.Windows.Forms.GroupBox();
            this.gui_ExportTextures = new System.Windows.Forms.CheckBox();
            this.gui_CancelBtn = new System.Windows.Forms.Button();
            this.gui_ExportBtn = new System.Windows.Forms.Button();
            this.gui_fileTree = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gui_yShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_xShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_zDownscale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_xyDownscale)).BeginInit();
            this.gbTextureControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gui_ModPath
            // 
            this.gui_ModPath.Location = new System.Drawing.Point(72, 13);
            this.gui_ModPath.Name = "gui_ModPath";
            this.gui_ModPath.ReadOnly = true;
            this.gui_ModPath.Size = new System.Drawing.Size(327, 20);
            this.gui_ModPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mod Folder:";
            // 
            // gui_FolderBtn
            // 
            this.gui_FolderBtn.Image = global::CodeImp.DoomBuilder.BuilderModes.Properties.Resources.Folder;
            this.gui_FolderBtn.Location = new System.Drawing.Point(405, 10);
            this.gui_FolderBtn.Name = "gui_FolderBtn";
            this.gui_FolderBtn.Size = new System.Drawing.Size(30, 24);
            this.gui_FolderBtn.TabIndex = 2;
            this.gui_FolderBtn.UseVisualStyleBackColor = true;
            this.gui_FolderBtn.Click += new System.EventHandler(this.evt_FolderButton);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.gui_yShift);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.gui_xShift);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.gui_zDownscale);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.gui_xyDownscale);
            this.groupBox1.Location = new System.Drawing.Point(12, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 75);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transformations";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Shifts:";
            // 
            // gui_yShift
            // 
            this.gui_yShift.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gui_yShift.Location = new System.Drawing.Point(226, 45);
            this.gui_yShift.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.gui_yShift.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.gui_yShift.Name = "gui_yShift";
            this.gui_yShift.Size = new System.Drawing.Size(67, 20);
            this.gui_yShift.TabIndex = 11;
            this.gui_yShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(96, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "X:";
            // 
            // gui_xShift
            // 
            this.gui_xShift.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gui_xShift.Location = new System.Drawing.Point(119, 45);
            this.gui_xShift.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.gui_xShift.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.gui_xShift.Name = "gui_xShift";
            this.gui_xShift.Size = new System.Drawing.Size(67, 20);
            this.gui_xShift.TabIndex = 8;
            this.gui_xShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Downscales:";
            // 
            // gui_zDownscale
            // 
            this.gui_zDownscale.DecimalPlaces = 2;
            this.gui_zDownscale.Location = new System.Drawing.Point(226, 19);
            this.gui_zDownscale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gui_zDownscale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gui_zDownscale.Name = "gui_zDownscale";
            this.gui_zDownscale.Size = new System.Drawing.Size(67, 20);
            this.gui_zDownscale.TabIndex = 6;
            this.gui_zDownscale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Z:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "XY:";
            // 
            // gui_xyDownscale
            // 
            this.gui_xyDownscale.DecimalPlaces = 2;
            this.gui_xyDownscale.Location = new System.Drawing.Point(119, 19);
            this.gui_xyDownscale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gui_xyDownscale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gui_xyDownscale.Name = "gui_xyDownscale";
            this.gui_xyDownscale.Size = new System.Drawing.Size(67, 20);
            this.gui_xyDownscale.TabIndex = 0;
            this.gui_xyDownscale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbTextureControls
            // 
            this.gbTextureControls.Controls.Add(this.gui_ExportTextures);
            this.gbTextureControls.Location = new System.Drawing.Point(12, 141);
            this.gbTextureControls.Name = "gbTextureControls";
            this.gbTextureControls.Size = new System.Drawing.Size(423, 100);
            this.gbTextureControls.TabIndex = 4;
            this.gbTextureControls.TabStop = false;
            // 
            // gui_ExportTextures
            // 
            this.gui_ExportTextures.AutoSize = true;
            this.gui_ExportTextures.Location = new System.Drawing.Point(6, 0);
            this.gui_ExportTextures.Name = "gui_ExportTextures";
            this.gui_ExportTextures.Size = new System.Drawing.Size(100, 17);
            this.gui_ExportTextures.TabIndex = 0;
            this.gui_ExportTextures.Text = "Export Textures";
            this.gui_ExportTextures.UseVisualStyleBackColor = true;
            // 
            // gui_CancelBtn
            // 
            this.gui_CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gui_CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.gui_CancelBtn.Location = new System.Drawing.Point(360, 575);
            this.gui_CancelBtn.Name = "gui_CancelBtn";
            this.gui_CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.gui_CancelBtn.TabIndex = 10;
            this.gui_CancelBtn.Text = "Cancel";
            this.gui_CancelBtn.UseVisualStyleBackColor = true;
            this.gui_CancelBtn.Click += new System.EventHandler(this.evt_CancelButton);
            // 
            // gui_ExportBtn
            // 
            this.gui_ExportBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gui_ExportBtn.Location = new System.Drawing.Point(279, 575);
            this.gui_ExportBtn.Name = "gui_ExportBtn";
            this.gui_ExportBtn.Size = new System.Drawing.Size(75, 23);
            this.gui_ExportBtn.TabIndex = 9;
            this.gui_ExportBtn.Text = "Export";
            this.gui_ExportBtn.UseVisualStyleBackColor = true;
            this.gui_ExportBtn.Click += new System.EventHandler(this.evt_ButtonExport);
            // 
            // gui_fileTree
            // 
            this.gui_fileTree.Location = new System.Drawing.Point(12, 264);
            this.gui_fileTree.Name = "gui_fileTree";
            this.gui_fileTree.Size = new System.Drawing.Size(423, 305);
            this.gui_fileTree.TabIndex = 11;
            // 
            // idStudioExporterForm
            // 
            this.AcceptButton = this.gui_ExportBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.gui_CancelBtn;
            this.ClientSize = new System.Drawing.Size(447, 610);
            this.Controls.Add(this.gui_fileTree);
            this.Controls.Add(this.gui_CancelBtn);
            this.Controls.Add(this.gui_ExportBtn);
            this.Controls.Add(this.gbTextureControls);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gui_FolderBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gui_ModPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "idStudioExporterForm";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Export to idStudio";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gui_yShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_xShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_zDownscale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gui_xyDownscale)).EndInit();
            this.gbTextureControls.ResumeLayout(false);
            this.gbTextureControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox gui_ModPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button gui_FolderBtn;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown gui_xyDownscale;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown gui_zDownscale;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown gui_yShift;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown gui_xShift;
		private System.Windows.Forms.GroupBox gbTextureControls;
		private System.Windows.Forms.CheckBox gui_ExportTextures;
		private System.Windows.Forms.Button gui_CancelBtn;
		private System.Windows.Forms.Button gui_ExportBtn;
		private System.Windows.Forms.TreeView gui_fileTree;
	}
}