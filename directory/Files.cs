using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace directory
{
    class Files : FilesAndDirs
    {
        public int size;
        public string oldpath;
        public string normalpath;
        public string name;
        public Files(string path)
        {
            oldpath = path;   
            path = path.Substring(0, path.LastIndexOf(" ")) + path.Substring(path.LastIndexOf("."));
            name = path.Substring(path.LastIndexOf("\\"));
            normalpath = path;
            size = 0;
            Size();
        }

        public override int Size()
        {
            string size = "";
            StreamReader streamReader = new StreamReader(oldpath);
            char[] sizetext = streamReader.ReadToEnd().ToCharArray();
            streamReader.Close();
            int i = 0;
            while (char.IsDigit(sizetext[i]) && i < sizetext.Length)
            {
                size += sizetext[i];
                i++;
            }

            this.size = int.Parse(size);

            return int.Parse(size);
        }
    }
}
