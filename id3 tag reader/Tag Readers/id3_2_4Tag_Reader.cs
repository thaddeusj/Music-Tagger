using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApplication1.Tag_Readers
{
    /*
     * Reads and ID3 2.4 tag.
     * 
     */

    public class id3_2_4Tag_Reader : ITagReader
    {

        
        
        public Directory_Structure_Classes.MusicFileNode readTag(string path)
        {

            using (BinaryReader b = new BinaryReader(File.Open(path,FileMode.Open)))
            {
                string ID3 = b.ReadChar().ToString() + b.ReadChar() + b.ReadChar();

                if (ID3 != "ID3")
                {
                    //If there is no prepended tag, we must search for an appended tag.
                    //Appended tags must have footers, so we need only consider the last 10 bytes.


                    if (b.BaseStream.Length > 10)
                    {
                        b.BaseStream.Seek(-10, SeekOrigin.End);


                        if ((b.ReadChar().ToString() + b.ReadChar() + b.ReadChar()) != "3DI")
                        {
                            throw new System.FormatException("This is not an ID3 tagged mp3!");
                        }

                        if (b.ReadByte() != 4)
                        {
                            throw new System.FormatException("Not ID3 v2.4");
                        }

                        b.ReadByte();

                        byte flag = b.ReadByte();



                    }

                }
                else
                {
                    if (b.ReadByte() != 4)
                    {
                        throw new System.FormatException("Not ID3 v2.4");
                    }

                    b.ReadByte();

                    byte flag = b.ReadByte();


                }





            }




            return new Directory_Structure_Classes.MusicFileNode(path);
        }

        public bool isValidTag(byte[] tag, byte[] footer = null)
        {
            if (((char)tag[0]).ToString() + (char)tag[1] + (char)tag[2] != "ID3") return false;
            if (tag[3] != 4) return false;
            return true;
        }
    }
}
