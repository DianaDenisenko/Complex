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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace СканКартинки
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
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
        public Window1()
        {
            InitializeComponent();
            LoadTest();
        }

        public void LoadTest()
        {
            dt_user = Select("SELECT Наименование FROM категории WHERE Статус is null");
            for(int i = 0; i < dt_user.Rows.Count; i++)
            {
                DataRow a = dt_user.Rows[i];
                string content = a[0].ToString();
                Button btn = new Button();
                SP1.Children.Add(btn);
                btn.Content = content;
                btn.Height = 70;

                Thickness th = new Thickness();
                th.Left = 5; th.Top = 5; th.Right = 5; th.Bottom = 5; //бордеры
                btn.BorderThickness = th;
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255,255,255));

                Color c = new Color();
                c.R = 20; c.G = 35; c.B = 40; c.A = 100;    // задний фон
                btn.Background = new SolidColorBrush(c);

                Thickness t = new Thickness();
                t.Bottom = 5;
                btn.Margin = t;

                btn.FontSize = 24;
                btn.Foreground = Brushes.White;

                btn.Click += PrintTest_But_Click;
            }
        }

        private void ScrollViewer_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            SV1.LineDown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SP1.Children.RemoveRange(0, SP1.Children.Count);
            this.Close();
        }

        private void PrintTest_But_Click(object sender, RoutedEventArgs e)
        {
            SP1.Children.RemoveRange(0, SP1.Children.Count);
            G1.Visibility = Visibility.Hidden;
            G2.Visibility = Visibility.Visible;
            Button btn = (Button)sender;
            string content = btn.Content.ToString();
            dt_user = Select("SELECT ID_Категории,Наименование FROM категории WHERE Статус is null");
            int ID_cat = 0;
            for(int i = 0; i < dt_user.Rows.Count; i++)
            {
                if(content == dt_user.Rows[i][1].ToString())
                {
                    ID_cat = (int)dt_user.Rows[i][0];
                }
            }
            dt_user = Select($"SELECT Наименование,ID_Каталога FROM каталог WHERE Статус is null and ID_Категории = {ID_cat}");
            for (int i = 0; i < dt_user.Rows.Count; i++)
            {
                DataRow a = dt_user.Rows[i];
                string cont = a[0].ToString();
                Button b = new Button();
                SP2.Children.Add(b);
                b.Content = cont;
                b.Name = "I"+a[1].ToString();
                b.Height = 70;

                Thickness th = new Thickness();
                th.Left = 5; th.Top = 5; th.Right = 5; th.Bottom = 5; //бордеры
                b.BorderThickness = th;
                b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                Color c = new Color();
                c.R = 20; c.G = 35; c.B = 40; c.A = 100;    // задний фон
                b.Background = new SolidColorBrush(c);

                Thickness t = new Thickness();
                t.Bottom = 5;
                b.Margin = t;

                b.FontSize = 24;
                b.Foreground = Brushes.White;

                b.Click += LoadTest_Click; 
            }
        }
     
        private void LoadTest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string[] name = btn.Name.Split('I');
            int id = int.Parse(name[1]);
            try
            {
                Tests t = new Tests(id);
                t.ShowDialog();
            }
            catch { MessageBox.Show("Ошибка загрузки теста."); }
            
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            G1.Visibility = Visibility.Visible;
            G2.Visibility = Visibility.Hidden;
            SP2.Children.RemoveRange(0, SP2.Children.Count);
            LoadTest();
        }
    }
}
