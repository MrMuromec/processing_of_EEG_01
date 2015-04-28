﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication1
{
    public partial class ImportExcel : Form
    {
        private List<C_measurement> Tables_excel = new List<C_measurement>(); // Все измерения
        private C_measurement table_excel = new C_measurement(); // Измерение
        private bool background; // ФОН (да/нет)

        public ImportExcel(List<C_measurement> tables_excel, bool background)
        {
            this.Tables_excel.AddRange(tables_excel);
            this.background = background;
            InitializeComponent();
        }
        private void Open_excel_Click(object sender, EventArgs e) // Открытие книги excel
        {
            string str;
            bool matches = false; // Индикатор совпадений (что бы не грузить одно и то же)
            foreach (C_measurement t_e in Tables_excel)
            {
                t_e.get(out str);
                matches = matches || (str == textBox1.Text);
            }
            if (matches)
                MessageBox.Show(textBox1.Text + " был загружен ранее", "Ошибка загрузки");
            else
                try
                {
                    table_excel.Add(textBox1.Text);
                    //Создаём приложение.
                    Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
                    //Открываем книгу.                                                                                                                                                        
                    Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Open(
                        textBox1.Text,
                        0,
                        false,
                        5,
                        "",
                        "",
                        false,
                        Microsoft.Office.Interop.Excel.XlPlatform.xlWindows,
                        "",
                        true,
                        false,
                        0,
                        true,
                        false,
                        false);
                    //Выбираем таблицу(лист).
                    Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;
                    ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];
                    //читаем
                    Microsoft.Office.Interop.Excel.Range range;
                    for (int i = 2; (range = ObjWorkSheet.get_Range("A" + i.ToString())).Text != ""; i++)
                    {
                        textBox2.Text = "строка " + i.ToString();
                        table_excel.Add((double.Parse(range.Text)));
                    }
                    table_excel.Add(background);
                    //Сохраняем результат
                    Tables_excel.Add(table_excel);
                    textBox2.Text = "Загрузка завершена";
                    textBox3.Text = Tables_excel.Count.ToString();
                    button2.Visible = true;
                    ObjExcel.Quit();
                }
                catch (System.Runtime.InteropServices.COMException e1)
                {
                    MessageBox.Show(e1.ToString(), "Ошибка загрузки");
                }
                catch (System.FormatException e2)
                {
                    MessageBox.Show(e2.ToString(), "Ошибка загрузки");
                }
        }
        private void Open_table_Click(object sender, EventArgs e) // Выбор файлв (книги excel)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }
        private void Display_Click(object sender, EventArgs e) // Отображение результатов
        {
            Display form = new Display(Tables_excel, background);
            form.MdiParent = this.MdiParent;
            form.Show(); 
            Close();            
        }
        private void ImportExcel_Load(object sender, EventArgs e) // Загрузка формы
        {
            textBox3.Text = Tables_excel.Count.ToString();
        }
    } 
}
