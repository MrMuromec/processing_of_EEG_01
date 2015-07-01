using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class C_spekrt
    {
        public List<double> Table_f = new List<double>(); // амплитудный спектр
        public List<double> Table_fe = new List<double>(); //  спектр мощности

        private double df = 1000; // Шаг дискритизации (1k Гц)

        public C_EEG_rhythms1 rhythms_f = new C_EEG_rhythms1(); // Ритмы аплитудгного
        public C_EEG_rhythms1 rhythms_fe = new C_EEG_rhythms1(); // Ритмы мощностного

        public void fft_of_alglib(List<double> table_exel) // Спектр (амплитудный+мощностной)
        {
            // перегон из списка в масссив
            double[] table_array;
            table_exel.CopyTo(table_array = new double[table_exel.Count], 0);
            // комплексный спектр
            alglib.complex[] table_f; 
            alglib.fftr1d(table_array, out table_f);
            // расчёт спектров
            _f(table_f);
            _fe(table_f);
        }
        private void _f(alglib.complex[] table_f) // Спектр амплитудный
        {
            foreach (alglib.complex point_f in table_f)
                Table_f.Add(alglib.math.abscomplex(point_f));
            rhythms_f.Sum(Table_f, df); // Ритмы
        }
        private void _fe(alglib.complex[] table_f) // Спектр мощностной
        {
            for (int i = 0; i < table_f.Count(); i++)
                Table_fe.Add(alglib.math.abscomplex(alglib.math.csqr(table_f[i])) * (double)(i * df / (2 * table_f.Count())));
            rhythms_fe.Sum(Table_fe, df); // Ритмы
        }
    }
}
