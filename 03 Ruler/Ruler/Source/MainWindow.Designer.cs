//
// Adaptive Vision Library .NET Example - 'Ruler' example
//
// Simple application that uses Adaptive Vision Library .NET to find the stripe on image and measure its width.
//
// {$Copyright} {$Year} {$CompanyName}
// Version: {$Version}
//

namespace Ruler
{
    partial class MainWindow
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
                image.Dispose();
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
            this.labelChooseImage = new System.Windows.Forms.Label();
            this.textImagePath = new System.Windows.Forms.TextBox();
            this.buttonBrowseForImage = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelMouseLocationXY = new System.Windows.Forms.Label();
            this.radioButtonDarkStripe = new System.Windows.Forms.RadioButton();
            this.radioButtonBrightStripe = new System.Windows.Forms.RadioButton();
            this.labelInstensity = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.pictureBox = new Ruler.ExtendedPictureBox();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.radioButtonAnyStripe = new System.Windows.Forms.RadioButton();
            this.panelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panelSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChooseImage
            // 
            this.labelChooseImage.AutoSize = true;
            this.labelChooseImage.Location = new System.Drawing.Point(14, 23);
            this.labelChooseImage.Name = "labelChooseImage";
            this.labelChooseImage.Size = new System.Drawing.Size(74, 13);
            this.labelChooseImage.TabIndex = 0;
            this.labelChooseImage.Text = "Image to load:";
            // 
            // textImagePath
            // 
            this.textImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textImagePath.Location = new System.Drawing.Point(137, 20);
            this.textImagePath.Name = "textImagePath";
            this.textImagePath.ReadOnly = true;
            this.textImagePath.Size = new System.Drawing.Size(472, 20);
            this.textImagePath.TabIndex = 1;
            // 
            // buttonBrowseForImage
            // 
            this.buttonBrowseForImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseForImage.Location = new System.Drawing.Point(615, 20);
            this.buttonBrowseForImage.Name = "buttonBrowseForImage";
            this.buttonBrowseForImage.Size = new System.Drawing.Size(36, 20);
            this.buttonBrowseForImage.TabIndex = 2;
            this.buttonBrowseForImage.Text = "...";
            this.buttonBrowseForImage.UseVisualStyleBackColor = true;
            this.buttonBrowseForImage.Click += new System.EventHandler(this.loadImage_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Image files (*.bmp, *.png, *.jpg, *.tif)|*.bmp;*.png;*.jpg;*.jpeg;*.tif;*.tiff";
            // 
            // labelMouseLocationXY
            // 
            this.labelMouseLocationXY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMouseLocationXY.AutoSize = true;
            this.labelMouseLocationXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelMouseLocationXY.Location = new System.Drawing.Point(946, 568);
            this.labelMouseLocationXY.Name = "labelMouseLocationXY";
            this.labelMouseLocationXY.Size = new System.Drawing.Size(0, 13);
            this.labelMouseLocationXY.TabIndex = 10;
            // 
            // radioButtonDarkStripe
            // 
            this.radioButtonDarkStripe.AutoSize = true;
            this.radioButtonDarkStripe.Location = new System.Drawing.Point(471, 65);
            this.radioButtonDarkStripe.Name = "radioButtonDarkStripe";
            this.radioButtonDarkStripe.Size = new System.Drawing.Size(76, 17);
            this.radioButtonDarkStripe.TabIndex = 6;
            this.radioButtonDarkStripe.Text = "Dark stripe";
            this.radioButtonDarkStripe.UseVisualStyleBackColor = true;
            // 
            // radioButtonBrightStripe
            // 
            this.radioButtonBrightStripe.AutoSize = true;
            this.radioButtonBrightStripe.Location = new System.Drawing.Point(335, 65);
            this.radioButtonBrightStripe.Name = "radioButtonBrightStripe";
            this.radioButtonBrightStripe.Size = new System.Drawing.Size(80, 17);
            this.radioButtonBrightStripe.TabIndex = 5;
            this.radioButtonBrightStripe.Text = "Bright stripe";
            this.radioButtonBrightStripe.UseVisualStyleBackColor = true;
            // 
            // labelInstensity
            // 
            this.labelInstensity.AutoSize = true;
            this.labelInstensity.Location = new System.Drawing.Point(14, 67);
            this.labelInstensity.Name = "labelInstensity";
            this.labelInstensity.Size = new System.Drawing.Size(78, 13);
            this.labelInstensity.TabIndex = 3;
            this.labelInstensity.Text = "Stripe intensity:";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.BackColor = System.Drawing.SystemColors.Control;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelStatus.Location = new System.Drawing.Point(24, 562);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(669, 22);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status Label";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPictureBox.Controls.Add(this.pictureBox);
            this.panelPictureBox.Location = new System.Drawing.Point(27, 144);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(666, 398);
            this.panelPictureBox.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.pictureBox.Image = null;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(666, 398);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            this.pictureBox.ScanningSegmentChosen += new System.EventHandler<Ruler.ScanningSegementEventArgs>(this.pictureBox_ScanningSegmentChosen);
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.BackColor = System.Drawing.SystemColors.Control;
            this.panelSettings.Controls.Add(this.radioButtonAnyStripe);
            this.panelSettings.Controls.Add(this.textImagePath);
            this.panelSettings.Controls.Add(this.labelChooseImage);
            this.panelSettings.Controls.Add(this.labelInstensity);
            this.panelSettings.Controls.Add(this.buttonBrowseForImage);
            this.panelSettings.Controls.Add(this.radioButtonBrightStripe);
            this.panelSettings.Controls.Add(this.radioButtonDarkStripe);
            this.panelSettings.Location = new System.Drawing.Point(27, 25);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(666, 102);
            this.panelSettings.TabIndex = 0;
            // 
            // radioButtonAnyStripe
            // 
            this.radioButtonAnyStripe.AutoSize = true;
            this.radioButtonAnyStripe.Checked = true;
            this.radioButtonAnyStripe.Location = new System.Drawing.Point(137, 65);
            this.radioButtonAnyStripe.Name = "radioButtonAnyStripe";
            this.radioButtonAnyStripe.Size = new System.Drawing.Size(142, 17);
            this.radioButtonAnyStripe.TabIndex = 4;
            this.radioButtonAnyStripe.TabStop = true;
            this.radioButtonAnyStripe.Text = "Any stripe (dark or bright)";
            this.radioButtonAnyStripe.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(718, 603);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.panelPictureBox);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelMouseLocationXY);
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Adaptive Vision Library .NET Example - Ruler";
            this.panelPictureBox.ResumeLayout(false);
            this.panelPictureBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelChooseImage;
        private System.Windows.Forms.TextBox textImagePath;
        private System.Windows.Forms.Button buttonBrowseForImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelMouseLocationXY;
        private System.Windows.Forms.RadioButton radioButtonDarkStripe;
        private System.Windows.Forms.RadioButton radioButtonBrightStripe;
        private System.Windows.Forms.Label labelInstensity;
        private System.Windows.Forms.Label labelStatus;
        private ExtendedPictureBox pictureBox;
        private System.Windows.Forms.Panel panelPictureBox;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.RadioButton radioButtonAnyStripe;
    }
}

