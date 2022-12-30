using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Text2Image
{
    static class Program
    {
        public Program()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.ControlBox = true;
        }
        /// The main entry point for the application.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new FrmSteganography());
        }
    }
}









//NA
