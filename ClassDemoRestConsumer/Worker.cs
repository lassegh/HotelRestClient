using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotelModels;
using Newtonsoft.Json;

namespace ClassDemoRestConsumer
{
    internal class Worker
    {

        private string URI;

        public Worker(string uri)
        {
            URI = uri+"/hotel";
        }

        public void Start()
        {
            List<Hotel> hotels = GetAll();

            foreach (var hotel in hotels)
            {
                Console.WriteLine("Hotel:: " + hotel.Name);
            }

            Console.WriteLine("Henter nummer 2");
            Console.WriteLine("Hotel :: " + GetOne(2).Name);


            Console.WriteLine("Sletter nummer 6");
            Console.WriteLine("Resultat = " + Delete(6));

            Console.WriteLine("Opretter nyt hotel ");
            Console.WriteLine("Resultat = " + Post(new Hotel("Prindsen", "Algade 47, 4000 Roskilde", 4)));

            Console.WriteLine("Opdaterer nr 3");
            Console.WriteLine("Resultat = " + Put(3, new Hotel("Pouls", "Et sted i Hillerød", 5)));

            // Lister alle hoteller igen
            Console.WriteLine("Lister alle hoteller igen:");
            hotels = GetAll();

            foreach (var hotel in hotels)
            {
                Console.WriteLine("Hotel:: " + hotel.Name);
            }

            Put(3, new Hotel("Scandic", "Ved Ringen 2, 4000 ROskilde", 4));
        }


        public List<Hotel> GetAll()
        {
            List<Hotel> hoteller = new List<Hotel>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Task<string> resTask = client.GetStringAsync(URI);
                    String jsonStr = resTask.Result;

                    hoteller = JsonConvert.DeserializeObject<List<Hotel>>(jsonStr);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Kunne ikke hentes -kontroller forbindelse til server");
                    Console.WriteLine(e);
                }
                
            }


            return hoteller;
        }



        private Hotel GetOne(int id)
        {
            Hotel hotel = new Hotel();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                    String jsonStr = resTask.Result;

                    hotel = JsonConvert.DeserializeObject<Hotel>(jsonStr);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Kunne ikke hentes -kontroller forbindelse til server");
                    Console.WriteLine(e);
                }
                
            }


            return hotel;
        }

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

        private bool Post(Hotel hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
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

        private bool Put(int id, Hotel hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
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