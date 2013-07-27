using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApplication1
{
    public class FrameInfo
    {
        public string frame_type;
        public int frame_size;
        private BinaryReader b;

        public FrameInfo()
        {
            frame_size = 0;
            frame_type = "";
        }

        public FrameInfo(BinaryReader incomingReader)
        {
            frame_size = 0;
            frame_type = "a";
            b = incomingReader;
        }

        public void strip_info()
        {
            if (b != null)
            {
                Encoding encoding = Encoding.GetEncoding(28591); //ID3 frame IDs are ISO encoded

                frame_type = new string(encoding.GetChars(b.ReadBytes(4)));
                frame_size = tagger.ID3_tagSize(b);
                b.ReadBytes(2);
            }
            else throw new Exception("binaryreader not initialised");

        }

        public void strip_info(byte[] frameHeader)
        {

            if (frameHeader.Length == 10)
            {
                byte[] frameTypeBytes = new byte[4];
                for(int i = 0; i < 4; i++) frameTypeBytes[i] = frameHeader[i];

                byte[] frameSizeBytes = new byte[4];
                for (int i = 4; i < 8; i++) frameSizeBytes[i-4] = frameHeader[i];


                Encoding encoding = Encoding.GetEncoding(28591); //ID3 frame IDs are ISO encoded

                frame_type = new string(encoding.GetChars(frameTypeBytes));
                frame_size = tagger.ID3_tagSize(frameSizeBytes);

                int test = frame_size;
                string test2 = frame_type;


            }
            else
            {

                throw new Exception("improperly formatted frame header");
            }
        }

    }
}
