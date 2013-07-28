


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
        public Options_Window()
        {
            InitializeComponent();


            
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
                if (application_settings.addDirectory(dialog.FileName))
                {
                    directory_list.ItemsSource = application_settings.getDirectoryNames();
                }
            }


            this.Activate();          
        }

        private void remove_dir_Click(object sender, RoutedEventArgs e)
        {
            if (directory_list.SelectedItem != null)
            {

                application_settings.removeDirectory(directory_list.SelectedItem.ToString());
                directory_list.Items.Remove(directory_list.SelectedItem);

            }
        }

        private void save_settings_Click(object sender, RoutedEventArgs e)
        {
            application_settings.saveSettings();
        }

        private void delim_box_changed(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length > 1) ((TextBox)sender).Text = ((TextBox)sender).Text[0].ToString();

            //Here we update the settings based on the textbox changed.

            if (sender == artist_delim_box)
            {
                application_settings.Artist_delim = "%" + ((TextBox)sender).Text;
                
            }

            if (sender == album_delim_box)
            {
                application_settings.Album_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == track_delim_box)
            {
                application_settings.Track_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == title_delim_box)
            {
                application_settings.Title_delim = "%" + ((TextBox)sender).Text;

            }


            if (sender == year_delim_box)
            {
                application_settings.Year_delim = "%" + ((TextBox)sender).Text;

            }



            //If any delimiter would be duplicated, we null the old delimiter


            if (application_settings.Album_delim == ((TextBox)sender).Text && sender != album_delim_box)
            {

                album_delim_box.Text = "";
                application_settings.Album_delim = "";
            }
            if (application_settings.Artist_delim == ((TextBox)sender).Text && sender != artist_delim_box)
            {

                artist_delim_box.Text = "";
                application_settings.Artist_delim = "";
            }
            if (application_settings.Track_delim == ((TextBox)sender).Text && sender != track_delim_box)
            {

                track_delim_box.Text = "";
                application_settings.Track_delim = "";
            }
            if (application_settings.Title_delim == ((TextBox)sender).Text && sender != title_delim_box)
            {

                title_delim_box.Text = "";
                application_settings.Title_delim = "";
            }
            if (application_settings.Year_delim == ((TextBox)sender).Text && sender != year_delim_box)
            {

                year_delim_box.Text = "";
                application_settings.Year_delim = "";
            }


        }

        

        
        

        
    }
}
