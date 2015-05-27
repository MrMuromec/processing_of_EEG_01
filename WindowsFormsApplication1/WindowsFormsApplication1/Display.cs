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
        private List<C_rhythm> rhythms = new List<C_rhythm>(); // Список ритмов
        private C_EEG_rhythms1 sr_rhythms = new C_EEG_rhythms1(); // Учреднённый ритм

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

            textBox1.Text = " ";

            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        Tables_excels[comboBox2.SelectedIndex].get(out points);
                        d_paint(points, 150, 250, 50, 2);

                        break;
                    }
                case 1:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getf(out points);
                        d_paint(points, 150, 250, 50, 2);
                        textBox1.Text = text_of_rhythms(Tables_excels[comboBox2.SelectedIndex].rhythms_f);
                        break;
                    }
                case 2:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getfe(out points);
                        d_paint(points, 150, 250, 50, 2);
                        textBox1.Text = text_of_rhythms(Tables_excels[comboBox2.SelectedIndex].rhythms_fe);
                        break;
                    }
                default: break;
            }
            if (background) sr();
        }
        private void sr() // Рассчёт и отображение среднего
        {
            rhythms.Clear();
            textBox2.Text = " ";

            switch (comboBox1.SelectedIndex)
            {
                case 1:
                {
                    foreach (C_measurement element in Tables_excels) 
                        if (element.get())
                            rhythms.AddRange(element.rhythms_f.rhythms);                       
                    break;
                }
                case 2:
                {
                    foreach (C_measurement element in Tables_excels)
                        if (element.get())
                            rhythms.AddRange(element.rhythms_fe.rhythms);
                    break;
                }
                default: break;
            }           
            if (comboBox1.SelectedIndex!=0)
            {
                sr_rhythms.sr(rhythms);
                textBox2.Text = text_of_rhythms(sr_rhythms);
            }
        }
        private void d_paint(List<uint> points, byte Red, byte Green, byte Blue, byte width) // Отрисовка
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
            string str;
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
        private string text_of_rhythms(C_EEG_rhythms1 rhythms_) // Формирование текста
        {
            string str = "";
            for (int i = 0; i < rhythms_.rhythms.Count() ;i++ )
            {
                str += str_(rhythms_.rhythms[i].get_sum(), rhythms_.rhythms[i].get_name());
            }
            return str;
        }
        private string str_(double zn, string str) // формирование строки
        {
            string str_zn = (100 * zn).ToString().Substring(0, 5).Replace('.', ',');
            if (str_zn.Split(new char[] { ',' })[0].Length != str_zn.Split(new char[] { ',' })[1].Length)
                str_zn = str_zn.Insert(0, " ").Substring(0, 5);
            return str + " = " + str_zn + "%" + "\n";
        }
        private void button2_Click(object sender, EventArgs e) // Загрузка реакции
        {
            C_measurement fon = new C_measurement(); // Измерение

            background = false;

            rhythms.Clear();
            foreach (C_measurement element in Tables_excels)
                if (element.get())
                    rhythms.AddRange(element.rhythms_f.rhythms);
            sr_rhythms.sr(rhythms);
            fon.r_f(sr_rhythms);

            rhythms.Clear();
            foreach (C_measurement element in Tables_excels)
                if (element.get())
                    rhythms.AddRange(element.rhythms_fe.rhythms);
            sr_rhythms.sr(rhythms);
            fon.r_fe(sr_rhythms);

            fon.Add_(background);

            ImportExcel form = new ImportExcel(Tables_excels, background);
            form.MdiParent = this.MdiParent;
            form.Show(); 
            this.Close();
        }
    }
}
