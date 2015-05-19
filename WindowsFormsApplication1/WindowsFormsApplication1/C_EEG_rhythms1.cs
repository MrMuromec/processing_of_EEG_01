using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class C_EEG_rhythms1
    {
        public C_rhythm r_Delta = new C_rhythm(0.5, 4);
        public C_rhythm r_Theta = new C_rhythm(4, 8);
        public C_rhythm r_Alpha = new C_rhythm(8, 13);
        public C_rhythm r_Beta1 = new C_rhythm(14, 20);
        public C_rhythm r_Beta2 = new C_rhythm(20, 30);
        public C_rhythm r_Gamma = new C_rhythm(30, 150); // 150 просто что бы < 220
        private double sum_fe = 0; // сумма всего спектра (только ритмы)

        public void Sum(List<double> point, double df)
        {
            r_Alpha.Sum(point, df);
            r_Beta1.Sum(point, df);
            r_Beta2.Sum(point, df);
            r_Delta.Sum(point, df);
            r_Gamma.Sum(point, df);
            r_Theta.Sum(point, df);
            sum_fe = r_Alpha.get_sum() + r_Beta1.get_sum() + r_Beta2.get_sum() + r_Delta.get_sum() + r_Gamma.get_sum() + r_Theta.get_sum();
            r_Alpha.Sum(sum_fe);
            r_Beta1.Sum(sum_fe);
            r_Beta2.Sum(sum_fe);
            r_Delta.Sum(sum_fe);
            r_Gamma.Sum(sum_fe);
            r_Theta.Sum(sum_fe);
        }
    }
}
