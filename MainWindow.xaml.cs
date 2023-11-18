using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        private int CurrencyId = 0;
        private double FromAmount = 0;
        private double ToAmount = 0;

        public MainWindow()
        {
            InitializeComponent();
            GetData();
            SetComboBoxItems();
        }

        


        //OKNO 1 - KALKULATOR WALUTOWY
        private void Przelicz_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdza czy wartość w textbox jest null lub pusta
            if (textbox1_KalkulatorWalutowy.Text == null || textbox1_KalkulatorWalutowy.Text.Trim() == "" || textbox1_KalkulatorWalutowy.Text == "0")
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("PODAJ ILOŚĆ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA TEXTBOX
                textbox1_KalkulatorWalutowy.Focus();
                return;
            }
            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1

            else if (comboBox1_KalkulatorWalutowy.SelectedValue == null || comboBox1_KalkulatorWalutowy.SelectedIndex == 0)
            {
                MessageBox.Show("ZAZNACZ: WYBIERZ WALUTĘ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 1
                comboBox1_KalkulatorWalutowy.Focus();
                return;
            }

            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1
            else if (comboBox2_KalkulatorWalutowy.SelectedValue == null || comboBox2_KalkulatorWalutowy.SelectedIndex == 0)
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("ZAZNACZ: PRZEKONWERTUJ NA", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 2
                comboBox2_KalkulatorWalutowy.Focus();
                return;
            }
            //WARTOŚĆ DLA OBU OKIEN COMBOBOX POSIADA TAKĄ SAMĄ WALUTĘ
            else if (comboBox1_KalkulatorWalutowy.Text == comboBox2_KalkulatorWalutowy.Text)
            {
                MessageBox.Show("BŁĄD KONWERSJI TAKICH SAMYCH WALUT", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //WYŚWIETL WYKONAJ DZIAŁANIE I WYSWIETL WYNIK
            else if (comboBox2_KalkulatorWalutowy.SelectedValue != null
                //PRÓBA PARSOWANIA Z TEXTBOX NA DOUBLE
                && double.TryParse(textbox1_KalkulatorWalutowy.Text, out double ilosc))
            {


                // Konwertuj wartości na liczby
                double value1 = Convert.ToDouble(comboBox1_KalkulatorWalutowy.SelectedValue);
                double value2 = Convert.ToDouble(comboBox2_KalkulatorWalutowy.SelectedValue);


                // Wykonaj działanie
                double wynik = ilosc * value1 / value2;


                // Wyświetl wynik lub użyj go w zależności od potrzeb
                Label1_KalkulatorWalutowy.Content = comboBox2_KalkulatorWalutowy.Text + " " + wynik.ToString("N3");
            }
            else
            {
                MessageBox.Show("SPRAWDZ I WYPEŁNIJ POPRAWNIE WSZYSTKIE OKNA");
            }
        }


        //OKNO 2 - KONWENTER WALUTOWY

        public void mycon()
        {
            String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(Conn);
            con.Open();
        }


        private void Konwertuj_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdza czy wartość w textbox jest null lub pusta
            if (textbox1_KonwenterWalutowy.Text == null || textbox1_KonwenterWalutowy.Text.Trim() == "" || textbox1_KonwenterWalutowy.Text == "0")
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("PODAJ ILOŚĆ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA TEXTBOX
                textbox1_KonwenterWalutowy.Focus();
                return;
            }
            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1
            
             else if (comboBox1_KonwenterWalutowy.SelectedValue == null || comboBox1_KonwenterWalutowy.SelectedIndex == 0)
            {
                MessageBox.Show("ZAZNACZ: WYBIERZ WALUTĘ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 1
                comboBox1_KonwenterWalutowy.Focus();
                return;
            } 
            
            //SPRAWDZA CZY WALUTA JEST WYBRANA W COMBOBOX 1
            else if (comboBox2_KonwenterWalutowy.SelectedValue == null || comboBox2_KonwenterWalutowy.SelectedIndex == 0)
            {
                //POKAŻ KOMUNIKAT
                MessageBox.Show("ZAZNACZ: PRZEKONWERTUJ NA", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                //USTAW SKUPIENIE NA COMBOBOX 2
                comboBox2_KonwenterWalutowy.Focus();
                return;
            }
            //WARTOŚĆ DLA OBU OKIEN COMBOBOX POSIADA TAKĄ SAMĄ WALUTĘ
            else if (comboBox1_KonwenterWalutowy.Text == comboBox2_KonwenterWalutowy.Text)
            {
                MessageBox.Show("BŁĄD KONWERSJI TAKICH SAMYCH WALUT", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //WYŚWIETL WYKONAJ DZIAŁANIE I WYSWIETL WYNIK
            else if (comboBox2_KonwenterWalutowy.SelectedValue != null
                //PRÓBA PARSOWANIA Z TEXTBOX NA DOUBLE
                && double.TryParse(textbox1_KonwenterWalutowy.Text, out double ilosc))
            {


                // Konwertuj wartości na liczby
                double value1 = Convert.ToDouble(comboBox1_KonwenterWalutowy.SelectedValue);
                double value2 = Convert.ToDouble(comboBox2_KonwenterWalutowy.SelectedValue);
                

                // Wykonaj działanie
                double wynik = ilosc * value1 / value2;


                // Wyświetl wynik lub użyj go w zależności od potrzeb
                Label1_KonwenterWalutowy.Content = comboBox2_KonwenterWalutowy.Text + " " + wynik.ToString("N3");
            }
            else
            {
                MessageBox.Show("SPRAWDZ I WYPEŁNIJ POPRAWNIE WSZYSTKIE OKNA");
            }
        }

        private void Wyczysc_Click(object sender, RoutedEventArgs e)
        {

        }


        /* POPRAWA KODU, KOD WCZEŚNIEJSZY: 
         * 
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
                    comboBox1_KonwenterWalutowy.ItemsSource = dtWaluta.DefaultView;
                    comboBox1_KonwenterWalutowy.DisplayMemberPath = "Text";
                    comboBox1_KonwenterWalutowy.SelectedValuePath = "Value";
                    comboBox1_KonwenterWalutowy.SelectedIndex = 0; // Wybierz pierwszy element
                    comboBox1_KonwenterWalutowy.SelectedIndex = 1; // Wybierz pierwszy element

                    //DODANIE WYNIKU Z JSON DO COMBOBOX 2
                    comboBox2_KonwenterWalutowy.ItemsSource = dtWaluta.DefaultView;
                    comboBox2_KonwenterWalutowy.DisplayMemberPath = "Text";
                    comboBox2_KonwenterWalutowy.SelectedValuePath = "Value";
                    comboBox2_KonwenterWalutowy.SelectedIndex = 0; // Wybierz pierwszy element
                    comboBox2_KonwenterWalutowy.SelectedIndex = 1; // Wybierz pierwszy element

                    //DODANIE WYNIKU Z JSON DO COMBOBOX 3
                    combobox1_PortfelWalutowy.ItemsSource = dtWaluta.DefaultView;
                    combobox1_PortfelWalutowy.DisplayMemberPath = "Text";
                    combobox1_PortfelWalutowy.SelectedValuePath = "Value";
                    combobox1_PortfelWalutowy.SelectedIndex = 0; // Wybierz pierwszy element
                    combobox1_PortfelWalutowy.SelectedIndex = 1; // Wybierz pierwszy element
                }
                else
                {
                    // Obsługa błędu
                }
            }
        }

        */

        private async void SetComboBoxItems()
        {
            // Wywołanie metody WalutaJson asynchronicznie
            DataTable dt = await WalutaJson();

            // Wywołanie metody WalutaPortfel asynchronicznie
            DataTable dtPortfel = await WalutaPortfel();

            // Wywołanie metody WalutaJson asynchronicznie
            DataTable dtJson = await WalutaJson();

            DataTable dtZnajdzWspolneWaluty = await ZnajdzWspolneWaluty();


            // Sprawdzenie, czy tabela nie jest pusta
            if (dt != null)
            {
                // DODANIE WYNIKU Z JSON DO OKNA 1 - KALKULATOR WALUTOWY COMBO1 I COMBO2
                comboBox1_KalkulatorWalutowy.ItemsSource = dt.DefaultView;
                comboBox1_KalkulatorWalutowy.DisplayMemberPath = "Text";
                comboBox1_KalkulatorWalutowy.SelectedValuePath = "Value";
                comboBox1_KalkulatorWalutowy.SelectedIndex = 0; // Wybierz pierwszy element
                                                               

                comboBox2_KalkulatorWalutowy.ItemsSource = dt.DefaultView;
                comboBox2_KalkulatorWalutowy.DisplayMemberPath = "Text";
                comboBox2_KalkulatorWalutowy.SelectedValuePath = "Value";
                comboBox2_KalkulatorWalutowy.SelectedIndex = 0; // Wybierz pierwszy element


                // DODANIE WYNIKU Z JSON DO COMBOBOX 1
                comboBox1_KonwenterWalutowy.ItemsSource = dtZnajdzWspolneWaluty.DefaultView;
                comboBox1_KonwenterWalutowy.DisplayMemberPath = "CurrencyName";
                comboBox1_KonwenterWalutowy.SelectedValuePath = "Value";
                comboBox1_KonwenterWalutowy.SelectedIndex = 0; // Wybierz pierwszy element

                // DODANIE WYNIKU Z JSON DO COMBOBOX 2
                comboBox2_KonwenterWalutowy.ItemsSource = dt.DefaultView;
                comboBox2_KonwenterWalutowy.DisplayMemberPath = "Text";
                comboBox2_KonwenterWalutowy.SelectedValuePath = "Value";
                comboBox2_KonwenterWalutowy.SelectedIndex = 0; // Wybierz pierwszy element

                // DODANIE WYNIKU Z JSON DO COMBOBOX 3
                combobox1_PortfelWalutowy.ItemsSource = dt.DefaultView;
                combobox1_PortfelWalutowy.DisplayMemberPath = "Text";
                combobox1_PortfelWalutowy.SelectedValuePath = "Value";
                combobox1_PortfelWalutowy.SelectedIndex = 0; // Wybierz pierwszy element


            }
            else
            {
                // Obsługa błędu, jeśli tabela jest pusta
                // Tutaj można dodać kod obsługujący brak danych
            }
        }


        private async Task RefreshComboBox1()
        {
            DataTable dt = await ZnajdzWspolneWaluty();

            if (dt != null)
            {
                comboBox1_KonwenterWalutowy.ItemsSource = dt.DefaultView;
                comboBox1_KonwenterWalutowy.DisplayMemberPath = "CurrencyName";
                comboBox1_KonwenterWalutowy.SelectedValuePath = "Value";
                comboBox1_KonwenterWalutowy.SelectedIndex = 0;
            }
            else
            {
                comboBox1_KonwenterWalutowy.ItemsSource = null;
            }
        }


        private async Task<DataTable> WalutaPortfel()
        {
            mycon();

            // Inicjalizacja DataTable
            DataTable dt = new DataTable();

            // Pobranie danych z bazy danych
            cmd = new SqlCommand("SELECT Amount, CurrencyName FROM Portfel_Walutowy", con);
            cmd.CommandType = CommandType.Text;

            // Utworzenie adaptera danych
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            // Wypełnienie DataTable danymi z bazy danych
            adapter.Fill(dt);

            // Zamknięcie połączenia z bazą danych
            con.Close();

            // Zwrócenie DataTable
            return dt;
        }

        private async Task<DataTable> WalutaJson()
        {
            // Inicjalizacja DataTable z dwiema kolumnami: "Text" i "Value"
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");

            // Dodanie dwóch wierszy do tabeli
            dt.Rows.Add("--WYBIERZ--", 0);
            dt.Rows.Add("PLN", 1);

            // Import kursów walut przez HTTP, za pomocą API NBP
            using (HttpClient client = new HttpClient())
            {
                // Adres URL API NBP z kursami walut
                string url = "http://api.nbp.pl/api/exchangerates/tables/A/?format=json";
                HttpResponseMessage response = await client.GetAsync(url);

                // Sprawdzenie, czy odpowiedź z API jest poprawna
                if (response.IsSuccessStatusCode)
                {
                    // Odczytanie zawartości odpowiedzi (JSON)
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    // Deserializacja JSON do obiektu
                    var currencyTable = JsonConvert.DeserializeObject<List<CurrencyTable>>(jsonResult);

                    // Pobranie kursów walut i dodanie ich do tabeli
                    foreach (var currency in currencyTable.First().Rates)
                    {
                        dt.Rows.Add(currency.Currency, currency.Mid);
                    }

                    // Zwrócenie tabeli jako wyniku
                    return dt;
                }
                else
                {
                    // Obsługa błędu w przypadku niepowodzenia pobierania danych z API
                    // Tutaj można dodać kod obsługujący błąd, np. MessageBox.Show("Błąd pobierania danych.");
                    return null; // Można zwrócić null lub inny obiekt w przypadku błędu
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

        private async Task<DataTable> ZnajdzWspolneWaluty()
        {
            DataTable walutaPortfelTable = await WalutaPortfel();
            DataTable walutaJsonTable = await WalutaJson();

            // Inicjalizacja nowej DataTable, która będzie zawierać wspólne waluty
            DataTable wspolneWalutyTable = new DataTable();
            wspolneWalutyTable.Columns.Add("CurrencyName", typeof(string)); // Nazwa waluty
            wspolneWalutyTable.Columns.Add("Value", typeof(decimal)); // Wartość Value

            // Iteracja przez wiersze z obu tabel i dodawanie wspólnych walut do nowej tabeli
            foreach (DataRow rowPortfel in walutaPortfelTable.Rows)
            {
                string currencyNamePortfel = rowPortfel["CurrencyName"].ToString();

                foreach (DataRow rowJson in walutaJsonTable.Rows)
                {
                    string currencyNameJson = rowJson["Text"].ToString();

                    if (currencyNamePortfel == currencyNameJson)
                    {
                        string valueJson = rowJson["Value"].ToString(); // Pobierz wartość "Value" jako string
                        wspolneWalutyTable.Rows.Add(currencyNameJson, Convert.ToDecimal(valueJson));
                        break; // Przerwij pętlę, gdy znajdziesz dopasowanie
                    }
                }
            }

            // Zwróć tabelę zawierającą wspólne waluty
            return wspolneWalutyTable;
        }

        /*
        private async void WyswietlWspolneWaluty()
        {
            DataTable wspolneWalutyTable = await ZnajdzWspolneWaluty();

            if (wspolneWalutyTable != null && wspolneWalutyTable.Rows.Count > 0)
            {
                StringBuilder message = new StringBuilder();

                foreach (DataRow row in wspolneWalutyTable.Rows)
                {
                    string currencyName = row["CurrencyName"].ToString();
                    decimal value = Convert.ToDecimal(row["Value"]);

                    message.AppendLine($"Waluta: {currencyName}, Wartość: {value}");
                }

                MessageBox.Show(message.ToString(), "Wspólne Waluty");
            }
            else
            {
                MessageBox.Show("Brak wspólnych walut.", "Brak danych");
            }
        }



        FUNKCJA POMOCNICZA WYSWIETLA KOMUNIKAT
        */

        private bool isRefreshed = false;

        private async void OnTabKonwenterPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isRefreshed)
            {
                isRefreshed = true;
                await RefreshComboBox1();
            }
        }

        private async void OnTabPortfelPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isRefreshed = false;
                
            }
        }


        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textbox1_PortfelWalutowy.Text == null || textbox1_PortfelWalutowy.Text.Trim() == "" || combobox1_PortfelWalutowy.SelectedIndex == 0)
                {
                    MessageBox.Show("Wprowadź wartość");
                    textbox1_PortfelWalutowy.Focus();
                    return;
                }
                else
                {
                    if (CurrencyId > 0)
                    {
                        if (MessageBox.Show("Chcesz zaktualizować?", "Informacja", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dtWaluta = new DataTable();
                            cmd = new SqlCommand("UPDATE Portfel_Walutowy SET Amount = @Amount, CurrencyName = @CurrencyName WHERE Id = @Id", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
                            cmd.Parameters.AddWithValue("@Amount", textbox1_PortfelWalutowy.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", combobox1_PortfelWalutowy.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Aktualizacja");

                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Chcesz zapisać?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("INSERT INTO Portfel_Walutowy(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Amount", textbox1_PortfelWalutowy.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", combobox1_PortfelWalutowy.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Zapis ukończony poprawnie", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    ClearMaster();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearMaster()
        {
            try
            {
                textbox1_PortfelWalutowy.Text = string.Empty;
                btnZapisz.Content = "Zapisz";
                GetData();
                CurrencyId = 0;
                textbox1_PortfelWalutowy.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetData()
        {
            // Metoda służy do połączenia z bazą danych i otwarcia połączenia   
            mycon();

            // Utwórz obiekt DataTable
            DataTable dtWaluta = new DataTable();

            // Napisz zapytanie SQL do pobrania danych z tabeli bazy danych. Zapytanie jest zapisane w cudzysłowiu, a po przecinku podane jest połączenie   
            cmd = new SqlCommand("SELECT * FROM Portfel_Walutowy", con);

            // CommandType definiuje rodzaj polecenia, które ma być wykonane, na przykład Text, StoredProcedure, TableDirect    
            cmd.CommandType = CommandType.Text;

            // Przyjmuje parametr zawierający tekst polecenia obiektu SelectCommand.
            da = new SqlDataAdapter(cmd);

            // DataAdapter służy jako most między DataSet a źródłem danych do pobierania i zapisywania danych. Operacja Fill dodaje wiersze do obiektów DataTable docelowych w DataSet    
            da.Fill(dtWaluta);

            // dt nie jest null i liczba wierszy jest większa niż 0
            if (dtWaluta != null && dtWaluta.Rows.Count > 0)
            {
                // Przypisz dane DataTable do dgvCurrency za pomocą właściwości ItemSource. 
                dgvCurrency.ItemsSource = dtWaluta.DefaultView;
            }
            else
            {
                dgvCurrency.ItemsSource = null;
            }
            // Zamknij połączenie z bazą danych
            con.Close();
        }

        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                // Utwórz obiekt DataGrid
                DataGrid grd = (DataGrid)sender;

                // Utwórz obiekt DataRowView
                DataRowView row_selected = grd.CurrentItem as DataRowView;

                // Jeśli row_selected nie jest null
                if (row_selected != null)
                {
                    // Ilość elementów dgvCurrency większa niż zero
                    if (dgvCurrency.Items.Count > 0)
                    {
                        if (grd.SelectedCells.Count > 0)
                        {
                            // Pobierz wartość kolumny Id z wybranego wiersza i przypisz ją do zmiennej CurrencyId
                            CurrencyId = Int32.Parse(row_selected["Id"].ToString());

                            // DisplayIndex jest równy zero w edytowanej komórce
                            if (grd.SelectedCells[0].Column.DisplayIndex == 0)
                            {
                                // Pobierz wartość kolumny Amount z wybranego wiersza i ustaw ją w polu tekstowym textbox1_PortfelWalutowy
                                textbox1_PortfelWalutowy.Text = row_selected["Amount"].ToString();

                                // Pobierz wartość kolumny CurrencyName z wybranego wiersza i ustaw ją w polu tekstowym combobox1_PortfelWalutowy
                                combobox1_PortfelWalutowy.Text = row_selected["CurrencyName"].ToString();
                                btnZapisz.Content = "Update";     // Zmień tekst przycisku zapisu na "Update"
                            }

                            // DisplayIndex jest równy jeden w komórce usuniętej
                            if (grd.SelectedCells[0].Column.DisplayIndex == 1)
                            {
                                // Wyświetl okno dialogowe potwierdzenia
                                if (MessageBox.Show("Czy chcesz usunąć ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    mycon();
                                    DataTable dt = new DataTable();

                                    // Wykonaj zapytanie usuwające rekord z tabeli za pomocą Id
                                    cmd = new SqlCommand("DELETE FROM Portfel_Walutowy WHERE Id = @Id", con);
                                    cmd.CommandType = CommandType.Text;

                                    // CurrencyId ustawione w parametrze @Id i przesyłane w instrukcji usuwania
                                    cmd.Parameters.AddWithValue("@Id", CurrencyId);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    MessageBox.Show("Usuwanie ukończone poprawnie", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClearMaster();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Zdarzenie zmiany wyboru ComboBox dla waluty źródłowej w celu uzyskania ilości waluty po zmianie wyboru nazwy waluty
        private void cmbFromCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Jeśli wybrana wartość cmbFromCurrency nie jest równa null i nie równa się zero
                if (comboBox1_KonwenterWalutowy.SelectedValue != null && int.Parse(comboBox1_KonwenterWalutowy.SelectedValue.ToString()) != 0 && comboBox1_KonwenterWalutowy.SelectedIndex != 0)
                {
                    // Wybrana wartość cmbFromCurrency ustawiona w zmiennej CurrencyFromId
                    int CurrencyFromId = int.Parse(comboBox1_KonwenterWalutowy.SelectedValue.ToString());

                    mycon();
                    DataTable dt = new DataTable();

                    // Zapytanie SELECT do pobrania ilości z bazy danych za pomocą identyfikatora
                    cmd = new SqlCommand("SELECT Amount FROM Portfel_Walutowy WHERE Id = @CurrencyFromId", con);
                    cmd.CommandType = CommandType.Text;

                    if (CurrencyFromId != 0)
                        // CurrencyFromId ustawione w parametrze @CurrencyFromId i przekazane jako parametr w zapytaniu
                        cmd.Parameters.AddWithValue("@CurrencyFromId", CurrencyFromId);

                    da = new SqlDataAdapter(cmd);

                    // Ustawienie danych zwróconych przez zapytanie w tabeli danych
                    da.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                        //Pobierz wartość kolumny 'amount' z tabeli danych i przypisz tę wartość do zmiennej 'FromAmount', która została zadeklarowana globalnie.
                        FromAmount = double.Parse(dt.Rows[0]["Amount"].ToString());

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Zdarzenie zmiany wyboru ComboBox dla waluty docelowej w celu uzyskania ilości waluty po zmianie wyboru nazwy waluty
        private void cmbToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Jeśli wybrana wartość cmbToCurrency nie jest równa null i nie równa się zero
                if (comboBox2_KonwenterWalutowy.SelectedValue != null && int.Parse(comboBox2_KonwenterWalutowy.SelectedValue.ToString()) != 0 && comboBox2_KonwenterWalutowy.SelectedIndex != 0)
                {
                    // Wybrana wartość cmbToCurrency ustawiona w zmiennej CurrencyToId
                    int CurrencyToId = int.Parse(comboBox2_KonwenterWalutowy.SelectedValue.ToString());

                    mycon();

                    DataTable dt = new DataTable();
                    //Select query for get Amount from database using id
                    cmd = new SqlCommand("SELECT Amount FROM Portfel_Walutowy WHERE Id = @CurrencyToId", con);
                    cmd.CommandType = CommandType.Text;

                    if (CurrencyToId != 0)
                        // Zapytanie SELECT do pobrania ilości z bazy danych za pomocą identyfikatora
                        cmd.Parameters.AddWithValue("@CurrencyToId", CurrencyToId);

                    da = new SqlDataAdapter(cmd);

                    // Ustawienie danych zwróconych przez zapytanie w tabeli danych
                    da.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                        // Pobranie wartości kolumny Amount z tabeli danych i ustawienie wartości w zmiennej globalnej ToAmount
                        ToAmount = double.Parse(dt.Rows[0]["Amount"].ToString());
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #region Preview Key Down Events
        // Zdarzenie Key Down dla cmbFromCurrency
        private void cmbFromCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Jeśli użytkownik naciśnie klawisz Tab lub Enter, zostanie wykonane zdarzenie cmbFromCurrency_SelectionChanged
            if (e.Key == Key.Tab || e.SystemKey == Key.Enter)
            {
                cmbFromCurrency_SelectionChanged(sender, null);
            }
        }

        // Zdarzenie Key Down dla cmbToCurrency
        private void cmbToCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Jeśli użytkownik naciśnie klawisz Tab lub Enter, zostanie wykonane zdarzenie cmbToCurrency_SelectionChanged
            if (e.Key == Key.Tab || e.SystemKey == Key.Enter)
            {
                cmbToCurrency_SelectionChanged(sender, null);
            }
        }

       
    }
}

#endregion