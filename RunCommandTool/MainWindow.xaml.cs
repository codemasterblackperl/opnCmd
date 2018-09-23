using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RunCommandTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitFunction();
        }

        ObservableCollection<string> _directories;

        private void BtnAddDirectory(object sender, RoutedEventArgs e)
        {
            AddDirectory(TxtDirectory.Text.Trim());
        }

        private void BtnOpenPowershellClick(object sender, RoutedEventArgs e)
        {
            using(Process p=new Process())
            {
                p.StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    WorkingDirectory = _directories[DgDirs.SelectedIndex]
                };
                p.Start();
            }
        }

        private void BtnBrowseDirectory(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            //var res = dlg.ShowDialog();
            //if (res != System.Windows.Forms.DialogResult.OK)
            //    return;
            var dlg = new CommonOpenFileDialog
            {
                Title = "Select Folder",
                IsFolderPicker = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),

                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            var res = dlg.ShowDialog();
            if (res != CommonFileDialogResult.Ok)
                return;

            TxtDirectory.Text = dlg.FileName;
        }

        #region Methods

        private void InitFunction()
        {
            _directories = new ObservableCollection<string>();

            if (!File.Exists("direct"))
            {
                 var stream=File.Create("direct");
                stream.Close();
            }
            var directories=File.ReadAllLines("direct");
            foreach (var dir in directories)
                _directories.Add(dir);

            DgDirs.ItemsSource = _directories;
        }

        private void AddDirectory(string location)
        {
            _directories.Add(location);
            File.WriteAllLines("direct", _directories.ToArray());
        }

        #endregion

       
    }
}
