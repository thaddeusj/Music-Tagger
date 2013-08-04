using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication1
{
    

    public class tagger : INotifyPropertyChanged
    {

        private string title;
        public string Title 
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        private string album;
        public string Album
        {
            get { return album; }
            set
            {
                album = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Album"));
            }
        }

        private string track;
        public string Track
        {
            get { return track; }
            set
            {
                track = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Track"));
            }
        }

        private string artist;
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Artist"));
            }
        }

        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Year"));
            }
        }

        private string unique_id;
        public string Unique_id
        {
            get { return unique_id; }
            set
            {
                unique_id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("unique_id"));
            }
        }

        //Filenames are stored with FULL PATH

        public string File_Name_Formatted { get; set; }

        private string file_Name_Original;
        public string File_Name_Original
        {
            get { return file_Name_Original; }
            set
            {
                file_Name_Original = value;
                OnPropertyChanged(new PropertyChangedEventArgs("File_Name_Original"));
            }
        }


        public static string track_delim = "%k";

        const string TITLE_TAG = "TIT2";
        const string ALBUM_TAG = "TALB";
        const string TRACK_TAG = "TRCK";
        const string ARTIST_TAG = "TPE1";
        const string RELEASE_DATE_TAG = "TDRL";
        const string RECORDING_DATE_TAG = "TDRC";
        const string ENCODING_DATE_TAG = "TDEN";

        public Dictionary<string, string> delims;
        public int delimLen;

        public tagger()
        {
            Title = "";
            Album = "";
            Track = "";
            Artist = "";
            Year = "";

            File_Name_Formatted = "";

            delims = new Dictionary<string, string>();
            
            //delimiters for filename formatting:
            delims.Add("%a", Album);
            delims.Add("%r", Artist);
            delims.Add("%y", Year);
            delims.Add(track_delim, Track);
            delims.Add("%t", Title);
            delimLen = 2;

        }

        public tagger(string ttitle, string talbum, string tartist, string tyear, string ttrack, string tfile_name, string tformat_temp, string tuid)
        {

            Title = ttitle;
            Album = talbum;
            Artist = tartist;
            Year = tyear;
            Track = ttrack;
            File_Name_Original = tfile_name;
            File_Name_Formatted = tformat_temp;
            Unique_id = tuid;

            delims = new Dictionary<string, string>();

            //delimiters for filename formatting:
            delims.Add("%a", Album);
            delims.Add("%r", Artist);
            delims.Add("%y", Year);
            delims.Add(track_delim, Track);
            delims.Add("%t", Title);
            delims.Add("%u", Unique_id);
            delimLen = 2;

        }

        // PRIMITIVE VERSION: ASSUME THAT FILE HEADER IS IN ONE PIECE, ie NO FOOTER
        // ALSO, ASSUME IT'S 2.4.0
        public void read_ID3Tag(string path)
        {
            if (File.Exists(path))
            {
                using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
                {



                    //GET TAG LENGTH: first, skip the tag identifiers

                    string ID3 = b.ReadChar().ToString() + b.ReadChar() + b.ReadChar();
                    if (ID3 != "ID3") throw new System.FormatException("This is not an ID3 tagged mp3!");

                    byte version = b.ReadByte();

                    b.ReadByte(); 

                    
                    
                    byte flag = b.ReadByte(); //we ignore the second version byte and flag
                    bool hasFooter = ((flag & (byte)16) == (byte)16) ? true : false;
                    bool unsync = ((flag & 128) == 128) ? true : false;

                    if (version == 4 && !hasFooter) read_ID3_2point4(b);
                }
            }
        }

        public void read_ID3_2point3(BinaryReader b)
        {

        }

        //performance issue with throwing around binaryreaders?

        //public void read_ID3_2point4(BinaryReader b)
        //{
        //    int tag_length = 0;
        //    int currentByte = 0;

        //    tag_length = ID3_tagSize(b);

        //    while (currentByte < tag_length)
        //    {
        //        currentByte += 10;

        //        FrameInfo f = new FrameInfo(b);
        //        f.strip_info();

        //        switch (f.frame_type)
        //        {

        //            case TITLE_TAG:

        //                Title = read_frame2point4(b, f);

        //                break;
        //            case ALBUM_TAG:

        //                Album = read_frame2point4(b, f);

        //                break;
        //            case ARTIST_TAG:

        //                Artist = read_frame2point4(b, f);

        //                break;
        //            case RELEASE_DATE_TAG:
                        
        //                Year = read_frame2point4(b, f);
        //                if (Year.Length > 3) Year = Year.Substring(0, 4);
        //                break;
                        
        //            case RECORDING_DATE_TAG:
        //                {
        //                    if (Year.Length == 0)
        //                    {
        //                        Year = read_frame2point4(b, f);
        //                        if(Year.Length >3)Year = Year.Substring(0, 4);
        //                        break;

        //                    }
        //                    else
        //                    {
        //                        b.ReadBytes(f.frame_size);
        //                    }
        //                    break;
        //                }
        //            case TRACK_TAG:
        //                {

        //                    Track = read_frame2point4(b, f);
        //                    char[] seps = { '/' };
        //                    Track = Track.Split(seps)[0];

        //                    break;
        //                }
        //            default:
        //                b.ReadBytes(f.frame_size);
        //                break;
        //        }
        //        currentByte += f.frame_size;
        //    }

        //    delims["%a"] = Album;
        //    delims["%r"] = Artist;
        //    delims["%y"] = Year;
        //    delims["%k"] = Track;
        //    delims["%t"] = Title;

        //}

        //rewritten method, to avoid passing b as parameters

        public void read_ID3_2point4(BinaryReader b)
        {
            int tag_length = 0;
            int currentByte = 0;

            tag_length = ID3_tagSize(b);

            byte[] tagBytes = b.ReadBytes(tag_length);

            while (currentByte + 10< tag_length) //the tag may be improperly formatted, so as to end in the middle of a frame header, so we check to see if there actually is a next frame header
            {
                

                byte[] frameHead = new byte[10];
                for (int i = 0; i < 10; i++) frameHead[i] = tagBytes[currentByte + i];

                currentByte += 10;

                FrameInfo f = new FrameInfo();
                f.strip_info(frameHead);

                if (currentByte + f.frame_size < tag_length)
                {
                    byte[] frameContent = new byte[f.frame_size];
                    for (int i = 0; i < frameContent.Length; i++) frameContent[i] = tagBytes[i + currentByte];


                    switch (f.frame_type)
                    {

                        case TITLE_TAG:

                            Title = read_frame2point4(frameContent);

                            break;
                        case ALBUM_TAG:

                            Album = read_frame2point4(frameContent);

                            break;
                        case ARTIST_TAG:

                            Artist = read_frame2point4(frameContent);

                            break;
                        case RELEASE_DATE_TAG:

                            Year = read_frame2point4(frameContent);
                            if (Year.Length > 3) Year = Year.Substring(0, 4);
                            break;

                        case RECORDING_DATE_TAG:
                            {
                                if (Year.Length == 0)
                                {
                                    Year = read_frame2point4(frameContent);
                                    if (Year.Length > 3) Year = Year.Substring(0, 4);
                                    break;

                                }

                                break;
                            }
                        case TRACK_TAG:
                            {

                                Track = read_frame2point4(frameContent);
                                char[] seps = { '/' };
                                Track = Track.Split(seps)[0];

                                break;
                            }
                        default:
                            break;
                    }
                }


               
                currentByte += f.frame_size;
            }

            delims["%a"] = Album;
            delims["%r"] = Artist;
            delims["%y"] = Year;
            delims["%k"] = Track;
            delims["%t"] = Title;

        }

        public string read_frame2point4(byte[] frameContent)
        {

            byte encodingByte = frameContent[0];

            //ISO-8859-1: begins and ends with $00, (guaranteed to begin with $00 at least)
            if (encodingByte == 0)
            {
                Encoding encoding = Encoding.GetEncoding(28591);

                if (frameContent[frameContent.Length - 1] != 0)
                {
                    byte[] contentBytes = new byte[frameContent.Length - 1];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));
                }
                else
                {
                    byte[] contentBytes = new byte[frameContent.Length - 2];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));

                }

            }

            //utf-16 character encoding: each character is two bytes
            //ID3 frames encoded in UTF-16 start with $01 and end in $00 00
            if (encodingByte == 1)
            {
                UnicodeEncoding encoding = new UnicodeEncoding();

                if (frameContent[frameContent.Length - 1] != 0 || frameContent[frameContent.Length - 2]!= 0)
                {
                    byte[] contentBytes = new byte[frameContent.Length - 3];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 3];
                    }

                    return new string(encoding.GetChars(contentBytes));
                }
                else
                {
                    byte[] contentBytes = new byte[frameContent.Length - 5];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 3];
                    }

                    return new string(encoding.GetChars(contentBytes));

                }


            }


            //same as UTF16, but no byte order mark
            //frames start with $02, end with $00 00
            if (encodingByte == 2)
            {

                UnicodeEncoding encoding = new UnicodeEncoding();

                if (frameContent[frameContent.Length - 1] != 0 || frameContent[frameContent.Length - 2] != 0)
                {
                    byte[] contentBytes = new byte[frameContent.Length - 1];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));
                }
                else
                {
                    byte[] contentBytes = new byte[frameContent.Length - 3];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));

                }

            }


            //UTF-8 encoded: starts with $03, terminated by $00
            if (encodingByte == 3)
            {
                UTF8Encoding encoding = new UTF8Encoding();

                if (frameContent[frameContent.Length - 1] != 0)
                {
                    byte[] contentBytes = new byte[frameContent.Length - 1];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));
                }
                else
                {
                    byte[] contentBytes = new byte[frameContent.Length - 2];

                    for (int i = 0; i < contentBytes.Length; i++)
                    {
                        contentBytes[i] = frameContent[i + 1];
                    }

                    return new string(encoding.GetChars(contentBytes));

                }


            }


            return "";
        }
        

        
        public string read_frame2point4(BinaryReader b, FrameInfo f)
        {
            string s = "";
            byte encodingByte = b.ReadByte();


            //utf-16 character encoding: each character is two bytes
            //ID3 frames encoded in UTF-16 start with $01 and end in $00 00
            if (encodingByte == 1)
            {
                byte[] BOM = b.ReadBytes(2); //read BOM
                byte[] frameBytes = b.ReadBytes(f.frame_size - 5);
                UnicodeEncoding encoding = new UnicodeEncoding();

                s = new string(encoding.GetChars(frameBytes));

                byte[] lastBytes = b.ReadBytes(2); //read last tag bytes 
                if (lastBytes[0] != 0 || lastBytes[1] != 0)
                {
                    s += new string(encoding.GetChars(lastBytes));

                }

            }

            //UTF-8 encoded: starts with $03, terminated by $00
            if (encodingByte == 3)
            {
                byte[] frameBytes = b.ReadBytes(f.frame_size - 2);
                UTF8Encoding encoding = new UTF8Encoding();

                s = new string(encoding.GetChars(frameBytes));

                byte[] lastByte = b.ReadBytes(1); //read last tag bytes 
                if (lastByte[0] != 0)
                {
                    s += new string(encoding.GetChars(lastByte));
                }

            }

            //same as UTF16, but no byte order mark
            if (encodingByte == 2)
            {
                byte[] frameBytes = b.ReadBytes(f.frame_size - 3);
                UnicodeEncoding encoding = new UnicodeEncoding();

                s = new string(encoding.GetChars(frameBytes));

                byte[] lastBytes = b.ReadBytes(2); //read last tag bytes 
                if (lastBytes[0] != 0 || lastBytes[1] != 0)
                {
                    s += new string(encoding.GetChars(lastBytes));

                }
            }

            //ISO-8859-1: begins and ends with $00
            if (encodingByte == 0)
            {
                byte[] frameBytes = b.ReadBytes(f.frame_size - 2);
                Encoding encoding = Encoding.GetEncoding(28591);

                s = new string(encoding.GetChars(frameBytes));

                byte[] lastByte = b.ReadBytes(1); //read last tag bytes 
                if (lastByte[0] != 0)
                {
                    s += new string(encoding.GetChars(lastByte));
                }

            }


            return s;

        }


        //helper function to recover an ID3 tag or frame size

        public static int ID3_tagSize(BinaryReader b)
        {
            int i = 0;
            byte[] arr = new byte[4];

            for (int j = 0; j < 4; j++)
            {
                arr[j] = b.ReadByte();
                
            }


            for (int j = 0; j < arr.Length; j++)
            {
                i += ((int)arr[j]) * (1 << (7 * (arr.Length - 1 - j)));
            }


            Debug.Print(i.ToString());
            return i;

            
        }

        public static int ID3_tagSize(byte[] sizeBytes)
        {
            int i = 0;

            for (int j = 0; j < sizeBytes.Length; j++)
            {
                i += ((int)sizeBytes[j]) * (1 << (7 * (sizeBytes.Length - 1 - j)));
            }


            Debug.Print(i.ToString());
            return i;


        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);

        }

    }
}
