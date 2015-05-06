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
        private bool background = true; // ФОН (да/нет)
        private string address; // Адрес загруженной книги
        private alglib.complex[] table_f; // спектр
        private List<double> Table_f = new List<double>(); // спектр
        private List<double> Table_fe = new List<double>(); // энергетический спектр 
        private double df = 1000; // Шаг дискритизации (1k Гц)

        private C_rhythm r_Alpha = new C_rhythm(8,13);
        private C_rhythm r_Beta = new C_rhythm(14,40);
        private C_rhythm r_Theta = new C_rhythm(4,8);
        private C_rhythm r_Delta = new C_rhythm(0.5,4); 
        private C_rhythm r_Gamma = new C_rhythm(40,150); // 150 просто что бы < 220

        public void Add(double table_exel_point) // Добавление точки измерения
        {
            this.table_exel.Add(table_exel_point);            
        }
        public void Add(bool background) // Добавление статуса значения
        {            
            this.background = background;
            fft_of_alglib();
        }
        public void Add(string address) // Добавление адреса
        {
            this.address = address;
        }
        public void get (out string address) // Чтение адреса
        {
            address = this.address;
        }
        public void getf(out List<uint> table_f_point) // Чтение спектра
        {
            double[] r = new double[2] { Table_f.Min(), Table_f.Max() - Table_f.Min() };
            table_f_point = new List<uint>();
            for (int i=0; i<Table_f.Count/2 ; i++ )
            {
                table_f_point.Add((uint)((uint.MaxValue - 1) * (Table_f[i] - r[0]) / (r[1])));
            }
        }
        public void getfe(out List<uint> table_fe_point) // Чтение энергетического спектра
        {
            double[] r = new double[2] { Table_fe.Min(), Table_fe.Max() - Table_fe.Min() };
            table_fe_point = new List<uint>();
            for (int i = 0; i < Table_fe.Count / 2; i++)
            {
                table_fe_point.Add((uint)((uint.MaxValue - 1) * (Table_fe[i] - r[0]) / (r[1])));
            }
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
        private void fft_of_alglib() // Спектр + энергетический спектр
        {
            double[] table_array;
            table_exel.CopyTo(table_array = new double[table_exel.Count], 0);
            alglib.fftr1d(table_array, out table_f);
            foreach (alglib.complex point_f in table_f)
            {
                Table_f.Add(alglib.math.abscomplex(point_f));
            }
            for (int i = 0; i < table_f.Count(); i++)
            {
                Table_fe.Add(alglib.math.abscomplex(alglib.math.csqr(table_f[i])) * (df * i));
            }
            r_Alpha.Sum(Table_fe,df);
            r_Beta.Sum(Table_fe, df);
            r_Delta.Sum(Table_fe, df);
            r_Gamma.Sum(Table_fe, df);
            r_Theta.Sum(Table_fe, df);
        }
    }

}
