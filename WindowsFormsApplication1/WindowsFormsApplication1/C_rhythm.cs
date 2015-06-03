using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class C_rhythm 
    {
        private double f_max; // максимальная частота
        private int f_max_i;
        private double f_min; // минимальная частота
        private int f_min_i;
        private double sum = 0; // сумма на диапазоне
        private string name_str; // Название ритма

        public C_rhythm(double f_min, double f_max, string name_str)
        {
            this.f_min = f_min;
            this.f_max = f_max;
            this.name_str = name_str;
        }
        public void Sum (List<double> point, double df) // Абсолютная суммма
        {
            f_max_i = (int)(point.Count() * f_max / df);
            f_min_i = (int)(point.Count() * f_min / df);
            sum = 0;
            for (int i = f_min_i; i < point.Count(); i++)
                if (i < f_max_i)
                    sum = sum + point[i];
                else break;
        }
        public void Sum(double SUM_rhythm) // Нормировка суммы
        {
            sum = sum/SUM_rhythm;
        }
        public double get_sum() // запрос суммы
        {
            return sum;
        }
        public int[] get_f_i () // запрос границ в точках
        {
            int[] f_i= new int[2] {f_min_i,f_max_i};
            return f_i;
        }
        public string get_name() // запрос названия диапазона
        {
            return name_str;
        }
        public double[] get_f() // запрос границ в точках
        {
            double[] f = new double[2] { f_min, f_max };
            return f;
        }
        public void sr_Sum(double[] f_min_max, string name_str, int[] f_i) // Усреднение сумм (копирование описания)
        {
            this.f_min = f_min_max[0];
            this.f_max = f_min_max[1];
            this.name_str = name_str;
            this.f_min_i = f_i[0];
            this.f_max_i = f_i[1];
        }
        public void sr_Sum(double SUM_rhythms) // Усреднение сумм (результат)
        {
            sum = SUM_rhythms;
        }
    }
}
