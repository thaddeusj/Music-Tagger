using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Directory_Structure_Classes
{

    /*  FileNode stores file data for a DirectoryTree directory structure
     * 
     *  
     * 
     *  Behavior: a FileNode should
     *                  -store the file path
     *                  -implement comparison of files by path
     *                                    
     *  Justification for general implementation, as opposed to simply storing the paths in a list of strings:
     *      The goal is to have the directory structure store the metadata as well.
     *      Hence, a subclass of FileNode should only be used if the metadata of the files is being stored for use. 
     * 
     */
    
    abstract class FileNode : IComparable<FileNode>
    {
        public string FileName { set; get; }

        public FileNode(string path)
        {
            FileName = path;
        }

        public int CompareTo(FileNode f)
        {
            return FileName.CompareTo(f.FileName);
        }
    }
}
