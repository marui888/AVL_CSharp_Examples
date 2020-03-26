using System.Windows.Forms;

namespace template_matching_designer
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable. 
        /// </summary>
        /// 
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.loadImageButton = new System.Windows.Forms.Button();
            this.createModelbutton = new System.Windows.Forms.Button();
            this.findObjectsButton = new System.Windows.Forms.Button();
            this.previewImagebox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modelPreviewImageBox = new System.Windows.Forms.PictureBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.referenceLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewImagebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPreviewImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 500;
            // 
            // loadImageButton
            // 
            this.loadImageButton.Location = new System.Drawing.Point(13, 13);
            this.loadImageButton.Name = "loadImageButton";
            this.loadImageButton.Size = new System.Drawing.Size(127, 23);
            this.loadImageButton.TabIndex = 0;
            this.loadImageButton.Text = "1. Load image";
            this.loadImageButton.UseVisualStyleBackColor = true;
            this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
            // 
            // createModelbutton
            // 
            this.createModelbutton.Enabled = false;
            this.createModelbutton.Location = new System.Drawing.Point(146, 13);
            this.createModelbutton.Name = "createModelbutton";
            this.createModelbutton.Size = new System.Drawing.Size(127, 23);
            this.createModelbutton.TabIndex = 1;
            this.createModelbutton.Text = "2. Train Edge Model";
            this.createModelbutton.UseVisualStyleBackColor = true;
            this.createModelbutton.Click += new System.EventHandler(this.createModelbutton_Click);
            // 
            // findObjectsButton
            // 
            this.findObjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.findObjectsButton.Enabled = false;
            this.findObjectsButton.Location = new System.Drawing.Point(522, 466);
            this.findObjectsButton.Name = "findObjectsButton";
            this.findObjectsButton.Size = new System.Drawing.Size(198, 23);
            this.findObjectsButton.TabIndex = 2;
            this.findObjectsButton.Text = "3. Find Objects";
            this.findObjectsButton.UseVisualStyleBackColor = true;
            this.findObjectsButton.Click += new System.EventHandler(this.findObjectsButton_Click);
            // 
            // previewImagebox
            // 
            this.previewImagebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewImagebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewImagebox.Location = new System.Drawing.Point(13, 68);
            this.previewImagebox.Name = "previewImagebox";
            this.previewImagebox.Size = new System.Drawing.Size(503, 392);
            this.previewImagebox.TabIndex = 3;
            this.previewImagebox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Found objects:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(519, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Model image:";
            // 
            // modelPreviewImageBox
            // 
            this.modelPreviewImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modelPreviewImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modelPreviewImageBox.Location = new System.Drawing.Point(522, 68);
            this.modelPreviewImageBox.Name = "modelPreviewImageBox";
            this.modelPreviewImageBox.Size = new System.Drawing.Size(198, 165);
            this.modelPreviewImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.modelPreviewImageBox.TabIndex = 6;
            this.modelPreviewImageBox.TabStop = false;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(523, 262);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(197, 198);
            this.propertyGrid.TabIndex = 7;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(522, 246);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Search parameters:";
            // 
            // referenceLabel
            // 
            this.referenceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referenceLabel.Location = new System.Drawing.Point(13, 469);
            this.referenceLabel.Name = "referenceLabel";
            this.referenceLabel.Size = new System.Drawing.Size(503, 20);
            this.referenceLabel.TabIndex = 9;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 501);
            this.Controls.Add(this.referenceLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.modelPreviewImageBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.previewImagebox);
            this.Controls.Add(this.findObjectsButton);
            this.Controls.Add(this.createModelbutton);
            this.Controls.Add(this.loadImageButton);
            this.MinimumSize = new System.Drawing.Size(748, 539);
            this.Name = "MainWindow";
            this.Text = "Adaptive Vision Library - Template Matching Designer";
            ((System.ComponentModel.ISupportInitialize)(this.previewImagebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPreviewImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Timer timer;
        private Button loadImageButton;
        private Button createModelbutton;
        private Button findObjectsButton;
        private PictureBox previewImagebox;
        private Label label1;
        private Label label2;
        private PictureBox modelPreviewImageBox;
        private PropertyGrid propertyGrid;
        private Label label3;
        private Label referenceLabel;
    }
}

