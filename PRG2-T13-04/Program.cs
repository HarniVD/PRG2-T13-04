using PRG2_T13_04;
using static System.Runtime.InteropServices.JavaScript.JSType;
internal class Program
{
    static List<Airline> airlineList = new List<Airline>();
    static Dictionary<string, BoardingGate> boardingDict = new Dictionary<string, BoardingGate>();
    private static void Main(string[] args)
    {
        Dictionary<string, Flight> flightsDict = new Dictionary<string, Flight>();
        LoadFlights(flightsDict);
        LoadAirline();
        ListFlights(flightsDict, aDict);
    }
    public static void LoadFlights(Dictionary<string, Flight> flightsDict)
    {
        using (StreamReader sr = new StreamReader("flights.csv"))
        {
            sr.ReadLine();
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] lineList = line.Split(',');
                Flight newFlight = null;
                if (lineList[lineList.Length - 1] == "DDJB")
                {
                    newFlight = new DDJBFlight(lineList[0], lineList[1], lineList[2], Convert.ToDateTime(lineList[3]), "Scheduled");
                }
                else if (lineList[lineList.Length - 1] == "CFFT")
                {
                    newFlight = new CFFTFlight(lineList[0], lineList[1], lineList[2], Convert.ToDateTime(lineList[3]), "Scheduled");
                }
                else if (lineList[lineList.Length - 1] == "LWTT")
                {
                    newFlight = new LWTTFlight(lineList[0], lineList[1], lineList[2], Convert.ToDateTime(lineList[3]), "Scheduled");
                }
                else
                {
                    newFlight = new NORMFlight(lineList[0], lineList[1], lineList[2], Convert.ToDateTime(lineList[3]), "Scheduled");
                }
                flightsDict.Add(newFlight.FlightNumber, newFlight);
                line = sr.ReadLine();
            }
        }
    }
    public static void ListFlights(Dictionary<string, Flight> flightsDict, Dictionary<string, Airline> aDict)
    {
        Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival");
        foreach (KeyValuePair<string, Flight> kvp in flightsDict)
        {
            string airlineName = FindFlightAirline(aDict, kvp.Value).Name;
            Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", kvp.Value.FlightNumber, airlineName, kvp.Value.Origin, kvp.Value.Destination, kvp.Value.ExpectedTime);
        }

    }
    public static Airline FindFlightAirline(Dictionary<string, Airline> aDict, Flight fl)
    {
        foreach (Airline a in aDict.Values)
        {
            foreach (Flight f in a.Flights.Values)
            {
                if (f.FlightNumber == fl.FlightNumber)
                {
                    return a;
                }
            }
        }
        Console.WriteLine("No matching airline found");
        return new Airline();
    }


    private static void LoadAirline()
    {
        string[] details = File.ReadAllLines("airlines.csv");
        Console.WriteLine("Loading Airlines...");
        for (int i = 1; i < details.Length; i++)
        {
            string[] info = details[i].Split(',');
            Airline airline = new Airline(info[0], info[1]);
            airlineList.Add(airline);


        }
        Console.WriteLine("{0} Airlines Loaded!", airlineList.Count);
    }

    private static void LoadBoardingGate()
    {
        string[] details = File.ReadAllLines("boardinggates.csv ");
        Console.WriteLine("Loading Boarding Gates...");
        for (int i = 1; i < details.Length; i++)
        {
            string[] info = details[i].Split(',');
        }
    }
}