//
// Adaptive Vision Library .NET Example - "Cap" example
//
// Simple application that uses Adaptive Vision Library .NET to find whether the bottle cap is correctly closed
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Collections.Generic;
using AvlNet;

namespace cap
{
    class CapInspection : IDisposable
    {
        #region Private fields

        /// <summary>
        /// Input image format, used for creation of ScanMaps
        /// </summary>
        private readonly ImageFormat format;

        /// <summary>
        /// Path used to perform scanning for bottle
        /// </summary>
        private readonly Path scanningPath;

        /// <summary>
        /// ScanMap used to find bottle on image
        /// </summary>
        private readonly ScanMap scanMap;

        #region Constants 

        /// <summary>
        /// Segments used to scan against defects on cap
        /// </summary>
        private readonly Segment2D[] scanSegments =
        {
            new Segment2D(10.0f, 21.0f, 10.0f, 74.0f),
            new Segment2D(178.0f, 21.0f, 178.0f, 74.0f),
            new Segment2D(350.0f, 21.0f, 350.0f, 74.0f)
        };

        /// <summary>
        /// Default drawing style
        /// </summary>
        private readonly DrawingStyle defaultStyle = new DrawingStyle
        {
            DrawingMode = DrawingMode.HighQuality,
            Filled = false,
            Opacity = 1.0f,
            PointShape = PointShape.Circle,
            PointSize = 1.0f,
            Thickness = 3.0f
        };

        /// <summary>
        /// 1D Edge scanning parameters used to detect defects
        /// </summary>
        private readonly EdgeScanParams scanParams = new EdgeScanParams(
            ProfileInterpolationMethod.Quadratic4,
            0.6f,
            4.0f,
            EdgeTransition.DarkToBright);

        /// <summary>
        /// Defect box size
        /// </summary>
        private const int DefectBoxSize = 40;

        /// <summary>
        /// Inspection result message font size
        /// </summary>
        private const float FontSize = 32.0f;

        #endregion
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Default and sole constructor.
        /// </summary>
        /// <param name="imageFormat">Format of image on which inspection will be performed</param>
        /// <param name="scanPath">Path on which scanning for bottle will be performed</param>
        public CapInspection(ImageFormat imageFormat, Path scanPath)
        {
            format = imageFormat;
            scanningPath = scanPath;
            //mr:: ScanMap需要Dispose(), scanMap作为字段, 在CapInspection管理字段的Dispose.
            scanMap = new ScanMap();
            /*
             public static void CreateScanMap
            (
	            AvlNet.ImageFormat inImageFormat,
	            AvlNet.Path inScanPath,
	            AvlNet.CoordinateSystem2D? inScanPathAlignment,
	            int inScanWidth,
	            AvlNet.InterpolationMethod inImageInterpolation,
	            AvlNet.ScanMap outScanMap,
	            AvlNet.Path outAlignedScanPath,
	            IList<AvlNet.Path> diagSamplingPoints,
	            out float diagSamplingStep
            )
            */

            //mr:: ScanMap依据ScanPath而生成.
            AVL.CreateScanMap(format, scanPath, null, 5, InterpolationMethod.Bilinear, scanMap);
        }

        #endregion

        #region Public methods 

        /// <summary>
        /// Performs cap inspection and draws results
        /// </summary>
        /// <param name="inputImage">Image with bottle. Inspection results will be drawn on it.</param>
        public void DoInspection(Image inputImage)
        {
            var localSystem = GetBottleCoordinateSystem(inputImage);

            var defects = new List<Edge1D>();

            foreach (var scanSegment in scanSegments)
            {
                using (var currentMap = new ScanMap())
                {
                    Edge1D? edge = null;

                    AVL.CreateScanMap(  format, 
                                        new Path(scanSegment), 
                                        localSystem, 
                                        70, 
                                        InterpolationMethod.Bilinear, 
                                        currentMap);

                    AVL.ScanSingleEdge(inputImage, 
                                        currentMap, 
                                        scanParams, 
                                        Selection.Best, 
                                        null, 
                                        out edge);

                    if (edge.HasValue)
                        defects.Add(edge.Value);
                }
            }

            DrawResults(inputImage, defects);

            //void avl::DrawCoordinateSystem
            //(
            //    avl::Image & ioImage,

            //    const avl::CoordinateSystem2D&inCoordinateSystem,
            // atl::Optional <const avl::CoordinateSystem2D&> inCoordinateSystemAlignment,
            // const avl::Pixel&inColor,
            // const avl::DrawingStyle &inDrawingStyle,
            // const float inArrowSize,

            //    const float inPixelScale
            //)

            foreach (var scanSegment in scanSegments)
            {
                //mr:: local坐标系 变成 image 坐标系
                AVL.AlignSegment(scanSegment, localSystem, false, out Segment2D alignedSegment);

                AVL.DrawSegment(inputImage, alignedSegment, Pixel.White, defaultStyle, MarkerType.None, 5);
            }
            //mr:: 画出寻找local坐标系原点的scanPath
            AVL.DrawPath(inputImage, scanningPath, Pixel.Yellow, defaultStyle);
            //mr:: 画出local坐标系.
            AVL.DrawCoordinateSystem(inputImage, localSystem, Pixel.Green, defaultStyle, 10, 50);
        }

        /// <summary>
        /// Cleans up held resources
        /// </summary>
        public void Dispose()
        {
            if (scanMap != null)
                scanMap.Dispose();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Draws colorful inspection results on provided image
        /// </summary>
        /// <param name="inputImage">Black-white bottle image</param>
        /// <param name="defects">List of detected defects. When empty, positive message is drawn.</param>
        private void DrawResults(Image inputImage, List<Edge1D> defects)
        {
            // When input image is monochromatic, one has to convert it multichannel do draw color markers
            if (inputImage.Depth == 1)
            {
                using (var colorImage = new Image())
                {
                    AVL.ConvertToMultichannel(inputImage, 
                        null, 
                        3, 
                        colorImage);
                    //mr:: 调用Reset() 这是AVL中的标准操作.
                    inputImage.Reset(colorImage);
                }
            }

            foreach (var defect in defects)
            {
                Rectangle2D defectRectangle;

                AVL.CreateRectangle(defect.Point, 
                    Anchor2D.MiddleCenter, 
                    0.0f, 
                    DefectBoxSize, 
                    DefectBoxSize,
                    out defectRectangle);

                AVL.DrawRectangle(inputImage, 
                    defectRectangle, 
                    null, 
                    Pixel.Red, 
                    defaultStyle);
            }

            var status = "PASS";
            var textColor = Pixel.Green;

            if (defects.Count > 0)
            {
                status = "FAIL";
                textColor = Pixel.Red;
            }

            AVL.DrawString(inputImage, 
                status, 
                new Location(300, 20),
                null, 
                Anchor2D.MiddleCenter, 
                textColor, 
                defaultStyle, 
                FontSize, 
                0.0f, 
                null);

            
        }

        /// <summary>
        ///  Finds bottle and computes its local coordinate system
        /// </summary>
        /// <param name="inputImage">Image with bottle</param>
        /// <returns>Bottles local coordinate system</returns>
        private CoordinateSystem2D GetBottleCoordinateSystem(Image inputImage)
        {
            CoordinateSystem2D ret;

            EdgeScanParams scanningParams = new EdgeScanParams(ProfileInterpolationMethod.Quadratic4, 0.6f, 10.0f, EdgeTransition.BrightToDark);

            Edge1D? edge;
            AVL.ScanSingleEdge(inputImage, scanMap, scanningParams, Selection.Best, null, out edge);


            if (edge.HasValue)
                //public static void CreateCoordinateSystemFromPoint
                //(
                //    AvlNet.Point2D inPoint,
                //    float inAngle,
                //    float inScale,
                //    float inScaleDivisor,
                //    out AvlNet.CoordinateSystem2D outCoordinateSystem
                //)
                AVL.CreateCoordinateSystemFromPoint(edge.Value.Point, 0.0f, 1.0f, 1.0f, out ret);
            else
                //mr:: CoordinateSystem2D 有一个缺省坐标系
                ret = CoordinateSystem2D.Default;

            return ret;
        }

        #endregion
    }
}
