using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;

namespace MoveFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathSource  = ConfigurationManager.AppSettings["pathSource"];
            string pathDestination = ConfigurationManager.AppSettings["pathDestination"];
            string pathArchive =Path.Combine( ConfigurationManager.AppSettings["pathArchive"],$"{DateTime.Now:ddMMyyyy}");
            string logPath = Directory.GetCurrentDirectory() + @"\Log.txt";
            //create folder in path archive 
            if (!Directory.Exists(pathArchive)) Directory.CreateDirectory(pathArchive);
            // create file Log.txt
            if (!File.Exists(logPath)) File.Create(logPath).Dispose();
            // using (StreamWriter writer = new StreamWriter(logPath)) { } ;
            Console.WriteLine("START=======>[" + DateTime.Now.ToString("T") + "]");
            while (true)
            { 
                //Utils.movefiles(pathSource, pathArchive);
                //Thread.Sleep(5000);
                //Utils.copyfiles(pathArchive, pathDestination);
                Utils.copyAndMoveFiles(pathSource, pathDestination, pathArchive);
                Console.WriteLine("==> IN SLEEP FOR 2 MINUTES : ["+ DateTime.Now.ToString("T") + "]");
                Thread.Sleep(120000);
                Console.WriteLine("==> RESTART : [" + DateTime.Now.ToString("T") + "]");
            }

        }
    }
}
