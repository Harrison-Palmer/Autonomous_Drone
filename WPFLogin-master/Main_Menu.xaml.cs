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
using System.Xml;
using System.Threading.Tasks;
using System.Diagnostics;

//using System.Windows.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Main_Menu.xaml
    /// </summary>
    /// 


    public partial class Main_Menu : Window
    {

        Stopwatch stopwatch = new Stopwatch();

        bool isSearching = false;
        string myImage = "C:\\Users\\hpalmer\\Source\\Repos\\Drone_ui\\WPFLogin-master\\user-1-glyph-icon_MkuBPp8O.png";

        public Main_Menu()
        {
            InitializeComponent();
            Safe_to_Fly();
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

        private void Start_Search_Click(object sender, RoutedEventArgs e)
        {
            //resets the upload fail/pass dialog box
            status_box.Foreground = Brushes.Yellow;
            status_box.Content = "User Selecting target. ";

            status_label.Foreground = Brushes.Yellow;
            status_label.Content = "Standby";

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
                
                //sets variable myImage to the uploaded image
                myImage = openFileDialog.FileName;
                //sets image box to the image path in myImage
                search_for_image.Source = setImage();
                
                try
                {
                    if (click_ok == true)
                    {
                        transfer = new FTPImageTransfer("ftp://192.168.168.1", "drone", "NEVERAGAIN");
                        string data = "";//DateTime.Now.ToString();
                        string name = "ui_image" + data + ".png"; // + DateTime.Now;
                                                                  //uploads image with name of ui_Image and the date

                        transfer.Upload(openFileDialog.FileName, name);
                        //comms.SendImage(name);

                        status_box.Foreground = Brushes.Green;
                        if (Start_Search.Content == "New Search")
                            status_box.Content = "Image succesfully uploaded, Searching for new target. ";
                        else
                            status_box.Content = "Image succesfully uploaded, Drone starting. ";

                        status_label.Foreground = Brushes.Green;
                        status_label.Content = "Active";

                        //starts the timer, if user picks new target timer does not reset
                        if (!(stopwatch.Elapsed.Seconds > 0))
                            stopwatch.Start();
                       // stopwatch.Stop();
                        timer_label.Content = stopwatch.Elapsed;

                    }
                    else
                    {
                        isSearching = true;
                    }
                }
                catch (Exception ex)
                {
                    status_box.Foreground = Brushes.Red;
                    status_box.Content = "Image failed to upload, Drone not started. ";

                    status_label.Foreground = Brushes.Red;
                    status_label.Content = "Inactive";
                }
                
            }            
        }

        private void Stop_Button1_Click(object sender, RoutedEventArgs e)
        {
            isSearching = false;
            
            //reset images
            search_for_image.Source = null;
            person_found.Source = null;

            status_box.Foreground = Brushes.Red;
            status_box.Content= "Drone Stopped. ";

            status_label.Foreground = Brushes.Red;
            status_label.Content = "Inactive";

           // RetrieveImage();
        }

        private void Continue_Searching_Click(object sender, RoutedEventArgs e)
        {
            //TODO - make drone continue searching

            status_box.Foreground = Brushes.Green;
            status_box.Content = "continuing Search. ";
        }

        //private static UI_Network comms = new UI_Network();

        //TODO integrate & remove
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
        //TODO integrate & remove
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

        //forces the drone to stop
        private void KillSwitch_Click(object sender, RoutedEventArgs e)
        {
           //  comms.SendKill();
        }

        void Safe_to_Fly()
        {
            bool connect = true;
            string data = "";
            int value;

            safe_to_fly_status.Foreground = Brushes.Black;

            XmlDocument pullWeather = new XmlDocument();

            try
            {
                pullWeather.Load("http://api.openweathermap.org/data/2.5/weather?zip=03060,us&mode=xml&appid=f95d4be882833be32b011342f0b6abc5");

                data = pullWeather.SelectSingleNode("/current/weather").Attributes[0].InnerText;
            }
            catch (Exception ex)
            {
                connect = false;

                status_box.Foreground = Brushes.Red;
                status_box.Content = "Failed to retrieve weather information. ";

                safe_to_fly_status.Foreground = Brushes.Black;
                safe_to_fly_status.Content = "Unknown";
            }

            Int32.TryParse(data, out value);

            if (value / 100 == 8)
            {
                safe_to_fly_status.Foreground = Brushes.Green;
                safe_to_fly_status.Content = "Safe";
            }
            else if (connect != false)
            {
                safe_to_fly_status.Foreground = Brushes.Red;
                safe_to_fly_status.Content = "Not Safe";
            }
            /*codes => http://openweathermap.org/weather-conditions */


        }

        void RetrieveImage()
        {
            UI_Network Network = new UI_Network();

            myImage = "/*set to the incoming image**/";

            person_found.Source = setImage();
        }

        BitmapImage setImage()
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(myImage);
            b.EndInit();
            return b;
        }

    }
}
