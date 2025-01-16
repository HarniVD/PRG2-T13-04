using System;
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
            foreach (KeyValuePair<string, Flight> kvp in flights)
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
            foreach (KeyValuePair<string, Flight> kvp in flights)
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
            foreach (KeyValuePair<string, Flight> kvp in flights)
            {
                fee += kvp.Value.CalculateFees();

            }
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
            Flights = new Dictionary<string, Flight>();



        }
    }
}