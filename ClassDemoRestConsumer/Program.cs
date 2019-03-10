using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDemoRestConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string URI = "http://localhost:52476/API";
            WorkerFacilities wf = new WorkerFacilities(URI);
            wf.Start();

            Console.ReadLine();
        }
    }
}
