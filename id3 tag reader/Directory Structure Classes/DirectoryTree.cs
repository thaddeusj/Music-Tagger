﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WpfApplication1.Directory_Structure_Classes
{

    /* DirectoryTree stores a directory structure as a tree:
     *      It has two types of children:
     *          -subDirs:   DirectoryTree children: represent subdirectories
     *          -(Optional)files:     FileNode children: represent files in the directory 
     * 
     * Behavior:
     *      A DirectoryTree should be able to:
     *          -add and remove FileNodes from files
     *          -add and remove DirectoryTrees from subdirs
     *                    
     *          -save the directory path
     *          -sort subdirectories by path
     *          
     * 
     *      We would like a DirectoryTree to automatically add subdirectories,
     *      and have those subdirectory add their subdirectories, etc.
     *          
     * Specific subclasses of DirectoryTree should be used for different types of files,
     * but subdirectory behavior can most likely be done in general
     * 
     * Because each use of DirectoryTree may involve working with different file types, each subclass should implement a function
     * to differentiate between admissible and non-admissible files, hence we require an abstract selection function.
     * In the case where we wish to ignore all files, isFileAdmissible should simply return false.
     * 
     */ 

    abstract public class DirectoryTree : IComparable<DirectoryTree>
    {
        private ObservableCollection<DirectoryTree> subdirs;
        public ObservableCollection<DirectoryTree> Subdirs
        {
            get { return subdirs; }
            set { subdirs = value; }

        }

        private DirectoryTree parent;
        public DirectoryTree Parent
        {
            set { parent = value; }
            get { return parent; }
        }

        private string directoryPath;

        public string DirectoryPath
        {
            set { directoryPath = value; }
            get { return directoryPath; }
        }

        public DirectoryTree()
        {
            subdirs = new ObservableCollection<DirectoryTree>();
            directoryPath = "";
        }

        public DirectoryTree(string path)
        {
            subdirs = new ObservableCollection<DirectoryTree>();
            directoryPath = path;
            parent = null;

            //each directorytree should add subdirectories in constructor

        }

        public void addSubDir(DirectoryTree newSubDir)
        {
            if (!subdirs.Contains(newSubDir)) { subdirs.Add(newSubDir); }
            newSubDir.parent = this;
        }




        //Alphabetical comparison of directory paths.
        public int CompareTo(DirectoryTree d)
        {
            return this.DirectoryPath.CompareTo(d.DirectoryPath);
        }


        //Abstract Functions
        abstract public bool isFileAdmissible(string filePath);
        abstract public void getFiles();

       
    }
}
