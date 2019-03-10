using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotelModels;
using Newtonsoft.Json;

namespace ClassDemoRestConsumer
{
    class WorkerFacilities
    {
        private string URI;
        private Worker worker;

        public WorkerFacilities(string uri)
        {
            URI = uri + "/facility";
            worker = new Worker(uri);
        }

        /// <summary>
        /// Menu / UI
        /// </summary>
        public void Start()
        {
            Console.Clear();
            Console.WriteLine("1) Vis hele facility tabellen");
            Console.WriteLine("2) Vis facilities for et enkelt hotel");
            Console.WriteLine("3) Opret en facility til et hotel");
            Console.WriteLine("4) Ret en facility");
            Console.WriteLine("5) Slet en facility");

            int input;

            bool result = int.TryParse(Console.ReadLine(), out input);
            while (!result || 1 > input || input > 5 )
            {
                Console.WriteLine("\nIndtast ønskede menupunkt");
                result = int.TryParse(Console.ReadLine(), out input);
            }

            switch (input)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Facility tabel");
                    Console.WriteLine();
                    List<Facility> facilityList = GetAll();
                    Console.WriteLine("FacilityID\tHotelID\tFacility navn");
                    foreach (var facility in facilityList)
                    {
                        Console.WriteLine($"{facility.FacilityID}\t\t{facility.HotelID}\t{facility.FacilityName}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Tryk på en tast...");
                    Console.ReadKey();
                    Start();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Hotel tabel");
                    Console.WriteLine();
                    List<Hotel> hotelList = worker.GetAll();
                    Console.WriteLine("HotelID\tNavn\t \t \tAdresse");
                    foreach (var hotel in hotelList)
                    {
                        Console.WriteLine($"{hotel.HotelID}\t{hotel.Name}\t \t{hotel.Address}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Hvilket hotel vil du se faciliteter på? - tast hotelID");
                    int inputHotel;

                    bool resultHotel = int.TryParse(Console.ReadLine(), out inputHotel);
                    while (!resultHotel)
                    {
                        Console.WriteLine("\nIndtast det ønskede hotelID");
                        resultHotel = int.TryParse(Console.ReadLine(), out inputHotel);
                    }
                    Console.Clear();
                    Console.WriteLine("Facility tabel for et bestemt hotel");
                    Console.WriteLine();
                    List<Facility> facilityListSpecific = GetOne(inputHotel);
                    Console.WriteLine("FacilityID\tHotelID\tFacility navn");
                    foreach (var facility in facilityListSpecific)
                    {
                        Console.WriteLine($"{facility.FacilityID}\t \t{facility.HotelID}\t{facility.FacilityName}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Tryk på en tast...");
                    Console.ReadKey();
                    Start();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Opret ny facility");
                    Console.WriteLine("Indtast hotelID for ny facility");
                    int hotelID;
                    bool hotelIdTryParse = int.TryParse(Console.ReadLine(), out hotelID);
                    while (!hotelIdTryParse)
                    {
                        Console.WriteLine("\nIndtast det ønskede hotelID");
                        hotelIdTryParse = int.TryParse(Console.ReadLine(), out hotelID);
                    }

                    Console.WriteLine("Indtast navn på facility");
                    string facilityName = Console.ReadLine();
                    if (Post(new Facility(hotelID,facilityName)))
                    {
                        Console.WriteLine("Facility blev oprettet");
                    }else Console.WriteLine("Facility blev ikke oprettet");
                    Console.WriteLine("Tryk på en tast...");
                    Console.ReadKey();
                    Start();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Ret en facility fra tabel");
                    Console.WriteLine();
                    List<Facility> facilityLists = GetAll();
                    Console.WriteLine("FacilityID\tHotelID\tFacility navn");
                    foreach (var facility in facilityLists)
                    {
                        Console.WriteLine($"{facility.FacilityID}\t{facility.HotelID}\t{facility.FacilityName}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Indtast ID på den facility, der skal rettes");
                    int facilityIDUpdate;
                    bool facilityIdUpdateTryParse = int.TryParse(Console.ReadLine(), out facilityIDUpdate);
                    while (!facilityIdUpdateTryParse)
                    {
                        Console.WriteLine("Indtast ID på den facility, der skal rettes");
                        facilityIdUpdateTryParse = int.TryParse(Console.ReadLine(), out facilityIDUpdate);
                    }

                    Console.WriteLine("Indtast nyt eller eksisterende hotelID");
                    int hotelIDUpdate;
                    bool hotelIdUpdateTryParse = int.TryParse(Console.ReadLine(), out hotelIDUpdate);
                    while (!hotelIdUpdateTryParse)
                    {
                        Console.WriteLine("Indtast nyt eller eksisterende hotelID");
                        hotelIdUpdateTryParse = int.TryParse(Console.ReadLine(), out hotelIDUpdate);
                    }

                    Console.WriteLine("Indtast nyt eller eksisterende navn på facility");
                    string facilityNameUpdate = Console.ReadLine();

                    if (Put(facilityIDUpdate,new Facility(hotelIDUpdate,facilityNameUpdate)))
                    {
                        Console.WriteLine("Facility blev opdateret");
                    }else Console.WriteLine("Facility blev ikke opdateret");

                    Console.WriteLine("Tryk på en tast...");
                    Console.ReadKey();
                    Start();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Slat en facility fra tabel");
                    Console.WriteLine();
                    List<Facility> facilityListe = GetAll();
                    Console.WriteLine("FacilityID\tHotelID\tFacility navn");
                    foreach (var facility in facilityListe)
                    {
                        Console.WriteLine($"{facility.FacilityID}\t{facility.HotelID}\t{facility.FacilityName}");
                    }

                    Console.WriteLine();

                    int facilityIDDelete;
                    bool facilityIdDeleteTryParse = int.TryParse(Console.ReadLine(), out facilityIDDelete);
                    while (!facilityIdDeleteTryParse)
                    {
                        Console.WriteLine("Indtast ID på den facility, der skal Slettes");
                        facilityIdDeleteTryParse = int.TryParse(Console.ReadLine(), out facilityIDDelete);
                    }

                    if (Delete(facilityIDDelete))
                    {
                        Console.WriteLine("Facility blev slettet");
                    }else Console.WriteLine("Facility blev ikke slettet");

                    Console.WriteLine();
                    Console.WriteLine("Tryk på en tast...");
                    Console.ReadKey();
                    Start();
                    break;
                default:

                    break;
            }
        }

        /// <summary>
        /// Henter alle data fra tabellen - bruger GET
        /// </summary>
        /// <returns>Liste af objekter</returns>
        private List<Facility> GetAll()
        {
            List<Facility> facilityList = new List<Facility>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Task<string> resTask = client.GetStringAsync(URI);
                    String jsonStr = resTask.Result;

                    facilityList = JsonConvert.DeserializeObject<List<Facility>>(jsonStr);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Kunne ikke hentes -kontroller forbindelse til server");
                    Console.WriteLine(e);
                }

            }


            return facilityList;
        }


        /// <summary>
        /// Henter alle data med specifikt ID - bruger GET
        /// </summary>
        /// <param name="id">HotelID</param>
        /// <returns>Liste af objekter</returns>
        private List<Facility> GetOne(int id)
        {
            List<Facility> facilityList = new List<Facility>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                    String jsonStr = resTask.Result;

                    facilityList = JsonConvert.DeserializeObject<List<Facility>>(jsonStr);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Kunne ikke hentes -kontroller forbindelse til server");
                    Console.WriteLine(e);
                }

            }


            return facilityList;
        }

        /// <summary>
        /// Sletter fra tabel med specifikt ID - bruger DELETE
        /// </summary>
        /// <param name="id">ID på facility</param>
        /// <returns>bool</returns>
        private bool Delete(int id)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {

                try
                {
                    Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);

                    HttpResponseMessage resp = deleteAsync.Result;
                }
                catch (Exception e)
                {
                    ok = false;
                    Console.WriteLine(e);
                }
            }


            return ok;
        }

        /// <summary>
        /// Opretter ny facility - bruger POST
        /// </summary>
        /// <param name="facility">Et objekt af facility</param>
        /// <returns>bool</returns>
        private bool Post(Facility facility)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(facility);
                StringContent content = new StringContent(jsonStr, Encoding.ASCII, "application/json");

                try
                {
                    Task<HttpResponseMessage> postAsync = client.PostAsync(URI, content);

                    HttpResponseMessage resp = postAsync.Result;
                }
                catch (Exception e)
                {
                    ok = false;
                    Console.WriteLine(e);

                }
            }


            return ok;
        }

        /// <summary>
        /// Opdaterer et bestemt sted i tabellen - bruger PUT
        /// </summary>
        /// <param name="id">ID'et på det specifikke sted</param>
        /// <param name="facility">Objekt af facility</param>
        /// <returns>bool</returns>
        private bool Put(int id, Facility facility)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(facility);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                try
                {
                    Task<HttpResponseMessage> putAsync = client.PutAsync(URI + "/" + id, content);

                    HttpResponseMessage resp = putAsync.Result;
                }
                catch (Exception e)
                {
                    ok = false;
                    Console.WriteLine(e);
                }
            }


            return ok;
        }
    }
}
