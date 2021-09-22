using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoveFiles
{
    class Utils
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static bool CheckLot(string nameFile, string path)
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line == nameFile)
                {
                    return true;
                }
            }

            return false;
        }
        public static void movefiles(string source, string archive)
        {
            string[] files = Directory.GetFiles(source, "*.ZIP");
            DateTime dateNow = DateTime.Now;
            string currentPath = Directory.GetCurrentDirectory() + @"\LOG.txt";
            
            if (!File.Exists(currentPath))
            {
                File.Create(currentPath).Dispose();
            }
            else
            {
                using (StreamWriter sq = new StreamWriter(currentPath)) { }
            }
            if (!Directory.Exists(archive))
            {
                Console.WriteLine("Path archive introuvable");
            }

            foreach (var file in files)
            {
                    string name = Path.GetFileName(file);

                if (!File.Exists(Path.Combine(archive,name)) && ((dateNow - File.GetLastWriteTime(file)).TotalMinutes >= -58 || (dateNow - File.GetLastWriteTime(file)).TotalMinutes >= 2))
                {
                  
                    File.Move(file, file.Replace(source, archive));

                    Console.WriteLine("File Moved From Source To Archive : " +name);
                }
                                       
            }
        }
        static string  pathLot = Directory.GetCurrentDirectory() + @"\Log.txt";
        public static void copyfiles(string archive, string destination)
        {
            string[] files = Directory.GetFiles(archive,"*.zip");
            
            if (!Directory.Exists(destination))
            {
                Console.WriteLine("Path destination introuvable");
            }
            foreach (var file in files)
            {
                
                string name = Path.GetFileName(file);
              
                if (!CheckLot(name, pathLot) && File.GetLastWriteTime(file).Year==DateTime.Today.Year && File.GetLastWriteTime(file).Month == DateTime.Today.Month && File.GetLastWriteTime(file).Day == DateTime.Today.Day  )
                {
                   
                    File.Copy(file, file.Replace(archive, destination), true);
                    using (StreamWriter sw = File.AppendText(pathLot))
                    {
                        sw.WriteLine(name);
                        sw.Dispose();
                        sw.Close();
                    }
                    Console.WriteLine("File Copy From Archive To Destination : " + name);
                }

            }
        }
        public static void copyAndMoveFiles(string source , string destination , string archive , string archiveDouble)
        {
            string[] files = Directory.GetFiles(source,"*.zip");
            int time_out =Convert.ToInt32(ConfigurationManager.AppSettings["time_out"]);
            DateTime dateNow = DateTime.Now;
            if (!Directory.Exists(destination))
            {
                Console.WriteLine("Path destination introuvable");
                logger.Info("Path destination introuvable");
            }
            foreach (var file in files)
            {
                
                string name = Path.GetFileName(file);
                FileInfo info = new FileInfo(file);
                if ( (dateNow - File.GetLastWriteTime(file)).TotalMinutes >= 2 && info.Length > 0)
                {
                    try
                    {  if(!File.Exists(Path.Combine(archive, name)))
                        {
                            File.Copy(file, file.Replace(source, archive), false);
                            Console.WriteLine("File Copy From Source To Archive : " + name);
                            logger.Info("File Copy From Source To Archive : " + name);
                        }
                        else
                        {
                            File.Copy(file, file.Replace(source, archiveDouble), true);
                            Console.WriteLine("File Copy From Source To Folder Double : " + name);
                            logger.Info("File is already exist in archive  : " + name);
                            logger.Info("File Copy From Source To Folder Double : " + name);
                        }
                        File.Move(file, file.Replace(source, destination));
                        Console.WriteLine("File Moved From Source To Destination : " + name);
                        logger.Info("File Moved From Source To Destination : " + name);
                    }
                    catch(Exception ex)
                    {
                       
                        Console.WriteLine("Erreur Move in file : " + name);
                        logger.Error("Erreur Move in file : "+ ex.Message +" => "+ name);     
                    }
                    
                }

            }
        }

    }
}