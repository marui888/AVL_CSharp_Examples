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
            //mr:: ʹ��PropertyGrid�ؼ�������ʾ��������
            propertyGrid.SelectedObject = parameters;

            // Load default image
            LoadImage("..\\..\\..\\..\\..\\_media\\different.png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //mr?? ΪɶҪ������ֶ�.private System.ComponentModel.IContainer components = null;
                components.Dispose();

                // Release designer resources
                //mr:: Designer��Դ��ҪDispose
                edgeModelDesigner.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Find objects using a template matching technique.
        /// </summary>
        void FindObjects()
        {
            //mr?? ���ﲢû���� Nullable; Object2D��class; 
            var foundObjects = new List<Object2D>();

            // Use data from parameters object. Reducing ranges of parameters must be performed
            // to avoid exceptions.

            //mr?? ���AVL������AVS�е�ͬ��Fiter��Ƚ�, ����������.
            AVL.LocateMultipleObjects_Edges(sourceImage, //����ͼ��
                edgeModel, //mr?? ģ������ �����edgeModel��û��ʹ��SafeNullable<T>����
                0, //��С������ֵ
                Math.Max(0.1f, parameters.EdgeThresholdLevel),//�߽�ǿ����Сֵ
                EdgePolarityMode.Ignore, //��ָ���߽缫��
                EdgeNoiseLevel.High, //����ΪHigh��ʾģ����ͼ���еĴ�����Ŀ����ϴ�, ����ΪLow��ʾ����С
                false,//�Ƿ�ƥ�����ͼ���Ե��Ŀ��, false��ʾ����ƥ��
                Math.Max(0, Math.Min(1.0f, parameters.MinimalScore)), //��ЧĿ������ƥ��÷�
                Math.Max(0.1f, parameters.MinimalDistances),//Ŀ�����С����
                foundObjects //����ҵ�������Ŀ��
                );

            // Draw rectangles on an image to show results.

            using (var previewImage = new Image(sourceImage))
            {

                if (parameters.DrawResultsOnPreview)
                {
                    foreach (var match in foundObjects)
                    {
                        //mr:: ���ҵ�Ŀ����ⲿ�����ο�. match��Object2D����, match.Match��Rectangle2D����
                        AVL.DrawRectangle(previewImage, match.Match, Pixel.Red, new DrawingStyle());
                    }
                }

                if (previewImagebox.Image != null)
                    previewImagebox.Image.Dispose();

                //mr:: ����System.Drawing.Bitmap����,��ʾ��pictureBox��.
                previewImagebox.Image = previewImage.CreateBitmap();
            }

            //mr?? https://www.fab-image.com/en/home/
            if (edgeModelDesigner.ReferenceFrame != null)
                //mr::���Ŀ����ԭͼ���ϵ�����λ�� (Exact position of the model object in the image.)
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

             //mr:: ����EdgeModelDesigner
            if (File.Exists("config.avconfig"))
                edgeModelDesigner.LoadParameters("config.avconfig");

            edgeModelDesigner.Backgrounds = new[] { sourceImage };

            if (edgeModelDesigner.ShowDialog() != DialogResult.OK)
                return;

            //mr:: ����EdgeModeDesigner���ò�������
            edgeModelDesigner.SaveParameters("config.avconfig");

            findObjectsButton.Enabled = true;

            //mr:: ԭ�е�ģ����ҪDispose().
            if (edgeModel != null)
                edgeModel.Dispose();

            //mr:: ��Designer��ȡ�����ɵ�EdgeModel����; ���edgeModel����EdgeModel����.
            edgeModel = edgeModelDesigner.GetEdgeModel();

            // Prepare preview model image

            // 1. Paint not selected parts of image using gray color.
            //mr:: ��ѡ�����������ͼ���ɻ�ɫ
            using (var templateComplementRegion = new Region())
            using (Image
                modelPreview = new Image(sourceImage), //mr:: �����copyͼ��
                modelImage = new Image())
            {
                //mr:: ������
                AVL.RegionComplement(
                    edgeModelDesigner.TemplateRegion, //��������. ��EdgeModeDesigner����
                    templateComplementRegion //�������
                    );

                //mr:: ��ͼ��Image�ϻ���һ������Region, �ı���modelPreview
                //  ����RegionԤ�Ⱦ�������. ���API�����������.
                AVL.DrawRegion(
                    modelPreview, //����Image
                    templateComplementRegion, //����Region
                    null, // �������������ϵͳ
                    Pixel.Gray, //��ɫ
                    1.0f // ͸����
                    );


                // 2. Crop image to the bounding box of model template.

                Box boundingBox; //Box��struct����
                //mr:: ��ģ��Region������С�������; 
                //public static void RegionBoundingBox
                //(
                //    AvlNet.Region inRegion,
                //    out AvlNet.Box outBoundingBox
                //)
                AVL.RegionBoundingBox(
                    edgeModelDesigner.TemplateRegion, //��������Region����
                    out boundingBox //��򱣴�, ע�������û����ת�ľ���.������BoundingBox��Region��ȫ�غ�.
                    );

                //mr:: ��ȡͼ��. inSelection����������Box����.
                //public static void CropImage(
                //    AvlNet.Image inImage,
                //    AvlNet.Box inSelection,
                //    out AvlNet.Image outImage
                //)
                AVL.CropImage(
                    modelPreview, //ԭͼ��
                    boundingBox, //��С����ʾ��ͼ�ķ�Χ
                    modelImage //���ͼ��
                    );

                //mr:: PictureBoxԭ��Image��Դ��ҪDispose
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
