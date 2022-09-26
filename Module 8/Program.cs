using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters.Binary;
namespace Serialization
{
    class Program
    {
    
        static void Main(string[] args)
        {
        }
        static void Task1(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);  //получим все файлы

                foreach (string s in files)
                {
                    FileInfo file = new FileInfo(s);
                    if ((DateTime.Now - file.LastAccessTime) > TimeSpan.FromMinutes(30))    //доступ к файлу был сделан >30 мин назад
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine("Security Exception:\n\n{0}", e.Message);
                        }
                    }
                }
                string[] Directories = Directory.GetDirectories(path);  //получим все каталоги

                foreach (string d in Directories)
                {
                    DirectoryInfo dist = new DirectoryInfo(d);
                    if ((DateTime.Now - dist.LastAccessTime) > TimeSpan.FromMinutes(30))    //доступ к каталогу был сделан >30 мин назад
                    {
                        try { 
                            dist.Delete(true);
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine("Security Exception:\n\n{0}", e.Message);
                        }
                    }
                }
            }
        }
        static long Task2(string path)
        {
            long result = 0;
            if (Directory.Exists(path))
            {
            
                string[] files = Directory.GetFiles(path);  //получим все файлы

                foreach (string s in files)
                {
                    result += new FileInfo(s).Length;
                }
                string[] Directories = Directory.GetDirectories(path);  //получим все каталоги
                foreach (string d in Directories)
                {
                    result += Task2(d);

                }
            }
            return result;
        }
        static void Task3(string path)
        {
            if (Directory.Exists(path))
            {
                long old_memory = Task2(path);
                Console.WriteLine("Исходный размер папки: {0} байт", old_memory);
                long deleted_number = 0;

                string[] files = Directory.GetFiles(path);  //получим все файлы

                foreach (string s in files)
                {
                    FileInfo file = new FileInfo(s);
                    if ((DateTime.Now - file.LastAccessTime) > TimeSpan.FromMinutes(30))    //доступ к файлу был сделан >30 мин назад
                    {
                        try
                        {
                            file.Delete();
                            deleted_number++;
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine("Security Exception:\n\n{0}", e.Message);
                        }
                    }
                }
                string[] Directories = Directory.GetDirectories(path);  //получим все каталоги

                foreach (string d in Directories)
                {
                    DirectoryInfo dist = new DirectoryInfo(d);
                    if ((DateTime.Now - dist.LastAccessTime) > TimeSpan.FromMinutes(30))    //доступ к каталогу был сделан >30 мин назад
                    {
                        try
                        {
                            dist.Delete(true);
                            deleted_number++;
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine("Security Exception:\n\n{0}", e.Message);
                        }
                    }
                }
                Console.WriteLine("Удалено {0} каталогов и файлов", deleted_number);
                long memory = Task2(path);
                Console.WriteLine("Освобождено: {0} байт", old_memory- memory);
                Console.WriteLine("Текущий размер папки: {0} байт",memory);
            }
        }
    
    
        static void Task4()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var fs = new FileStream(@"D:\Downloads\Students.dat", FileMode.OpenOrCreate))
            {

                /*
                 нифига эта шляпа не работает
                 */
                var Student = (Students)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");
            }
        }
    }
    [Serializable]
    class Students
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Students(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
}
