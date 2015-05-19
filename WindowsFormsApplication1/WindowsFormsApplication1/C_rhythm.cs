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


        public C_rhythm(double f_min, double f_max)
        {
            this.f_min = f_min;
            this.f_max = f_max;
        }
        public void Sum (List<double> point, double df)
        {
            f_max_i = (int)(point.Count() * f_max / df);
            f_min_i = (int)(point.Count() * f_min / df);
            sum = 0;
            for (int i = f_min_i; i < point.Count(); i++)
                if (i < f_max_i)
                    sum = sum + point[i];
                else break;
        }
        public void Sum(double SUM_rhythm)
        {
            sum = sum/SUM_rhythm;
        }
        public double get_sum()
        {
            return (double)sum;
        }
        public int[] get_f_i ()
        {
            int[] f_i= new int[2] {f_min_i,f_max_i};
            return f_i;
        }
    }
}
