using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using CloudsGame;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using WpfApp1;
using Corners;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Events;
using СканКартинки;
namespace ГлОкно
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Authorization.Visibility = Visibility.Visible;
            Editdate.DisplayDateEnd = DateTime.Now.AddYears(-5);
            Editdate.DisplayDateStart = DateTime.Now.AddYears(-70);
            TBDATE.DisplayDateEnd = DateTime.Now.AddYears(-5);
            TBDATE.DisplayDateStart = DateTime.Now.AddYears(-70);
        }
        public void Cleaning1()
        {
            MainGrid.Visibility = Visibility.Hidden;
            MiniGames.Visibility = Visibility.Hidden;
            StatGrid.Visibility = Visibility.Hidden;
            FigStat.Visibility = Visibility.Hidden;
            KitStat.Visibility = Visibility.Hidden;
            PDF1.Text = "";
            PDF2.Text = "";
            PDF3.Text = "";
            PDF4.Text = "";
            Chart.Series.Clear();
            Chart1.Series.Clear();
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cleaning1();
            MainGrid.Visibility = Visibility.Visible;
            MiniGames.Visibility = Visibility.Hidden;
            cchart();
        }

        

        private void Label_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Cleaning1();
            MiniGames.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
           
            CloudsGame.MainWindow mw = new CloudsGame.MainWindow(Convert.ToInt32(dt_user.Rows[0][0].ToString()));
            this.Hide();
            mw.ShowDialog();
            ShowDialog();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WpfApp1.MainWindow mw1 = new WpfApp1.MainWindow();

            mw1.ShowDialog();
            ShowDialog();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Registration.Visibility = Visibility.Visible;
        }
        public byte[] ConvertImageToByte(Image img)
        {
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                var bmp = img.Source as BitmapImage;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(ms);
                arr = ms.ToArray();
            }
            return arr;
        }
        private void EnterBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите картинку для добавления в тест.";
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (ofd.ShowDialog() == true)
            {
                BitmapImage BI = new BitmapImage();
                BI.BeginInit();
                BI.UriSource = new Uri(ofd.FileName, UriKind.RelativeOrAbsolute);
                BI.EndInit();
                //ImageContainer.Width = BI.Width;
                //ImageContainer.Height = BI.Height;
                MainIMG.Source = BI;
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
                sqlCmd.CommandText = $"Update accounts set Png=@am where Login ='{login}'";
                sqlCmd.Parameters.Add("@am", MySqlDbType.VarBinary, 60000);
                sqlCmd.Parameters["@am"].Value = ConvertImageToByte(MainIMG);
                sqlConnection.Open(); // открываем базу данных
                sqlCmd.ExecuteNonQuery();
                sqlConnection.Close();
                MessageBox.Show("Фото сохранено");
            }
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
        private void EnterBut_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Registration1.Visibility = Visibility.Visible;
        }

        private void EnterBut_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Registration1.Visibility = Visibility.Hidden;
        }

        private void EnterBut_Copy_Click(object sender, RoutedEventArgs e)
        {
            Registration.Visibility = Visibility.Hidden;
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
        DataTable dt_user = new DataTable();
        public void Cleaning()
        {
            TBFAM.Clear();
            TBNAME.Clear();
            TBOTCH.Clear();
            TBDATE.Text = "";
            TBCLASS.Clear();
            REGLOG.Clear();
            REGPASS.Clear();
            Registration.Visibility = Visibility.Hidden;
            Registration1.Visibility = Visibility.Hidden;
        }
        private void EnterBut_Copy3_Click(object sender, RoutedEventArgs e)
        {
            dt_user = Select("Select Id_Account, Login from accounts where Status is NULL");
            if (TBFAM.Text != "" && TBNAME.Text != "" && TBOTCH.Text != "" && TBCLASS.Text != "" && TBDATE.Text != "" && REGLOG.Text != "" && REGLOG.Text != "")
            {
                for (int i = 0; i < dt_user.Rows.Count; i++)
                {

                    if (dt_user.Rows[i][1].ToString() == REGLOG.Text)
                    {
                        MessageBox.Show("Логин занят.");
                        REGLOG.Text = "";
                        break;
                    }
                }

                if (REGPASS.Password.Length > 7 )
                {
                    if (REGLOG.Text != "" && TBCLASS.Text =="123123" && WHO.Content.ToString() == "Ученик")
                    {
                        Insert($"Insert into accounts (Login,Password,Birthday,Name,Firstname,LastName,Type)values('{REGLOG.Text}','{REGPASS.Password.GetHashCode()}','{TBDATE.SelectedDate.Value.ToString("yyyMMdd")}','{TBNAME.Text}','{TBFAM.Text}','{TBOTCH.Text}','{"Преподаватель"}')");
                        MessageBox.Show("Регистрация прошла успешно.");
                        Cleaning();
                    }
                    else
                    {
                        if (REGLOG.Text != ""  && WHO.Content.ToString() == "Преподаватель")
                        {
                            Insert($"Insert into accounts (Login,Password,Birthday,Class,Name,Firstname,LastName)values('{REGLOG.Text}','{REGPASS.Password.GetHashCode()}','{TBDATE.SelectedDate.Value.ToString("yyyMMdd")}',{TBCLASS.Text},'{TBNAME.Text}','{TBFAM.Text}','{TBOTCH.Text}')");
                            MessageBox.Show("Регистрация прошла успешно.");
                            Cleaning();
                        }
                        else
                        {
                            if (REGLOG.Text != "")
                              MessageBox.Show("Пароль неверный.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Длина пароля должна составлять восемь или более символов.");
                }
            }
        }
        string login;
        public void pngmeth()
        {
            DataTable png = Select($"SELECT Png FROM accounts WHERE Login = '{login}' AND Png IS NOT NULL");
            if (png.Rows.Count > 0)
            {
                for (int i = 0; i < png.Rows.Count; i++)
                {
                    byte[] byte_img = (byte[])png.Rows[i][0];
                    using (MemoryStream ms = new MemoryStream(byte_img, 0, byte_img.Length))
                    {
                        ms.Write(byte_img, 0, byte_img.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = ms;
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.EndInit();
                        MainIMG.Source = bi;

                    }
                }
            }
            else
            {
                MainIMG.Source = new BitmapImage(new Uri("Res/d1cbaae7-79df-4b31-884f-3694e64f514a-profile_image-300x300.png", UriKind.Relative));

            }
        }
        private void EnterBut_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                dt_user = Select($"Select Password from accounts where Login = '{AUTOLOG.Text}'");
                if (dt_user.Rows.Count != 0)
                {
                    string f = dt_user.Rows[0][0].ToString();
                    string f1 = AUTOPASS.Password.GetHashCode().ToString();
                    if (dt_user.Rows[0][0].ToString() == AUTOPASS.Password.GetHashCode().ToString())
                    {
                        login = AUTOLOG.Text;
                        Authorization.Visibility = Visibility.Hidden; AUTOLOG.Clear(); AUTOPASS.Clear();
                        dt_user = Select($"Select Name,Firstname,Lastname,Score,Class,Png, Type from accounts where Login = '{login}'");
                        MainNameFam.Content = dt_user.Rows[0][1].ToString() + " " + dt_user.Rows[0][0] + " " + dt_user.Rows[0][2];
                        NameFam.Content = dt_user.Rows[0][1].ToString() + " " + dt_user.Rows[0][0] + " " + dt_user.Rows[0][2];
                   
                        if (dt_user.Rows[0][6].ToString() == "Преподаватель")
                        {
                            Button_For_Prepod.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Button_For_Prepod.Visibility = Visibility.Hidden;
                           
                        }
                            pngmeth();
                     
                            dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
                        int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());
                       
                        pipChartt.Series.Clear();
                        try
                        {
                            dt_user = Select($"Select CorrectAnswers, UncorrectAnswers,Time from figures where ID_Account = {id}");
                            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                            pipChartt.Series.Add(new PieSeries
                            {
                                Title = "Неправильно",
                                Fill = Brushes.Red,
                                DataLabels = true,
                                LabelPoint = labelPoint,
                                StrokeThickness = 2,
                                Values = new ChartValues<double> { Convert.ToDouble(dt_user.Rows[dt_user.Rows.Count - 1][1].ToString()) }
                            });
                            pipChartt.Series.Add(new PieSeries
                            {
                                Title = "Правильно",
                                Fill = Brushes.Green,
                                DataLabels = true,
                                LabelPoint = labelPoint,
                                StrokeThickness = 2,
                                Values = new ChartValues<double> { Convert.ToDouble(dt_user.Rows[dt_user.Rows.Count - 1][0].ToString()) }
                            });
                       
                        }
                        catch { }

                    }
                    else
                    {
                        MessageBox.Show("Проверьте логин или пароль.");
                    }

                }
                else
                {
                    MessageBox.Show("Проверьте логин или пароль.");
                }
        }
            catch { MessageBox.Show("Ошибка подключения."); }
}

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Cleaning1();
            Authorization.Visibility = Visibility.Visible;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            dt_user = Select($"Select Name, Firstname, Lastname, Class, Birthday from accounts where Login = '{login}'");
            Editname.Text = dt_user.Rows[0][0].ToString();
            Editfam.Text = dt_user.Rows[0][1].ToString();
            Edditotch.Text = dt_user.Rows[0][2].ToString();
            Editclass.Text = dt_user.Rows[0][3].ToString();
            Editdate.Text = dt_user.Rows[0][4].ToString();
            GridEdit.Visibility = Visibility.Visible;

        }

        private void EnterBut1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Сохранение", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (Editname.Text != "" && Editfam.Text != "" && Edditotch.Text != "" && Editclass.Text != "" && Editdate.Text != "")
                {
                    Update($"Update accounts set Name = '{Editname.Text}', Firstname = '{Editfam.Text}', Lastname='{Edditotch.Text}',Class='{Editclass.Text}', Birthday = '{Editdate.SelectedDate.Value.ToString("yyyMMdd")}' where Login ='{login}'");
                    GridEdit.Visibility = Visibility.Hidden; 
                    dt_user = Select($"Select Name,Firstname,Lastname,Score,Class,Birthday,Png from accounts where Login = '{login}'");
                    MainNameFam.Content = dt_user.Rows[0][1].ToString() + " " + dt_user.Rows[0][0] + " " + dt_user.Rows[0][2];
                    NameFam.Content = dt_user.Rows[0][1].ToString() + " " + dt_user.Rows[0][0] + " " + dt_user.Rows[0][2];
                   

                  
                }
                else
                {
                    MessageBox.Show("Введите все данные");
                }
            }
        }
        public void cchart()
        {
            try
            {
                dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
                int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());

                pipChartt.Series.Clear();
                dt_user = Select($"Select CorrectAnswers, UncorrectAnswers,Time from figures where ID_Account = {id}");
                Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                pipChartt.Series.Add(new PieSeries
                {
                    Title = "Неправильно",
                    Fill = Brushes.Red,
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    StrokeThickness = 2,
                    Values = new ChartValues<double> { Convert.ToDouble(dt_user.Rows[dt_user.Rows.Count - 1][1].ToString()) }
                });
                pipChartt.Series.Add(new PieSeries
                {
                    Title = "Правильно",
                    Fill = Brushes.Green,
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    StrokeThickness = 2,
                    Values = new ChartValues<double> { Convert.ToDouble(dt_user.Rows[dt_user.Rows.Count - 1][0].ToString()) }
                });
            }
            catch { }
        }
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

        private void EnterBut1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            GridEdit.Visibility = Visibility.Hidden;
        }

        private void EnterBut1_Copy3_Click(object sender, RoutedEventArgs e)
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

            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = sqlConnection;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = $"Update accounts set Png=NULL where Login ='{login}'";
         
     
            sqlConnection.Open(); // открываем базу данных
            sqlCmd.ExecuteNonQuery();
            sqlConnection.Close();
            MainIMG.Source = new BitmapImage(new Uri("Res/d1cbaae7-79df-4b31-884f-3694e64f514a-profile_image-300x300.png", UriKind.Relative));
        }

        private void closer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void remover_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }

            }
            catch { }
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
          
            dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
            Corners.MainWindow mw = new Corners.MainWindow(Convert.ToInt32(dt_user.Rows[0][0].ToString()));
            this.Hide();
            mw.ShowDialog();
            ShowDialog();
        }

        private void Label_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Программа разработана Ефремовым Владиславом и Денисенко Дианой, группа ПО-32");
        }

        private void pipChart_DataClick(object sender, ChartPoint chartPoint)
        {
            dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
            int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());


            dt_user = Select($"Select  Time from figures where ID_Account = {id}");
            MessageBox.Show(dt_user.Rows[dt_user.Rows.Count - 1][0].ToString());
        }
        bool ch = false;
        private void Window_Initialized(object sender, EventArgs e)
        {
            if (!ch)
            {
                cchart();
            }
            ch = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Cleaning1();
            FigStat.Visibility = Visibility.Visible;
        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PDF1.Text != "" && PDF2.Text != "")
                {
                    Chart.Series.Clear();
                    dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
                    int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());

                    dt_user = Select($"Select CorrectAnswers, UnCorrectAnswers, Date,Time from figures WHERE ID_Account = {id} AND Date BETWEEN '{PDF1.SelectedDate.Value.ToString("yyyMMdd")}' AND {PDF2.SelectedDate.Value.ToString("yyyMMdd")}");
                    List<int> mas = new List<int>();
                    List<DateTime> lt = new List<DateTime>();
                    for (DateTime i = PDF1.SelectedDate.Value; i <= PDF2.SelectedDate.Value; i = i.AddDays(1))
                    {
                        lt.Add(i);
                    }

                    for (int j = 0; j < lt.Count; j++)
                    {
                        for (int i = 0; i < dt_user.Rows.Count; i++)
                        {
                            if (lt[j].ToString("yyyyMMdd") == Convert.ToDateTime(dt_user.Rows[i][2].ToString()).ToString("yyyyMMdd"))
                            {
                                mas.Add(Convert.ToInt32(dt_user.Rows[i][0].ToString()));
                            }
                        }

                        Chart.Series.Add(new LineSeries
                        {

                            Values = new ChartValues<int>(mas),
                            StrokeThickness = 6,
                            Title = lt[j].ToString("d")
                        });

                        mas.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Введите все даты.");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к сети.");
            }
           
        }

        private void Button_For_Prepod_Click(object sender, RoutedEventArgs e)
        {
            Configurator conf = new Configurator();
            this.Hide();
            conf.ShowDialog();
            ShowDialog();
        }

        private void Label_MouseDown_4(object sender, MouseButtonEventArgs e)
                    {
            Cleaning1();
            StatGrid.Visibility = Visibility.Visible;
        }

        private void pipChartt_Copy_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PDF3.Text != "" && PDF4.Text != "")
                {
                    Chart1.Series.Clear();
                    dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
                    int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());

                    dt_user = Select($"Select CountAnswers, Date from kit WHERE ID_Account = {id} AND Date BETWEEN '{PDF3.SelectedDate.Value.ToString("yyyMMdd")}' AND {PDF4.SelectedDate.Value.ToString("yyyMMdd")}");
                    List<int> mas = new List<int>();
                    List<DateTime> lt = new List<DateTime>();
                    for (DateTime i = PDF3.SelectedDate.Value; i <= PDF4.SelectedDate.Value; i = i.AddDays(1))
                    {
                        lt.Add(i);
                    }

                    for (int j = 0; j < lt.Count; j++)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (lt[j].ToString("yyyyMMdd") == Convert.ToDateTime(dt_user.Rows[i][1].ToString()).ToString("yyyyMMdd"))
                            {
                                mas.Add(i);
                            }
                        }

                        Chart1.Series.Add(new LineSeries
                        {

                            Values = new ChartValues<int>(mas),
                            StrokeThickness = 6,
                            Title = lt[j].ToString("d")
                        });

                        mas.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Введите все даты.");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к сети.");
            }
        }
    
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Cleaning1();
            KitStat.Visibility = Visibility.Visible;
        }

        private void WHO_Click(object sender, RoutedEventArgs e)
        {
            switch (WHO.Content)
            {
               case "Ученик":
                    LBCL.Content = "Класс";
                    WHO.Content = "Преподаватель"; break;
               case "Преподаватель":
                    LBCL.Content = "Пароль";
                    WHO.Content = "Ученик"; break;
            }
          
        }

        private void TBNAME_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inp = e.Text[0];
            if (!Char.IsLetter(inp))
                e.Handled = true;
            if ((sender as TextBox).Text.Length == 1)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.ToUpper();
                (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
            }
        }

        private void TBCLASS_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inp = e.Text[0];
            if (!char.IsControl(inp) && !char.IsDigit(inp))
            {
                e.Handled = true;
            }
        }

        private void Label_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            dt_user = Select($"Select ID_Account from accounts where Login = '{login}'");
            int id = Convert.ToInt32(dt_user.Rows[0][0].ToString());

            СканКартинки.Window1 conf = new СканКартинки.Window1(id);
            this.Hide();
            conf.ShowDialog();
            ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           
            СканКартинки.Stat conf = new СканКартинки.Stat();
            this.Hide();
            conf.ShowDialog();
            ShowDialog();
        }
    }
}
