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
using Kelettravel2024;
using NetworkHelper;


namespace kelet2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string host = "http://localhost:3000";
        public MainWindow()
        {
            InitializeComponent();
            lbCelok.ItemsSource = Backend.GET($"{host}/celok").Send().ToList<Celok>();
            lbCelok.DisplayMemberPath = "celok_nev";
        }

        private void lbCelok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Celok cel = lbCelok.SelectedItem as Celok;
            lblCelNev.Content = cel.celok_nev;
            lblCelHonap.Content = cel.celok_kultura_honap;
        }

        private void Felvitel_Click(object sender, RoutedEventArgs e)
        {
            Kapcsolatfelvetel adatok = new Kapcsolatfelvetel()
            {
                nev = tbNev.Text,
                email = tbEmail.Text,
                telefon = tbTelefon.Text,
                megjegyzes = tbMegjegyzes.Text

            };

            if (adatok.Hianyos)
            {
                MessageBox.Show("Minden mező kitöltése kötelező");
            }
            else
            {
                string uzenet = Backend.POST($"{host}/kapcsolatok").Body(adatok).Send().Message;
                MessageBox.Show(uzenet);
            }
        }
    }
}
