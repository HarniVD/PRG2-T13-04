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
    internal class LWTTFlight : Flight
    {
        private double requestFee;

        public double RequestFee
        { get { return requestFee; } set { requestFee = value; } }

        public double CalculateFees()
        {
            double fee = 0;
            if (Origin == "Singapore (SIN)")
            { fee += 500 + RequestFee + base.CalculateFees(); }
            else if (Destination == "Singapore (SIN)")
            { fee += 800 + RequestFee + base.CalculateFees(); }
            return fee;
        }
        public override string ToString()
        { return base.ToString() + "\nFees for the FLight: " + CalculateFees(); }

        public LWTTFlight() : base()
        { }

        public LWTTFlight(string n, string o, string d, DateTime e, string s) : base(n, o, d, e, s)
        { RequestFee = 500; }
    } 
        
    }

