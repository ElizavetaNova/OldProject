using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


namespace directory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь каталога, который необходимо обработать:");
            string path = Console.ReadLine();
            DirectoryInfo rootdirectory = new DirectoryInfo(path);
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            directories.AddRange(rootdirectory.GetDirectories("*"));
            string rootpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string subpath = "dest\\Utrom's secrets";
            DirectoryInfo dirInfo = new DirectoryInfo(rootpath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);

            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(rootdirectory.GetFiles("*"));
            List<string> newpaths = new List<string>();

            foreach (FileInfo file in files)
            {
                string normalnamefile = file.Name.Substring(0, file.Name.LastIndexOf(' ')) + file.Name.Substring(file.Name.LastIndexOf('.'));
                string newpath = rootpath + "\\" + subpath + "\\" + normalnamefile;
                if (File.Exists(newpath))
                {
                    if (SizeFile(file.FullName) > SizeFile(newpath))//if файл из старой директории больше, чем тот, что в новой
                    {
                        File.Copy(file.FullName, newpath, true);
                    }
                }
                else
                {
                    File.Copy(file.FullName, newpath);
                }
            }
            foreach (DirectoryInfo directoryInfo in directories)
            {
                string fullnormalnamefolder = rootpath + "\\" + subpath + "\\" + directoryInfo.FullName.Substring(0, directoryInfo.FullName.LastIndexOf(' '));

                if (Directory.Exists(fullnormalnamefolder))
                {
                    if (SizeDirectory(directoryInfo.FullName) > SizeDirectory(fullnormalnamefolder))
                    {
                        DirectoryCopy(directoryInfo.FullName, fullnormalnamefolder);
                    }
                }
                else
                {
                    DirectoryCopy(directoryInfo.FullName, fullnormalnamefolder);
                }
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
                file.CopyTo(tempPath, false);
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }

        static public int SizeDirectory(string path)
        {
            int size = 0;
            List<string> filenames = new List<string>();
            filenames.AddRange(Directory.GetFiles(path));
            foreach (string file in filenames)
            {
                size += SizeFile(file);
            }
            return size;
        }
        static public int SizeFile(string newpath)
        {
            string size = "";
            StreamReader streamReader = new StreamReader(newpath);
            char[] sizetext = streamReader.ReadToEnd().ToCharArray();
            streamReader.Close();
            int i = 0;
            while (char.IsDigit(sizetext[i]) && i < sizetext.Length)
            {
                size += sizetext[i];
                i++;
            }
            return int.Parse(size);
        }
    }
}
