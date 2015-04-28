using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    public partial class Display : Form
    {
        List<C_measurement> Tables_excel = new List<C_measurement>(); // Все измерения
        bool background; // ФОН (да/нет)
        public Display(List<C_measurement> tables_excel, bool background)
        {
            this.Tables_excel.AddRange(tables_excel);
            this.background = background;
            InitializeComponent();
        }
        private void Display_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                List<uint> points = new List<uint>();                
                Tables_excel[0].get(out points);

                double yy = (double) pictureBox1.Size.Height / (points.Max() - points.Min());
                double xx = (double) pictureBox1.Size.Width / (points.Count);
                
                System.Drawing.Point[] PS = new Point[points.Count];
                for (int i = 0; i < points.Count; i++)
                {
                    PS[i].X = (int)(i * xx);
                    PS[i].Y = (int)(pictureBox1.Size.Height - points[i] * yy);
                }
                g.DrawLines(Pens.DarkGreen,PS);
            }
        }
    }
}
