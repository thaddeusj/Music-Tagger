using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace WpfApplication1.Directory_Structure_Classes
{
    /*  MusicDirectoryTree:
     *      Represents a music directory structure.
     *      Files are stored as MusicFileNodes    
     *      
     *      There is a static list of permissible file extensions
     * 
     *      
     * 
     * 
     */


    public class MusicDirectoryTree : DirectoryTree
    {
        
        private ObservableCollection<MusicFileNode> files;
        public ObservableCollection<MusicFileNode> Files
        {
            set { files = value; }
            get { return files; }
        }

        

        //recursively adds subdirectories

        public MusicDirectoryTree() : base() { }

        public MusicDirectoryTree(string path)
            : base(path)
        {
            files = new ObservableCollection<MusicFileNode>();

            getFiles();

            foreach (string subDirPath in System.IO.Directory.GetDirectories(path))
            {
                addSubDir(new MusicDirectoryTree(subDirPath));
            }

        }



        /*
         * Adding files to the directory can be done in one of two ways:
         *      1) add the file, without metadata
         *      2) add the file and metadata
         *      
         * Since metadata can be added to the filenode later, we have to ensure that we add a given file only once.
         */

        public void addFile(string path)
        {
            bool isInFiles = false;
            foreach (MusicFileNode file in files)
            {
                if (file.FileName.CompareTo(path) == 0) isInFiles = true;
            }

            if (isFileAdmissible(path) && !isInFiles) files.Add(new MusicFileNode(path));

        }

        public void addFile(string title, string album, string year, string track, string artist, string path)
        {
            bool isInFiles = false;
            foreach (MusicFileNode file in files)
            {
                if (file.FileName.CompareTo(path) == 0) isInFiles = true;
            }

            if (isFileAdmissible(path) && !isInFiles) files.Add(new MusicFileNode(title, album, artist, year, track, path));
        }


        /*
         * Removing is done by filename.
         */


        public void removeFile(string path)
        {
            files =new ObservableCollection<MusicFileNode>((files.Where(x => x.FileName != path).ToList()));

        }

        public override bool isFileAdmissible(string filePath)
        {
            return application_settings.fileExtensions.Contains(Path.GetExtension(filePath));
        }

        public override void getFiles()
        {
            foreach (string fileName in System.IO.Directory.GetFiles(DirectoryPath))
            {
                if (isFileAdmissible(fileName))
                {
                    addFile(fileName);
                }
            }
        }

    }
}
