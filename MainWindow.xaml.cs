using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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

namespace APLIKACJA_KONWENTER
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Waluta();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdza czy wartość w textbox jest null lub pusta
            if (textbox1.Text == null || textbox1.Text.Trim() == "" || textbox1.Text == "0")
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("PODAJ ILOŚĆ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA TEXTBOX
                textbox1.Focus();
                return;
            }
            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1
            else if (comboBoxWaluty1.SelectedValue == null || comboBoxWaluty1.SelectedIndex == 0)
            {
                MessageBox.Show("ZAZNACZ: WYBIERZ WALUTĘ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 1
                comboBoxWaluty1.Focus();
                return;
            }
            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1
            else if (comboBoxWaluty2.SelectedValue == null || comboBoxWaluty2.SelectedIndex == 0)
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("ZAZNACZ: PRZEKONWERTUJ NA", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 2
                comboBoxWaluty2.Focus();
                return;
            }
            //WARTOŚĆ DLA OBU OKIEN COMBOBOX POSIADA TAKĄ SAMĄ WALUTĘ
            else if (comboBoxWaluty1.Text == comboBoxWaluty2.Text)
            {
                MessageBox.Show("BŁĄD KONWERSJI TAKICH SAMYCH WALUT", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //WYŚWIETL WYKONAJ DZIAŁANIE I WYSWIETL WYNIK
            else if (comboBoxWaluty1.SelectedValue != null && comboBoxWaluty2.SelectedValue != null 
               //PRÓBA PARSOWANIA Z TEXTBOX NA DOUBLE
                && double.TryParse(textbox1.Text, out double ilosc))
            {

                double ConvertedValue;
                // Konwertuj wartości na liczby
                double value1 = Convert.ToDouble(comboBoxWaluty1.SelectedValue);
                double value2 = Convert.ToDouble(comboBoxWaluty2.SelectedValue);

                // Wykonaj działanie
                double wynik = ilosc * value1 / value2;


                // Wyświetl wynik lub użyj go w zależności od potrzeb
                Label_WartoscPoKonwersji.Content = comboBoxWaluty2.Text + " " + wynik.ToString("N3");
            }
            else
            {
                MessageBox.Show("SPRAWDZ I WYPEŁNIJ POPRAWNIE WSZYSTKIE OKNA");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }


        private async void Waluta()
        {
            DataTable dtWaluta = new DataTable();
            dtWaluta.Columns.Add("Text");
            dtWaluta.Columns.Add("Value");

            dtWaluta.Rows.Add("--WYBIERZ--", 0);
            //API Z NBP PRZEDSTAWIA Z TABELI PRZELICZENIA WALUT NA 1PLN DLATEGO WARTOŚĆ DLA PLN JEST STAŁA
            dtWaluta.Rows.Add("PLN", 1);
            


            //IMPORT WALUT PRZEZ HTTP, ZA POMOCĄ API
            using (HttpClient client = new HttpClient())
            {
                //API DLA NBP KURSY WALUT
                
                string url = "http://api.nbp.pl/api/exchangerates/tables/A/?format=json";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    // Deserializacja JSON do obiektu
                    var currencyTable = JsonConvert.DeserializeObject<List<CurrencyTable>>(jsonResult);

                    // Pobieranie kursów walut
                    foreach (var currency in currencyTable.First().Rates)
                    {
                        dtWaluta.Rows.Add(currency.Currency, currency.Mid);
                    }

                    //DODANIE WYNIKU Z JSON DO COMBOBOX 1
                    comboBoxWaluty1.ItemsSource = dtWaluta.DefaultView;
                    comboBoxWaluty1.DisplayMemberPath = "Text";
                    comboBoxWaluty1.SelectedValuePath = "Value";
                    comboBoxWaluty1.SelectedIndex = 0; // Wybierz pierwszy element
                    comboBoxWaluty1.SelectedIndex = 1; // Wybierz pierwszy element

                    //DODANIE WYNIKU Z JSON DO COMBOBOX 2
                    comboBoxWaluty2.ItemsSource = dtWaluta.DefaultView;
                    comboBoxWaluty2.DisplayMemberPath = "Text";
                    comboBoxWaluty2.SelectedValuePath = "Value";
                    comboBoxWaluty2.SelectedIndex = 0; // Wybierz pierwszy element
                    comboBoxWaluty2.SelectedIndex = 1; // Wybierz pierwszy element
                }
                else
                {
                    // Obsługa błędu
                }
            }
        }
        // Klasa do deserializacji JSON
        public class Rate
        {
            public string Currency { get; set; }
            public decimal Mid { get; set; }
        }
        public class CurrencyTable
        {
            public List<Rate> Rates { get; set; }
        }
    }
}
