using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Directory_Structure_Classes;
using System.IO;

namespace WpfApplication1.Tag_Readers
{
    public class Tag_Decider
    {

        public delegate MusicFileNode tag_read_function(string path, byte[] tag, byte[] footer = null);


        public static tag_read_function tag_decider(string path)
        {

            tag_read_function tr = null;

            if (Path.GetExtension(path) == "mp3") tr = mp3_tag_decider(path);


            return tr;
        }



        public static tag_read_function mp3_tag_decider(string path)
        {

            tag_read_function tr = null;



            //if (id3_2_4Tag_Reader.isValidTag(header)) tr = id3_2_4Tag_Reader.readTag;
            
          

            return tr;


        }


    }
}
