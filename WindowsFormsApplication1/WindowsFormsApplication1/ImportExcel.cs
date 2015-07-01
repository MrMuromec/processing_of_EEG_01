using System;
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
        private List<C_measurement> _Tables_excels = new List<C_measurement>(); // Все измерения
        private SortedDictionary<uint, C_spekrt> _spektrs = new SortedDictionary<uint, C_spekrt>(); // Колекция спектров ///////////////////////////////////////////////////////////
        private C_measurement _table_excel ; // Измерение
        private bool _background; // ФОН (да/нет)

        public ImportExcel(List<C_measurement> tables_excels, bool background)
        {
            this._Tables_excels.AddRange(tables_excels);
            this._background = background;
            InitializeComponent();
        }
        public ImportExcel(bool background)
        {
            this._background = background;
            InitializeComponent();
        }

        private void Open_excel_Click(object sender, EventArgs e) // Открытие книги excel
        {
            string str;
            bool matches = false; // Индикатор совпадений (что бы не грузить одно и то же)
            foreach (C_measurement t_e in _Tables_excels)
            {
                t_e.get(out str);
                matches = matches || (str == textBox1.Text);
            }
            if (matches)
                MessageBox.Show(textBox1.Text + " был загружен ранее", "Ошибка загрузки");
            else
                try
                {
                    _table_excel = new C_measurement();
                    _table_excel.Add_(textBox1.Text);
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
                    for (int i = 2; (range = ObjWorkSheet.get_Range("A" + i.ToString())).Text != ""; i++) // Самое тормознутое место программы
                    {
                        textBox2.Text = "строка " + i.ToString();
                        _table_excel.Add_((double.Parse(range.Text)));
                    }
                    _table_excel.Add_(_background);

                    _table_excel._put_key((uint)(2 + _spektrs.Keys.Where(Key => Key != (0 & 1)).Count())); // Сохранения ключа для спектров
                    _spektrs.Add(_table_excel._get_key(), new C_spekrt()); // Добавленире позиции под спектры в словарь для доступа по ключу

                    //Сохраняем результат
                    _Tables_excels.Add(_table_excel);
                    textBox2.Text = "Загрузка завершена";
                    ObjExcel.Quit();

                    textBox3.Text = _Tables_excels.Count.ToString();
                    button2.Visible = true;
                    if (!_background) // Если грузилась реакция
                    {
                        Result form = new Result(_Tables_excels, _background);
                        form.MdiParent = this.MdiParent;
                        form.Show();
                        Close();
                    }
                    else
                    {
                        int i;
                        _spektrs[_Tables_excels[(i = _Tables_excels.Count() - 1)]._get_key()].fft_of_alglib(_Tables_excels[i].get());
                    }
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
        private void Open_table_Click(object sender, EventArgs e) // Выбор файла (книги excel)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }
        private void Display_Click(object sender, EventArgs e) // Отображение промежуточных результатов
        {
            Display1 form = new Display1(_Tables_excels, _background);
            form.MdiParent = this.MdiParent;
            form.Show();
            Close();  
        }
        private void ImportExcel_Load(object sender, EventArgs e) // Загрузка формы
        {
            textBox3.Text = _Tables_excels.Count.ToString();
            if (_Tables_excels.Count != 0) button2.Visible = true;
        }
    } 
}
