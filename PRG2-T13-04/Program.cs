using PRG2_T13_04;
internal class Program
{
    private static void Main(string[] args)
    {
        Dictionary<string, Flight> flightsDict = new Dictionary<string, Flight>();
        LoadFlights(flightsDict);
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
}