
using ByggemarkedEFClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace ByggemarkedAnsat
{
    /// <summary>
    /// Interaction logic for BookingOverblikPaaKunde.xaml
    /// </summary>
    public partial class BookingOverblikPaaKunde : Window
    {
        private Kunder kunde = new Kunder();
        private bool finStand = true;
        private List<Bookinger> itemsList = new List<Bookinger>();
        private Bookinger booking = new Bookinger();
        private ByggemarkedEntities db = new ByggemarkedEntities();

        public BookingOverblikPaaKunde(Kunder kunde)
        {
            this.kunde = kunde;
            InitializeComponent();
            GetBookings();
            statusBarNavn.Content = $"Kunde: {kunde.Navn}";
            statusBarAntal.Content = $"Antal Bookinger: {kunde.Bookinger.Count}";
        }

        /*
         * ---------------------------------------------------------------------------------------------------------------------------
         */

        // Indlæser alle bookinger i en combobox for den søgte kunde:
        private void GetBookings()
        {
            foreach (Bookinger b in kunde.Bookinger)
            {
                itemsList.Add(b);
            }
            itemsList.Sort((x, y) => DateTime.Compare(x.PeriodeStart, y.PeriodeStart));

            bookingComboBox.ItemsSource = itemsList;
        }

        /*
         * ---------------------------------------------------------------------------------------------------------------------------
         */

        private void bookingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Bookinger b = (Bookinger)cmb.SelectedItem;
            booking = b;

            b.TotalPris = BeregnPris(b);

            lblStatus.Content = b.Status;
            lblSenesteSkift.Content = b.PeriodeStart;
            txtBlockDetaljer.Text = $"KundeID:\t{b.KundeId}" +
                $"\nBookingID:\t{b.BookingId}" +
                $"\nVærktøj:\t\t{b.Vaerktoej.ToString()}" +
                $"\n\nPeriode:\t\t{b.PeriodeStart.Date}" +
                $"\n\t\t\ttil\n\t\t{b.PeriodeSlut.Date}";
            lblPris.Content = $"{b.TotalPris} kr";
        }

        private void btn_ClickSlet(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Vil du slette reservationen?", "Slet?", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Bookinger bk = (from b in db.Bookinger where b.BookingId == booking.BookingId select b).SingleOrDefault();
                    db.Bookinger.Remove(bk);
                    db.SaveChanges();
                    itemsList.Remove(bk);
                    GetBookings();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_ClickOpdater(object sender, RoutedEventArgs e)
        {
            booking = db.Bookinger.Where(x => x.BookingId == booking.BookingId).FirstOrDefault();
            if (booking.Status.Equals("Udleveret"))
            {
                MessageBoxResult result = MessageBox.Show("Er værktøjet i samme stand som ved udlevering?", "Godkend tilbagelevering?", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        finStand = false;
                        break;
                }
                booking.Status = "Tilbageleveret";
                lblStatus.Content = "Tilbageleveret";
                lblPris.Content = $"{BeregnPris(booking)} kr";
            }
            else if (booking.Status.Equals("Reserveret"))
            {
                booking.Status = "Udleveret";
                lblStatus.Content = "Udleveret";
            }

            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            MessageBox.Show("Opdatering Gennemført!");
        }

        /*
         * ---------------------------------------------------------------------------------------------------------------------------
         */

        /*
         * Metode til at udregne den totale pris for en booking.
         * Metoden er med eller uden depositum,
         * alt efter hvilken stand værktøjet bliver afleveret i.
         */
        private decimal BeregnPris(Bookinger b)
        {
            int antalDage = (b.PeriodeSlut - b.PeriodeStart).Days;
            decimal bookingPris;

            if (finStand)
            {
                bookingPris = (b.Vaerktoej.DoegnPris * antalDage);
            }
            else
            {
                bookingPris = (b.Vaerktoej.DoegnPris * antalDage) + b.Vaerktoej.Depositum;
            }
            return bookingPris;
        }

        /*
         * ---------------------------------------------------------------------------------------------------------------------------
         */

        /*
         * Menu metoder, disse kan bruges til at finde kundeinfo,
         * eller afslutte opslaget af kunden.
         */

        // Indlæser relevant kundeinformation:
        private void MenuItemInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                $"Navn:\t\t{kunde.Navn}" +
                $"\nAdresse:\t\t{kunde.Adresse}" +
                $"\nEmail:\t\t{kunde.Email}" +
                $"\nAntal Bookinger:\t{kunde.Bookinger.Count()}",
                "Kundeinfo",
                MessageBoxButton.OK);
        }

        // Afslutter opslaget, og returnere brugeren til søgevinduet (MainWindow):
        private void MenuItemAfslut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Vil du afslutte visningen?", "Afslut?", MessageBoxButton.OKCancel);

            switch (result)
            {
                case MessageBoxResult.OK:
                    Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        // Kort info omkring dette desktop program:
        private void MenuItemOm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"- Dette program kan administrerer diverse værktøjsbookinger, fra kunder af byggemarkedet." +
                $"\n- Det kan bruges til at annullére en aftalt booking, så længe det givne værktøj ikke er udleveret endnu." +
                $"\n- Når et værktøj/redskab bliver afleveret tilbage, så vurderes dettes stand." +
                $"\n- Standen er med til at udregne prisen, og om depositum skal tilagebetales.", "Om BookingCenter");
        }


    }
}
