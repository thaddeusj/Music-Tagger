using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApplication1
{
    class FileRenamer
    {
        private string[] files;
        private string directory;

        public FileRenamer()
        {
            directory = "";
        }

        public FileRenamer(string dir)
        {
            directory = dir;

            if(Directory.Exists(directory))
            {
                //assume directory is the absolute pathname
                files = Directory.GetFiles(directory);
            }
        }

    }
}
