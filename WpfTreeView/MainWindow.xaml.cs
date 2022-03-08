using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region On Loaded
        /// <summary>
        /// Whent hte item first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get every logical drive on the machine
            foreach (var drive in System.IO.Directory.GetLogicalDrives())
            {
                //Create a new item for it
                var item = new TreeViewItem()
                {
                    //Get the header 
                    Header = drive,
                    //and full path
                    Tag = drive
                };

                //Add a dummy item
                item.Items.Add(null);

                //Listen out foritem being expanded
                item.Expanded += Folder_Expanded;

                //Addit to the main tree view
                FolderView
                    
                    
                    
                    
                    .Items.Add(item);
            }
        }

        #endregion

        #region Folder Expanded

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            //If the item only contains dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //Clear dummy data
            item.Items.Clear();

            //Get full path
            var fullPath = (string)item.Tag;

            #endregion

            #region Get Folders

            //Create a blank list for the directories
            var directories = new List<string>();


            //Try and get the directories from the folder
            //ignoring any issues in the process
            try
            {

                var dirs = Directory.GetDirectories(fullPath);


                if (dirs.Length > 0)
                    directories.AddRange(dirs);

            }
            catch { }

            //For each directory....
            directories.ForEach(directoryPath =>
            {
                //Create directry item
                var subItem = new TreeViewItem()
                {
                    //Set headert as folder name
                    Header = GetFileFolderName(directoryPath),
                    //And add tag as full path
                    Tag = directoryPath
                };

                //Add dummy item so we can expand folder
                subItem.Items.Add(null);

                // handle expanding
                subItem.Expanded += Folder_Expanded;

                //add this item to the parent
                item.Items.Add(subItem);

            });

            #endregion

            #region Get Files

            //Create a blank list for filesa
            var files = new List<string>();

            // try and get files from folders
            //ignoing ny issues doing so

            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    files.AddRange(fs);
            }

            catch { }

            //for each file....
            files.ForEach(filePath =>
           {
               // create file item
               var subItem = new TreeViewItem()
               {
                   //set header as file name
                   Header = GetFileFolderName(filePath),
                   //and tag the full path
                   Tag = filePath
                };

                //Add the item to the parwent
                item.Items.Add(subItem);

            });
           
            #endregion

        }

    #endregion



    #region Helpers


    /// <summary>
    /// find the file or folder namefrom a full path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileFolderName(string path)
                {
                    // if we have no path, return empty
                    if (string.IsNullOrEmpty(path))
                        return string.Empty;

                    //Make all slashes back slashes
                    var normalizedPath = path.Replace('/', '\\');

                    //Find the last bakslash in the path
                    var lastIndex = normalizedPath.LastIndexOf('\\');

            //If we don't find a backslash,  return the path itself
            if (lastIndex <= 0)
                return path;

            //Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        #endregion`
    }
}
