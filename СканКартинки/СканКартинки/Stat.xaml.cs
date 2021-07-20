using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
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

namespace СканКартинки
{
    /// <summary>
    /// Логика взаимодействия для Stat.xaml
    /// </summary>
    public partial class Stat : Window
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
        DataTable student;
        DataTable dt_user;
        DataTable dt_user1;
        public Stat()
        {
            InitializeComponent();

            DG1.CanUserAddRows = false;
            DG1.CanUserDeleteRows = false;
            DG1.CanUserResizeColumns = false;
            DG1.CanUserResizeRows = false;
            DG1.CanUserReorderColumns = false;
            DG1.IsReadOnly = true;

            DG2.CanUserAddRows = false;
            DG2.CanUserDeleteRows = false;
            DG2.CanUserResizeColumns = false;
            DG2.CanUserResizeRows = false;
            DG2.CanUserReorderColumns = false;
            DG2.IsReadOnly = true;
        }
        void LoadTable()
        {
            for (int i_for = 0; i_for < 3; i_for++)
            {
                if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                try
                {
                    student = Select("SELECT ID_Account,Firstname as Фамилия,Name as Имя,Lastname as Отчество,Class as Класс FROM accounts WHERE Status is null and Type is null");
                    break;
                }
                catch { }
            }
            DG1.ItemsSource = student.DefaultView;
            DG1.Columns[0].Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            G2.Visibility = Visibility.Hidden;
            LoadTable();
        }

        private void DG1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             try
             {
                DataRowView row = (DataRowView)DG1.Items[DG1.SelectedIndex];
                int ID_Student = (int)row[0];
                for (int i_for = 0; i_for < 3; i_for++)
                {
                    if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                    try
                    {
                       dt_user = Select($"" +
$"SELECT distinct ответы_юзера.ID_Ответа,каталог.Наименование as Тест, ответы_юзера.ДатаПрохождения " +
$"FROM ответы_юзера,ответы,тесты,каталог " +
$"WHERE ID_Юзера = {ID_Student} and ответы_юзера.ID_Ответа = ответы.ID_Прохождения and тесты.ID_Теста = ответы.ID_Теста and каталог.ID_Каталога = тесты.ID_Каталог");
                        break;
                    }
                    catch { }
                }
                DG2.ItemsSource = dt_user.DefaultView;
                DG2.Columns[0].Visibility = Visibility.Hidden;
             }
             catch { MessageBox.Show("Ошибка. Повторите операцию.");}
        }

        private void DG2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SP1.Children.RemoveRange(0, SP1.Children.Count);
                DataRowView row = (DataRowView)DG2.Items[DG2.SelectedIndex];
                int ID_Ans = (int)row[0];
                for (int i_for = 0; i_for < 3; i_for++)
                {
                    if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету."); return; }
                    try
                    {
                        dt_user = Select($"" +
 $"SELECT тесты.Заголовок,тесты.Вариант1,тесты.Вариант2,тесты.Вариант3,тесты.Вариант4,тесты.Вариант5,тесты.Ответы,ответы.Ответ " +
 $"FROM ответы,тесты " +
 $"WHERE ответы.ID_Прохождения = {ID_Ans} and тесты.ID_Теста = ответы.ID_Теста and тесты.Статус is null");
                        break;
                    }
                    catch { MessageBox.Show("запрос не робит");  }
                }
                int good_ans_count = (int)Select($"select КолвоОтветов FROM ответы_юзера WHERE ID_Ответа = {ID_Ans}").Rows[0][0];

                PieChart pc = new PieChart();
                Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                SeriesCollection sc = new SeriesCollection();
                sc.Add(
                       new PieSeries
                       {
                           Title = "Правильные ответы",
                           Values = new ChartValues<int> { good_ans_count },
                           DataLabels = true,
                           LabelPoint = labelPoint,
                           Fill = System.Windows.Media.Brushes.Green
                       });
                sc.Add(
                       new PieSeries
                       {
                           Title = "Неправильные ответы",
                           Values = new ChartValues<int> { dt_user.Rows.Count-good_ans_count },
                           DataLabels = true,
                           LabelPoint = labelPoint,
                           Fill = System.Windows.Media.Brushes.Red
                       });
                pc.Series = sc;
                pc.Height = 200;
                pc.Width = 200;
                SP1.Children.Add(pc);
                double percent = ((double)good_ans_count / ((double)dt_user.Rows.Count)) * 100D;
                Label L2 = new Label();
                L2.Content = $"Результат {Math.Round(percent, 2)}% ({Math.Round(percent) / 10} балла(ов))";
                L2.HorizontalContentAlignment = HorizontalAlignment.Center;
                L2.FontSize = 24;
                L2.Foreground = Brushes.White;
                SP1.Children.Add(L2);
                for (int i = 0; i < dt_user.Rows.Count; i++)
                    {

                        DataRow dr = dt_user.Rows[i];
                        Grid g = new Grid();
                        g.Height = 500;
                        TextBlock tb = new TextBlock();
                        tb.TextWrapping = TextWrapping.Wrap;
                        tb.FontSize = 20;
                        tb.Text = dr[0].ToString();
                        tb.Margin = new Thickness(0,30,0,0);
                        tb.Foreground = Brushes.White;
                        SP1.Children.Add(tb);
                        string[] good_ans = dr[6].ToString().Split('|');
                        string[] user_ans = dr[7].ToString().Split('|');
                        for (int j = 1; j < 6; j++)
                        {
                            Label ans = new Label();
                            if (dr[j].ToString() != "")
                            {
                                ans.FontSize = 16;
                                ans.Foreground = Brushes.White;
                                ans.Content = dr[j].ToString();
                                foreach (string u_a in user_ans)
                                {
                                    if (u_a == ans.Content.ToString()) { ans.Background = Brushes.Red; }
                                }
                                foreach (string a in good_ans)
                                {
                                    if (a == ans.Content.ToString()) { ans.Background = Brushes.Green; }
                                }
                                SP1.Children.Add(ans);
                            }
                        }

                    }
                

            }
            catch { MessageBox.Show("Ошибка. Повторите операцию."); }
        }
    }
}
