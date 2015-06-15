using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class C_measurement
    {
        private List<double> table_exel = new List<double>(); // данные
        private int min_i, max_i; 
        private bool background = true; // ФОН (да/нет) 
        private string address = ""; // Адрес загруженной книги
        public C_spekrt spek = new C_spekrt(); // Спектры + ритмы
        
        public void Add_(double table_exel_point) // Добавление точки измерения
        {
            this.table_exel.Add(table_exel_point);            
        }
        public void Add_(bool background) // Добавление статуса значения
        {            
            this.background = background;
            min_i = 0;
            max_i = table_exel.Count();
            if (get_bool() && sr_bool()) spek.fft_of_alglib(table_exel); 
        }
        public void Add_(string address) // Добавление адреса
        {
            this.address = address;
        }
        public void get (out string address) // Чтение адреса
        {
            address = this.address;
        }
        public int[] get_i() // запрос границ в точках
        {
            int[] f_i = new int[2] { min_i, max_i };
            return f_i;
        }
        public void get(out List<uint> table_point) // Чтение сигнала
        {
            double[] r = new double[2] { table_exel.Min(), table_exel.Max() - table_exel.Min() };
            table_point = new List<uint>();
            foreach (double one_point in table_exel)
            {
                table_point.Add((uint)((uint.MaxValue - 1) * (one_point - r[0]) / (r[1])));
            }
        }
        public void getf(out List<uint> table_f_point) // Чтение спектра
        {
            double[] r = new double[2] { spek.Table_f.Min(), spek.Table_f.Max() - spek.Table_f.Min() };
            table_f_point = new List<uint>();

            for (int i = 0; i < spek.rhythms_f.max_i(); i++)
            {
                table_f_point.Add((uint)((uint.MaxValue - 1) * (spek.Table_f[i] - r[0]) / (r[1])));
            }
        }
        public void getfe(out List<uint> table_fe_point) // Чтение энергетического спектра
        {
            double[] r = new double[2] { spek.Table_fe.Min(), spek.Table_fe.Max() - spek.Table_fe.Min() };
            table_fe_point = new List<uint>();

            for (int i = 0; i < spek.rhythms_fe.max_i(); i++)
            {
                table_fe_point.Add((uint)((uint.MaxValue - 1) * (spek.Table_fe[i] - r[0]) / (r[1])));
            }
        }
        public bool get_bool() // Кто (фод = да)
        {
            return background;
        }
        public bool sr_bool() // Сравнение на наличие адреса
        {
            return address != "";
        }
        public void r_f(C_EEG_rhythms1 rhythms_f)
        {
            spek.rhythms_f = rhythms_f;
        }
        public void r_fe(C_EEG_rhythms1 rhythms_fe)
        {
            spek.rhythms_fe = rhythms_fe;
        }
    }

}
