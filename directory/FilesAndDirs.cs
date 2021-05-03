using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace directory
{
    abstract class FilesAndDirs
    {
        int size;
        string path;
        string name;
        public abstract int Size(/*string path*/);

    }
}
