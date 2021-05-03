using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.IO.Compression;


namespace directory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь каталога, который необходимо обработать:");

            string mypath = Console.ReadLine();
            //String mypath = @"C:\Users\Serega\Desktop\1\student-2021-assignment-master\student-2021-assignment-master\src\Utrom's secrets";

            DirectoryInfo droot = new DirectoryInfo(mypath);
            
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            directories.AddRange(droot.GetDirectories("*"));
            
            string rootpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            
            string subpath = "dest\\Utrom's secrets";
            DirectoryInfo dirInfo = new DirectoryInfo(rootpath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);
            
            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(droot.GetFiles("*"));
            List<FileInfo> filesName = new List<FileInfo>();

            List<string> newpaths = new List<string>();

            foreach (FileInfo file in files)
            {                
                string obrez = file.FullName.Substring(file.FullName.LastIndexOf(" "), file.FullName.IndexOf(".") - file.FullName.LastIndexOf(" "));
                         
                string newpath = file.FullName.Replace(obrez, ""); //obrezat'
                
                if (File.Exists(rootpath+"\\" + subpath+"\\"+file.Name.Replace(obrez, "")))
                {
                    if (Razmer(file.FullName)>Razmer(rootpath +"\\"+ subpath + "\\" + file.Name.Replace(obrez, "")))//if файл из старой директории больше, чем тот, что в новой
                    {
                        File.Copy(file.FullName, rootpath + "\\" + subpath + "\\" + file.Name.Replace(obrez, ""), true);
                    }
                }
                else
                {
                    File.Copy(file.FullName, rootpath+"\\" + subpath + "\\" + file.Name.Replace(obrez, ""));
                }
                
            }
            foreach(DirectoryInfo directoryInfo in directories)
            {
                string obrez = directoryInfo.FullName.Substring(directoryInfo.FullName.LastIndexOf(" "));
                string newpath = directoryInfo.FullName.Replace(obrez, "");
                if (Directory.Exists(rootpath + "\\" + subpath + "\\" + directoryInfo.Name.Replace(obrez, "")))
                {
                    if(GetSize(directoryInfo.FullName) > GetSize(rootpath + "\\" + subpath + "\\" + directoryInfo.Name.Replace(obrez, "")))
                    {
                        DirectoryCopy(directoryInfo.FullName, rootpath + "\\" + subpath + "\\" + directoryInfo.Name.Replace(obrez, ""));
                    }
                    
                }
                else
                {
                    DirectoryCopy(directoryInfo.FullName, rootpath + "\\" + subpath + "\\" + directoryInfo.Name.Replace(obrez, ""));
                }
            }
            ZipFile.CreateFromDirectory(rootpath+"\\" + subpath, rootpath+"\\" + subpath + ".zip");
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
                file.CopyTo(tempPath, false);
            }
                        
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath);
                }
        }

    static public int GetSize(string path)
        {
            int size = 0;
            List<string> info = new List<string>();
            info.AddRange(Directory.GetFiles(path));
            foreach (string file in info)
            {
                size += Razmer(file);
            }
            return size;
        }
        static public int Razmer(string newpath)
        {
            string size = "";
            StreamReader streamReader = new StreamReader(newpath);
            char[] sizetext = streamReader.ReadToEnd().ToCharArray();
            streamReader.Close();
            int i = 0;
            while (char.IsDigit(sizetext[i]) && i<sizetext.Length)
            {
                size += sizetext[i];
                i++;
            }
            return int.Parse(size);
        }
    }
}
