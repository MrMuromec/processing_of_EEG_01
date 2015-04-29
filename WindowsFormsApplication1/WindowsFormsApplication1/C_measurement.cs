﻿using System;
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
        public void get(out List<uint> table_point) // Чтение сигнала
        {
            double[] r = new double[2] { table_exel.Min(), table_exel.Max() - table_exel.Min() };
            table_point = new List<uint>();
            foreach (double one_point in table_exel)
            {
                table_point.Add((uint)((uint.MaxValue - 1) * (one_point - r[0]) / (r[1])));
            }
        }

        private void fft_of_alglib() // Спектр
        {
            double[] table_array;
            table_exel.CopyTo(table_array = new double[table_exel.Count], 0);
            alglib.fftr1d(table_array, out table_f);
            foreach (alglib.complex point_f in table_f)
            {
                Table_f.Add(alglib.math.abscomplex(alglib.math.csqr(point_f)));
            }
        }
    }

}
