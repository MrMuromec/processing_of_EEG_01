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
    public partial class Display1 : Form
    {
        private List<C_measurement> Tables_excels = new List<C_measurement>(); // Все измерения
        private bool background; // ФОН (да/нет)
        private string str;
        public Display1(List<C_measurement> tables_excel, bool background)
        {
            this.Tables_excels.AddRange(tables_excel);
            this.background = background;
            InitializeComponent();
        }
        private void Display_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            List<uint> points = new List<uint>();

            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        Tables_excels[comboBox2.SelectedIndex].get(out points);
                        d_paint(points, 150, 250, 50, 2);

                        textBox1.Text = 
                        textBox2.Text = 
                        textBox3.Text = 
                        textBox4.Text = 
                        textBox5.Text = 
                        textBox6.Text = " ";
                        break;
                    }
                case 1:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getf(out points);
                        d_paint(points, 150, 250, 50, 2);

                        textBox1.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Delta.get_sum())).ToString() + "%";
                        textBox2.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Theta.get_sum())).ToString() + "%";
                        textBox3.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Alpha.get_sum())).ToString() + "%";
                        textBox4.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Beta1.get_sum())).ToString() + "%";
                        textBox5.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Beta2.get_sum())).ToString() + "%";
                        textBox6.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_f.r_Gamma.get_sum())).ToString() + "%";
                        break;
                    }
                case 2:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getfe(out points);
                        d_paint(points, 150, 250, 50, 2);

                        textBox1.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Delta.get_sum())).ToString() + "%";
                        textBox2.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Theta.get_sum())).ToString() + "%";
                        textBox3.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Alpha.get_sum())).ToString() + "%";
                        textBox4.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Beta1.get_sum())).ToString() + "%";
                        textBox5.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Beta2.get_sum())).ToString() + "%";
                        textBox6.Text = ((int)(100 * Tables_excels[comboBox2.SelectedIndex].rhythms_fe.r_Gamma.get_sum())).ToString() + "%";
                        break;
                    }
                default: break;
            }
            points.Clear();
            if (background) sr(points);
            points.Clear();
        }
        private void sr(List<uint> points)
        {
            double[] rhythms_sr= new double[6] {0,0,0,0,0,0};

            switch (comboBox1.SelectedIndex)
            {
                case 1:
                {
                    foreach (C_measurement element in Tables_excels) 
                    {
                        element.getf(out points);
                        if (element.get())
                        {
                            rhythms_sr[0] += element.rhythms_f.r_Delta.get_sum();
                            rhythms_sr[1] += element.rhythms_f.r_Theta.get_sum();
                            rhythms_sr[2] += element.rhythms_f.r_Alpha.get_sum();
                            rhythms_sr[3] += element.rhythms_f.r_Beta1.get_sum();
                            rhythms_sr[4] += element.rhythms_f.r_Beta2.get_sum();
                            rhythms_sr[5] += element.rhythms_f.r_Gamma.get_sum();
                        }
                    }                         
                    break;
                }
                case 2:
                {
                    foreach (C_measurement element in Tables_excels)
                    {
                        element.getfe(out points);
                        if (element.get())
                        {
                            rhythms_sr[0] += element.rhythms_fe.r_Delta.get_sum();
                            rhythms_sr[1] += element.rhythms_fe.r_Theta.get_sum();
                            rhythms_sr[2] += element.rhythms_fe.r_Alpha.get_sum();
                            rhythms_sr[3] += element.rhythms_fe.r_Beta1.get_sum();
                            rhythms_sr[4] += element.rhythms_fe.r_Beta2.get_sum();
                            rhythms_sr[5] += element.rhythms_fe.r_Gamma.get_sum();
                        }
                    }
                    break;
                }
                default: break;
            }
            if (comboBox1.SelectedIndex!=0)
            {
                if (Tables_excels[Tables_excels.Count - 1].get())
                    for (int i = 0; i < 6; i++)
                        rhythms_sr[i] = rhythms_sr[i] / Tables_excels.Count();
                else
                    for (int i = 0; i < 6; i++)
                        rhythms_sr[i] = rhythms_sr[i] / (Tables_excels.Count() - 1);
                textBox12.Text = ((100 * rhythms_sr[0])).ToString().Substring(0, 5) +"%";
                textBox11.Text = ((100 * rhythms_sr[1])).ToString().Substring(0, 5) + "%";
                textBox10.Text = ((100 * rhythms_sr[2])).ToString().Substring(0, 5) + "%";
                textBox9.Text = ((100 * rhythms_sr[3])).ToString().Substring(0, 5) + "%";
                textBox8.Text = ((100 * rhythms_sr[4])).ToString().Substring(0, 5) + "%";
                textBox7.Text = ((100 * rhythms_sr[5])).ToString().Substring(0, 5) + "%";
            }
            else
            {
                textBox12.Text = 
                textBox11.Text = 
                textBox10.Text = 
                textBox9.Text = 
                textBox8.Text = 
                textBox7.Text = " ";
            }
        }
        private void d_paint(List<uint> points, byte Red, byte Green, byte Blue, byte width)
        {
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                double yy = (double)pictureBox1.Size.Height / (points.Max() - points.Min());
                double xx = (double)pictureBox1.Size.Width / (points.Count());
                System.Drawing.Point[] PS = new Point[points.Count];
                for (int i = 0; i < points.Count(); i++)
                {
                    PS[i].X = (int)(i * xx);
                    PS[i].Y = (int)(pictureBox1.Size.Height - points[i] * yy);
                }
                g.DrawLines(new Pen(Color.FromArgb(Red, Green, Blue), width), PS);
            }
        }
        private void Display_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;

            for (int i = 0; i<Tables_excels.Count ; i++)
            {
                Tables_excels[i].get(out str);
                comboBox2.Items.Add(str);
            }
            comboBox2.SelectedIndex = Tables_excels.Count - 1;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ImportExcel form = new ImportExcel(Tables_excels, background);
            form.MdiParent = this.MdiParent;
            form.Show(); 
            this.Close();
        }
    }
}
