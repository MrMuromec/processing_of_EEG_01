using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class C_rhythm 
    {
        private double f_max; // максимальная частота
        private double f_min; // минимальная частота
        private double sum = 0; // сумма на диапазоне 


        public C_rhythm(double f_min, double f_max)
        {
            this.f_min = f_min;
            this.f_max = f_max;
        }
        public void Sum (List<double> point, double df)
        {
            for (int i = (int)(f_min/df); i < point.Count() ; i++ )
                if (i < (f_max / df)) sum = sum + point[i];
        }
    }
}
