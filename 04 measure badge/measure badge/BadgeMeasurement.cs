//
// Adaptive Vision Library .NET Example - "Measure badge" example
//
// Simple application that uses Adaptive Vision Library .NET. It performs simple 1D measurement on provided metal badge images.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvlNet;

namespace MeasureBadge
{
    internal class BadgeMeasurement
    {
        #region Private fields

        private string imageDirectory;

        private string[] imagePaths;
        private int currentImageIndex;

        /// <summary>
        /// Lastly measured distance between badge holes
        /// </summary>
        private float lastDistance;

        /// <summary>
        /// Local dimensions and position of leftmost hole
        /// </summary>
        /// //mr?? ΪɶCircle2D x, y�Ǹ���
        private readonly Circle2D leftExpectedCircle = new Circle2D(-160.077148f, -1.70134f, 17.548f);

        /// <summary>
        /// Local dimensions and position of rightmost hole
        /// </summary>
        private readonly Circle2D rightExpectedCircle = new Circle2D(157.903f, -1.344f, 17.548f);

        /// <summary>
        /// Parameters used to scan circles on badge image
        /// </summary>
        private readonly EdgeScanParams scanParams = new EdgeScanParams(ProfileInterpolationMethod.Quadratic4,
                                                                        1.0f, 10.0f, EdgeTransition.BrightToDark);

        /// <summary>Style used to drawing circles</summary>
        private readonly DrawingStyle drawingStyle = new DrawingStyle(
            drawingMode: DrawingMode.HighQuality,
            opacity: 1.0f,
            thickness: 3.0f,
            filled: false,
            pointShape: PointShape.Circle,
            pointSize: 2.0f);

        #endregion


        #region Public properties

        /// <summary>
        /// Allows to set and get directory containing badge images
        /// </summary>
        public string ImagesDirectory
        {
            get { return imageDirectory; }
            set
            {
                if (!string.Equals(imageDirectory, value))
                {
                    imageDirectory = value;
                    imagePaths = System.IO.Directory.GetFiles(value, "*.png");
                }
            }
        }

        /// <summary>
        /// Returns formatted string with measured distance between badge holes on last image
        /// </summary>
        public string LastMeasuredDistance
        {
            get
            {
                return (float.IsNaN(lastDistance) ? string.Empty : lastDistance.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads, process and returns next image
        /// </summary>
        /// <returns>Badge image with measurement results drawn on it</returns>
        public void GetNextMeasuredImage(Image image)
        {
            if (imagePaths == null || !imagePaths.Any())
                throw new ApplicationException("Image directory is undefined or no image could be found.");

            if (currentImageIndex >= imagePaths.Length)
            {
                // start from beginning
                currentImageIndex = 0;
            }

            //mr:: �����Image�� AvlNet.Image
            AVL.LoadImage(imagePaths[currentImageIndex], false, image);

            // Reset last distance
            lastDistance = float.NaN;

            //mr:: ���ɱ�������ϵ
            // Localize badge and get its local coordinate system
            var localSystem = GetBadgeLocalCoordinateSystem(image);


            using (var circleMap = new CircleFittingMap())
            {
                Circle2D? leftCircle, rightCircle;

                // Create fitting map for leftmost circle
                //mr:: �������Map
                AVL.CreateCircleFittingMap(
                    new ImageFormat(image),
                    new CircleFittingField(leftExpectedCircle, 10.0f),  
                    localSystem, //mr:: ���������local����ϵ, �� CircleFittingField���ǻ���local����ϵ��.
                    20, 
                    3,
                    InterpolationMethod.Bilinear,
                    circleMap);

                // Fit circle on image
                //mr:: ��ʼ���(�ṩ ���Map, EdgeScanParams�Ȳ���) 
                AVL.FitCircleToEdges(image, 
                    circleMap, 
                    scanParams,
                    Selection.Best,
                    0.1f,
                    CircleFittingMethod.GeometricLandau, 
                    out leftCircle);

                // Create fitting map for rightmost circle
                AVL.CreateCircleFittingMap(new ImageFormat(image), 
                    new CircleFittingField(rightExpectedCircle, 10.0f), 
                    localSystem, 20, 3,
                    InterpolationMethod.Bilinear, 
                    circleMap);

                //Fit circle on image
                AVL.FitCircleToEdges(image, circleMap, scanParams, 
                    Selection.Best, 0.1f,
                    CircleFittingMethod.GeometricLandau, 
                    out rightCircle);

                DrawResults(image, leftCircle, rightCircle);
            }

            ++currentImageIndex;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Draw measurement results on current image
        /// </summary>
        private void DrawResults(Image image, Circle2D? leftCircle, Circle2D? rightCircle)
        {
            if (image.Depth != 3)
            {
                using (var conversionImage = new Image())
                {
                    //mr::Creates a multichannel image from a monochromatic one by replicating its channel
                    AVL.ConvertToMultichannel(image, null, 3, conversionImage);
                    //mr:: Image.Reset()
                    image.Reset(conversionImage);
                    //mr:: ���ܰ�input, output��ͬ�Ĳ���ʹ��.
                    // AVL.ConvertToMultichannel(image, null, 3, image);
                }
            }


            if (!leftCircle.HasValue || !rightCircle.HasValue)
            {
                AVL.DrawString(image, "Hole was not found.",
                    new Location(260, 30), null, Anchor2D.MiddleCenter, Pixel.Black,
                    drawingStyle, 32, 0, Pixel.Red);
            }
            else
            {
                Segment2D circleConnectingSegment;

                //mr:: AVL.DrawString()��������ַ�
                
                AVL.PointToPointDistance(leftCircle.Value.Center, rightCircle.Value.Center, 1.0f, out lastDistance,
                    out circleConnectingSegment);

                AVL.DrawCircle(image, leftCircle.Value, null, Pixel.Green, drawingStyle);

                AVL.DrawCircle(image, rightCircle.Value, null, Pixel.Green, drawingStyle);

                AVL.DrawSegment(image, circleConnectingSegment, null, Pixel.Green, drawingStyle, MarkerType.TwoArrows, 5.0f);
            }
        }

        /// <summary>
        /// Performs image segmentation, localizes badge on image and creates relative coordinate system
        /// </summary>
        /// <returns>Coordinate system local to badge present on image</returns>
        private CoordinateSystem2D GetBadgeLocalCoordinateSystem(Image image)
        {
            using (var badgeBlob = new Region())
            {
                AVL.ThresholdToRegion(image, null, null, 128.0f, 0.0f, badgeBlob);

                //Point2D blobMassCenter;
                //AVL.RegionMassCenter(badgeBlob, out blobMassCenter);

                AVL.RegionMassCenter(badgeBlob, out Point2D blobMassCenter);

                float orientationAngle;
                AVL.RegionOrientation(badgeBlob, out  orientationAngle);


                orientationAngle = (orientationAngle >= 90.0f ? orientationAngle - 180.0f : orientationAngle);

                CoordinateSystem2D localSystem;                
                AVL.CreateCoordinateSystemFromPoint(blobMassCenter, orientationAngle, 1.0f, 1.0f, out localSystem);

                return localSystem;
            }
        }

        #endregion

    }
}
