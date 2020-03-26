//
// Adaptive Vision Library .NET Example - "Streaming GigE camera AvlNet" example
//
// Simple application that uses Adaptive Vision Library .NET to connect to the GigEVision camera and display acquired images in the ZoomingVideoBox control.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;

namespace streaming_GigE_camera_AvlNet
{

    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
