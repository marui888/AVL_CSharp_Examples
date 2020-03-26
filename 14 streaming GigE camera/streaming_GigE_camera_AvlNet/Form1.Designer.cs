namespace streaming_GigE_camera_AvlNet
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.videoBox = new HMI.Controls.ZoomingVideoBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.addressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.manufacturerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.modelLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.frameRateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startButton,
            this.stopButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // startButton
            // 
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(35, 22);
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopButton.Enabled = false;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(35, 22);
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatusLabel,
            this.addressLabel,
            this.modelLabel,
            this.manufacturerLabel,
            this.frameRateLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.AutoSize = false;
            this.connectionStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(100, 17);
            this.connectionStatusLabel.Text = "Disconnected";
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = false;
            this.addressLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(100, 17);
            // 
            // manufacturerLabel
            // 
            this.manufacturerLabel.Name = "manufacturerLabel";
            this.manufacturerLabel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.manufacturerLabel.Size = new System.Drawing.Size(4, 17);
            // 
            // modelLabel
            // 
            this.modelLabel.Name = "modelLabel";
            this.modelLabel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.modelLabel.Size = new System.Drawing.Size(4, 17);
            // 
            // frameRateLabel
            // 
            this.frameRateLabel.Name = "frameRateLabel";
            this.frameRateLabel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // videoBox
            // 
            this.videoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.videoBox);
            this.Name = "Form1";
            this.Text = "Streaming GigE Camera (AvlNet)";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HMI.Controls.ZoomingVideoBox videoBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel addressLabel;
        private System.Windows.Forms.ToolStripStatusLabel manufacturerLabel;
        private System.Windows.Forms.ToolStripStatusLabel modelLabel;
        private System.Windows.Forms.ToolStripStatusLabel frameRateLabel;
    }
}