using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Directory_Structure_Classes;

namespace WpfApplication1.Tag_Readers
{

    /*
     * Tag Readers should be able to read a music file tag, and return in the form of a music file node
     *  and verify that a header is a valid header for that type of tag.
     */


    interface ITagReader
    {
        MusicFileNode readTag(string path);
        bool isValidTag(byte[] tag, byte[] footer = null);
        

    }
}
