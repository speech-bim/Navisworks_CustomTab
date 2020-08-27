using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Autodesk.Navisworks.Api.Automation;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CustomTab
{
    public partial class OpenPluginWindow : Window
    {
        // Default paths for directories;
        private string Dir_Document = "c:\\";
        private string Dir_TextFile = "c:\\";
        private string Dir_SavedFile = "c:\\";
        private string fullPath_Document = "";
        private string fullPath_TextFile = ""; 
        private string fullpath_SavedFile = "";

        private string File_paths = $@"C:\Users\{Environment.UserName}\File_paths.txt";

        public OpenPluginWindow()
        {
            InitializeComponent();
            // If textfile exists read it, else we will create it later
            if(File.Exists(File_paths))
            {
                // Restore paths of Directories
                using (StreamReader sr = new StreamReader(File_paths))
                {
                    Dir_Document = sr.ReadLine();
                    Dir_TextFile = sr.ReadLine();
                    Dir_SavedFile = sr.ReadLine();
                }
            }
        }

        private void Select_Document(object sender, EventArgs e)
        {
            try
            {
                // Reset values of form
                proBar.Value = 0; 
                // Reset the progress bar value
                ExecutionTime_label.Content = "";
                proBar_label.Content = "";

                // Open File Dialog to load Navisworks Document
                OpenFileDialog openFileDialog = new OpenFileDialog();               
                openFileDialog.InitialDirectory = Dir_Document;
                openFileDialog.Filter = "Navisworks files (*.nwc,*.nwd)|*.nwc;*.nwd|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    // Check the extension of the selected document 
                    string ext = Path.GetExtension(openFileDialog.FileName);
                    if (ext == ".nwc" || ext == ".nwd")
                    {
                        // Get the path of selected document to save it later
                        Dir_Document = Path.GetDirectoryName(openFileDialog.FileName);
                        fullPath_Document = openFileDialog.FileName;
                        Doc_label.Content = fullPath_Document;
                    }
                    else
                    {
                        throw new System.ArgumentException("The extension of selected file has to be (.nwc or .nwd) ");
                    }
                }
            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show("InvalidOperationException: " + exp.Message);
            }

            catch (ArgumentException exp)
            {
                MessageBox.Show("ArgumentException: " + exp.Message);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error : " + exp.Message);
            }
        }
        private void Select_TextFile(object sender, EventArgs e)
        {
            try
            {
                // Reset values of form
                proBar.Value = 0;
                // Reset the progress bar value
                ExecutionTime_label.Content = "";
                proBar_label.Content = "";

                // Open File Dialog to load textfile of the paramters
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = (string)Dir_TextFile;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    // Check the extension of the selected file
                    string ext = Path.GetExtension(openFileDialog.FileName);
                    if (ext == ".txt")
                    {
                        // Get the path of selected file to save it later
                        Dir_TextFile = Path.GetDirectoryName(openFileDialog.FileName);
                        fullPath_TextFile = openFileDialog.FileName;
                        textFile_label.Content = fullPath_TextFile;
                    }
                    else
                    {
                        throw new System.ArgumentException("The extension of selected file has to be (.txt) ");
                    }
                }
            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show("InvalidOperationException: " + exp.Message);
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show("ArgumentException: " + exp.Message);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error : " + exp.Message);
            }

        }
        private async void Run_Plugin(object sender, RoutedEventArgs e)
        {
            NavisworksApplication navisworksApplication = null;
            try
            {
                // Reset values of form
                ExecutionTime_label.Content = "";
                proBar_label.Content = "";

                // Open File Dialog to choose the directory of the output file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Dir_SavedFile;
                saveFileDialog.Filter = "Navisworks files (*.nwd)|*.nwd|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Update the Form to inform the user that the Plugin has started
                    proBar.Value = 1;
                    proBar_label.Foreground = Brushes.Red;
                    proBar_label.Content = "Файл обрабатывается";
                    
                    // Start timer to calculate  the execution time
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    // Get extension of the entered filename
                    string ext = Path.GetExtension(saveFileDialog.FileName);

                    // Check if user entered the extension of filename or not 
                    if (ext == ".nwd")
                    {
                        fullpath_SavedFile = saveFileDialog.FileName;
                    }
                    else if (ext == "")
                    // If the user didn't enter the extension of the output file, add it
                    {
                        fullpath_SavedFile = saveFileDialog.FileName + ".nwd";
                    }
                    else
                    {
                        throw new System.ArgumentException("The extension of selected file has to be (.nwd) ");
                    }

                    // The path of directory where to save outputfile 
                    Dir_SavedFile = Path.GetDirectoryName(saveFileDialog.FileName);

                    // Save paths to text file
                    string[] paths = { Dir_Document, Dir_TextFile, Dir_SavedFile };

                    if (File.Exists(File_paths))
                    {
                        File.Delete(File_paths);
                    }
                    File.WriteAllLines(File_paths, paths, Encoding.UTF8);

                    // Start another thread to run the Plugin
                    await Task.Run(async () => {
                        try
                        {
                            // Start Navisworks
                            navisworksApplication = new NavisworksApplication();
                            // Enter Document to Navisworks
                            navisworksApplication.OpenFile(fullPath_Document);
                            // Run Plugin
                            navisworksApplication.ExecuteAddInPlugin("CustomTab_main.Speech", fullPath_TextFile, "Yes");
                            // Save the output file
                            navisworksApplication.SaveFile(fullpath_SavedFile);
                        }
                        catch (Exception exp) {
                            MessageBox.Show("Error: " + exp.Message);
                        }
                    });

                    // Update the Form to inform the user that the Plugin has finished
                    proBar.Value = 3;
                    // Execution time of Plugin
                    ExecutionTime_label.Content = $"Время Выполнения: {watch.ElapsedMilliseconds/1000} s";

                    proBar_label.Foreground = Brushes.Green;           
                    proBar_label.Content = "Файл готов!";
                }

            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show("InvalidOperationException: " + exp.Message);
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show("ArgumentException: " + exp.Message);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception: " + exp.Message);
            }
            finally
            {
                // Close Navisworks
                if (navisworksApplication != null)
                {
                    navisworksApplication.Dispose();
                }
            }
        }
    }
}