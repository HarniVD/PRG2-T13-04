//==========================================================
// Student Number : S10268170
// Student Name : Chan Jing Hui
// Partner Name : Vijayan Devi Harni
//==========================================================

using PRG2_T13_04;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;
internal class Program
{
    static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    static Dictionary<string, BoardingGate> boardingDict = new Dictionary<string, BoardingGate>();
    static Dictionary<string, Flight> flightsDict = new Dictionary<string, Flight>();
    static Queue<Flight> queue = new Queue<Flight>();
    static List<BoardingGate> listBG = new List<BoardingGate>();
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
                    Console.WriteLine("Please choose an option between 0 and 9");
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
            else if (option == 4)
            {
                while (true)
                {
                    string? opt = AddFlight(flightsDict);
                    if (opt == "Y")
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            else if (option == 5)
            {
                DisplayAirLineFlights();
            }
            else if (option == 6)
            {
                ModifyFlight();
            }
            else if (option == 7)
            {
                List<Flight> sortingList = new List<Flight>();
                foreach (KeyValuePair<string, Flight> kvp in flightsDict)
                {
                    sortingList.Add(kvp.Value);
                }
                sortingList.Sort();
                Dictionary<string, Flight> sortedDict = new Dictionary<string, Flight>();
                foreach (Flight f in sortingList)
                {
                    sortedDict.Add(f.FlightNumber, f);
                }
                Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-22}{4,-30}{5,-14}{6,-22}{7,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival", "Status", "Special Request Code", "Boarding Gate");
                foreach (KeyValuePair<string, Flight> kvp in sortedDict)
                {
                    string airlineName = FindFlightAirline(airlineDict, kvp.Value).Name;
                    string code = kvp.Value.GetType().Name[0..4];
                    if (code == "NORM")
                    {
                        code = "";
                    }
                    string gateName = "";
                    foreach (BoardingGate bg in boardingDict.Values)
                    {
                        if (bg.Flight != null)
                        {
                            if (bg.Flight.FlightNumber == kvp.Key)
                            {
                                gateName = bg.GateName;
                            }
                        }
                    }
                    Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-22}{4,-30}{5,-14}{6,-22}{7,-22}", kvp.Value.FlightNumber, airlineName, kvp.Value.Origin, kvp.Value.Destination, kvp.Value.ExpectedTime, kvp.Value.Status, code, gateName);

                }
            }

            else if (option == 8)
            {
                foreach (BoardingGate bg in boardingDict.Values)
                {
                    if (bg.Flight == null && !(listBG.Contains(bg)))
                    {
                        listBG.Add(bg);
                    }
                }
                foreach (Flight flight in flightsDict.Values)
                {
                    bool assigned = false;
                    foreach (BoardingGate bg in boardingDict.Values)
                    {
                        if (bg.Flight != null)
                        {
                            if (bg.Flight.FlightNumber == flight.FlightNumber)
                            {
                                assigned = true;
                            }
                        }
                    }
                    if (assigned == false && !(queue.Contains(flight)))
                    {
                        queue.Enqueue(flight);
                    }
                }
                ProcessFlights(queue, listBG);
            }
            else if (option == 9)
            {
                CalculateAirlineFees();
            }
        }
    }
    //Displaying of Menu
    public static int DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n8. Process unassigned flights to boarding gates\r\n9. Display total fee per airline for the day\r\n0. Exit");
        Console.WriteLine();
        Console.WriteLine("Please select your option:");
        int option = 0;
        option = Convert.ToInt32(Console.ReadLine());
        if (!(option >= 0 && option <= 9))
        {
            throw new ArgumentOutOfRangeException();
        }
        return option;

    }

    // Loading of Flights
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

    //Feature 1 done
    public static void ListFlights(Dictionary<string, Flight> flightsDict, Dictionary<string, Airline> aDict)
    {
        Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival");
        foreach (KeyValuePair<string, Flight> kvp in flightsDict)
        {
            string airlineName = FindFlightAirline(aDict, kvp.Value).Name;
            Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", kvp.Value.FlightNumber, airlineName, kvp.Value.Origin, kvp.Value.Destination, kvp.Value.ExpectedTime);

        }

    }
    // For Finding of Airline
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

    // Loading of Airline Flights
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

    // Feature 3 with Data Validation done
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
                    foreach (BoardingGate bg in boardingDict.Values)
                    {
                        if (bg.Flight != null)
                        {
                            if (bg.Flight.FlightNumber == flightNo)
                            {
                                Console.WriteLine("This flight is already assigned");
                                return;
                            }
                        }
                    }
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
                        if (bg.Flight == null)
                        {
                            boardingCheck = true;
                            Console.WriteLine($"Supports DDJB: {bg.SupportsDDJB}");
                            Console.WriteLine($"Supports CFFT: {bg.SupportsCFFT}");
                            Console.WriteLine($"Supports LWTT: {bg.SupportsLWTT}");
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
                string code = flightsDict[flightNo].GetType().Name[0..4];
                bool supports = false;
                if (code == "NORM")
                {
                    supports = true;
                }
                else if (code == "DDJB")
                {
                    if (boardingDict[boardingName].SupportsDDJB == true)
                    {
                        supports = true;
                    }
                }
                else if (code == "CFFT")
                {
                    if (boardingDict[boardingName].SupportsCFFT == true)
                    {
                        supports = true;
                    }
                }
                else
                {
                    if (boardingDict[boardingName].SupportsLWTT == true)
                    {
                        supports = true;
                    }
                }
                if (supports == false)
                {
                    Console.WriteLine($"Boarding gate does not support special request code {code}");
                    return;
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
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string updateChoice = Console.ReadLine();
        if (updateChoice == "N")
        {
            flightsDict[flightNo].Status = "On Time";
            boardingDict[boardingName].Flight = flightsDict[flightNo];
            Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {boardingName}!");
        }
        else if (updateChoice == "Y")
        {
            Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
            Console.WriteLine("Please select the new status of the flight:");
            int newStatus = 0;
            try
            {
                newStatus = Convert.ToInt32(Console.ReadLine());
                if (!(newStatus >= 1 && newStatus <= 3))
                {
                    throw new Exception();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter an integer");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter an option between 1 and 3");
                return;
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
            boardingDict[boardingName].Flight = flightsDict[flightNo];
            Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {boardingName}!");
        }
        else
        {
            Console.WriteLine("Invalid Option");
        }
    }

    //Loading of Airline
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

    //Loading of Boarding Gates
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

    //Feature 2 
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

    // Feature 5 with Data Validation done 
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

        bool airline = false;
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            if (kvp.Value.Code == code)
            {
                airline = true;
                bool run = true;
                while (run == true)
                {
                    Console.WriteLine("List of Flights for {0}", kvp.Value.Name);
                    Console.WriteLine("=============================================");
                    Console.WriteLine("{0,-22}{1,-22}{2,-22}", "Flight Number", "Origin", "Destination");
                    foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                    { Console.WriteLine("{0,-22}{1,-22}{2,-22}", f.Value.FlightNumber, f.Value.Origin, f.Value.Destination); }
                    Console.Write("Enter Flight Number: ");
                    string? number = Console.ReadLine();
                    bool found = false;
                    foreach (KeyValuePair<string, Flight> flight in kvp.Value.Flights)
                    {
                        if (flight.Value.FlightNumber == number)
                        {
                            Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}{5,-11}{6,-23}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival ", "Status", "Boarding Gate");
                            string bg = "Unassigned";
                            foreach (KeyValuePair<string, BoardingGate> boarding in boardingDict)

                            {
                                if (boarding.Value.Flight != null)
                                {
                                    if (boarding.Value.Flight.FlightNumber == number)
                                    {
                                        bg = boarding.Value.GateName;

                                    }
                                    else
                                    { continue; }


                                }

                            }
                            Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-26}{5,-11}{6,-23}", number, kvp.Value.Name, flight.Value.Origin, flight.Value.Destination, flight.Value.ExpectedTime, flight.Value.Status, bg);
                            found = true;
                            run = false;
                            break;

                        }
                    }

                    if (found == false)
                    {
                        Console.WriteLine("Flight number cannot be found");
                        continue;
                    }
                }

            }
        }
        if (airline == false)
        {
            Console.WriteLine("Airline is not found");
        }




    }




    //Feature 6 with Data Validation done
    private static void ModifyFlight()
    {
        Console.WriteLine("{0,-16}{1,-17}", "Airline Code", "Airline Name");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            Console.WriteLine("{0,-16}{1,-17}", kvp.Value.Code, kvp.Value.Name);
        }

        Console.Write("Enter Airline Code: ");
        string? code = Console.ReadLine();
        bool find = false;
        Console.WriteLine("=============================================");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            if (kvp.Value.Code == code)
            {
                find = true;
                bool flight_found = false;
                Console.WriteLine("List of Flights for {0}", kvp.Value.Name);
                Console.WriteLine("=============================================");
                Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival");
                foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                { Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}", f.Value.FlightNumber, kvp.Value.Name, f.Value.Origin, f.Value.Destination, f.Value.ExpectedTime); }
                Console.Write("Choose an existing Flight to modify or delete: ");
                string? flight = Console.ReadLine();
                string bg = "Unassigned";
                foreach (KeyValuePair<string, BoardingGate> boarding in boardingDict)

                {
                    if (boarding.Value.Flight != null)
                    {
                        if (boarding.Value.Flight.FlightNumber == flight)
                        {
                            bg = boarding.Value.GateName;

                        }
                        else
                        { continue; }


                    }
                }

                foreach (KeyValuePair<string, Flight> f in kvp.Value.Flights)
                {
                    if (f.Value.FlightNumber == flight)
                    {
                        flight_found = true;
                        Console.WriteLine("1.Modify Flight");
                        Console.WriteLine("2.Delete Flight");
                        Console.WriteLine("Choose an option: ");
                        int option = Convert.ToInt32(Console.ReadLine());
                        //If the option is Modify Flight:
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
                                string? s = Console.ReadLine();
                                if (s != "DDJB" || s != "CFFT" || s != "LWTT")
                                {
                                    Console.WriteLine("Invalid Special Request Code.");
                                    break;
                                }
                                else
                                {
                                    string? origin = f.Value.Origin;
                                    string? destination = f.Value.Destination;
                                    DateTime datetime = f.Value.ExpectedTime;
                                    string? status = f.Value.Status;
                                    flightsDict.Remove(flight);
                                    if (s == "DDJB")
                                    {
                                        Flight c = new DDJBFlight(flight, origin, destination, datetime, status);
                                        flightsDict.Add(flight, c);
                                    }
                                    else if (s == "NORM")
                                    {
                                        Flight c = new NORMFlight(flight, origin, destination, datetime, status);
                                        flightsDict.Add(flight, c);
                                    }
                                    else if (s == "CFFT")
                                    {
                                        Flight c = new CFFTFlight(flight, origin, destination, datetime, status);
                                        flightsDict.Add(flight, c);
                                    }
                                    else if (s == "LWTT")
                                    {
                                        Flight c = new LWTTFlight(flight, origin, destination, datetime, status);
                                        flightsDict.Add(flight, c);
                                    }
                                }



                            }


                            if (opt == 4)
                            {
                                Console.WriteLine("Enter Boarding Gate Number: ");
                                string? boarding_gate = Console.ReadLine();
                                bool found = false;
                                foreach (KeyValuePair<string, BoardingGate> b in boardingDict)
                                {
                                    if (b.Key == boarding_gate)
                                    {
                                        found = true;
                                        bg = boarding_gate;
                                        foreach (KeyValuePair<string, Flight> assign in flightsDict)
                                        {
                                            if (assign.Value.FlightNumber == flight)
                                            { b.Value.Flight = assign.Value; }
                                        }
                                    }
                                    else { continue; }
                                }
                                if (found == false)
                                {
                                    Console.WriteLine("Boarding Gate Number not found");
                                    break;
                                }

                            }

                            else
                            {
                                Console.WriteLine("Option not found.");
                                break;
                            }
                            // Displaying of Information about Flights
                            Console.WriteLine("Flight Updated!");
                            Console.WriteLine("Flight Number: {0}", f.Value.FlightNumber);
                            Console.WriteLine("Airline Name: {0}", kvp.Value.Name);
                            Console.WriteLine("Origin: {0}", f.Value.Origin);
                            Console.WriteLine("Destination: {0}", f.Value.Destination);
                            Console.WriteLine("Expected Departure/Arrival Time: {0}", f.Value.ExpectedTime);
                            Console.WriteLine("Status: {0}", f.Value.Status);
                            foreach (KeyValuePair<string, Flight> flights in flightsDict)
                            {
                                if (flights.Key == flight)
                                {
                                    string specialString = flights.Value.GetType().Name[0..4];
                                    {
                                        specialString = "None";
                                    }
                                    Console.WriteLine("Special Request Code: {0}", specialString);
                                }

                            }
                            Console.WriteLine("Boarding Gate: {0}", bg);



                        }
                        //If the option is Delete Flight
                        else if (option == 2)
                        {
                            Console.WriteLine("Are you sure you want to delete [Y/N]:");
                            string confirm = Console.ReadLine();
                            if (confirm == "Y")
                            {
                                flightsDict.Remove(flight);
                            }
                            else
                            { break; }
                        }

                        else
                        {
                            Console.WriteLine("Option not found;");
                            break;
                        }

                        // Listing of all Flights
                        Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-22}{5,-11}{6,-20}{7,-23}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure / Arrival ", "Status", "Special Request Code", "Boarding Gate");
                        foreach (KeyValuePair<string, Flight> f1 in flightsDict)

                        {
                            string gate = "Unassigned";
                            foreach (KeyValuePair<string, BoardingGate> b1 in boardingDict)
                            {
                                if (b1.Value.Flight != null)
                                {
                                    if (b1.Value.Flight.FlightNumber == f1.Value.FlightNumber)
                                    { gate = b1.Value.GateName; }
                                }
                            }
                            Console.WriteLine("{0,-22}{1,-22}{2,-22}{3,-22}{4,-26}{5,-11}{6,-20}{7,-23}", f1.Value.FlightNumber, kvp.Value.Name, f1.Value.Origin, f1.Value.Destination, f1.Value.ExpectedTime, f1.Value.Status, f1.Value.GetType().Name[0..4], gate);
                        }

                    }



                }
                if (flight_found == false)
                { Console.WriteLine("Flight not found"); }


            }
        }
        if (find == false)
        {
            Console.WriteLine("Airline not found");
        }
    }







    public static string AddFlight(Dictionary<string, Flight> flightsDict)
    {
        Console.Write("Enter Flight Number: ");
        string? flightNo = null;
        DateTime? expectedTime = null;
        try
        {
            flightNo = Console.ReadLine();
            if (flightsDict.ContainsKey(flightNo))
            {
                Console.WriteLine("Flight with same Flight Number already exists.");
                return "";
            }
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
            if (flightNo.Length > 6)
            {
                throw new Exception();
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Invalid airline code");
            return "";
        }
        catch (Exception)
        {
            Console.WriteLine("Flight Number must be <1000");
            return "";
        }

        Console.Write("Enter Origin: ");
        string? origin = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string? destination = Console.ReadLine();
        Console.Write("Enter Expected Departure / Arrival Time(dd / mm / yyyy hh: mm): ");
        try
        {
            expectedTime = Convert.ToDateTime(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Incorrect Format. Please enter the time in the format above.");
            return "";
        }

        Console.Write("Enter Special Request Code(CFFT/ DDJB / LWTT / None): ");
        try
        {
            string? code = Console.ReadLine();
            Flight newFlight = null;
            if (code == "None")
            {
                newFlight = new NORMFlight(flightNo, origin, destination, expectedTime.Value, "Scheduled");
            }
            else if (code == "CCFT")
            {
                newFlight = new CFFTFlight(flightNo, origin, destination, expectedTime.Value, "Scheduled");
            }
            else if (code == "DDJB")
            {
                newFlight = new DDJBFlight(flightNo, origin, destination, expectedTime.Value, "Scheduled");
            }
            else if (code == "LWTT")
            {
                newFlight = new LWTTFlight(flightNo, origin, destination, expectedTime.Value, "Scheduled");
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            flightsDict.Add(flightNo, newFlight);
            foreach (Airline a in airlineDict.Values)
            {
                if (a.Code == flightNo[0..2])
                {
                    a.AddFlight(newFlight);
                }
            }
            Console.WriteLine($"Flight {flightNo} has been added");
            Console.WriteLine("Would you like to add another flight? (Y/N)");
            string? option = Console.ReadLine();
            return option;
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Please enter a valid Special Request Code or 'None'");
            return "";
        }
    }
    static double counter = 0;
    public static void ProcessFlights(Queue<Flight> queue, List<BoardingGate> listBG)
    {
        double tempCount2 = 0;
        int flightCount = queue.Count;
        int gateCount = listBG.Count;
        Console.WriteLine();
        Console.WriteLine($"Flights with no assigned boarding gate: {flightCount}");
        Console.WriteLine($"Boarding gates with no assigned flight number: {gateCount}");
        Console.WriteLine();
        Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-22}{4,-35}{5,-24}{6,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Special Request Code", "Boarding Gate");
        int qLength = queue.Count;
        for (int i = 0; i < qLength; i++)
        {
            Flight nextFlight = queue.Dequeue();
            BoardingGate nextBG = null;
            string code = nextFlight.GetType().Name[0..4];
            if (code == "CFFT")
            {
                foreach (BoardingGate bg in listBG)
                {
                    if (bg.SupportsCFFT == true)
                    {
                        nextBG = bg;
                        break;
                    }
                }
            }
            else if (code == "DDJB")
            {
                foreach (BoardingGate bg in listBG)
                {
                    if (bg.SupportsDDJB == true)
                    {
                        nextBG = bg;
                        break;
                    }
                }
            }
            else if (code == "LWTT")
            {
                foreach (BoardingGate bg in listBG)
                {
                    if (bg.SupportsLWTT == true)
                    {
                        nextBG = bg;
                        break;
                    }
                }
            }
            else
            {
                foreach (BoardingGate bg in listBG)
                {
                    if (bg.SupportsLWTT == false && bg.SupportsDDJB == false && bg.SupportsLWTT == false)
                    {
                        nextBG = bg;
                    }
                }
            }
            boardingDict[nextBG.GateName].Flight = nextFlight;
            flightsDict[nextFlight.FlightNumber].Status = "On Time";
            listBG.Remove(nextBG);
            string airlineName = FindFlightAirline(airlineDict, nextFlight).Name;
            if (code == "NORM")
            {
                code = "";
            }
            Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-22}{4,-35}{5,-24}{6,-22}", nextFlight.FlightNumber, airlineName, nextFlight.Origin, nextFlight.Destination, nextFlight.ExpectedTime, code, nextBG.GateName);
            counter += 1;
        }
        foreach (BoardingGate bg in boardingDict.Values)
        {
            if (bg.Flight != null)
            {
                tempCount2 += 1;
            }
        }
        Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {counter}");
        Console.WriteLine($"{counter / tempCount2 * 100:F2}% of currently assigned flights and boarding gates were processed automatically");
    }

    public static void CalculateAirlineFees()
    {
        int count = 0;
        
        Console.WriteLine("{0,-16}{1,-17}", "Airline Code", "Airline Name");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            Console.WriteLine("{0,-16}{1,-17}", kvp.Value.Code, kvp.Value.Name);
        }

        Console.Write("Enter Airline Code: ");
        string? code = Console.ReadLine();
        Console.WriteLine("=============================================");
        bool found = false;
        foreach (KeyValuePair<string, Airline> airline in airlineDict)
        {
            if (airline.Key == code)
            {
                found = true;
                foreach (KeyValuePair<string, BoardingGate> boarding in boardingDict)
                {
                    foreach (KeyValuePair<string, Flight> kvp in airline.Value.Flights)
                    {
                        if (kvp.Value == boarding.Value.Flight)
                        {
                            count++;
                            break;
                        }
                        else { continue; }
                    }
                }
                    if (count == airline.Value.Flights.Count)
                {
                    double charge = airline.Value.CalculateFees();
                    Console.WriteLine("Total charge for {0} airline: {1:C2}", airline.Key, charge);
                }
                else { Console.WriteLine("Not all the flights have been assigned to a boarding gate.Please ensure that all flights have been assigned to a boarding gate."); }

            }


        }
        if (found == false)
        { Console.WriteLine("No such airline available."); }
    }
       
    }

