using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class C_EEG_rhythms1
    {
        // Диапазоны ритмов не должны перекрываться 
        public List<C_rhythm> rhythms = new List<C_rhythm>() { new C_rhythm(0.5, 4, "Delta"), new C_rhythm(4, 8, "Theta"), new C_rhythm(8, 13, "Alpha"), new C_rhythm(14, 20, "Beta1"), new C_rhythm(20, 30, "Beta2"), new C_rhythm(30, 150, "Gamma") };

        public void Sum(List<double> point, double df) // Получение ритмов
        {
            double sum_fe = 0; // сумма всего спектра (только ритмы)
            for (int i = 0; i < rhythms.Count;i++ )
            {
                rhythms[i].Sum(point, df);
                sum_fe += rhythms[i].get_sum();
            }
            for (int i = 0; i < rhythms.Count; i++)
                rhythms[i].Sum(sum_fe);
        }
        public void sr(List<C_rhythm> rhythms_) // Усреднение
        {
            int N;
            double SUM_rhythms;
            for (int i = 0; i < rhythms.Count; i++) // Цикл для прохода ритмов (ритмы усреднённые)
            {
                N = 0;
                SUM_rhythms = 0;
                for (int j = 0; j < rhythms_.Count; j++) // Цикл для прохода всех ритмов 
                    if (rhythms[i].get_name() == rhythms_[j].get_name())
                    {
                        N++;
                        SUM_rhythms += rhythms_[j].get_sum();
                        rhythms[i].sr_Sum(rhythms_[j].get_f(), rhythms_[j].get_name(), rhythms_[j].get_f_i());
                    }
                rhythms[i].sr_Sum(SUM_rhythms/N);
            }                
        }
        public int max_i () // Максимальный отсчёт из ритмов
        {
            int max = 0;
            for (int i = 0; i < rhythms.Count; i++)
                if (rhythms[i].get_f_i()[1] > max) max = rhythms[i].get_f_i()[1];
            return max;
        }
        /*
        public void norm(List<double> point, double df, double sr_sum) // Получение ритмов для реакции
        {
            for (int i = 0; i < rhythms.Count; i++)
                rhythms[i].Sum(point, df);
            for (int i = 0; i < rhythms.Count; i++)
                rhythms[i].Sum(sr_sum);
        }
         * */
    }
}
