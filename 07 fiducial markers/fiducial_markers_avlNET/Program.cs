//
// Adaptive Vision Library .NET Example - "Fiducial markers" example
//
// Simple application that uses Adaptive Vision Library .NET to locate fiducial markers on PCB image.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;

namespace fiducial_markers_avlNET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
