using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Directory_Structure_Classes
{
    class MusicFileNode : FileNode
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
            }
        }

        private string album;
        public string Album
        {
            get { return album; }
            set
            {
                album = value;
            }
        }

        private string track;
        public string Track
        {
            get { return track; }
            set
            {
                track = value;
            }
        }

        private string artist;
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
            }
        }

        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                year = value;
            }
        }

        public MusicFileNode(string tit, string alb, string art, string yea, string tra, string path)
            : base(path)
        {
            title = tit;
            album = alb;
            artist = art;
            year = yea;
            track = tra;
        }

        public MusicFileNode(string path) : base(path) { ;}



    }
}
