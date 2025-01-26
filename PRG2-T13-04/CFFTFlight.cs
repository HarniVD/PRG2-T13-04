//==========================================================
// Student Number : S10268170
// Student Name : Chan Jing Hui
// Partner Name : Vijayan Devi Harni
//==========================================================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_04
{
    internal class CFFTFlight:Flight
    {
        private double requestFee;

        public double RequestFee
        { get { return requestFee; } set { requestFee = value; } }

        public override double CalculateFees()
        {
            double fee = 300;
            if (Origin == "Singapore")
            { fee += 500 + RequestFee; }
            else if (Destination == "Singapore")
            { fee += 800 + RequestFee; }
            return fee;
        }
        public override string ToString()
        { return base.ToString() + "\nFees for the FLight: " + CalculateFees(); }

        public CFFTFlight() : base()
        { }

        public CFFTFlight(string n, string o, string d, DateTime e, string s) : base(n, o, d, e, s)
        { RequestFee = 150; }
    }
}
