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

        private static int delim_length_largest = 2;
        const int MAX_DELIM_LENGTH = 2;




        public static void addDirectory(string path)
        {
            if (!(directoryForest.Select(x => x.DirectoryPath).Contains(path))) directoryForest.Add(new MusicDirectoryTree(path)); 

        }

        public static void removeDirectory(string path)
        {
            directoryForest = directoryForest.Where(x => x.DirectoryPath != path).ToList();

        }



    }
}
