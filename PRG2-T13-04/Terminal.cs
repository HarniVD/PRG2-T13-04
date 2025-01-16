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
    internal class Terminal
    {
        private string terminalName;
        public string TerminalName { get; set; }
        private Dictionary<string, Airline> airlines;
        public Dictionary<string, Airline> Airlines { get; set; }
        private Dictionary<string, Flight> flights;
        public Dictionary<string, Flight> Flights { get; set; }
        private Dictionary<string, BoardingGate> boardingGates;
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        private Dictionary<string, double> gateFees;
        public Dictionary<string, double> GateFees { get; set; }

        public bool AddAirline(Airline a)
        {
            foreach (KeyValuePair<string, Airline> kvp in Airlines)
            {
                if (kvp.Key == a.Code)
                {
                    return false;
                }
            }
            Airlines.Add(a.Code, a);
            return true;
        }
        public bool AddBoardingGate(BoardingGate bg)
        {
            foreach (KeyValuePair<string, BoardingGate> kvp in BoardingGates)
            {
                if (kvp.Key == bg.GateName)
                {
                    return false;
                }
            }
            BoardingGates.Add(bg.GateName, bg);
            return true;
        }
        public Airline GetAirlineFromFlight(Flight fl)
        {
            foreach (Airline a in Airlines.Values)
            {
                foreach (Flight f in Flights.Values)
                {
                    if (f.FlightNumber == fl.FlightNumber)
                    {
                        return a;
                    }
                }
            }
            return new Airline();
        }
        public void PrintAirlineFees()
        {
            foreach (Airline a in Airlines.Values)
            {
                Console.WriteLine($"Final total fees for {a.Name} is {a.CalculateFees()}");
            }
        }
        public override string ToString()
        {
            string strings = "\nAirlines:\n";
            foreach (KeyValuePair<string, Airline> kvp in Airlines)
            {
                strings += $"{kvp.Key}\n";
            }
            strings += "Flights Numbers:\n";
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                strings += $"{kvp.Key}\n";
            }
            strings += "Boarding Gates:\n";
            foreach (KeyValuePair<string, BoardingGate> kvp in BoardingGates)
            {
                strings += $"{kvp.Key}\n";
            }
            strings += "Gate Fees:\n";
            foreach (KeyValuePair<string, double> kvp in GateFees)
            {
                strings += $"{kvp.Key}: {kvp.Value}\n";
            }
            return $"Terminal Name: {TerminalName}" + strings; ;
        }
        public Terminal()
        {
            TerminalName = "Terminal 5";
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }
    }
}
