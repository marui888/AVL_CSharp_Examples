//
// Adaptive Vision Library .NET Example - "Measure badge" example
//
// Simple application that uses Adaptive Vision Library .NET. It performs simple 1D measurement on provided metal badge images.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;

namespace MeasureBadge
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
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new MainWindow());
        }
    }
}
