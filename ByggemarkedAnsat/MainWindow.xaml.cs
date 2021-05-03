
using ByggemarkedEFClassLibrary;
using System;
using System.Collections.Generic;
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

namespace ByggemarkedAnsat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        ByggemarkedEntities context = new ByggemarkedEntities();

        public MainWindow()
        {
            InitializeComponent();
            emailTextBox.Focus();
        }

        /*
         * BtnSoeg_Click bruges til at tilføje funktion til knappen "SØG" på startsiden.
         * I tilfælde af at der findes en kunde på den indtastede email,
         */
        private void BtnSoeg_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            int countCheck = context.Kunder.Count();
            int count = 0;

            foreach (Kunder k in context.Kunder)
            {
                count++;
                if (k.Email.Equals(email))
                {
                    MessageBox.Show($"Du har fundet en kunde!\n\nNavn: {k.Navn}\nE-mail: {k.Email}");
                    BookingOverblikPaaKunde BOPK = new BookingOverblikPaaKunde(k);
                    BOPK.Show();
                    emailTextBox.Clear();
                    break;
                }
                else if (count >= countCheck)
                {
                    MessageBox.Show("Der er ingen kunde registreret på denne email!", "Søgefejl");
                    emailTextBox.Focus();
                    emailTextBox.SelectAll();
                }
            }
        }

        private void BtnLuk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
