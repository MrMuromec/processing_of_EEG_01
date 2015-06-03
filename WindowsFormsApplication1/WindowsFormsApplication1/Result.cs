using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Result : Form
    {
        private List<C_measurement> Tables_excels = new List<C_measurement>(); // Все измерения
        private bool background; // ФОН (да/нет)
        public Result (List<C_measurement> tables_excel, bool background)
        { 
            this.Tables_excels.AddRange(tables_excel);
            this.background = background;
            InitializeComponent();
        }
        private void Result_Paint(object sender, PaintEventArgs e) // Событие перерисовки
        {
            // Отоброжать результат в виде значений + график
        }
    }
}
