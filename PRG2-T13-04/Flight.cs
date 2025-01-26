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
    internal abstract class Flight : IComparable<Flight>
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;

        public string FlightNumber
        {  get { return flightNumber; } set { flightNumber = value; } }

        public string Origin
            { get { return origin; } set { origin = value; } }

        public string Destination
            { get { return destination; } set { destination = value; } }

        public DateTime ExpectedTime
            { get { return expectedTime; } set { expectedTime = value; } }

        public string Status
            { get { return status; } set { status = value; } }

        public abstract double CalculateFees();
        public int CompareTo(Flight other)
        {
            return ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return "Flight Number: " + FlightNumber + "\nOrigin of Flight: " + Origin + "\nDestination of Flight" + Destination + "\nExpected Time of Flight: " + ExpectedTime + "\nStatus: " + Status;

        }

        public Flight()
        { }

        public Flight(string n, string o, string d,DateTime e,string s)
        { FlightNumber = n; Origin = o; Destination = d; ExpectedTime = e; Status = s; }


    }
}
