using System;
using System.Collections.Generic;
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
            using (StreamWriter sw = File.AppendText(currentPath))
            {
                sw.WriteLine(" Nombre fichier zip trouvé :"+files.Length);
                sw.Dispose();
                sw.Close();
            }
            foreach (var file in files)
            {
                    string name = Path.GetFileName(file);
                using (StreamWriter sw = File.AppendText(currentPath))
                {
                    sw.WriteLine("file name"+name);
                    sw.WriteLine("time modified : " + File.GetLastWriteTime(file));
                    sw.Dispose();
                    sw.Close();
                }
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
        public static void copyAndMoveFiles(string source , string destination , string archive)
        {
            string[] files = Directory.GetFiles(source,"*.zip");
            DateTime dateNow = DateTime.Now;
            if (!Directory.Exists(destination))
            {
                Console.WriteLine("Path destination introuvable");
            }
            foreach (var file in files)
            {

                string name = Path.GetFileName(file);

                if ((dateNow.AddHours(1) - File.GetLastWriteTime(file)).TotalMinutes >= 2 || (dateNow - File.GetLastWriteTime(file)).TotalMinutes >= 2)
                {
                    try
                    {
                        File.Copy(file, file.Replace(source, destination), true);
                        Console.WriteLine("File Copy From Source To Destination : " + name);
                        File.Move(file, file.Replace(source, archive));
                        Console.WriteLine("File Moved From Source To Archive : " + name);
                    }
                    catch
                    {
                        Console.WriteLine("Erreur Copy in file : " + name);
                        using (StreamWriter sw = File.AppendText(pathLot))
                        {
                            sw.WriteLine("!!!!!!! Erreur Copy in file : " + name +" ["+dateNow.ToString()+"]");
                            sw.Dispose();
                            sw.Close();
                        }
                    }
                    
                }

            }
        }

    }
}