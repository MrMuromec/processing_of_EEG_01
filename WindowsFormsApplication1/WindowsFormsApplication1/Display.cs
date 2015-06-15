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
        private C_EEG_rhythms1 sr_rhythms = new C_EEG_rhythms1(); // Усреднённый ритм

        public Display1(List<C_measurement> tables_excel, bool background)
        {
            this.Tables_excels.AddRange(tables_excel);
            this.background = background;
            InitializeComponent();
        }
        private void Display_Paint(object sender, PaintEventArgs e) // Событие отрисовки
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            List<uint> points = new List<uint>();
            List<int> ambit = new List<int>();

            textBox1.Text = " ";

            switch(comboBox1.SelectedIndex) // выбор отображаемых параметров
            {
                case 0:
                    {
                        Tables_excels[comboBox2.SelectedIndex].get(out points);
                      
                        ambit.Add(0);
                        ambit.Add(Tables_excels[comboBox2.SelectedIndex].get_i()[0]);
                        ambit.Add(Tables_excels[comboBox2.SelectedIndex].get_i()[1]);
                        ambit.Add(points.Count());
                        d_paint2(points, ambit);

                        break;
                    }
                case 1:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getf(out points);

                        ambit.Add(0);
                        for (int i = 0; i < Tables_excels[comboBox2.SelectedIndex].spek.rhythms_f.rhythms.Count(); i++ )
                        {
                            ambit.Add(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_f.rhythms[i].get_f_i()[0]);
                            ambit.Add(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_f.rhythms[i].get_f_i()[1]);
                        }
                        ambit.Add(points.Count());
                        d_paint2(points, ambit);

                        textBox1.Text = text_of_rhythms(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_f);
                        break;
                    }
                case 2:
                    {
                        Tables_excels[comboBox2.SelectedIndex].getfe(out points);

                        ambit.Add(0);
                        for (int i = 0; i < Tables_excels[comboBox2.SelectedIndex].spek.rhythms_fe.rhythms.Count(); i++)
                        {
                            ambit.Add(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_fe.rhythms[i].get_f_i()[0]);
                            ambit.Add(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_fe.rhythms[i].get_f_i()[1]);
                        }
                        ambit.Add(points.Count());
                        d_paint2(points, ambit);

                        textBox1.Text = text_of_rhythms(Tables_excels[comboBox2.SelectedIndex].spek.rhythms_fe);
                        break;
                    }
                default: break;
            }
            if (background) sr(); // Если фон то выводим среднее
        }
        private void d_paint2(List<uint> points, List<int> ambit ) // Отрисовка
        {
            List<byte> Red = new List<byte>();
            List<byte> Green = new List<byte>();
            List<byte> Blue = new List<byte>();
            byte width = 2;

            for (int i = 0; i < ambit.Count() - 1; i++) // Планы на будущее: не изголяться и назначить каждому интервалу свой цвет
            {
                Red.Add((byte)((double)255*i/ambit.Count()));
                Green.Add((byte)((double)255 * (ambit.Count() - i) / ambit.Count()));
                Blue.Add((byte)((double)255 * i / ambit.Count()));
            }

            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                double yy = (double)pictureBox1.Size.Height / (points.Max() - points.Min());
                double xx = (double)pictureBox1.Size.Width / (points.Count());

                for (int j = 0; j < ambit.Count() - 1; j++)
                {
                    if ((ambit[j + 1] - ambit[j])>0)
                    {
                        System.Drawing.Point[] PS = new Point[ambit[j + 1] - ambit[j]];
                        for (int i = ambit[j]; i < ambit[j + 1]; i++)
                        {
                            PS[i - ambit[j]].X = (int)(i * xx);
                            PS[i - ambit[j]].Y = (int)(pictureBox1.Size.Height - points[i] * yy);
                        }
                        g.DrawLines(new Pen(Color.FromArgb(Red[j], Green[j], Blue[j]), width), PS);
                    }
                }
            }
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
                        if (element.get_bool() && element.sr_bool())
                            rhythms.AddRange(element.spek.rhythms_f.rhythms);                       
                    break;
                }
                case 2:
                {
                    foreach (C_measurement element in Tables_excels)
                        if (element.get_bool() && element.sr_bool())
                            rhythms.AddRange(element.spek.rhythms_fe.rhythms);
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
        private void Display_Load(object sender, EventArgs e) // Загрузка формы 
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
        private void button1_Click(object sender, EventArgs e) // Загрузка нового измерения
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
            C_measurement fon = new C_measurement(); // Измерение для среднего фона           

            // Средний спектр - ритмы
            rhythms.Clear();
            foreach (C_measurement element in Tables_excels)
                if (element.get_bool())
                    rhythms.AddRange(element.spek.rhythms_f.rhythms);
            sr_rhythms.sr(rhythms);
            fon.r_f(sr_rhythms);

            // Средний спектр миощности - ритмы
            rhythms.Clear();
            foreach (C_measurement element in Tables_excels)
                if (element.get_bool())
                    rhythms.AddRange(element.spek.rhythms_fe.rhythms);
            sr_rhythms.sr(rhythms);
            fon.r_fe(sr_rhythms);
            
            fon.Add_(background);
            Tables_excels.Add(fon);
            
            background = false; // Теперь не фон, а последнее измерение

            ImportExcel form = new ImportExcel(Tables_excels, background);
            form.MdiParent = this.MdiParent;
            form.Show(); 
            this.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Выбор измерения и пероерисовка
        {
            this.Invalidate();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // Выбор режима и вызов перерисовки
        {
            this.Invalidate();
        }
    }
}
