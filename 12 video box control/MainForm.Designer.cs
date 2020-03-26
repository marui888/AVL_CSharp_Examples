namespace VideoBoxControl
{
    partial class MainForm
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
                zoomingVideoBox1.Dispose();
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
            this.zoomingVideoBox1 = new HMI.Controls.ZoomingVideoBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupSizeMode = new System.Windows.Forms.GroupBox();
            this.radioFit = new System.Windows.Forms.RadioButton();
            this.radioOriginalSize = new System.Windows.Forms.RadioButton();
            this.groupImage = new System.Windows.Forms.GroupBox();
            this.radioPeppers = new System.Windows.Forms.RadioButton();
            this.radioPlane = new System.Windows.Forms.RadioButton();
            this.groupImageSize = new System.Windows.Forms.GroupBox();
            this.radioSize128 = new System.Windows.Forms.RadioButton();
            this.radioSize256 = new System.Windows.Forms.RadioButton();
            this.radioSize512 = new System.Windows.Forms.RadioButton();
            this.groupZoomButtons = new System.Windows.Forms.GroupBox();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupSizeMode.SuspendLayout();
            this.groupImage.SuspendLayout();
            this.groupImageSize.SuspendLayout();
            this.groupZoomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // zoomingVideoBox1
            // 
            this.zoomingVideoBox1.BackColor = System.Drawing.SystemColors.Window;
            this.zoomingVideoBox1.DisplayMode = VideoBoxBase.VideoBoxDisplayMode.GDI;
            this.zoomingVideoBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomingVideoBox1.Location = new System.Drawing.Point(0, 0);
            this.zoomingVideoBox1.Name = "zoomingVideoBox1";
            this.zoomingVideoBox1.Size = new System.Drawing.Size(600, 597);
            this.zoomingVideoBox1.TabIndex = 0;
            this.zoomingVideoBox1.SizeModeChanged += new System.EventHandler(this.zoomingVideoBox1_SizeModeChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.zoomingVideoBox1);
            this.panel1.Location = new System.Drawing.Point(163, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 601);
            this.panel1.TabIndex = 4;
            // 
            // groupSizeMode
            // 
            this.groupSizeMode.Controls.Add(this.radioFit);
            this.groupSizeMode.Controls.Add(this.radioOriginalSize);
            this.groupSizeMode.Location = new System.Drawing.Point(12, 103);
            this.groupSizeMode.Name = "groupSizeMode";
            this.groupSizeMode.Size = new System.Drawing.Size(145, 108);
            this.groupSizeMode.TabIndex = 1;
            this.groupSizeMode.TabStop = false;
            this.groupSizeMode.Text = "Size Mode";
            // 
            // radioFit
            // 
            this.radioFit.AutoCheck = false;
            this.radioFit.AutoSize = true;
            this.radioFit.Location = new System.Drawing.Point(15, 64);
            this.radioFit.Name = "radioFit";
            this.radioFit.Size = new System.Drawing.Size(113, 21);
            this.radioFit.TabIndex = 1;
            this.radioFit.Text = "Fit to Window";
            this.radioFit.UseVisualStyleBackColor = true;
            this.radioFit.Click += new System.EventHandler(this.radioFit_Click);
            // 
            // radioOriginalSize
            // 
            this.radioOriginalSize.AutoCheck = false;
            this.radioOriginalSize.AutoSize = true;
            this.radioOriginalSize.Checked = true;
            this.radioOriginalSize.Location = new System.Drawing.Point(15, 36);
            this.radioOriginalSize.Name = "radioOriginalSize";
            this.radioOriginalSize.Size = new System.Drawing.Size(109, 21);
            this.radioOriginalSize.TabIndex = 0;
            this.radioOriginalSize.TabStop = true;
            this.radioOriginalSize.Text = "Original Size";
            this.radioOriginalSize.UseVisualStyleBackColor = true;
            this.radioOriginalSize.Click += new System.EventHandler(this.radioOriginalSize_Click);
            // 
            // groupImage
            // 
            this.groupImage.Controls.Add(this.radioPeppers);
            this.groupImage.Controls.Add(this.radioPlane);
            this.groupImage.Location = new System.Drawing.Point(12, 217);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(145, 105);
            this.groupImage.TabIndex = 2;
            this.groupImage.TabStop = false;
            this.groupImage.Text = "Image Type";
            // 
            // radioPeppers
            // 
            this.radioPeppers.AutoSize = true;
            this.radioPeppers.Location = new System.Drawing.Point(15, 62);
            this.radioPeppers.Name = "radioPeppers";
            this.radioPeppers.Size = new System.Drawing.Size(82, 21);
            this.radioPeppers.TabIndex = 1;
            this.radioPeppers.Text = "Peppers";
            this.radioPeppers.UseVisualStyleBackColor = true;
            this.radioPeppers.Click += new System.EventHandler(this.imageConfigChanged);
            // 
            // radioPlane
            // 
            this.radioPlane.AutoSize = true;
            this.radioPlane.Checked = true;
            this.radioPlane.Location = new System.Drawing.Point(15, 35);
            this.radioPlane.Name = "radioPlane";
            this.radioPlane.Size = new System.Drawing.Size(65, 21);
            this.radioPlane.TabIndex = 0;
            this.radioPlane.TabStop = true;
            this.radioPlane.Text = "Plane";
            this.radioPlane.UseVisualStyleBackColor = true;
            this.radioPlane.Click += new System.EventHandler(this.imageConfigChanged);
            // 
            // groupImageSize
            // 
            this.groupImageSize.Controls.Add(this.radioSize128);
            this.groupImageSize.Controls.Add(this.radioSize256);
            this.groupImageSize.Controls.Add(this.radioSize512);
            this.groupImageSize.Location = new System.Drawing.Point(12, 328);
            this.groupImageSize.Name = "groupImageSize";
            this.groupImageSize.Size = new System.Drawing.Size(145, 132);
            this.groupImageSize.TabIndex = 3;
            this.groupImageSize.TabStop = false;
            this.groupImageSize.Text = "Image Size";
            // 
            // radioSize128
            // 
            this.radioSize128.AutoSize = true;
            this.radioSize128.Location = new System.Drawing.Point(15, 88);
            this.radioSize128.Name = "radioSize128";
            this.radioSize128.Size = new System.Drawing.Size(71, 21);
            this.radioSize128.TabIndex = 2;
            this.radioSize128.Text = "128 px";
            this.radioSize128.UseVisualStyleBackColor = true;
            this.radioSize128.Click += new System.EventHandler(this.imageConfigChanged);
            // 
            // radioSize256
            // 
            this.radioSize256.AutoSize = true;
            this.radioSize256.Location = new System.Drawing.Point(15, 61);
            this.radioSize256.Name = "radioSize256";
            this.radioSize256.Size = new System.Drawing.Size(71, 21);
            this.radioSize256.TabIndex = 1;
            this.radioSize256.Text = "256 px";
            this.radioSize256.UseVisualStyleBackColor = true;
            this.radioSize256.Click += new System.EventHandler(this.imageConfigChanged);
            // 
            // radioSize512
            // 
            this.radioSize512.AutoSize = true;
            this.radioSize512.Checked = true;
            this.radioSize512.Location = new System.Drawing.Point(15, 34);
            this.radioSize512.Name = "radioSize512";
            this.radioSize512.Size = new System.Drawing.Size(71, 21);
            this.radioSize512.TabIndex = 0;
            this.radioSize512.TabStop = true;
            this.radioSize512.Text = "512 px";
            this.radioSize512.UseVisualStyleBackColor = true;
            this.radioSize512.Click += new System.EventHandler(this.imageConfigChanged);
            // 
            // groupZoomButtons
            // 
            this.groupZoomButtons.Controls.Add(this.buttonZoomOut);
            this.groupZoomButtons.Controls.Add(this.buttonZoomIn);
            this.groupZoomButtons.Location = new System.Drawing.Point(12, 12);
            this.groupZoomButtons.Name = "groupZoomButtons";
            this.groupZoomButtons.Size = new System.Drawing.Size(145, 85);
            this.groupZoomButtons.TabIndex = 0;
            this.groupZoomButtons.TabStop = false;
            this.groupZoomButtons.Text = "Change Zoom";
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonZoomOut.Location = new System.Drawing.Point(81, 34);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomOut.TabIndex = 1;
            this.buttonZoomOut.Text = "-";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonZoomIn.Location = new System.Drawing.Point(31, 34);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomIn.TabIndex = 0;
            this.buttonZoomIn.Text = "+";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 625);
            this.Controls.Add(this.groupZoomButtons);
            this.Controls.Add(this.groupImageSize);
            this.Controls.Add(this.groupImage);
            this.Controls.Add(this.groupSizeMode);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "VideoBox Control Demo";
            this.panel1.ResumeLayout(false);
            this.groupSizeMode.ResumeLayout(false);
            this.groupSizeMode.PerformLayout();
            this.groupImage.ResumeLayout(false);
            this.groupImage.PerformLayout();
            this.groupImageSize.ResumeLayout(false);
            this.groupImageSize.PerformLayout();
            this.groupZoomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HMI.Controls.ZoomingVideoBox zoomingVideoBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupSizeMode;
        private System.Windows.Forms.RadioButton radioFit;
        private System.Windows.Forms.RadioButton radioOriginalSize;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.RadioButton radioPeppers;
        private System.Windows.Forms.RadioButton radioPlane;
        private System.Windows.Forms.GroupBox groupImageSize;
        private System.Windows.Forms.RadioButton radioSize128;
        private System.Windows.Forms.RadioButton radioSize256;
        private System.Windows.Forms.RadioButton radioSize512;
        private System.Windows.Forms.GroupBox groupZoomButtons;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonZoomIn;
    }
}

