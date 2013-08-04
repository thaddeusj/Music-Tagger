using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public const string def_track_delim = "%k";
        public const string def_title_delim = "%t";
        public const string def_artist_delim = "%r";
        public const string def_album_delim = "%a";
        public const string def_year_delim = "%y";


        private static string setting_file_name = "settings.ini";


        public static MusicDirectoryTree directoryForest;


        //File types are, for now, '.mp3'
        public static List<string> fileExtensions;

        //private static int delim_length_largest = 2;
        const int MAX_DELIM_LENGTH = 2;




        public static bool addDirectory(string path)
        {
            if (directoryForest == null || !(directoryForest.Subdirs.Select(x => x.DirectoryPath).Contains(path)))
            {
                bool isSubDir = false;

                foreach (MusicDirectoryTree dt in directoryForest.Subdirs)
                {
                    if (path.Contains(dt.DirectoryPath))
                    {
                        isSubDir = true;
                        break;
                    }

                }

                
                if (!isSubDir) 
                {


                    ObservableCollection<MusicDirectoryTree> tempdirFor = new ObservableCollection<MusicDirectoryTree>();

                    foreach (MusicDirectoryTree dt in directoryForest.Subdirs)
                    {
                        if (!dt.DirectoryPath.Contains(path))
                        {
                            tempdirFor.Add(dt);
                        }

                    }

                    directoryForest.Subdirs.Clear();
                    foreach (MusicDirectoryTree dt in tempdirFor)
                    {
                        directoryForest.Subdirs.Add(dt);
                    }

                    directoryForest.Subdirs.Add(new MusicDirectoryTree(path));

                    return true; 
                }

                else return false;
            }

            return false;

        }

        public static void removeDirectory(string path)
        {
            directoryForest.Subdirs = new ObservableCollection<DirectoryTree>(directoryForest.Subdirs.Where(x => x.DirectoryPath != path).ToList());

        }

        //public static List<string> getDirectoryNames()
        //{

        //    return directoryForest.Select(x => x.DirectoryPath).ToList();


        //}



        public static void Initialise()
        {
            fileExtensions = new List<string>();
            directoryForest = new MusicDirectoryTree();

            fileExtensions.Add(".mp3");

            if (!loadSettings())
            {
                Track_delim = def_track_delim;
                Title_delim = def_title_delim;
                Artist_delim = def_artist_delim;
                Album_delim = def_album_delim;
                Year_delim = def_year_delim;
            }
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
                w.WriteLine("");

                foreach (DirectoryTree dt in directoryForest.Subdirs)
                {
                    w.WriteLine(dt.DirectoryPath);
                }

            }
        }

        /*
         * loadSettings loads the settings from setting_file_name following save scheme
         * 
         * 
         */

        public static bool loadSettings()
        {
            if (File.Exists(setting_file_name))
            {
                using (StreamReader r = new StreamReader(setting_file_name))
                {
                    string delim_line = r.ReadLine();
                    char[] delim_line_sep = { ';' };

                    string[] delims = delim_line.Split(delim_line_sep);
                    Artist_delim = delims[0];
                    Album_delim = delims[1];
                    Title_delim = delims[2];
                    Track_delim = delims[3];
                    Year_delim = delims[4];


                    string extensions_line = r.ReadLine();
                    char[] extensions_line_sep = { ';' };

                    fileExtensions = extensions_line.Split(extensions_line_sep).ToList();
                    fileExtensions.Remove(fileExtensions[fileExtensions.Count-1]); //because of how saving is done, the line ends in a ;, so we need to remove the last element of the list, which is a  ""

                    string dirPath;
                    while ((dirPath = r.ReadLine()) != null)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            addDirectory(dirPath);
                        }
                    }

                }

                return true;
            }

            else
            {
                return false;

            }
        }

        //public static void copyDirForest(List<MusicDirectoryTree> dirF)
        //{
        //    directoryForest = dirF;
        //}

        //public static void copyDirForestOut(out List<MusicDirectoryTree> dirF)
        //{
        //    dirF = new List<MusicDirectoryTree>();
        //    dirF = directoryForest;
        //}


    }
}
