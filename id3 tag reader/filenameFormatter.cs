using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    class filenameFormatter
    {

        //assumption: ALL delims will have the same length
        //idea: search for delims, then replace delims with corresponding replaceText
        public static string formatFilename(string format, Dictionary<string,string> delimsToReplace, int delimLength, int tracks, string track_delim)
        {
            string formattedFilename = "";
            int pos = 0;
            string currSubstring = "";

            if (format.Length == 0) throw new Exception("Enter a format.");

            for (pos = 0; pos < format.Length - delimLength + 1; pos++)
            {
                currSubstring = format.Substring(pos, delimLength);
                if(delimsToReplace.ContainsKey(currSubstring)) 
                {
                    if (currSubstring != track_delim)
                    {
                        formattedFilename += delimsToReplace[currSubstring];
                        pos++;
                        if (pos == format.Length - 2) currSubstring = format.Substring(pos, delimLength);
                    }
                    else
                    {
                        int track_id_length = 0;
                        int n = 1;
                        while (tracks >= n)
                        {
                            n *= 10;
                            track_id_length++;
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.Capacity = track_id_length;

                        while (sb.Length < sb.Capacity - delimsToReplace[currSubstring].Length) sb.Append("0");
                        sb.Append(delimsToReplace[currSubstring]);

                        formattedFilename += sb.ToString();

                        pos++;
                        if (pos == format.Length - 2) currSubstring = format.Substring(pos, delimLength);
                    }
                }
                else formattedFilename += format[pos].ToString();
            }

            if (!delimsToReplace.ContainsKey(currSubstring)) formattedFilename += format[format.Length - 1];
            



            return formattedFilename;

        }

        public static bool isValidFilename(string filename)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

            for (int i = 0; i < filename.Length; i++)
            {
                if (invalidChars.Contains<char>(filename[i])) return false;
            }

            return true;
        }


        
    }
}
