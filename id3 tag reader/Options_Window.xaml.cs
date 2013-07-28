


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication1.Directory_Structure_Classes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 


    /*
     * TODO: implement cancel button.
     *          -ie, store changes temporarily until either applied or window is OK'd out of.
     *          -do not push changes otherwise
     * 
     */

    public partial class Options_Window : Window
    {
        public static string Track_delim { get; set; }
        public static string Title_delim { get; set; }
        public static string Artist_delim { get; set; }
        public static string Album_delim { get; set; }
        public static string Year_delim { get; set; }

        private static List<MusicDirectoryTree> directoryForest;


        public Options_Window()
        {
            InitializeComponent();

            application_settings.copyDirForestOut(out directoryForest);
            
            Track_delim = application_settings.Track_delim;
            Title_delim = application_settings.Title_delim;
            Artist_delim = application_settings.Artist_delim;
            Album_delim = application_settings.Album_delim;
            Year_delim = application_settings.Year_delim;

            year_delim_box.Text = Year_delim[1].ToString();
            track_delim_box.Text = Track_delim[1].ToString();
            album_delim_box.Text = Album_delim[1].ToString();
            artist_delim_box.Text = Artist_delim[1].ToString();
            title_delim_box.Text = Title_delim[1].ToString();


        }

        private void add_dir_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            
            #if DEBUG
                dialog.DefaultDirectory = @"C:\Users\Thad\Documents\Programming Practice\id3 tag reader\Test files\";
            #elif !DEBUG
                dialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            #endif
                        
            dialog.IsFolderPicker = true;

            
            if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                if (addDirectory(dialog.FileName))
                {
                    directory_list.ItemsSource = directoryForest.Select(x => x.DirectoryPath);
                }
            }


            this.Activate();          
        }

        private void remove_dir_Click(object sender, RoutedEventArgs e)
        {
            if (directory_list.SelectedItem != null)
            {

                directoryForest = directoryForest.Where(x => x.DirectoryPath != directory_list.SelectedItem.ToString()).ToList();
                //application_settings.removeDirectory(directory_list.SelectedItem.ToString());
                directory_list.Items.Remove(directory_list.SelectedItem);

            }
        }

        private void save_settings_Click(object sender, RoutedEventArgs e)
        {

            application_settings.Artist_delim = Artist_delim;
            application_settings.Album_delim = Album_delim;
            application_settings.Title_delim = Title_delim;
            application_settings.Track_delim = Track_delim;
            application_settings.Year_delim = Year_delim;

            application_settings.copyDirForest(directoryForest);


            application_settings.saveSettings();
            this.Close();
        }

        private void delim_box_changed(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length > 1) ((TextBox)sender).Text = ((TextBox)sender).Text[((TextBox)sender).Text.Length-1].ToString();
            if (((TextBox)sender).Text == ";") ((TextBox)sender).Text = "";

            ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;

            //Here we update the settings based on the textbox changed.

            if (sender == artist_delim_box)
            {
                Artist_delim = "%" + ((TextBox)sender).Text;
                
            }

            if (sender == album_delim_box)
            {
                Album_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == track_delim_box)
            {
                Track_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == title_delim_box)
            {
                Title_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == year_delim_box)
            {
                Year_delim = "%" + ((TextBox)sender).Text;

            }



            //If any delimiter would be duplicated, we null the old delimiter


            if (Album_delim == "%" + ((TextBox)sender).Text && sender != album_delim_box)
            {

                album_delim_box.Text = "";
                Album_delim = "";
            }
            if (Artist_delim == "%" + ((TextBox)sender).Text && sender != artist_delim_box)
            {

                artist_delim_box.Text = "";
                Artist_delim = "";
            }
            if (Track_delim == "%" + ((TextBox)sender).Text && sender != track_delim_box)
            {

                track_delim_box.Text = "";
                Track_delim = "";
            }
            if (Title_delim == "%" + ((TextBox)sender).Text && sender != title_delim_box)
            {

                title_delim_box.Text = "";
                Title_delim = "";
            }
            if (Year_delim == "%" + ((TextBox)sender).Text && sender != year_delim_box)
            {

                year_delim_box.Text = "";
                Year_delim = "";
            }


        }

        //Same method as in application settings
        // Point is to add directory trees to the forest, without duplication
        // This method disallows adding a tree, if a supertree is already in the forest
        // and replaces a subtree with any supertree one adds.

        public static bool addDirectory(string path)
        {
            if (directoryForest == null || !(directoryForest.Select(x => x.DirectoryPath).Contains(path)))
            {
                bool isSubDir = false;

                foreach (MusicDirectoryTree dt in directoryForest)
                {
                    if (path.Contains(dt.DirectoryPath))
                    {
                        isSubDir = true;
                        break;
                    }

                }


                if (!isSubDir)
                {


                    var tempdirFor = directoryForest.ToList();

                    foreach (MusicDirectoryTree dt in directoryForest)
                    {
                        if (dt.DirectoryPath.Contains(path))
                        {
                            tempdirFor.Remove(dt);
                        }

                    }

                    directoryForest = tempdirFor;

                    directoryForest.Add(new MusicDirectoryTree(path));

                    return true;
                }

                else return false;
            }

            return false;

        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void default_settings_Click(object sender, RoutedEventArgs e)
        {
            Track_delim = application_settings.def_track_delim;
            Album_delim = application_settings.def_album_delim;
            Artist_delim = application_settings.def_artist_delim;
            Title_delim = application_settings.def_title_delim;
            Year_delim = application_settings.def_year_delim;


            year_delim_box.Text = Year_delim[1].ToString();
            track_delim_box.Text = Track_delim[1].ToString();
            album_delim_box.Text = Album_delim[1].ToString();
            artist_delim_box.Text = Artist_delim[1].ToString();
            title_delim_box.Text = Title_delim[1].ToString();
        }
        

        
    }
}
