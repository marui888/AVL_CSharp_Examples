//
// Adaptive Vision Library .NET Example - "Nails, screws and nuts" example
//
// Simple application that uses Adaptive Vision Library .NET to separate nails from other objects on image.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;

namespace nails_screws_and_nuts_avlNET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An exception occurred during program execution: " + ((Exception) e.ExceptionObject).Message);
            Application.Exit();
        }
    }
}
