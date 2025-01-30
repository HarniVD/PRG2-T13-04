using System;
//==========================================================
// Student Number : S10268170
// Student Name : Chan Jing Hui
// Partner Name : Vijayan Devi Harni
//==========================================================


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_04
{
    internal class Airline
    {
        private string name;
        private string code;
        private Dictionary<string, Flight> flights;

        public string Name
        { get { return name; } set { name = value; } }

        public string Code
        { get { return code; } set { code = value; } }

        public Dictionary<string, Flight> Flights
        { get { return flights; } set { flights = value; } }

        public bool AddFlight(Flight f)
        {
            // changed flights to Flights
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                if (kvp.Key == f.FlightNumber)
                {
                    return false;
                }
            }
            Flights.Add(f.FlightNumber, f);
            return true;

        }

        public bool RemoveFlight(Flight f)
        {
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                if (kvp.Key == f.FlightNumber)
                {
                    Flights.Remove(f.FlightNumber);
                    return true;
                }
            }
            return false;

        }

        public double CalculateFees()
        {
            double fee = 0;
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                string specialString = kvp.Value.GetType().Name[0..4];
                if (specialString == "CFFT")
                {

                    CFFTFlight cfft = (CFFTFlight) kvp.Value;
                    cfft.RequestFee = 150;
                    fee += cfft.CalculateFees();
                }
                else if (specialString == "DDJB")
                {
                    DDJBFlight ddjb = (DDJBFlight) kvp.Value;
                    ddjb.RequestFee = 300;
                    fee += ddjb.CalculateFees();
                }
                else if (specialString == "LWTT")
                {
                    LWTTFlight lwtt = (LWTTFlight) kvp.Value;
                    lwtt.RequestFee = 500;
                    fee += lwtt.CalculateFees();
                }
                else
                {
                    NORMFlight norm = (NORMFlight) kvp.Value;
                    fee += norm.CalculateFees();
                }


            }
            bool found = false;
            bool found1 = false;
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                if (kvp.Value.ExpectedTime.Hour < 11 || kvp.Value.ExpectedTime.Hour > 21)
                { found = true; }
                if (kvp.Value.Origin == "Dubai (DXB)" || kvp.Value.Origin == "Bangkok (BKK)" || kvp.Value.Origin == "Tokyo (NRT)")
                { found1 = true; }
            }
            if (found == true)
            { fee = fee - 110; }
            if (found1 == true)
            { fee = fee - 25; }
            double d = Flights.Count / 3;
            if (d>= 1)
            { fee = fee - (Math.Floor(d) * 350); }
            if (Flights.Count>5)
            { fee = 0.97 * fee; }
            

                return fee;
        }

        public override string ToString()
        {
            string output = "Name of AirLines: " + Name + "\nAirlines Code: " + Code + "\nFlights under Airline: ";
            foreach (KeyValuePair<string, Flight> kvp in flights)
            { output += "\n" + kvp.Value.FlightNumber; }
            return output;
        }

        public Airline()
        { }

        public Airline(string n, string c)
        {
            Name = n;
            Code = c;
            Flights = new Dictionary<string, Flight>();



        }
    }
}