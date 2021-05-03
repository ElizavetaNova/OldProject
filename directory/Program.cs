using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Linq;


namespace directory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь каталога, который необходимо обработать:");
            string path = Console.ReadLine();
            
            DirectoryInfo rootdirectory = new DirectoryInfo(path);


            List<Directories> directories = new List<Directories>();
            List<Files> files = new List<Files>(); 
            foreach (DirectoryInfo dir in rootdirectory.GetDirectories())
            {
                Directories directory = new Directories(dir.FullName);
                directories.Add(directory);
            }

            foreach(Directories dir in directories)
            {
                dir.Size();
            }
            foreach(FileInfo file in rootdirectory.GetFiles())
            {
                Files filenew = new Files(file.FullName);
                files.Add(filenew);
            }


            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    foreach (Files files1 in files)
                    {
                        if (files1.normalpath == files.ElementAt(i).normalpath)
                        {
                            if (files1.size > files.ElementAt(i).size)
                            {
                                files.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception) { }
           
            try
            {
                for (int i = 0; i < directories.Count; i++)
                {
                    foreach (Directories dir1 in directories)
                    {
                        if (dir1.name == directories.ElementAt(i).name)
                        {
                            if (dir1.size > directories.ElementAt(i).size)
                            {
                                directories.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

            string rootpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string subpath = "dest\\Utrom's secrets";
            DirectoryInfo dirInfo = new DirectoryInfo(rootpath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);

            foreach (Files file in files)
            {
                string newpath = rootpath + "\\" + subpath + "\\" + file.name;
                File.Copy(file.oldpath, newpath, true);
            }
            foreach (Directories dir in directories)
            {
                string fullnormalnamefolder = rootpath + "\\" + subpath + "\\" + dir.name;
                DirectoryCopy(dir.oldpath, fullnormalnamefolder);
            }
            ZipFile.CreateFromDirectory(rootpath + "\\" + subpath, rootpath + "\\" + subpath + ".zip");
            Console.WriteLine("Каталог обработан");
            Console.ReadKey();
        }
        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                try
                {
                    file.CopyTo(tempPath, false);
                }
                catch (Exception) { }
              
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);               
                try
                {
                    DirectoryCopy(subdir.FullName, tempPath);
                }
                catch (Exception) { }
            }
        }





        //static public int SizeDirectory(string path)
        //{
        //    int size = 0;
        //    List<string> filenames = new List<string>();
        //    filenames.AddRange(Directory.GetFiles(path));
        //    foreach (string file in filenames)
        //    {
        //        size += SizeFile(file);
        //    }
        //    return size;
        //}




        //static public int SizeFile(string newpath)
        //{
        //    string size = "";
        //    StreamReader streamReader = new StreamReader(newpath);
        //    char[] sizetext = streamReader.ReadToEnd().ToCharArray();
        //    streamReader.Close();
        //    int i = 0;
        //    while (char.IsDigit(sizetext[i]) && i < sizetext.Length)
        //    {
        //        size += sizetext[i];
        //        i++;
        //    }
        //    return int.Parse(size);
        //}
    }
}
