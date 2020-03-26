//
// Adaptive Vision Library .NET Example - 'Ruler' example
//
// Simple application that uses Adaptive Vision Library .NET to find the stripe on image and measure its width.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;

namespace Ruler
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
