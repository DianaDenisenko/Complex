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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloudsGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer1 = new System.Windows.Threading.DispatcherTimer();
        int accid;
        public MainWindow()
        {
            accid = 1;
            InitializeComponent();
            timer1.Tick += new EventHandler(timerTick1);
            timer1.Interval = new TimeSpan(0, 0, 0, 1);
          
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0,8);
            timer1.Start();
            Question();
         
        }
       double relativeMousePos, answerPos,cactPos;
        int score,sec;
        private void timerTick1(object sender, EventArgs e)
        {
           
            switch (RestartLabel.Content)
            {
                case "4": RestartLabel.Visibility = Visibility.Visible; RestartLabel.Content = "3";  break;
                case "3":   RestartLabel.Content = "2";  break;
                case "2": RestartLabel.Content = "1"; break;
                case "1":
                    RestartLabel.Visibility = Visibility.Hidden;
                    RestartLabel.Content = "4";
                    timer1.Stop();
                    timer.Start();
                    break;
            }

           
          
          
        }
        int correct =0;
        private void timerTick(object sender, EventArgs e)
        {
            sec++;
            if (sec == 20) { score++; sec = 0; }
          //  LB1.Content = "Score: " + score;
            Poss(Cactus1);
            Poss(Cactus2);
            Poss(Cactus3);
            Poss(Cactus4);
            relativeMousePos = Canvas.GetLeft(CloudOne);
           answerPos = Canvas.GetLeft(LB2);
            Canvas.SetLeft(CloudOne, relativeMousePos-5);
            Canvas.SetLeft(LB2, answerPos-5);
            Canvas.SetLeft(CloudTwo, relativeMousePos-5);
            Canvas.SetLeft(LB3, answerPos - 5);
            Canvas.SetLeft(CloudThree, relativeMousePos-5);
            Canvas.SetLeft(LB4, answerPos - 5);
            {
                if (Canvas.GetLeft(CloudOne) < -300)
                {
                    correct++;
                    LB1.Content = "Решено: " + correct;
                    Question();
                    Canvas.SetLeft(CloudOne, 1370);
                    Canvas.SetLeft(LB2, 1500);
                    Canvas.SetLeft(LB3, 1500);
                    Canvas.SetLeft(LB4, 1500);
                    Canvas.SetLeft(CloudTwo, 1370);
                    Canvas.SetLeft(CloudThree, 1370);
                  
                }
            }
            Check();
        }
        public void Poss(System.Windows.UIElement sender)
        {
            cactPos = Canvas.GetLeft(sender);
            Canvas.SetLeft(sender, cactPos - 5);
            if (Canvas.GetLeft(sender) < -300)
            {
                Canvas.SetLeft(sender, 1370);
            }

        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            
            var point = e.GetPosition(DragArena);
            if(Pause.Visibility != Visibility.Visible && FAIL.Visibility != Visibility.Visible)
            if (e.GetPosition(MainGrid).Y  >  104D && e.GetPosition(MainGrid).Y < 609D) {
                Canvas.SetTop(Gamer, e.GetPosition(MainGrid).Y - 67D);
            }
            
       
        }
        bool fail = false;
        public void Check()
        {
            if (Canvas.GetLeft(CloudOne) <= 55D && Canvas.GetLeft(CloudOne) >= 34D)
            {
                switch (ram)
                {
                    case 1:
                        if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudTwo)-50 && Canvas.GetTop(Gamer) <( Canvas.GetTop(CloudTwo) + 190-50))
                        {
                            Moln2.Visibility = Visibility.Visible;
                            timer.Stop();
                            fail = true;
                        }
                        else
                        {
                            if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudThree)-50 && Canvas.GetTop(Gamer) < Canvas.GetTop(CloudThree) +190-50)
                            {
                                Moln3.Visibility = Visibility.Visible;
                                timer.Stop();
                                fail = true;
                            }
                        }
                        break;
                    case 2:
                        if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudOne)-50 && Canvas.GetTop(Gamer) < Canvas.GetTop(CloudOne) + 190-50)
                        {
                            Moln1.Visibility = Visibility.Visible;
                            timer.Stop();
                            fail = true;
                        }
                        else
                        {
                            if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudThree)-50 && Canvas.GetTop(Gamer) < Canvas.GetTop(CloudThree) +190-50)
                            {
                                Moln3.Visibility = Visibility.Visible;
                                timer.Stop();
                                fail = true;
                            }
                        }
                        break;
                    case 3:
                        if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudOne)-50 && Canvas.GetTop(Gamer) < Canvas.GetTop(CloudOne) + 190 -50)
                        {
                            Moln1.Visibility = Visibility.Visible;
                            timer.Stop();
                            fail = true;
                        }
                        else
                        {
                            if (Canvas.GetTop(Gamer) > Canvas.GetTop(CloudTwo)-50 && Canvas.GetTop(Gamer) < Canvas.GetTop(CloudTwo) +190-50)
                            {
                                Moln2.Visibility = Visibility.Visible;
                                timer.Stop();
                                fail = true;
                            }
                        }
                        break;
                }
                if (fail)
                {
                    fail = false;
                    Fail1.Visibility = Visibility.Visible;
                    FAIL.Visibility = Visibility.Visible;
                    ScoreFail.Content = LB1.Content;
                    Insert($"Insert into kit (CountAnswers, Date,ID_Account) VALUES ({correct},'{DateTime.Now.ToString("yyyyMMddHHmmss")}',{accid}) "); //вызов добавления

                }
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer1.Stop();
            Pause1.Visibility = Visibility.Visible;
            Pause.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            score = 0;
            LB1.Content = "Решено: 0";
            correct = 0;
            Fail1.Visibility = Visibility.Hidden;
            FAIL.Visibility = Visibility.Hidden;
            timer1.Start();
            Question();
            Canvas.SetLeft(CloudOne, 1370);
            Canvas.SetLeft(LB2, 1500);
            Canvas.SetLeft(LB3, 1500);
            Canvas.SetLeft(LB4, 1500);
            Canvas.SetLeft(CloudTwo, 1370);
            Canvas.SetLeft(CloudThree, 1370);
            Moln1.Visibility = Visibility.Hidden;
            Moln2.Visibility = Visibility.Hidden;
            Moln3.Visibility = Visibility.Hidden;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Pause.Visibility = Visibility.Hidden;
            Pause1.Visibility = Visibility.Hidden;
            timer1.Start();
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            score = 0;
            LB1.Content = "Решено: 0";
            Pause1.Visibility = Visibility.Hidden;
            Pause.Visibility = Visibility.Hidden;
            timer1.Start();
            Question();
            Canvas.SetLeft(CloudOne, 1370);
            Canvas.SetLeft(LB2, 1500);
            Canvas.SetLeft(LB3, 1500);
            Canvas.SetLeft(LB4, 1500);
            Canvas.SetLeft(CloudTwo, 1370);
            Canvas.SetLeft(CloudThree, 1370);
            Moln1.Visibility = Visibility.Hidden;
            Moln2.Visibility = Visibility.Hidden;
            Moln3.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // DialogResult = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        Random r = new Random();
        int x1, x2, sum, clnum, clnum2, ram;
        public void Question()
        {
          
            
                 x1 = r.Next(0, 11);
                 x2 = r.Next(0, 11);
                if(score>100 && score <= 150)
                {
                    x1 = r.Next(10,30);
                    x2 = r.Next(10, 30);
                }
                if (score > 150 && score <= 300)
                {
                    x1 = r.Next(30, 101);
                    x2 = r.Next(30, 101);
                }
                if (score > 300)
                {
                    x1 = r.Next(0, 1001);
                    x2 = r.Next(0, 1001);
                }
                sum = x1 + x2;
                LB5.Content = x1 + " + " + x2 + " = ";
                ram = r.Next(1, 4);
                switch (ram){

                    case 1: 
                        LB2.Content = sum;

                    do
                    {
                        do
                        {
                            do
                            {
                                clnum = r.Next(4, 21);
                                clnum2 = r.Next(4, 21);
                                if (score > 100 && score <= 150)
                                {
                                    clnum = r.Next(15, 61);
                                    clnum2 = r.Next(4, 61);
                                }
                                if (score > 150 && score <= 300)
                                {
                                    clnum = r.Next(60, 200);
                                    clnum2 = r.Next(60, 200);
                                }
                                if (score > 300)
                                {
                                    clnum = r.Next(0, 2001);
                                    clnum2 = r.Next(0, 2001);
                                }
                            } while (clnum == sum);
                        } while (clnum2 == sum);
                    } while (clnum2 == clnum);

                    LB3.Content = clnum;
                        LB4.Content = clnum2;
                        break;  


                    case 2: 
                        LB3.Content = sum;

                    do
                    {
                        do
                        {
                            do
                            {
                                clnum = r.Next(4, 21);
                                clnum2 = r.Next(4, 21);
                                if (score > 100 && score <= 150)
                                {
                                    clnum = r.Next(15, 61);
                                    clnum2 = r.Next(4, 61);
                                }
                                if (score > 150 && score <= 300)
                                {
                                    clnum = r.Next(60, 200);
                                    clnum2 = r.Next(60, 200);
                                }
                                if (score > 300)
                                {
                                    clnum = r.Next(0, 2001);
                                    clnum2 = r.Next(0, 2001);
                                }
                            } while (clnum == sum);
                        } while (clnum2 == sum);
                    } while (clnum2 == clnum);

                    LB2.Content = clnum;
                        LB4.Content = clnum2;
                        break;
                        

                    case 3: 
                        LB4.Content = sum;
                    do
                    {
                        do
                        {
                            do
                            {
                                clnum = r.Next(4, 21);
                                clnum2 = r.Next(4, 21);
                                if (score > 100 && score <= 150)
                                {
                                    clnum = r.Next(15, 61);
                                    clnum2 = r.Next(4, 61);
                                }
                                if (score > 150 && score <= 300)
                                {
                                    clnum = r.Next(60, 200);
                                    clnum2 = r.Next(60, 200);
                                }
                                if (score > 300)
                                {
                                    clnum = r.Next(0, 2001);
                                    clnum2 = r.Next(0, 2001);
                                }
                            } while (clnum == sum );
                        } while (clnum2 == sum);
                    } while (clnum2 == clnum);

                        LB2.Content = clnum;
                        LB3.Content = clnum2;
                        break;
                }






             
        }
    }
}
