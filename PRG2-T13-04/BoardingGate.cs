using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_04
{
    internal class BoardingGate
    {
        private string gateName;
        public string GateName { get; set; }
        private bool supportsCFFT;
        public bool SupportsCFFT { get; set; }

        private bool supportsDDJB;
        public bool SupportsDDJB { get; set; }
        
        private bool supportsLWTT;
        public bool SupportsLWTT { get; set; }
        
        private Flight flight;
        public Flight Flight { get; set; }

        public double CalculateFees()
        {
            return 300;
        }
        public override string ToString()
        {
            return $"GateName: {GateName}, SupportsCFFT: {SupportsCFFT}, SupportsDDJB: {SupportsDDJB}, SupportsLWTT: {SupportsLWTT}, Flight: {Flight.ToString()}";
        }
        public BoardingGate()
        {
            GateName = "";
            SupportsCFFT = false;
            SupportsDDJB = false;
            SupportsLWTT = false;
            Flight = null;
        }
        public BoardingGate(string n, bool CFFT, bool DDJB, bool LWTT) 
        {
            GateName = n;
            SupportsCFFT = CFFT;
            SupportsDDJB = DDJB;
            SupportsLWTT = LWTT;
            Flight = null;
        }

    }
}
