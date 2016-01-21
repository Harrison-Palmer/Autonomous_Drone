using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Main_Menu.xaml
    /// </summary>
    /// 


    public partial class Main_Menu : Window
    {

        bool isSearching = false;
        string myImage = "C:\\Users\\hpalmer\\Source\\Repos\\Drone_ui\\WPFLogin-master\\user-1-glyph-icon_MkuBPp8O.png";

        public Main_Menu()
        {
            InitializeComponent();
        }      

    private void Home_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("test.xaml", UriKind.Relative));
        }

        //mouse hover - home
        private void Home_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var newW = new login();
            newW.Show();

            this.Close();
        }

        //
        private void Start_Search_Click(object sender, RoutedEventArgs e)
        {
            FTPImageTransfer transfer = null;
            if (!isSearching)
            {
                //shows continue searching button
                Continue_Searching.Visibility = Visibility.Visible;
                Start_Search.Content = "New Search";

                //makes filepicker window object
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //Only allows Images
                openFileDialog.Filter = "Image Files (*.png, *.jpg,*.bmp)|*.png;*.jpg;*.bmp";
                openFileDialog.FilterIndex = 1;
                //sets default directory when opening
                openFileDialog.InitialDirectory = @"C:\";
                //Title of image picker window
                openFileDialog.Title = "Select Image of person to be searched for";

                bool? click_ok = openFileDialog.ShowDialog();

                if (click_ok == true)
                {
                    // txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
                    transfer = new FTPImageTransfer("ftp://192.168.168.1", "drone", "NEVERAGAIN");
                    string data = "";//DateTime.Now.ToString();
                    string name = "ui_image" + data + ".png"; // + DateTime.Now;
                    //uploads image with name of ui_Image and the date

                    //will set the image to the selected one
                    myImage = openFileDialog.FileName;

                   // transfer.Upload(openFileDialog.FileName, name);
                    //comms.SendImage(name);

                    //sets imagebox with image
                     search_for_image.Source = setImage(); // Displays initial image on UI

                    RetrieveImage();

                    MessageBoxResult result =  MessageBox.Show("Is this the person you were looking for?", "Person Confirmation", MessageBoxButton.YesNoCancel);
                 
                    if (result == MessageBoxResult.Yes )
                    {
                        // comms.SendStop(); // stop the drone?


                    }


                    else if (result == MessageBoxResult.No)
                    {
                        MessageBoxResult keepsearching = MessageBox.Show("Would you like to keep searching?", "Continue searching", MessageBoxButton.YesNoCancel);

                        if (keepsearching == MessageBoxResult.Yes)
                        {

                            person_found.Source = null; // Sets found image to null
                            // Drone code?
                            //  comms.SendStart();

                        }

                        else if (keepsearching == MessageBoxResult.No)
                        {
                            // comms.SendStop();

                           
                        }

                        
                    }

   

                
                }
            }
            else
            {
                isSearching = true;

            }
        }

        void RetrieveImage()
        {
            //  UI_Network Network = new UI_Network();

            // myImage = @"K:\Senior\ariana.jpg";


            // Test File Path.. replace with path from Networks

            string filePath2 = @"K:\Senior\ariana.jpg";

            Uri fileUri2 = new Uri(filePath2);

            BitmapImage bitmapSource2 = new BitmapImage();
            bitmapSource2.BeginInit();
            bitmapSource2.CacheOption = BitmapCacheOption.None;
            bitmapSource2.UriSource = fileUri2;
;            bitmapSource2.EndInit();

            person_found.Source = bitmapSource2;

            

        }

        BitmapImage setImage()
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(myImage);
            b.EndInit();
            return b;
        }

        private void Stop_Button1_Click(object sender, RoutedEventArgs e) // Button controlling Network
        {

            isSearching = false;

            //RetrieveImage();



            //// Resets images
            person_found.Source = null;
            search_for_image.Source = null;



        }

        //private static UI_Network comms = new UI_Network();

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              //  comms.SendStart();
            }
           catch
            {
               
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              // comms.SendStop();
            }
            catch
            {
                Console.Write("");
            }
        }

        private void KillSwitch_Click(object sender, RoutedEventArgs e)
        {
           //  comms.SendKill();
        }
        
        private void Can_Start()
        {
            if (isSearching == false)
            {
                Start_Button.IsEnabled = false;
            }
            else
                Start_Button.IsEnabled = true;
        }
    }
}
