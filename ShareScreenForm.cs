using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ratserver
{
    public partial class ShareScreenForm : Form
    {
        Graphics graphics;
        Size clientScreenSize;
        public ShareScreenForm()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }

        public void SetBitmap(ref Bitmap bitmap)
        {
            var localImage = new Bitmap(bitmap, Bounds.Size);
            clientScreenSize = bitmap.Size;
            graphics.DrawImage(localImage, new Point(0, 0));
            localImage.Dispose();
            localImage = null;
        }
        const int LeftButtonCode = 1;
        const int RightButtonCode = 2;
        const int ScrollClick = 3;
        private void ShareScreenForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                int code;
                if (e.Button == MouseButtons.Left) code = LeftButtonCode;
                else if (e.Button == MouseButtons.Right) code = RightButtonCode;
                else code = ScrollClick;

                float xPerc = (float)e.Location.X / (float)Bounds.Width;
                float yPerc = (float)e.Location.Y / (float)Bounds.Height;
                int mousex = (int)(xPerc * clientScreenSize.Width);
                int mousey = (int)(yPerc * clientScreenSize.Height);

                ServerFunctions.SendKeyStroke(Form1.selectedPc, 0, code, mousex, mousey);
            }
        }
    }
}
