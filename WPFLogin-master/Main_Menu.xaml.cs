﻿using Microsoft.Win32;
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
//using System.Windows.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Main_Menu.xaml
    /// </summary>
    /// 


    public partial class Main_Menu : Window
    {

        bool isSearching = false;
        string myImage = "C:\\Users\\hpalmer.LRC\\Source\\Repos\\Drone_ui\\WPFLogin-master\\user-1-glyph-icon_MkuBPp8O.png";

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

        //TODO - make it not crash
        private void label3_Initialized(object sender, EventArgs e)
        {
         // this.Welcome_text.Content = this.monthCalendar1.SelectionRange.Start.ToShortDateString();
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

                    transfer.Upload(openFileDialog.FileName, name);
                    //comms.SendImage(name);

                    //sets imagebox with image
                    search_for_image.Source = setImage();
                }
            }
            else
            {
                isSearching = true;

            }
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

        private void Stop_Button1_Click(object sender, RoutedEventArgs e)
        {
            isSearching = false;
            //TODO reset images;
            RetrieveImage();
        }

        private static UI_Network comms = new UI_Network();

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                comms.SendStart();
            }
           catch
            {
               
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               comms.SendStop();
            }
            catch
            {
                Console.Write("");
            }
        }

        private void KillSwitch_Click(object sender, RoutedEventArgs e)
        {
             comms.SendKill();
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
