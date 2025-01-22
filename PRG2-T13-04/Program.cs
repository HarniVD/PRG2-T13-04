using PRG2_T13_04;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, BoardingGate> boardingDict = new Dictionary<string, BoardingGate>();
    // Changed the Flights Dictionary to a static variable
    static Dictionary<string, Flight> flightsDict = new Dictionary<string, Flight>();
    private static void Main(string[] args)
    {

        LoadAirline();
        LoadBoardingGate();
        LoadFlights(flightsDict);
        LoadAirlineFlights(flightsDict);


        Terminal terminal5 = new Terminal();
        terminal5.Airlines = airlineDict;
        while (true)
        {
            int option = 0;
            while (true)
            {
                try
                {
                    option = DisplayMenu();
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter an integer value");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Please choose an option between 0 and 7");
                }

            }
            if (option == 0)
            {
                break;
            }
            else if (option == 1)
            {
                ListFlights(flightsDict, airlineDict);
            }
            else if (option == 2)
            {
                ListBoardingGates();
            }
            else if (option == 3)
            {
                AssignBoardingGate(flightsDict);
            }
            else if (option == 5)
            {
                DisplayAirLineFlights();
            }
            else if (option == 6)
            {
                ModifyFlight();
            }
        }
    }
    public static int DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
        Console.WriteLine();
        Console.WriteLine("Please select your option:");
        int option = 0;
        option = Convert.ToInt32(Console.ReadLine());
        if (!(option >= 0 && option <= 7))
        {
            throw new ArgumentOutOfRangeException();
        }
        return option;

    }
    public static void LoadFlights(Dictionary<string, Flight> flightsDict)
    {
        using (StreamReader sr = new StreamReader("flights.csv"))
        {
            Console.WriteLine("Loading Flights...");
            sr.ReadLine();
            string line = sr.ReadLine();
            int count = 0;
            while (line != null)
            {
                count += 1;
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
            Console.WriteLine($"{count} Flights Loaded!");
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
    public static Airline? FindFlightAirline(Dictionary<string, Airline> aDict, Flight fl)
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
        return null;
    }
    public static void LoadAirlineFlights(Dictionary<string, Flight> flightsDict)
    {
        foreach (Flight f in flightsDict.Values)
        {
            string airlineCode = f.FlightNumber[0..2];
            foreach (Airline a in airlineDict.Values)
            {
                if (airlineCode == a.Code)
                {
                    bool result = a.AddFlight(f);
                }
            }
        }
    }
    public static void AssignBoardingGate(Dictionary<string, Flight> flightsDict)
    {
        string? flightNo = "";
        string? boardingName = "";
        try
        {

            Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================");
            Console.WriteLine("Enter Flight Number:");
            flightNo = Console.ReadLine();
            bool airlineCheck = false;
            foreach (string airlineCode in airlineDict.Keys)
            {
                if (flightNo[0..2] == airlineCode)
                {
                    airlineCheck = true;
                }
            }
            if (airlineCheck == false)
            {
                throw new ArgumentOutOfRangeException();
            }
            airlineCheck = false;
            foreach (Flight f in flightsDict.Values)
            {
                if (f.FlightNumber == flightNo)
                {
                    string specialString = f.GetType().Name[0..4];
                    if (specialString == "NORM")
                    {
                        specialString = "None";
                    }
                    airlineCheck = true;
                    Console.WriteLine($"Flight Number: {f.FlightNumber}");
                    Console.WriteLine($"Origin: {f.Origin}");
                    Console.WriteLine($"Destination: {f.Destination}");
                    Console.WriteLine($"Expected Time: {f.ExpectedTime}");
                    Console.WriteLine($"Special Request Code: {specialString}");
                }
            }
            if (airlineCheck == false)
            {
                throw new Exception($"Could not find flight {flightNo}");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Invalid airline code");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        while (true)
        {
            try
            {
                Console.WriteLine("Enter Boarding Gate Name:");
                boardingName = Console.ReadLine();
                bool boardingCheck = false;
                foreach (BoardingGate bg in boardingDict.Values)
                {
                    if (bg.GateName == boardingName)
                    {
                        if (bg.Flight.FlightNumber == null)
                        {
                            boardingCheck = true;
                            Console.WriteLine($"Supports DDJB: {bg.SupportsDDJB}");
                            Console.WriteLine($"Supports CFFT: {bg.SupportsCFFT}");
                            Console.WriteLine($"Supports LWTT: {bg.SupportsLWTT}");
                            bg.Flight = flightsDict[flightNo];
                            break;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                if (boardingCheck == false)
                {
                    throw new ArgumentOutOfRangeException();
                }
                break;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Could not find boarding gate");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Boarding gate already assigned");
                continue;
            }
        }
        while (true)
        {
            Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
            string updateChoice = Console.ReadLine();
            if (updateChoice == "N")
            {
                flightsDict[flightNo].Status = "On Time";
                Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {boardingName}!");
                break;
            }
            else if (updateChoice == "Y")
            {
                Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
                Console.WriteLine("Please select the new status of the flight:");
                int newStatus = 0;
                while (true)
                {
                    try
                    {
                        newStatus = Convert.ToInt32(Console.ReadLine());
                        if (!(newStatus >= 1 && newStatus <= 3))
                        {
                            throw new Exception();
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter an integer");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Please enter an option between 1 and 3");
                    }
                }
                if (newStatus == 1)
                {
                    flightsDict[flightNo].Status = "Delayed";
                }
                else if (newStatus == 2)
                {
                    flightsDict[flightNo].Status = "Boarding";
                }
                else if (newStatus == 3)
                {
                    flightsDict[flightNo].Status = "On Time";
                }
                Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {boardingName}!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid Option");
                continue;
            }
        }
    }

    private static void LoadAirline()
    {
        string[] details = File.ReadAllLines("airlines.csv");
        Console.WriteLine("Loading Airlines...");
        for (int i = 1; i < details.Length; i++)
        {
            string[] info = details[i].Split(',');
            Airline airline = new Airline(info[0], info[1]);
            airlineDict.Add(airline.Code, airline);


        }
        Console.WriteLine("{0} Airlines Loaded!", airlineDict.Count);
    }

    private static void LoadBoardingGate()
    {
        string[] details = File.ReadAllLines("boardinggates.csv ");
        Console.WriteLine("Loading Boarding Gates...");
        for (int i = 1; i < details.Length; i++)
        {
            string[] info = details[i].Split(',');
            BoardingGate boardinggate = new BoardingGate();
            boardinggate.GateName = info[0];
            boardinggate.SupportsCFFT = Convert.ToBoolean(info[2]);
            boardinggate.SupportsDDJB = Convert.ToBoolean(info[1]);
            boardinggate.SupportsLWTT = Convert.ToBoolean(info[3]);
            boardingDict.Add(info[0], boardinggate);
        }
        Console.WriteLine("{0} Boarding Gates Loaded!", boardingDict.Count);
    }

    private static void ListBoardingGates()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-5}", "Gate Name", "DDJB", "CFFT", "LWTT");
        foreach (KeyValuePair<string, BoardingGate> kvp in boardingDict)
        {
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-5}", kvp.Value.GateName, kvp.Value.SupportsDDJB, kvp.Value.SupportsCFFT, kvp.Value.SupportsLWTT);

        }


    }


    private static void DisplayAirLineFlights()
    {
        Console.WriteLine("{0,-16}{1,-17}", "Airline Code", "Airline Name");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            Console.WriteLine("{0,-16}{1,-17}", kvp.Value.Code, kvp.Value.Name);
        }

        Console.Write("Enter Airline Code: ");
        string? code = Console.ReadLine();
        Console.WriteLine("=============================================");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            if (kvp.Value.Code == code)
            {
                Console.WriteLine("List of Flights for {0}", kvp.Value.Name);
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival");
                foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                { Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", f.Value.FlightNumber, kvp.Value.Name, f.Value.Origin, f.Value.Destination, f.Value.ExpectedTime); }
                Console.Write("Enter Flight Number: ");
                string? number = Console.ReadLine();
                foreach (KeyValuePair<string, Flight> flight in kvp.Value.Flights)
                {
                    if (flight.Value.FlightNumber == number)
                    {  }

                }
            }
        }



    }

    private static void ModifyFlight()
    {
        Console.WriteLine("{0,-16}{1,-17}", "Airline Code", "Airline Name");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            Console.WriteLine("{0,-16}{1,-17}", kvp.Value.Code, kvp.Value.Name);
        }

        Console.Write("Enter Airline Code: ");
        string? code = Console.ReadLine();
        Console.WriteLine("=============================================");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            if (kvp.Value.Code == code)
            {
                Console.WriteLine("List of Flights for {0}", kvp.Value.Name);
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival");
                foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                { Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", f.Value.FlightNumber, kvp.Value.Name, f.Value.Origin, f.Value.Destination, f.Value.ExpectedTime); }
                Console.Write("Choose an existing Flight to modify or delete: ");
                string? flight = Console.ReadLine();
                /*string bg = "Unassigned";
                foreach (KeyValuePair<string, BoardingGate> boarding in boardingDict)

                {

                    if (boarding.Value.Flight.FlightNumber == flight)
                    {
                       bg = boarding.Value.GateName;

                    }
                    else
                    { continue; }

                }
                */
                foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                {
                    if (f.Value.FlightNumber == flight)
                    {
                        Console.WriteLine("1.Modify Flight");
                        Console.WriteLine("2.Delete Flight");
                        Console.WriteLine("Choose an option: ");
                        int option = Convert.ToInt32(Console.ReadLine());
                        if (option == 1)
                        {
                            Console.WriteLine("1. Modify Basic Information");
                            Console.WriteLine("2. Modify Status");
                            Console.WriteLine("3. Modify Special Request Code");
                            Console.WriteLine("4. Modify Boarding Gate");
                            Console.WriteLine("Choose an option:");
                            int opt = Convert.ToInt32(Console.ReadLine());
                            if (opt == 1)
                            {
                                Console.WriteLine("Enter new Origin: ");
                                string? origin = Console.ReadLine();
                                Console.WriteLine("Enter new Destination: ");
                                string? destination = Console.ReadLine();
                                Console.WriteLine("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                                DateTime dt = Convert.ToDateTime(Console.ReadLine());
                                f.Value.Origin = origin;
                                f.Value.Destination = destination;
                                f.Value.ExpectedTime = dt;

                            }
                            if (opt == 2)
                            {
                                Console.WriteLine("Enter status of the Flight: ");
                                string? status = Console.ReadLine();
                                f.Value.Status = status;

                            }

                            if (opt == 3)
                            {
                                Console.WriteLine("Enter Special Request Code: ");
                                string s = Console.ReadLine();
                                


                            }

                            if (opt == 4)
                            {


                            }

                            Console.WriteLine("Flight Updated!");
                            Console.WriteLine("Flight Number: {0}", f.Value.FlightNumber);
                            Console.WriteLine("Airline Name: {0}", kvp.Value.Name);
                            Console.WriteLine("Origin: {0}", f.Value.Origin);
                            Console.WriteLine("Destination: {0}", f.Value.Destination);
                            Console.WriteLine("Expected Departure/Arrival Time: {0}", f.Value.ExpectedTime);
                            Console.WriteLine("Status: {0}", f.Value.Status);
                            string specialString = f.Value.GetType().Name[0..4];
                            if (specialString == "NORM")
                            {
                                specialString = "None";
                            }
                            Console.WriteLine("Special Request Code: {0}", specialString);
                            //Console.WriteLine("Boarding Gate: {0}", bg);
                            


                        }

                        else if (option == 2 )
                        { Console.WriteLine("Are you sure you want to delete [Y/N]:");
                          string confirm = Console.ReadLine();
                            if (confirm == "Y")
                            {
                                flightsDict.Remove(flight);
                               

                            }
                            else
                            { break; }

                            ListFlights(flightsDict, airlineDict);


                        }




                    }


                }
            }
        }
    }
}
