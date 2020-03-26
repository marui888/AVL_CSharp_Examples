//
// Adaptive Vision Library .NET Example - "Template Matching Designer" example
//
// Simple demonstration applications which uses a Template Matching Designer
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;
using AvlNet;
using AvlNet.Designers;
using System.IO;
using System.Collections.Generic;

namespace template_matching_designer
{
    public partial class MainWindow : Form
    {
        private Image sourceImage;
        private EdgeModel edgeModel;

        // Object to be showed in Property Grid
        private readonly SearchParameters parameters = new SearchParameters();


        // Model designer
        private readonly EdgeModelDesigner edgeModelDesigner = new EdgeModelDesigner();

        //private readonly GrayModelDesigner grayModelDesigner = new GrayModelDesigner();

        public MainWindow()
        {

            InitializeComponent();

            // Setting default training parameters values
            
            edgeModelDesigner.MinAngle = -45;
            edgeModelDesigner.MaxAngle = 45;

            // Sow parameters object in property grid
            //mr:: 使用PropertyGrid控件方便显示对象属性
            propertyGrid.SelectedObject = parameters;

            // Load default image
            LoadImage("..\\..\\..\\..\\..\\_media\\different.png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //mr?? 为啥要加这个字段.private System.ComponentModel.IContainer components = null;
                components.Dispose();

                // Release designer resources
                //mr:: Designer资源需要Dispose
                edgeModelDesigner.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Find objects using a template matching technique.
        /// </summary>
        void FindObjects()
        {
            //mr?? 这里并没有用 Nullable; Object2D是class; 
            var foundObjects = new List<Object2D>();

            // Use data from parameters object. Reducing ranges of parameters must be performed
            // to avoid exceptions.

            //mr?? 这个AVL方法与AVS中的同名Fiter相比较, 参数有区别.
            AVL.LocateMultipleObjects_Edges(sourceImage, //输入图像
                edgeModel, //mr?? 模板数据 这里的edgeModel并没有使用SafeNullable<T>类型
                0, //最小金字塔值
                Math.Max(0.1f, parameters.EdgeThresholdLevel),//边界强度最小值
                EdgePolarityMode.Ignore, //不指定边界极性
                EdgeNoiseLevel.High, //设置为High表示模板与图像中的待查找目标差别较大, 设置为Low表示差别较小
                false,//是否匹配出现图像边缘的目标, false表示不用匹配
                Math.Max(0, Math.Min(1.0f, parameters.MinimalScore)), //有效目标的最低匹配得分
                Math.Max(0.1f, parameters.MinimalDistances),//目标间最小距离
                foundObjects //输出找到的所有目标
                );

            // Draw rectangles on an image to show results.

            using (var previewImage = new Image(sourceImage))
            {

                if (parameters.DrawResultsOnPreview)
                {
                    foreach (var match in foundObjects)
                    {
                        //mr:: 在找到目标的外部画矩形框. match是Object2D类型, match.Match是Rectangle2D类型
                        AVL.DrawRectangle(previewImage, match.Match, Pixel.Red, new DrawingStyle());
                    }
                }

                if (previewImagebox.Image != null)
                    previewImagebox.Image.Dispose();

                //mr:: 生成System.Drawing.Bitmap对象,显示在pictureBox中.
                previewImagebox.Image = previewImage.CreateBitmap();
            }

            //mr?? https://www.fab-image.com/en/home/
            if (edgeModelDesigner.ReferenceFrame != null)
                //mr::输出目标在原图像上的坐标位置 (Exact position of the model object in the image.)
                referenceLabel.Text = "Frame: " + edgeModelDesigner.ReferenceFrame.Value;
        }

        // Load and show image on a form.
        void LoadImage(string path)
        {
            if (!File.Exists(path))
                return;

            if (sourceImage == null)
                sourceImage = new Image();

            AVL.LoadImage(path, false, sourceImage);

            if (previewImagebox.Image != null)
                previewImagebox.Image.Dispose();

            previewImagebox.Image = sourceImage.CreateBitmap();
            createModelbutton.Enabled = true;
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            // Select image to open using OpenFileDialog.

            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "PNG files|*.png";
                openDialog.RestoreDirectory = true;
                openDialog.InitialDirectory = 
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\_media\\"));

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(openDialog.FileName);
                }
            }
        }

        private void createModelbutton_Click(object sender, EventArgs e)
        {
            edgeModelDesigner.ClearBackgroundImages();

             //mr:: 调用EdgeModelDesigner
            if (File.Exists("config.avconfig"))
                edgeModelDesigner.LoadParameters("config.avconfig");

            edgeModelDesigner.Backgrounds = new[] { sourceImage };

            if (edgeModelDesigner.ShowDialog() != DialogResult.OK)
                return;

            //mr:: 保存EdgeModeDesigner配置参数数据
            edgeModelDesigner.SaveParameters("config.avconfig");

            findObjectsButton.Enabled = true;

            //mr:: 原有的模板需要Dispose().
            if (edgeModel != null)
                edgeModel.Dispose();

            //mr:: 从Designer获取刚生成的EdgeModel数据; 这个edgeModel就是EdgeModel类型.
            edgeModel = edgeModelDesigner.GetEdgeModel();

            // Prepare preview model image

            // 1. Paint not selected parts of image using gray color.
            //mr:: 将选择区域以外的图像变成灰色
            using (var templateComplementRegion = new Region())
            using (Image
                modelPreview = new Image(sourceImage), //mr:: 相对于copy图像
                modelImage = new Image())
            {
                //mr:: 区域求补
                AVL.RegionComplement(
                    edgeModelDesigner.TemplateRegion, //输入区域. 由EdgeModeDesigner给出
                    templateComplementRegion //输出区域
                    );

                //mr:: 在图像Image上画出一个区域Region, 改变了modelPreview
                //  区域Region预先就生成了. 这个API命名容易误解.
                AVL.DrawRegion(
                    modelPreview, //输入Image
                    templateComplementRegion, //输入Region
                    null, // 参数区域的坐标系统
                    Pixel.Gray, //颜色
                    1.0f // 透明度
                    );


                // 2. Crop image to the bounding box of model template.

                Box boundingBox; //Box是struct类型
                //mr:: 以模板Region生成最小矩形外框; 
                //public static void RegionBoundingBox
                //(
                //    AvlNet.Region inRegion,
                //    out AvlNet.Box outBoundingBox
                //)
                AVL.RegionBoundingBox(
                    edgeModelDesigner.TemplateRegion, //输入区域Region类型
                    out boundingBox //外框保存, 注意外框是没有旋转的矩形.本例中BoundingBox与Region完全重合.
                    );

                //mr:: 截取图像. inSelection参数必须是Box类型.
                //public static void CropImage(
                //    AvlNet.Image inImage,
                //    AvlNet.Box inSelection,
                //    out AvlNet.Image outImage
                //)
                AVL.CropImage(
                    modelPreview, //原图像
                    boundingBox, //最小外框表示截图的范围
                    modelImage //输出图像
                    );

                //mr:: PictureBox原有Image资源需要Dispose
                if (modelPreviewImageBox.Image != null)
                    modelPreviewImageBox.Image.Dispose();


                modelPreviewImageBox.Image = modelImage.CreateBitmap();
            }

            FindObjects();
        }

        private void findObjectsButton_Click(object sender, EventArgs e)
        {
            FindObjects();
        }
    }
}
