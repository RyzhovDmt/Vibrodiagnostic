using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Vibrodiagnostic
{
    public class Status
    {
        public bool trend;
        public bool time_Less_24;
        public bool regr_out_of_range;
        public bool antiphase_vect;

        public bool not_continue;

        public bool levels_1_comp;
        public bool levels_2_comp;

        public bool overswing;
        public string answer;

        public Color RGcolor;
        public Color RVDcolor;
        public Color RSDcolor;
        public Color RNDcolor;

        public Status()
        {
            trend = false;
            time_Less_24 = false;
            regr_out_of_range = false;
            antiphase_vect = false;

            not_continue = true;

            levels_1_comp = false;
            levels_2_comp = false;

            overswing = false;
            answer = "";

            RGcolor = Color.Green;
            RVDcolor = Color.Green;
            RSDcolor = Color.Green;
            RNDcolor = Color.Green;
        }

        public string calc_prob(bool a1, bool a2, bool a3, bool a4)
        {
            string res = "";
            return res;
        }
    }
}
