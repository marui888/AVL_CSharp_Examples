//
// Adaptive Vision Library .NET Example - "Blister inspection" example
//
// Simple application that uses Adaptive Vision Library .NET to detect missing pills in blister.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Collections.Generic;
using AvlNet;

namespace blister_inspection
{
    class BlisterInspector : IDisposable
    {
        #region Private fields

        /// <summary>
        /// Reference Points marking pills place
        /// </summary>
        private readonly Point2D[] referencePoints =
        {
            new Point2D(171.483f, 121.251f),
            new Point2D(370.194f, 126.167f),
            new Point2D(571.644f, 123.777f),
            new Point2D(771.085f, 128.530f),
            new Point2D(967.293f, 127.534f),
            new Point2D(170.845f, 317.647f),
            new Point2D(371.472f, 322.536f),
            new Point2D(574.361f, 324.241f),
            new Point2D(772.259f, 317.141f),
            new Point2D(961.602f, 318.071f),
            new Point2D(177.171f, 556.918f),
            new Point2D(372.484f, 556.087f),
            new Point2D(569.593f, 551.422f),
            new Point2D(770.118f, 557.681f),
            new Point2D(962.179f, 557.160f),
            new Point2D(171.722f, 752.915f),
            new Point2D(370.396f, 755.501f),
            new Point2D(569.764f, 750.421f),
            new Point2D(769.133f, 753.231f),
            new Point2D(963.585f, 751.651f)
        };

        /// <summary>
        /// Indicates whether it is first inspection and thus initialization is needed
        /// </summary>
        private bool isFirstTime = true;

        /// <summary>
        /// Fitting maps used to locate blister
        /// </summary>
        private SegmentFittingMap horizontalFittingMap = new SegmentFittingMap();
        private SegmentFittingMap verticalFittingMap = new SegmentFittingMap();

        /// <summary>
        /// Segment used to scan against vertical edge of blister
        /// </summary>
        private readonly Segment2D verticalSegment = new Segment2D(103.333f, 137.556f, 103.333f, 410.889f);

        /// <summary>
        /// Segment used to scan against horizontal edge of blister
        /// </summary>
        private readonly Segment2D horizontalSegment = new Segment2D(294.444f, 35.333f, 601.111f, 35.333f);
        
        /// <summary>
        /// Parameters used to scan edges of blister
        /// </summary>
        private readonly EdgeScanParams scanParams = new EdgeScanParams(ProfileInterpolationMethod.Quadratic4, 1.0f, 5.0f, EdgeTransition.Any);

        /// <summary>
        /// Constants defining blister size
        /// </summary>
        private const int BlisterWidth = 1075;
        private const int BlisterHeight = 862;
        #endregion

        #region Public methods
        /// <summary>
        /// Performs inspection on blister image
        /// </summary>
        /// <param name="ioImage">Image which will be inspected, and on which results will be drawn</param>
        public void DoInspection(Image ioImage)
        {
            if (isFirstTime)
            {
                CreateScanningMaps(ioImage);
                isFirstTime = false;
            }

            Rectangle2D? blisterBoundingBox;

            LocateBlister(ioImage, out blisterBoundingBox);

            if (!blisterBoundingBox.HasValue)
                throw new ApplicationException("Couldn't locate blister on image!");

            PreprocessBlisterImage(ref ioImage, blisterBoundingBox.Value);

            var correctCircles = new List<Circle2D>();
            var incorrectCircles = new List<Circle2D>();

            foreach (var point in referencePoints)
            {
                Rectangle2D inspectionRect;
                AVL.CreateRectangle(point, Anchor2D.MiddleCenter, 0.0f, 102.0f, 102.0f, out inspectionRect);

                bool isPresent;
                AVL.CheckPresence_PixelAmount(ioImage, new ShapeRegion(inspectionRect, RectangularRoiMask.Ellipse), null,
                    HSxColorModel.HSV, 26,
                    52, 42, null, 62, null, 0.8f, 1.0f, out isPresent);

                Circle2D pillSpot;
                AVL.CreateCircle(point, Anchor2D.MiddleCenter, 62.0f, out pillSpot);

                if (isPresent)
                {
                    correctCircles.Add(pillSpot);
                }
                else
                {
                    incorrectCircles.Add(pillSpot);
                }
            }

            DrawResults(ioImage, correctCircles, incorrectCircles);
        }

        /// <summary>
        /// Cleans up any resources used
        /// </summary>
        public void Dispose()
        {
            if (horizontalFittingMap != null) horizontalFittingMap.Dispose();
            if (verticalFittingMap != null) verticalFittingMap.Dispose();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Creates bounding rectangle of blister based on detected edges
        /// </summary>
        private void CreateBlisterRectangleFromBorders(Segment2D? verticalBorder,
            Segment2D? horizontalBorder, out Rectangle2D? blisterRectangle)
        {
            if (verticalBorder == null || horizontalBorder == null)
            {
                blisterRectangle = new Rectangle2D();
                return;
            }

            Line2D vertical, horizontal;
            AVL.Segment2DToLine2D(verticalBorder.Value, out vertical);
            AVL.Segment2DToLine2D(horizontalBorder.Value, out horizontal);

            Point2D? pointOfIntersection;
            AVL.LineLineIntersection(vertical, horizontal, out pointOfIntersection);

            if (pointOfIntersection.HasValue)
            {
                float orientation;
                AVL.SegmentOrientation(horizontalBorder.Value, AngleRange._0_360, out orientation);

                Rectangle2D createdRectangle;
                AVL.CreateRectangle(pointOfIntersection.Value, Anchor2D.TopLeft, orientation, BlisterWidth, BlisterHeight,
                    out createdRectangle);
                blisterRectangle = createdRectangle;

                return;
            }
            blisterRectangle = null;
        }

        /// <summary>
        /// Applies preprocessing to blister image - crops and smooths it.
        /// </summary>
        private void PreprocessBlisterImage(ref Image ioImage, Rectangle2D blisterBoundingBox)
        {
            using (var cropped = new Image())
            {
                AVL.CropImageToRectangle(ioImage, blisterBoundingBox, null, CropScaleMode.InputScale, InterpolationMethod.NearestNeighbour, 0.0f, cropped);
                AVL.SmoothImage_Gauss(cropped, null, 3.0f, null, 2.0f, ioImage);
            }
        }

        /// <summary>
        /// Finds bounding box of blister on image
        /// </summary>
        private void LocateBlister(Image inImage, out Rectangle2D? blisterBoundingBox)
        {
            Segment2D? horizontalBorder, verticalBorder;

            AVL.FitSegmentToEdges(inImage, verticalFittingMap, scanParams, Selection.Best, null, 0.1f, null, out verticalBorder);

            AVL.FitSegmentToEdges(inImage, horizontalFittingMap, scanParams, Selection.Best, null, 0.1f, null, out horizontalBorder);

            if (horizontalBorder.HasValue && verticalBorder.HasValue)
            {
                CreateBlisterRectangleFromBorders(verticalBorder, horizontalBorder, out blisterBoundingBox );
                return;
            }
            blisterBoundingBox = null;
        }

        /// <summary>
        /// Initializes scan maps. Has to be performed once before every inspection.
        /// </summary>
        private void CreateScanningMaps(Image inImage)
        {
            //public static void CreateSegmentFittingMap
            //   (
            //       AvlNet.ImageFormat inImageFormat,
            //       AvlNet.SegmentFittingField inFittingField,
            //       int inScanCount,
            //       int inScanWidth,
            //       AvlNet.InterpolationMethod inImageInterpolation,
            //       AvlNet.SegmentFittingMap outFittingMap
            //   )

            //void avl::CreateSegmentFittingMap
            //    (

            //        const avl::ImageFormat&inImageFormat,
	           //     const avl::SegmentFittingField&inFittingField,
	           //     atl::Optional <const avl::CoordinateSystem2D&> inFittingFieldAlignment,
	           //     const int inScanCount,

            //        const int inScanWidth,
            //        avl::InterpolationMethod::Type inImageInterpolation,
	           //     SegmentFittingMap & outFittingMap,
	           //     atl::Array<avl::Segment2D> & diagScanSegments,
	           //     atl::Array<avl::Rectangle2D> & diagSamplingAreas
            //    )

            AVL.CreateSegmentFittingMap(new ImageFormat(inImage), 
                new SegmentFittingField(horizontalSegment, 200.0f), 
                null, 
                10, 5, 
                InterpolationMethod.Bilinear, 
                horizontalFittingMap);

            AVL.CreateSegmentFittingMap(new ImageFormat(inImage),
                new SegmentFittingField(verticalSegment, 200.0f),
                null, 
                10, 5, 
                InterpolationMethod.Bilinear, 
                verticalFittingMap);
        }

        /// <summary>
        /// Draws results of inspection on image
        /// </summary>
        private void DrawResults(Image ioImage, List<Circle2D> correctCircles, List<Circle2D> incorrectCircles)
        {
            var defaultStyle = new DrawingStyle
            {
                DrawingMode = DrawingMode.HighQuality,
                Filled = false,
                Opacity = 1.0f,
                PointShape = null,
                PointSize = 2.0f,
                Thickness = 4.0f
            };

            foreach (var correctCircle in correctCircles)
            {
                AVL.DrawCircle(ioImage, correctCircle, null, Pixel.Green, defaultStyle);
            }

            foreach (var incorrectCircle in incorrectCircles)
            {
                AVL.DrawCircle(ioImage, incorrectCircle, null, Pixel.Red, defaultStyle);
            }
        }

        #endregion
    }
}
