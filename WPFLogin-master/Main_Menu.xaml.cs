using System;
using System.Collections.Generic;
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
    public partial class Main_Menu : Window
    {

        bool isSearching = false;

        private System.Windows.Forms.MonthCalendar monthCalendar1;

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
        private void No_click(object sender, RoutedEventArgs e)
        {

        }
        private void Yes_click(object sender, RoutedEventArgs e)
        {

        }

        //TODO - make it not crash
        private void label3_Initialized(object sender, EventArgs e)
        {
         // this.Welcome_text.Content = this.monthCalendar1.SelectionRange.Start.ToShortDateString();
        }
        //
        private void Start_Search_Click(object sender, RoutedEventArgs e)
        {
            if (isSearching)
            {
                Continue_Searching.Visibility = Visibility.Visible;
                Start_Search.Content = "New Search";
                search_for_image.Fill = new ImageBrush
                {
                    ImageSource = new Image("C:\\Users\\hpalmer.LRC\\Documents\\Harrison & Jared & Ryan\\user-1-glyph-icon_MkuBPp8O.png")
                };
            }
            else
            {
                isSearching = true;

            }
        }
    }
}
