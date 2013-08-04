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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string format;
        public string Format
        {
            get { return format; }
            set
            {
                if (format != value)
                {
                    format = value;
                }
            }
        }
        private string directory;
        private string[] files;

        public tagger t;

        Dictionary<string, tagger> tags;

        public MainWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.WindowState = System.Windows.WindowState.Maximized;

            textBox1.IsReadOnly = true;
            tags = new Dictionary<string, tagger>();

            xml_gen_button.IsEnabled = false;

            application_settings.Initialise();
            treeView1.ItemsSource = application_settings.directoryForest.Subdirs;
        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
        //    fd.FileName = "song"; // Default file name
        //    fd.DefaultExt = ".mp3"; // Default file extension
        //    fd.Filter = ".mp3|*.mp3"; // Filter files by extension 

        //    Nullable<bool> res = fd.ShowDialog();

        //    if (res == true)
        //    {
        //        textBox1.Text = fd.FileName;

        //    }
        //}

        //private void readButton_Click(object sender, RoutedEventArgs e)
        //{
        //    label1.Content = "";

        //    if (File.Exists(textBox1.Text))
        //    {
        //        t = new tagger();
        //        t.read_ID3Tag(textBox1.Text);

        //        label1.Content = "Title: " + t.Title + "\n";
        //        label1.Content += "Album: " + t.Album + "\n";
        //        label1.Content += "Artist: " + t.Artist + "\n";
        //        label1.Content += "Track: " + t.Track + "\n";
        //        label1.Content += "Year: " + t.Year + "\n";

        //    }
        //}

        private void filename_formatter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Format = ((TextBox)e.Source).Text;
            filename_viewer.Content = "";

            if (textBox1.Text != "")
            {
                foreach (string key in tags.Keys)
                {
                    if (tags[key] != null)
                    {
                        try
                        {
                            tags[key].File_Name_Formatted =directory + "\\" + filenameFormatter.formatFilename(Format, tags[key].delims, tags[key].delimLen,tags.Keys.Count,tagger.track_delim);
                            tags[key].File_Name_Formatted += System.IO.Path.GetExtension(tags[key].File_Name_Original);
                            filename_viewer.Content += System.IO.Path.GetFileName( tags[key].File_Name_Formatted) + "\n";
                        }
                        catch (Exception ex)
                        {
                            filename_viewer.Content = ex.Message;
                        }

                    }
                }
            }
            
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            t = new tagger();
            textBox1.Text = "";
            Format = "";
            filename_viewer.Content = "";
            filename_formatter.Text = "";

            file_listview.Items.Clear();

            tags = new Dictionary<string, tagger>();
            xml_gen_button.IsEnabled = false;
        }

        private void filerename_Click(object sender, RoutedEventArgs e)
        {
            if (t != null)
            {
                if (File.Exists(textBox1.Text) && filenameFormatter.isValidFilename(t.File_Name_Formatted))
                {
                    File.Move(textBox1.Text, System.IO.Path.GetDirectoryName(textBox1.Text) + @"\" + t.File_Name_Formatted);
                    textBox1.Text = System.IO.Path.GetDirectoryName(textBox1.Text) + @"\" + t.File_Name_Formatted;
                }
            }

            bool can_rename_all = true;
            bool names_are_unique = true;


            if (tags != null && tags.Keys.Count > 0)
            {
                foreach (string key in tags.Keys)
                {
                    if (!File.Exists(tags[key].File_Name_Original) || !filenameFormatter.isValidFilename(System.IO.Path.GetFileName( tags[key].File_Name_Formatted)))
                    {
                        
                        can_rename_all = false;

                    }

                }
            }

            if (can_rename_all == true)
            {
                string[] names = tags.Keys.Select(key => tags[key].File_Name_Formatted).ToArray();

                if (names.Distinct().ToArray().Length != names.Length)
                {
                    names_are_unique = false;
                }
            }

            if (can_rename_all == true  && names_are_unique == true)
            {
                foreach (string key in tags.Keys)
                {
                    File.Move(tags[key].File_Name_Original, tags[key].File_Name_Formatted);
                    tags[key].File_Name_Original = tags[key].File_Name_Formatted;



                }

                xml_gen_button_Click(null, new RoutedEventArgs());

            }

            if (can_rename_all == false)
            {
                MessageBox.Show("Cannot rename some of the files. Make sure your filenames contain only valid characters, and that all of the files exist.");
                
            }
            if (names_are_unique == false)
            {
                MessageBox.Show("Some of your files will have the same name after renaming. Make sure your filenames are unique.");
            }
        }

        private void folder_browser_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dialog.DefaultDirectory = @"C:\Users\Thad\Documents\Programming Practice\id3 tag reader\Test files\";
            dialog.IsFolderPicker = true;

            bool proper_loading = false;

            tags = new Dictionary<string,tagger>();

            if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                directory = dialog.FileName;
                textBox1.Text = directory;

                if(Directory.Exists(directory))
                {
                    xml_gen_button.IsEnabled = true;

                    files = Directory.GetFiles(directory);

                    for (int i = 0; i < files.Length; i++)
                    {
                        if( System.IO.Path.GetExtension(files[i]) == ".mp3")file_listview.Items.Add(System.IO.Path.GetFileName(files[i]));

                    }



                    if (File.Exists(directory + "\\" + "albumtags.xml"))
                    {
                        XElement elements = XElement.Load(directory + "\\" + "albumtags.xml");

                        //filename_viewer.Content = elements.ToString();

                        IEnumerable<XElement> xml_files = elements.Elements("file").Where(file => file.Name == "file");
                        //work on this

                        foreach (XElement xml_file in xml_files)
                        {
                            tags.Add(xml_file.Attribute("path").Value.ToString(), new tagger(xml_file.Element("title").Value.ToString(),
                                                                                       xml_file.Element("album").Value.ToString(),
                                                                                       xml_file.Element("artist").Value.ToString(),
                                                                                       xml_file.Element("year").Value.ToString(),
                                                                                       xml_file.Element("track").Value.ToString(),
                                                                                       xml_file.Attribute("path").Value.ToString(),
                                                                                       xml_file.Attribute("path").Value.ToString(),
                                                                                       ((uint)xml_file.Attribute("path").Value.ToString().GetHashCode()).ToString()
                                                                                       ));
                            
                        }

                        foreach (string file in file_listview.Items)
                        {
                            if (!tags.ContainsKey(directory + "\\" +file))
                            {
                                proper_load();

                                proper_loading = true;
                                break;
                            }
                        }

                        if (proper_loading == false)
                        {
                            foreach (string key in tags.Keys)
                            {
                                if (!file_listview.Items.Contains(System.IO.Path.GetFileName( key)))
                                {
                                    proper_load();
                                    break;

                                }

                            }

                        }

                        foreach (string s in file_listview.Items)
                        {
                            filename_viewer.Content += s +"\n";

                        }

                    }
                    else
                    {
                        proper_load();
                    }

                    file_listview.DataContext = tags;
                    file_listview.Items.Clear();

                    foreach (string key in tags.Keys)
                    {
                        
                        file_listview.Items.Add(tags[key]);
                        
                    }



                }
            }

            
        }

        public void proper_load()
        {
            tags = new Dictionary<string, tagger>();

            foreach (string s in file_listview.Items)
            {
                if (System.IO.Path.GetExtension(s) == ".mp3")
                {
                    tags.Add(directory + "\\" + s, new tagger());
                    tags[directory + "\\" + s].read_ID3Tag(directory + "\\" + s);
                    tags[directory + "\\" + s].File_Name_Original = directory + "\\" + s;
                    tags[directory + "\\" + s].File_Name_Formatted = directory + "\\" + s;
                    tags[directory + "\\" + s].Unique_id = ((uint)(directory + "\\" + s).GetHashCode()).ToString();

                    

                }

            }

            
        }

        private void xml_gen_button_Click(object sender, RoutedEventArgs e)
        {

            if (tags.Keys.Count > 0)
            {

                xml_tagger.xml_gen(directory, tags);


            }
        }

        private void file_listview_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void menu_quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menu_options_Click(object sender, RoutedEventArgs e)
        {
            
            var options_window = new Options_Window();
            options_window.ShowDialog();
        }

        private void treeView1_Loaded(object sender, RoutedEventArgs e)
        {





        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            file_listview.ItemsSource = ((WpfApplication1.Directory_Structure_Classes.MusicDirectoryTree)((TreeView)sender).SelectedItem).Files;

            int i = 0;
            i++;


        }

        

        

        

        


    }
}


