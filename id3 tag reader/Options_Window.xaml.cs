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
    public partial class Options_Window : Window
    {
        public Options_Window()
        {
            InitializeComponent();

            

            
        }

        private void add_dir_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dialog.DefaultDirectory = @"C:\Users\Thad\Documents\Programming Practice\id3 tag reader\Test files\";
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


        
        
    }
}
