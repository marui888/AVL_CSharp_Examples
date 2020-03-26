//
// Adaptive Vision Library .NET Example - "Fiducial markers" example
//
// Simple application that uses Adaptive Vision Library .NET to locate fiducial markers on PCB image.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AvlNet;

namespace fiducial_markers_avlNET
{
    public partial class MainWindow : Form
    {
        #region Private fields

        /// <summary>
        /// Path to directory with sample images
        /// </summary>
        private const string ImagesDir = "../../../../../_media/fiducial_markers_img";

        private readonly string[] ImagePaths = System.IO.Directory.GetFiles(ImagesDir, "*.png");
        private int currentImageIndex;

        /// <summary>
        /// Path to image used to generate edge model of fiducial marker
        /// </summary>
        private const string TemplateImagePath = ImagesDir + "/template/template_image.png";

        /// <summary>
        /// Edge model used to find marker. Has to be initialized before usage.
        /// </summary>
        /// //mr:: �ο� SafeNullableRef<T>�Ķ��� 
        /// public sealed class SafeNullableRef<T> : NullableRef<T>, IDisposable where T : class, IDisposable
        private SafeNullableRef<EdgeModel> edgeModel = AvlNet.Nullable.CreateSafe<EdgeModel>();

        /// <summary>
        /// Drawing style used to present results
        /// </summary>
        private readonly DrawingStyle defaultStyle = new DrawingStyle
        {
            DrawingMode = DrawingMode.HighQuality,
            Filled = false,
            Opacity = 1.0f,
            PointShape = PointShape.Circle,
            Thickness = 5.0f
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default and sole constructor of MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Events' handling

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ImagePaths.Any())
                    throw new Exception(string.Format("No image found in directory '{0}'", ImagesDir));

                currentImageIndex = 0;

                //mr:: edgeModel��SafeNullableRef<EdgeModel>
                if (!edgeModel.HasValue)
                    GenerateEdgeModel();

                startButton.Enabled = false;
                timer.Enabled = true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            startButton.Enabled = false;


            //mr:: ÿ��timer Tick�¼���Ҫ�����µ�Image����. ̫Ӱ��������.
            using (Image
                currentImage = new Image(),
                backedImage = new Image())
            {
                if (currentImageIndex < ImagePaths.Length)
                {
                    AVL.LoadImage(ImagePaths[currentImageIndex], false, currentImage);

                    AVL.ConvertToMultichannel(currentImage, 3, backedImage);

                    //mr:: �ο� INullable<T>�Ķ���. 
                    //public interface INullable<T>
                    //{
                    //    T Value { get; }
                    //    bool HasValue { get; }

                    //    void Reset();
                    //    void Reset(T newValue);
                    //}

                
                    //mr:: �ο� NullableRef<T>�Ķ���
                    // public class NullableRef<T> : INullable<T> where T : class
                    //mr:: �ο� private SafeNullableRef<EdgeModel> edgeModel = AvlNet.Nullable.CreateSafe<EdgeModel>();
                    //mr!! ��������������.
                    var marker = new AvlNet.NullableRef<Object2D>();
                    //var marker = AvlNet.Nullable.Create<Object2D>();

                    if (edgeModel.HasValue)
                        //public static void LocateSingleObject_Edges
                        //(
                        //    AvlNet.Image inImage,
                        //    NullableRef<Region> inSearchRegion,
                        //    AvlNet.EdgeModel inEdgeModel,
                        //    int inMinPyramidLevel,
                        //    float inEdgeThreshold,
                        //    AvlNet.EdgePolarityMode inEdgePolarityMode,
                        //    AvlNet.EdgeNoiseLevel inEdgeNoiseLevel,
                        //    bool inIgnoreBoundaryObjects,
                        //    float inMinScore,
                        //    INullable<AvlNet.Object2D> outObject
                        //)
                        //AVS.LocateSingleObject_Edges()
                        AVL.LocateSingleObject_Edges(currentImage, 
                            null, 
                            edgeModel.Value, //mr:: edgeModel.Value����EdgeModel����
                            1, 
                            3, 
                            8.0f, 
                            EdgePolarityMode.Ignore, 
                            EdgeNoiseLevel.High, 
                            false, 
                            0.650f, 
                            marker); //mr:: marker��INullable<AvlNet.Object2D>����

                    if (marker.HasValue)
                        //public static void DrawRectangle
                        //(
                        //    AvlNet.Image ioImage,
                        //    AvlNet.Rectangle2D inRectangle,
                        //    AvlNet.CoordinateSystem2D? inRectangleAlignment,
                        //    AvlNet.Pixel inColor,
                        //    AvlNet.DrawingStyle inDrawingStyle
                        //)
                        AVL.DrawRectangle(backedImage, 
                            marker.Value.Match, //mr:: Match �� Rectangle2D
                            null, 
                            Pixel.Red, 
                            defaultStyle);

                    if (originalImageBox.Image != null)
                        originalImageBox.Image.Dispose();

                    if (resultImageBox.Image != null)
                        resultImageBox.Image.Dispose();

                    originalImageBox.Image = currentImage.CreateBitmap();

                    resultImageBox.Image = backedImage.CreateBitmap();

                    ++currentImageIndex;
                }
                else
                {
                    timer.Enabled = false;
                    startButton.Enabled = true;
                }
            }
        }

        #endregion

        #region Private methods

        private void GenerateEdgeModel()
        {
            using (var modelRegion = new Region())
            using (var templateImage = new Image())
            {
                AVL.LoadImage(TemplateImagePath, false, templateImage);

                Box templateboundingBox;

                //public static void CreateBox
                //(
                //    AvlNet.Location inLocation,
                //    AvlNet.Anchor2D inLocationAnchor,
                //    int inWidth,
                //    int inHeight,
                //    out AvlNet.Box outBox
                //)
                
                AVL.CreateBox(new Location(360, 330), Anchor2D.TopLeft, 250, 250, out templateboundingBox);

                //mr:: RegionOfInterest�Ķ���
                //public class RegionOfInterest : AvlComparableType<avl::RegionOfInterest>
                //mr:: Region�Ķ���
                //public class Region : AvlComparableType<avl::Region>

                //public static void CreateBoxRegion
                //(
                //    AvlNet.Box inBox,
                //    int inFrameWidth,
                //    int inFrameHeight,
                //    AvlNet.Region outRegion
                //)
                AVL.CreateBoxRegion(templateboundingBox, templateImage.Width, templateImage.Height, modelRegion);


                //public static void CreateEdgeModel
                //(
                //    AvlNet.Image inImage,
                //    NullableRef<AvlNet.Region> inTemplateRegion,
                //    AvlNet.Rectangle2D? inReferenceFrame,
                //    int inMinPyramidLevel,
                //    int? inMaxPyramidLevel,
                //    float inSmoothingStdDev,
                //    float inEdgeThreshold,
                //    float inEdgeHysteresis,
                //    float inMinAngle,
                //    float inMaxAngle,
                //    float inAnglePrecision,
                //    float inMinScale,
                //    float inMaxScale,
                //    float inScalePrecision,
                //    float inEdgeCompleteness,
                //    INullable<AvlNet.EdgeModel> outEdgeModel,
                //    out AvlNet.Point2D? outEdgeModelPoint,
                //    INullable<SafeList<AvlNet.Path>> diagEdges,  //mr:: ע��������Ͷ���
                //    INullable<SafeList<AvlNet.Image>> diagEdgePyramid  //mr:: ע��������Ͷ���
                //)

                //mr:: inTemplateRegion������ null, ������AvlNet.Region,������RegionOfInterest����
                AVL.CreateEdgeModel(templateImage, 
                    modelRegion,
                    null, 
                    0, 
                    null, 
                    0.0f, 
                    60.0f, 
                    40.0f, 
                    -45.0f, 45.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f, 
                    edgeModel); //mr!! ��CreateEdgeModel����������������� INullable<AvlNet.EdgeModel>����
                //mr:: �����涨��
                //     �ӿڶ�����������Ҫ��T�����޶�, ��Ϊ�ӿ��޷���������ʵ��.
                //public interface INullable<T>
                //{
                //    T Value { get; }
                //    bool HasValue { get; }

                //    void Reset();
                //    void Reset(T newValue);
                //}
            }
        }

        #endregion

    }
}
