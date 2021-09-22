using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Globalization;
using Thinktecture.IdentityModel.Constants;
using NLog;

namespace MoveFiles
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
           
            while (true)
            {
                try
                {
                    string pathSource = ConfigurationManager.AppSettings["pathSource"];
                    string pathDestination = ConfigurationManager.AppSettings["pathDestination"];
                    string pathArchive = Path.Combine(ConfigurationManager.AppSettings["pathArchive"], $"{DateTime.Now:ddMMyyyy}");                   
                    string pathArchiveDouble = Path.Combine(ConfigurationManager.AppSettings["pathArchive"], $"{DateTime.Now:ddMMyyyy}" + "_Double");
                    string heure_debut = ConfigurationManager.AppSettings["heure_debut"];
                    string heure_fin = ConfigurationManager.AppSettings["heure_fin"];
                    int jour1 =Convert.ToInt32(ConfigurationManager.AppSettings["jour1"]);
                    int jour2 =Convert.ToInt32(ConfigurationManager.AppSettings["jour2"]);
                    DateTime dateValue;
                    string dateString = DateTime.Now.ToString("MM/dd/yyyy");
                    dateValue = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                    int sleeping_time = Convert.ToInt32(ConfigurationManager.AppSettings["sleeping_time"]);
                    if (DateTime.Now > Convert.ToDateTime(heure_debut) && DateTime.Now < Convert.ToDateTime(heure_fin) && Convert.ToInt32( dateValue.DayOfWeek) != jour1 && Convert.ToInt32(dateValue.DayOfWeek) != jour2)
                    {
                        //create folder  path archive 
                        if (!Directory.Exists(pathArchive)) Directory.CreateDirectory(pathArchive);
                        if (!Directory.Exists(pathArchiveDouble)) Directory.CreateDirectory(pathArchiveDouble);
                        Console.WriteLine("START =======>[" + DateTime.Now.ToString("T") + "]");
                        Utils.copyAndMoveFiles(pathSource, pathDestination, pathArchive, pathArchiveDouble);
                        Console.WriteLine("==> IN SLEEP FOR 2 MINUTES : [" + DateTime.Now.ToString("T") + "]");
                        Thread.Sleep(sleeping_time * 60000);
                        Console.WriteLine("==> RESTART : [" + DateTime.Now.ToString("T") + "]");
                    }
                }
                catch(Exception ex )
                {
                    Console.WriteLine("Erreur : "+ex.Message);
                    logger.Error("Erreur : " + ex.Message);
                }
              
               
            }

        }
    }
}
