using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Threading;

namespace BTDTextureTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public XMLHandler XMLhandler;
        public ImageHandler imageHandler;
        public ConsoleHandler consoleHandler;
        ConsoleContent dc = new ConsoleContent();

        public MainWindow()
        {
            
            InitializeComponent();
            
            // consoleHandler = new ConsoleHandler(Scroller, Scroller);
            XMLhandler = new XMLHandler();
            DataContext = dc;
            imageHandler = new ImageHandler();
            
        }
        private void OnSpiltAllPressed(object sender, RoutedEventArgs e)
        {
            
            VistaFolderBrowserDialog ee = new VistaFolderBrowserDialog();
            ee.ShowDialog();
            if (ee.SelectedPath != "")
            {
                string[] files = Directory.GetFiles(ee.SelectedPath, "*.xml", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (!(files[i].Contains("_jam.xml")))
                    {

                        SpiltTexture(files[i].Replace(".xml", ".png"));
                    }
                }
            }
            else
            {
                Log("Invalid Path selected");
            }

        }
        
        private void OnCombineAllPressed(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog ee = new VistaFolderBrowserDialog();
            ee.ShowDialog();
            if (ee.SelectedPath != "")
            {
            }
            else
            {
                Log("Invalid Path selected");
            }
        }
        private void OnSpiltSinglePressed(object sender, RoutedEventArgs e)
        {
            Log("Waiting for user to select image...");
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //textBox1.Text = filename;
                SpiltTexture(filename);
            }
            else
            {
                Log("Error: No image selected");
            }
        }
        private void OnCombineSinglePressed(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog ee = new VistaFolderBrowserDialog();
            ee.ShowDialog();

            if (File.Exists(ee.SelectedPath + "_jam.xml"))
            {
                Log("Found XML");
                SpriteInformation oldsi = XMLhandler.ImportXML(ee.SelectedPath + "_jam.xml");
                if(oldsi != null)
                {
                    Log("XML file imported");
                    Stopwatch sp = new Stopwatch();
                    sp.Start();
                    List<string> potato = imageHandler.MakeSpriteSheet(ee.SelectedPath, oldsi);
                    sp.Stop();
                    for (int i = 0; i < potato.Count; i++)
                    {
                        Log(potato[i]);
                        
                        
                    }
                    Log("Rebuilt spritehseet in " + sp.Elapsed.ToString(@"mm\:ss"));



                }
            }
            else
            {
                Log("Could not find the _jam.XML for this folder, try spilting the texture");
            }
        }
        private void SpiltTexture(string filepath)
        {
            Log("Selected image: " + filepath);
            string xmlFiledir = filepath.Remove(filepath.Length - 3, 3) + "xml";
            if (File.Exists(xmlFiledir))
            {
                Log("XML file found, importing...");
                SpriteInformation spriteInformation = (XMLhandler.ImportXML(xmlFiledir));
                if (spriteInformation == null)
                {
                    Log("Error: XML file isnt valid");

                }
                else
                {
                    Log("XML imported!");
                    List<string> output = new List<string>();
                    bool imgwork = imageHandler.SpiltImage(spriteInformation, filepath, output);
                    for (int i = 0; i < output.Count; i++)
                    {
                        Log(output[i]);


                    }
                    if (imgwork)
                    {
                        Log("Image successfully split!");

                        string[] potato = filepath.Split('\\');
                        string pathe = filepath.Remove(filepath.Length - (potato[potato.Length - 1].Length), (potato[potato.Length - 1].Length));
                        pathe += @"Spilt Textures\" + spriteInformation.FrameInformation.Name + "_jam.xml";
                        XMLhandler.ExportXML(pathe, spriteInformation);
                    }
                    else
                    {
                        Log("Error: Image Spilting Failed");
                    }
                }
            }
            else
            {
                Log("Error: Image does not have a XML file");
            }
        }
        public void Log(string text)
        {
            dc.ConsoleInput = text;
            dc.RunCommand();
            Scroller.ScrollToBottom();
        }
    }

}
