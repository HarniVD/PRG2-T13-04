using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_04
{
    internal class NORMFlight:Flight
    {
        public override double CalculateFees()
        {
            double fee = 300;
            if (Origin == "Singapore")
            { fee += 500; }
            else if (Destination =="Singapore")
            { fee += 800; }
            return fee;
        }

        public NORMFlight() :base()
        { }
        
        public NORMFlight(string n, string o, string d, DateTime e, string s):base(n, o, d, e, s)
        { }

        public override string ToString() 
        {return base.ToString() + "\nFees for the FLight: "+CalculateFee(); }

        

    }

}
