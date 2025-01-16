using PRG2_T13_04;

internal class Program
{   static List<Airline> airlineList = new List<Airline>();
    static Dictionary<string,BoardingGate> boardingDict = new Dictionary<string,BoardingGate>();
    private static void Main(string[] args)
    {
        
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
        Console.WriteLine("{0} Airlines Loaded!",airlineList.Count);
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