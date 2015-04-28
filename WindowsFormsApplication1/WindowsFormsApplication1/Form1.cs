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
    public partial class Form1 : Form
    {
        List<C_measurement> Tables_excel = new List<C_measurement>(); // Все измерения
    
        public Form1()
        {
            InitializeComponent();
        }

        private void Import_Click(object sender, EventArgs e)
        {
            ImportExcel form = new ImportExcel(Tables_excel, true);
            form.MdiParent = this;
            form.Show();
        }
    }
}
