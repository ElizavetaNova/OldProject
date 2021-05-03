using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace directory
{
    class Directories : FilesAndDirs
    {
        public string name;
        public string path;
        public int size;
        public string oldpath;
        List<Files> files = new List<Files>();
        public Directories(string path)
        {
            oldpath = path;
            path = path.Substring(0, path.LastIndexOf(" "));
            name = path.Substring(path.LastIndexOf("\\"));
            this.path = path;
            Createfiles();
        }

       
        public void Createfiles() //????????????????
        {
            string[] filenames = Directory.GetFiles(oldpath);
            foreach(string filename in filenames)
            {
                Files file = new Files(filename);
                files.Add(file);
                Size();
                //size += file.Size();
            }
        }
        public override int Size(/*string path*/)
        {

            foreach (Files file in files)
            {
                size += file.Size();
            }
            return size;
        }
    }
}
