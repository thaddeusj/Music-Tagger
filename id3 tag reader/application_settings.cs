using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Directory_Structure_Classes;


namespace WpfApplication1
{

    /*
     * application_settings: holds global application settings
     *                       such as:
     *                              - music directory structure, including directories to consider (implemented)
     *                              - let user 
     * 
     * 
     * 
     */



    public static class application_settings
    {

        private static string track_delim = "%k";
        private static string title_delim = "%t";
        private static string artist_delim = "%r";
        private static string album_delim = "%a";
        private static string year_delim = "%y";

        private static List<MusicDirectoryTree> directoryForest;


        //File types are, for now, '.mp3'
        public static List<string> fileExtensions;

        private static int delim_length_largest = 2;
        const int MAX_DELIM_LENGTH = 2;




        public static bool addDirectory(string path)
        {
            if (directoryForest == null || !(directoryForest.Select(x => x.DirectoryPath).Contains(path)))
            {
                bool isSubDir = false;

                foreach (MusicDirectoryTree dt in directoryForest)
                {
                    if (path.Contains(dt.DirectoryPath))
                    {
                        isSubDir = true;
                        break;
                    }

                }

                
                if (!isSubDir) 
                { 
                    

                    var tempdirFor = directoryForest.ToList();

                    foreach (MusicDirectoryTree dt in directoryForest)
                    {
                        if (dt.DirectoryPath.Contains(path))
                        {
                            tempdirFor.Remove(dt);
                        }

                    }

                    directoryForest = tempdirFor;

                    directoryForest.Add(new MusicDirectoryTree(path));

                    return true; 
                }

                else return false;
            }

            return false;

        }

        public static void removeDirectory(string path)
        {
            directoryForest = directoryForest.Where(x => x.DirectoryPath != path).ToList();

        }

        public static List<string> getDirectoryNames()
        {

            return directoryForest.Select(x => x.DirectoryPath).ToList();


        }



        public static void Initialise()
        {
            fileExtensions = new List<string>();
            directoryForest = new List<MusicDirectoryTree>();

            fileExtensions.Add(".mp3");
        }


    }
}
