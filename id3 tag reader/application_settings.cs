using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Directory_Structure_Classes;
using System.IO;


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

        /*
         * A note on delimiters: we require all delimiters to start with %, and they may not contain a ;
         * 
         * For now, delimiters must be two characters, the first being %.
         *      TODO: implement variable delimiter lengths
         * 
         */

        public static string Track_delim { get; set; }
        public static string Title_delim { get; set; }
        public static string Artist_delim { get; set; }
        public static string Album_delim { get; set; }
        public static string Year_delim { get; set; }

        const string def_track_delim = "%k";
        const string def_title_delim = "%t";
        const string def_artist_delim = "%r";
        const string def_album_delim = "%a";
        const string def_year_delim = "%y";


        private static string setting_file_name = "settings.ini";


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

            Track_delim = def_track_delim;
            Title_delim = def_title_delim;
            Artist_delim = def_artist_delim;
            Album_delim = def_album_delim;
            Year_delim = def_year_delim;
        }



        /*
         * saveSettings saves the application settings to setting_file_name
         * 
         *      Settings are saved in this order:
         *          - Delimiters: artist;album;title;track;year
         *          - File extensions: ext1;ext2;...
         *          - Directory paths:
         *                  path1\n
         *                  path2\n
         *                  path3\n
         *                  ...
         *          
         * 
         */


        public static void saveSettings()
        {

            using (StreamWriter w = new StreamWriter(setting_file_name))
            {

                w.WriteLine(Artist_delim + ";" + Album_delim + ";" + Title_delim + ";" + Track_delim + ";" + Year_delim);

                foreach (string s in fileExtensions)
                {
                    w.Write(s + ";");

                }
                w.Write("\n");

                foreach (DirectoryTree dt in directoryForest)
                {
                    w.WriteLine(dt.DirectoryPath);
                }

            }
        }


    }
}
