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
    /// Логика взаимодействия для Configurator.xaml
    /// </summary>
    public partial class Configurator : Window
    {
        public void Update(string sqlc) // функция подключения к базе данных и обработка запросов
        {
            // создаём таблицу в приложении
            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "mysql.j8135376.myjino.ru",
                Database = "j8135376",
                UserID = "j8135376",
                Password = "fwFU3t4ks",
                OldGuids = true
            };

            MySqlConnection sqlConnection = new MySqlConnection(mySqlConnectionStringBuilder.ToString());

            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = sqlConnection;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = sqlc;
            sqlConnection.Open(); // открываем базу данных
            sqlCmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void Insert(string sqlc) // функция подключения к базе данных и обработка запросов
        {
            // создаём таблицу в приложении
            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "mysql.j8135376.myjino.ru",
                Database = "j8135376",
                UserID = "j8135376",
                Password = "fwFU3t4ks",
                OldGuids = true
            };

            MySqlConnection sqlConnection = new MySqlConnection(mySqlConnectionStringBuilder.ToString());

            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = sqlConnection;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = sqlc;
            sqlConnection.Open(); // открываем базу данных
            sqlCmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
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
        DataTable categories;
        DataTable catalog;
        public Configurator()
        {
            InitializeComponent();
          
            Categories();
        }
        int ID_Categories;
        int ID_Catalog;
        int ID_Test;
        int mode = 1 ;
        
        void Categories()
        {
            SP1.Children.RemoveRange(0,SP1.Children.Count);
            for(int i_for = 0; i_for < 3; i_for++) {
                if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                try
                {
                    categories = Select("SELECT ID_Категории,Наименование,Статус FROM категории");
                    break;
                }
                catch { }
                
            }
            L1.Content = "Категории";
            for (int i = 0; i < categories.Rows.Count; i++)
            {
                DataRow a = categories.Rows[i];
                string content = a[1].ToString() + a[2].ToString();
                Button btn = new Button();
                SP1.Children.Add(btn);
                btn.Content = content;
                btn.Height = 70;
                Thickness th = new Thickness();
                th.Left = 5; th.Top = 5; th.Right = 5; th.Bottom = 5;
                btn.BorderThickness = th;
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Color c = new Color();
                c.R = 20; c.G = 35; c.B = 40; c.A = 100;   
                btn.Background = new SolidColorBrush(c);
                Thickness t = new Thickness();
                t.Bottom = 5;
                btn.Margin = t;
                btn.FontSize = 24;
                btn.Foreground = Brushes.White;
                btn.Name = "C"+a[0].ToString();
                btn.Click += ShowCatalog_Click;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SP1.Children.Clear();
            if(L1.Content.ToString() == "Категории")Categories();
            if(L1.Content.ToString() == "Каталог")ShowCatalog(ID_Categories);
            if(L1.Content.ToString() == "Тесты") ShowTest(ID_Catalog);            
        }
        
        private void ShowCatalog_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ID_Categories= int.Parse(btn.Name.Split('C')[1]);
            if(mode == 1)
            { 
                SP1.Children.Clear();
                L1.Content = "Каталог";
                ShowCatalog(ID_Categories);
            }
            if(mode == 2)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите удалить запись:\n\"{btn.Content}\"?", "Удаление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes) 
                {
                    try
                    {
                        Update($"update категории set Статус = '(Удален)' where ID_Категории = {ID_Categories}");
                        Categories();
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
            if(mode == 3)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите восстановить запись:\n\"{btn.Content}\"?", "Восстановление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        Update($"update категории set Статус = NULL where ID_Категории = {ID_Categories}");
                        Categories();
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
        }
        void ShowCatalog(int ID_C)
        {
            SP1.Children.RemoveRange(0, SP1.Children.Count);
            for (int i_for = 0; i_for < 3; i_for++)
            {
                if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                try
                {
                    catalog = Select($"SELECT ID_Каталога,Наименование,Статус FROM каталог WHERE ID_Категории = {ID_C}");
                    break;
                }
                catch { }
            }
            L1.Content = "Каталог";
            for (int i = 0; i < catalog.Rows.Count; i++)
            {
                DataRow a = catalog.Rows[i];
                string content = a[1].ToString() + a[2].ToString();
                Button btn = new Button();
                SP1.Children.Add(btn);
                btn.Content = content;
                btn.Height = 70;
                Thickness th = new Thickness();
                th.Left = 5; th.Top = 5; th.Right = 5; th.Bottom = 5;
                btn.BorderThickness = th;
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Color c = new Color();
                c.R = 20; c.G = 35; c.B = 40; c.A = 100;
                btn.Background = new SolidColorBrush(c);
                Thickness t = new Thickness();
                t.Bottom = 5;
                btn.Margin = t;
                btn.FontSize = 24;
                btn.Foreground = Brushes.White;
                btn.Name = "K" + a[0].ToString();
                btn.Click += ShowTest_Click;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mode = 1;L3.Content = "Обычный";
            L3.Foreground = Brushes.White;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mode = 2; L3.Content = "Удаление";
            L3.Foreground = Brushes.Red;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            mode = 3; L3.Content = "Восстановление";
            L3.Foreground = Brushes.Green;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (L1.Content.ToString() == "Категории") 
            {
                G2.Visibility = Visibility.Visible;
                L4.Content = "Добавить категорию";
            }
            if (L1.Content.ToString() == "Каталог")
            {
                G2.Visibility = Visibility.Visible;
                L4.Content = "Добавить каталог";
            }
            if(L1.Content.ToString() == "Тесты")
            {
                MainWindow create = new MainWindow(ID_Catalog, false);
                create.ShowDialog();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            G2.Visibility = Visibility.Hidden;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (L1.Content.ToString() == "Категории")
            {
                if (TB1.Text.Length != 0)
                {
                    try
                    {
                        Insert($"INSERT into категории(Наименование) values ('{TB1.Text.ToString()}')");
                        MessageBox.Show("Новая категория успешно создана.");
                        G2.Visibility = Visibility.Hidden;
                        Categories();
                    }
                    catch { MessageBox.Show("Ошибка создания новой категории."); }
                }
                else { MessageBox.Show("Некорректно заполнено поле ввода"); }
            }
            if (L1.Content.ToString() == "Каталог")
            {
                if(TB1.Text.Length != 0)
                {
                    try
                    {
                        Insert($"insert into каталог(Наименование,ID_Категории) values('{TB1.Text}',{ID_Categories})");
                        MessageBox.Show("Новый каталог успешно создан.");
                        G2.Visibility = Visibility.Hidden;
                        ShowCatalog(ID_Categories);
                    }
                    catch { MessageBox.Show("Ошибка создания нового каталога"); }
                }
            }

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (L1.Content.ToString() == "Категории")
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите выйти в главное меню?", "Выход", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    this.Close();
                }
                else return;
            }
            if(L1.Content.ToString() == "Каталог")
            {
                Categories();
            }
            if(L1.Content.ToString() == "Тесты")
            {
                ShowCatalog(ID_Categories);
            }
        }

        private void ShowTest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ID_Catalog = int.Parse(btn.Name.Split('K')[1]);
            if (mode == 1)
            {
                SP1.Children.Clear();
                L1.Content = "Каталог";
                ShowTest(ID_Catalog);
            }
            if (mode == 2)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите удалить запись:\n\"{btn.Content}\"?", "Удаление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        Update($"update каталог set Статус = '(Удален)' where ID_Каталога = {ID_Catalog}");
                        ShowCatalog(ID_Categories);
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
            if (mode == 3)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите восстановить запись:\n\"{btn.Content}\"?", "Восстановление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        Update($"update каталог set Статус = NULL where ID_Категории = {ID_Categories}");
                        ShowCatalog(ID_Categories);
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
        }

        void ShowTest(int ID_Cat)
        {
            SP1.Children.RemoveRange(0, SP1.Children.Count);
            for (int i_for = 0; i_for < 3; i_for++)
            {
                if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                try
                {
                    categories = Select($"SELECT ID_Теста,Заголовок,Статус FROM тесты WHERE ID_Каталог = {ID_Cat}");
                    break;
                }
                catch { }
            }
            L1.Content = "Тесты";
            for (int i = 0; i < categories.Rows.Count; i++)
            {
                DataRow a = categories.Rows[i];
                string a1 = "";
                if(a[1].ToString().Length < 30) { a1 = a[1].ToString()+"..."; } else
                {
                    for (int j = 0; j < 30; j++) a1 += a[1].ToString()[j];
                    a1 += "...";
                }
                string content = a1 + a[2].ToString();
                Button btn = new Button();
                SP1.Children.Add(btn);
                btn.Content = content;
                btn.Height = 70;
                Thickness th = new Thickness();
                th.Left = 5; th.Top = 5; th.Right = 5; th.Bottom = 5;
                btn.BorderThickness = th;
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Color c = new Color();
                c.R = 20; c.G = 35; c.B = 40; c.A = 100;
                btn.Background = new SolidColorBrush(c);
                Thickness t = new Thickness();
                t.Bottom = 5;
                btn.Margin = t;
                btn.FontSize = 24;
                btn.Foreground = Brushes.White;
                btn.Name = "T" + a[0].ToString();
                btn.Click += CreateTest_Click;
            }
        }
        private void CreateTest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ID_Test= int.Parse(btn.Name.Split('T')[1]);
            if (mode == 1)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите открыть тест для редактирования?", "Редактирование", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        MainWindow RedactTest = new MainWindow(ID_Test, true);
                        RedactTest.ShowDialog();
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }               
                }
                             
            }
            if (mode == 2)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите удалить запись:\n\"{btn.Content}\"?", "Удаление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        Update($"update тесты set Статус = '(Удален)' where ID_Теста = {ID_Test}");
                        ShowTest(ID_Catalog);
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
            if (mode == 3)
            {
                MessageBoxResult mb = MessageBox.Show($"Вы действительно хотите восстановить запись:\n\"{btn.Content}\"?", "Восстановление", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    try
                    {
                        Update($"update тесты set Статус = NULL where ID_Теста = {ID_Test}");
                        ShowTest(ID_Test);
                    }
                    catch { MessageBox.Show("Ошибка проведения операции."); }
                }
                else { return; }
            }
        }
    }
}
