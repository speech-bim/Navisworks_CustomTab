using System;
using System.IO;
using System.Text;
using System.Xml;

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
        private Boolean document_hasChoosed = false;
        private Boolean txtFile_hasChoosed = false;



        public OpenPluginWindow()
        {
            InitializeComponent();
            // If textfile exists read it, else we will create it later
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
                        document_hasChoosed = true;
                    }
                    else
                    {
                        throw new System.ArgumentException(" Формат файла должен быть '.nwc' или '.nwd' \n " +
                                                           " The extension of selected file has to be '.nwc' or '.nwd' ");
                    }
                }
            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show( exp.Message, "Invalid Operation Exception" );
            }

            catch (ArgumentException exp)
            {
                MessageBox.Show(exp.Message, "Argument Exception");
            }
            catch (Exception exp)
            {
                MessageBox.Show( exp.Message, "Error");
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
                if (Dir_TextFile == "c:\\")
                {
                    openFileDialog.InitialDirectory = Dir_Document;
                }
                else
                {
                    openFileDialog.InitialDirectory = Dir_TextFile;
                }
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
                        txtFile_hasChoosed = true;
                    }
                    else
                    {
                        throw new System.ArgumentException(" Формат файла должен быть '.txt' \n " +
                                                           " The extension of selected file has to be '.txt' ");
                    }
                }
            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show(exp.Message, "Invalid Operation Exception");
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show( exp.Message, "Argument Exception ");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error");
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
                if(txtFile_hasChoosed && document_hasChoosed)
                {
                    // Open File Dialog to choose the directory of the output file
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (Dir_SavedFile == "c:\\")
                    {
                        saveFileDialog.InitialDirectory = Dir_Document;
                    }
                    else
                    {
                        saveFileDialog.InitialDirectory = Dir_SavedFile;
                    }

                    saveFileDialog.Filter = "Navisworks files (*.nwd)|*.nwd|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;

                    int returned_status = 0;
                    if (saveFileDialog.ShowDialog() == true)
                    {

                        // Start timer to calculate  the execution time
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        // Get extension of the entered filename
                        string ext = Path.GetExtension(saveFileDialog.FileName);

                        // Check if user entered the extension of filename or not 
                        if (ext == ".nwd")
                        {
                            // Update the Form to inform the user that the Plugin has started
                            proBar.Value = 1;
                            proBar_label.Foreground = Brushes.Red;
                            proBar_label.Content = "Файл обрабатывается";
                            fullpath_SavedFile = saveFileDialog.FileName;
                        }
                        else if (ext == "")
                        // If the user didn't enter the extension of the output file, add it
                        {
                            // Update the Form to inform the user that the Plugin has started
                            proBar.Value = 1;
                            proBar_label.Foreground = Brushes.Red;
                            proBar_label.Content = "Файл обрабатывается";
                            fullpath_SavedFile = saveFileDialog.FileName + ".nwd";
                        }
                        else
                        {
                            // Update the Form to inform the user that the Plugin has started
                            proBar.Value = 0;
                            proBar_label.Foreground = Brushes.Red;
                            proBar_label.Content = "Выберите правильный формат файла";
                            throw new System.ArgumentException(" Формат файла должен быть '.nwd' \n " +
                                                               " The extension of selected file has to be '.nwd' ");
                        }

                        // The path of directory where to save outputfile 
                        Dir_SavedFile = Path.GetDirectoryName(saveFileDialog.FileName);

                        // Start another thread to run the Plugin
                        await Task.Run(async () => {
                            try
                            {
                                // Start Navisworks
                                navisworksApplication = new NavisworksApplication();
                                //disable progress whilst we do this procedure
                                navisworksApplication.DisableProgress();
                                // Enter Document to Navisworks
                                navisworksApplication.OpenFile(fullPath_Document);
                                // Run Plugin
                                returned_status = navisworksApplication.ExecuteAddInPlugin("CustomTab_main.Speech", fullPath_TextFile, "Yes");

                                // Save the output file
                                navisworksApplication.SaveFile(fullpath_SavedFile);
                                //Re-enable progress
                                navisworksApplication.EnableProgress();
                            }
                            catch (AutomationException exp)
                            {
                                //An error occurred, display it to the user
                                MessageBox.Show(exp.Message, "Error ");
                            }

                            catch (AutomationDocumentFileException exp)
                            {
                                //An error occurred, display it to the user
                                MessageBox.Show(exp.Message, "Error ");
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show( exp.Message, "Error ");
                            }
                            finally
                            {
                                // Close Navisworks
                                if (navisworksApplication != null)
                                {
                                    navisworksApplication.Dispose();
                                }
                            }
                        });
                        //MessageBox.Show("Returned Value :" + return_status);
                        if (returned_status == 1)
                        {
                            // Update the Form to inform the user that the Plugin has finished
                            proBar.Value = 3;
                            // Execution time of Plugin
                            watch.Stop();
                            long t_seconds = watch.ElapsedMilliseconds / 1000;
                            long time_minutes = t_seconds / 60;
                            long time_seconds = t_seconds - (time_minutes * 60);
                            ExecutionTime_label.Content = $"Время Выполнения: {time_minutes} m : {time_seconds} s";

                            proBar_label.Foreground = Brushes.Green;
                            proBar_label.Content = "Файл готов!";
                        }
                        else
                        {
                            watch.Stop();
                            // Update the Form to inform the user that the Plugin has failed
                            proBar.Value = 0;

                            proBar_label.Foreground = Brushes.Red;
                            proBar_label.Content = "Ошибка... Процесс не удался";

                        }

                    }
                }
                else
                {
                    MessageBox.Show(" First you have to choose Naviswork document and paramters file \n" +
                                    " Cначала вам нужно выбрать файл Naviswork и текстовый файл с параметрами","Error");
                }
                

            }
            catch (InvalidOperationException exp)
            {
                MessageBox.Show( exp.Message, "Invalid Operation Exception" );
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show(exp.Message, "Argument Exception" );
            }
            catch (Exception exp)
            {
                MessageBox.Show( exp.Message, "Exception" );
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