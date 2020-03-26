//
// Adaptive Vision Library .NET Example - "Using video box control" example
//
// Simple application that uses ZoomingVideoBox control to display loaded image.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HMI.Controls;


namespace VideoBoxControl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadImage();
        }


        private void LoadImage()
        {
            AvlNet.TestImageId imageType;
            int imageSize;

            if (radioPlane.Checked)
                imageType = AvlNet.TestImageId.Plane;
            else
                imageType = AvlNet.TestImageId.Peppers;

            if (radioSize128.Checked)
                imageSize = 128;
            else if (radioSize256.Checked)
                imageSize = 256;
            else
                imageSize = 512;

            using (AvlNet.Image
                imageColor = new AvlNet.Image(),
                imageScaled = new AvlNet.Image())
            {
                //mr:: AvlNet.TestImageId imageType; 
                // imageType = AvlNet.TestImageId.Plane;
                //public static void TestImage
                //(
                //    AvlNet.TestImageId inImageId,
                //    NullableRef<AvlNet.Image> outRgbImage,
                //    NullableRef<AvlNet.Image> outMonoImage
                //)
                AvlNet.AVL.TestImage(imageType, imageColor, null);
                //mr:: 感觉这是在改变Imaged的大小, 而不是图像的显示大小.因为每次都重新load图像文件
                //public static void ResizeImage
                //(
                //    AvlNet.Image inImage,
                //    int? inNewWidth,
                //    int? inNewHeight,
                //    AvlNet.ResizeMethod inResizeMethod,
                //    AvlNet.Image outImage
                //)
                AvlNet.AVL.ResizeImage(imageColor, imageSize, imageSize, AvlNet.ResizeMethod.Area, imageScaled);
                //mr:: HMI VideoBox控件; 有SetImage()设置控件图像
                zoomingVideoBox1.SetImage(imageScaled);
            }
        }


        private void zoomingVideoBox1_SizeModeChanged(object sender, EventArgs e)
        {
            //mr:: VideoBoxBase空间,来自于VideoBoxBase.dll
            radioOriginalSize.Checked = zoomingVideoBox1.SizeMode == VideoBoxBase.ZoomingVideoBoxMode.OriginalSize;
            radioFit.Checked = zoomingVideoBox1.SizeMode == VideoBoxBase.ZoomingVideoBoxMode.Fit;
        }


        private void radioOriginalSize_Click(object sender, EventArgs e)
        {
             
            zoomingVideoBox1.SizeMode = VideoBoxBase.ZoomingVideoBoxMode.OriginalSize;
        }


        private void radioFit_Click(object sender, EventArgs e)
        {
            zoomingVideoBox1.SizeMode = VideoBoxBase.ZoomingVideoBoxMode.Fit;
        }


        private void imageConfigChanged(object sender, EventArgs e)
        {
            LoadImage();
        }


        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            zoomingVideoBox1.ZoomIn();
        }


        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            zoomingVideoBox1.ZoomOut();
        }
    }
}
