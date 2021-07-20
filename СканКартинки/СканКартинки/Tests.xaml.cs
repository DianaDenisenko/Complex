using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;

namespace СканКартинки
{
    /// <summary>
    /// Логика взаимодействия для Tests.xaml
    /// </summary>

    

    public partial class Tests : Window
    {
        public DataTable Select(string select) // функция подключения к базе данных и обработка запросов
        {

            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "mysql.j8135376.myjino.ru",
                Database = "j8135376",
                UserID = "j8135376",
                Password = "fwFU3t4ks",
                OldGuids = true
            };
            MySqlConnection sqlConnection = new MySqlConnection(mySqlConnectionStringBuilder.ToString());
            sqlConnection.Open();
            string sqlCmd = select;
            MySqlDataAdapter adr = new MySqlDataAdapter(sqlCmd, sqlConnection);
            adr.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            adr.Fill(dt);
            sqlConnection.Close();
            return dt;
        }
        DataTable dt_user;
        
        class T
        {
            public int id_test;
            public string head, good_answer;
            public string[] V;
            public string user_answer;
            public T(int id_test, string head, string[] V,string ans)
            {
                this.id_test = id_test; this.V = V;  good_answer = ans; this.head = head; user_answer = "";
            }
            public T() { }
            
        }
        public void LoadPNG(int id_test)
        {
            DataTable png = Select($"SELECT ImageData,PX,PY FROM пнг WHERE Статус is null and ID_Теста = {id_test}");
            if (png.Rows.Count == 0) return;
            for (int i = 0; i < png.Rows.Count; i++)
            {
                byte[] byte_img = (byte[])png.Rows[i][0];
                using (MemoryStream ms = new MemoryStream(byte_img, 0, byte_img.Length))
                {
                    ms.Write(byte_img, 0, byte_img.Length);
                    ms.Seek(0,SeekOrigin.Begin);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    
                    Image img = new Image();
                    img.Source = bi;
                    Thickness t = new Thickness();
                    t.Left = (double)png.Rows[i][1];
                    t.Top = (double)png.Rows[i][2];
                    img.Margin = t;
                    G2.Children.Add(img);
                }
            }
        }
        public Tests(int id)
        {
            InitializeComponent();
            try
            {
                Select("select * from категории where ID_Категории = 0");
            }
            catch { MessageBox.Show("Ошибка подключения к Интернету.");this.Close(); }
            CreateTests(id);
            RandomizeTests();
            ShowTest(test);
        }
        

        List<T> test_list = new List<T>();
        void CreateTests(int id)
        {
            dt_user = Select($"SELECT * FROM тесты WHERE Статус is null and ID_Каталог = {id}");
            for (int i = 0; i < dt_user.Rows.Count; i++)
            {
                DataRow r = dt_user.Rows[i];
                string[] s = new string[5] { r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString() };
                test_list.Add(  new T((int)r[0] , r[1].ToString(), s, r[7].ToString()));
            }
        }


        int test = 1;
        void ShowTest(int test)
        {
                G2.Children.RemoveRange(0,G2.Children.Count);
                T toshow = test_list[test - 1];
                L1.Content = "Вопрос " + test;
                TB1.Text = toshow.head;
                LoadPNG(toshow.id_test);
                CB1.Visibility = Visibility.Hidden; CB2.Visibility = Visibility.Hidden; CB3.Visibility = Visibility.Hidden;
                CB4.Visibility = Visibility.Hidden; CB5.Visibility = Visibility.Hidden;
                CB1.Content = "";CB2.Content = "";CB3.Content = "";CB4.Content = ""; CB5.Content = "";
                CB1.IsChecked = false; CB2.IsChecked = false; CB3.IsChecked = false; CB4.IsChecked = false; CB5.IsChecked = false;

                RB1.Visibility = Visibility.Hidden; RB2.Visibility = Visibility.Hidden; RB3.Visibility = Visibility.Hidden;
                RB4.Visibility = Visibility.Hidden; RB5.Visibility = Visibility.Hidden;
                RB1.Content = ""; RB2.Content = ""; RB3.Content = ""; RB4.Content = ""; RB5.Content = "";
                RB1.IsChecked = false; RB2.IsChecked = false; RB3.IsChecked = false; RB4.IsChecked = false; RB5.IsChecked = false;
            if (toshow.good_answer.Split('|').Length > 1)
            {
                if (toshow.V[0] != "") { CB1.Visibility = Visibility.Visible; CB1.Content = toshow.V[0]; }
                if (toshow.V[1] != "") { CB2.Visibility = Visibility.Visible; CB2.Content = toshow.V[1]; }
                if (toshow.V[2] != "") { CB3.Visibility = Visibility.Visible; CB3.Content = toshow.V[2]; }
                if (toshow.V[3] != "") { CB4.Visibility = Visibility.Visible; CB4.Content = toshow.V[3]; }
                if (toshow.V[4] != "") { CB5.Visibility = Visibility.Visible; CB5.Content = toshow.V[4]; }

                string[] s = toshow.user_answer.Split('|');
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == CB1.Content.ToString()) CB1.IsChecked = true;
                    if (s[i] == CB2.Content.ToString()) CB2.IsChecked = true;
                    if (s[i] == CB3.Content.ToString()) CB3.IsChecked = true;
                    if (s[i] == CB4.Content.ToString()) CB4.IsChecked = true;
                    if (s[i] == CB5.Content.ToString()) CB5.IsChecked = true;
                }
            }
            else
            {
                if (toshow.V[0] != "") { RB1.Visibility = Visibility.Visible; RB1.Content = toshow.V[0]; }
                if (toshow.V[1] != "") { RB2.Visibility = Visibility.Visible; RB2.Content = toshow.V[1]; }
                if (toshow.V[2] != "") { RB3.Visibility = Visibility.Visible; RB3.Content = toshow.V[2]; }
                if (toshow.V[3] != "") { RB4.Visibility = Visibility.Visible; RB4.Content = toshow.V[3]; }
                if (toshow.V[4] != "") { RB5.Visibility = Visibility.Visible; RB5.Content = toshow.V[4]; }

                string[] s = toshow.user_answer.Split('|');
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == RB1.Content.ToString()) RB1.IsChecked = true;
                    if (s[i] == RB2.Content.ToString()) RB2.IsChecked = true;
                    if (s[i] == RB3.Content.ToString()) RB3.IsChecked = true;
                    if (s[i] == RB4.Content.ToString()) RB4.IsChecked = true;
                    if (s[i] == RB5.Content.ToString()) RB5.IsChecked = true;
                }
            }
        }

        void RandomizeTests()
        {
            Random r = new Random();
            for(int i = 0; i < test_list.Count; i++) 
            {
                T answer = test_list[i];
                int count = 0;
                if (answer.V[0] != "") count++;
                if (answer.V[1] != "") count++;
                if (answer.V[2] != "") count++;
                if (answer.V[3] != "") count++;
                if (answer.V[4] != "") count++;
                for(int j = 0; j < count; j++)
                {
                    int k = r.Next(0, count);
                    string v = answer.V[j];
                    answer.V[j] = answer.V[k];
                    answer.V[k] = v;
                    string[] s = answer.good_answer.Split(',');
                    for(int ij = 0; ij < s.Length; ij++)
                    {
                        if(s[ij] == Convert.ToString(k + 1)) { s[ij] = Convert.ToString(j + 1); }
                        if(s[ij] == Convert.ToString(j + 1)) { s[ij] = Convert.ToString(k + 1); }
                    }
                    string good_ans = "";
                    for(int ss = 0; ss < s.Length; ss++)
                    {
                        if(ss == s.Length - 1)
                        {
                            good_ans += s[ss];
                        }
                        else
                        {
                            good_ans += s[ss] + ",";
                        }
                    }
                    answer.good_answer = good_ans;  
                }
                
            
            }
            for (int i = 0; i < test_list.Count; i++)
            {
                    
                    T t = new T();
                    t = test_list[i];
                    int index = r.Next(0, test_list.Count);
                    test_list[i] = test_list[index];
                    test_list[index] = t; 
            }
        }
        int[] AnalizeAnswer()
        {
            int true_ans = 0;
            int false_ans = 0;
            for(int i = 0; i < test_list.Count; i++)
            {
                string[] good = test_list[i].good_answer.Split('|');               
                List<string> user = new List<string>(); 
                string[] u = test_list[i].user_answer.Split('|');
                for(int j = 0; j < u.Length; j++)
                {
                    if (u[j] != "") user.Add(u[j]);
                }

                if (good.Length == user.Count) 
                {
                    int count = 0;
                    for(int k = 0; k < good.Length; k++)
                    {
                        for(int t = 0; t < user.Count; t++)
                        {
                            if (good[k] == user[t]) count++;
                        }
                    }
                    if (count == good.Length) { true_ans++; } else { false_ans++; }
                }
                else { false_ans++; }
            }
            return new int[] { true_ans, false_ans };
        }
        private void Button_Click(object sender, RoutedEventArgs e) // Вперёд
        {
            if (test + 1 > test_list.Count)
            {
                T t = test_list[test - 1];
                if (t.good_answer.Split('|').Length > 1)
                {
                string s = "";
                if (CB1.IsChecked == true) { s += CB1.Content + "|"; }
                if (CB2.IsChecked == true) { s += CB2.Content + "|"; }
                if (CB3.IsChecked == true) { s += CB3.Content + "|"; }
                if (CB4.IsChecked == true) { s += CB4.Content + "|"; }
                if (CB5.IsChecked == true) { s += CB5.Content + "|"; }
                t.user_answer = s;
                }
                else
                {
                    string s = "";
                    if (RB1.IsChecked == true) { s += RB1.Content + "|"; }
                    if (RB2.IsChecked == true) { s += RB2.Content + "|"; }
                    if (RB3.IsChecked == true) { s += RB3.Content + "|"; }
                    if (RB4.IsChecked == true) { s += RB4.Content + "|"; }
                    if (RB5.IsChecked == true) { s += RB5.Content + "|"; }
                    t.user_answer = s;
                }
                

                MessageBoxResult mb = MessageBox.Show("Закончить тест?", "Конец теста", MessageBoxButton.YesNo);
                if (mb != MessageBoxResult.Yes) return;
                
                G1.Visibility = Visibility.Hidden;
                G4.Visibility = Visibility.Visible;
                int[] a = AnalizeAnswer();

                double percent = ((double)a[0] / ((double)a[0] + (double)a[1]))*100D;
                L2.Content = $"Ваш результат {Math.Round(percent,2)}% ({Math.Round(percent)/10} балла(ов))";

                PieChart pc = new PieChart();
                Func<ChartPoint,string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                SeriesCollection sc = new SeriesCollection();
                sc.Add(
                       new PieSeries
                       {
                           Title = "Правильные ответы",
                           Values = new ChartValues<int> { a[0] },
                           DataLabels = true,
                           LabelPoint = labelPoint,
                           Fill = System.Windows.Media.Brushes.Green
                       });
                sc.Add(
                       new PieSeries
                       {
                           Title = "Неправильные ответы",
                           Values = new ChartValues<int> { a[1] },
                           DataLabels = true,
                           LabelPoint = labelPoint,
                           Fill = System.Windows.Media.Brushes.Red
                       });
                pc.Series = sc;
                G5.Children.Add(pc);
                
            }
            else
            {
                T t = test_list[test - 1];
                if(t.good_answer.Split('|').Length > 1)
                {
                string s = "";
                if (CB1.IsChecked == true) { s += CB1.Content + "|"; }
                if (CB2.IsChecked == true) { s += CB2.Content + "|"; }
                if (CB3.IsChecked == true) { s += CB3.Content + "|"; }
                if (CB4.IsChecked == true) { s += CB4.Content + "|"; }
                if (CB5.IsChecked == true) { s += CB5.Content + "|"; }
                t.user_answer = s;
                }
                else
                {
                    string s = "";
                    if (RB1.IsChecked == true) { s += RB1.Content + "|"; }
                    if (RB2.IsChecked == true) { s += RB2.Content + "|"; }
                    if (RB3.IsChecked == true) { s += RB3.Content + "|"; }
                    if (RB4.IsChecked == true) { s += RB4.Content + "|"; }
                    if (RB5.IsChecked == true) { s += RB5.Content + "|"; }
                    t.user_answer = s;
                }
                
                ShowTest(++test);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Назад
        {
            if (test - 1 < 1) return;
            T t = test_list[test - 1];
            if(t.good_answer.Split('|').Length > 1)
            {
                string s = "";
                if (CB1.IsChecked == true) { s += CB1.Content + "|"; }
                if (CB2.IsChecked == true) { s += CB2.Content + "|"; }
                if (CB3.IsChecked == true) { s += CB3.Content + "|"; }
                if (CB4.IsChecked == true) { s += CB4.Content + "|"; }
                if (CB5.IsChecked == true) { s += CB5.Content + "|"; }
                t.user_answer = s;
            }
            else
            {
                string s = "";
                if (RB1.IsChecked == true) { s += RB1.Content + "|"; }
                if (RB2.IsChecked == true) { s += RB2.Content + "|"; }
                if (RB3.IsChecked == true) { s += RB3.Content + "|"; }
                if (RB4.IsChecked == true) { s += RB4.Content + "|"; }
                if (RB5.IsChecked == true) { s += RB5.Content + "|"; }
                t.user_answer = s;
            }
            ShowTest(--test);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show("Вы действительно хотите выйти в меню тестов?", "Выход", MessageBoxButton.YesNo);
            if(mb == MessageBoxResult.Yes)
            {
                this.Close();
            }
            
        }
    }
}
