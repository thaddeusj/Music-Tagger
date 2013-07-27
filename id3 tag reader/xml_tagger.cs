using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace WpfApplication1
{
    public static class xml_tagger
    {


        public static void xml_gen(string directory, Dictionary<string, tagger> tags)
        {

            if (Directory.Exists(directory))
            {
                using (StreamWriter writer = new StreamWriter(directory + "\\" + "albumtags.xml"))
                {
                    string xml = "";

                    XElement elements = new XElement("directory",
                                            tags.Keys.Select(key => new XElement("file", new XAttribute("path", tags[key].File_Name_Original),
                                                                                                     new XElement("artist", tags[key].Artist),
                                                                                                     new XElement("album", tags[key].Album),
                                                                                                     new XElement("year", tags[key].Year),
                                                                                                     new XElement("track", tags[key].Track),
                                                                                                     new XElement("title", tags[key].Title))));

                    xml = elements.ToString();

                    writer.Write(xml);
                }
            }
        }

        public static void xml_read(string directory, out Dictionary<string, tagger> tags)
        {





            tags = new Dictionary<string, tagger>();
        }



    }
}
