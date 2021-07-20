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
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;


namespace Corners
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        DateTime now = DateTime.Now;
        DateTime end = DateTime.Now;
        int accid;
        public MainWindow()
        {
            accid = 1;
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
            DateTime now = DateTime.Now;
            DateTime end = DateTime.Now;
            InitializeComponent();
            MainGrid.Children.Add(ln);
            Panel.SetZIndex(ln, -1);
            Random r = new Random();
            int k = r.Next(1, 9);
            switch (k)
            {
                case 1: s = "PrTreug"; TregGrid.Visibility = Visibility.Visible; break;
                case 2: s = "OstrTreug"; TregGridOstr.Visibility = Visibility.Visible; break;
                case 3: s = "TupTreug"; TregGridTup.Visibility = Visibility.Visible; break;
                case 4: s = "RavnTreug"; TregGridRavnob.Visibility = Visibility.Visible; break;
                case 5: s = "RavnostTreug"; TregGridRavnost.Visibility = Visibility.Visible; break;
                case 6: s = "Kvadrat"; KvGrid.Visibility = Visibility.Visible; break;
                case 7: s = "Pryam"; PryamGrid.Visibility = Visibility.Visible; break;
              
                case 8: s = "Trap"; TrGrid.Visibility = Visibility.Visible; break;

            }
            mas[0] = k;
            
        }
        int corr = 0; int  uncorr = 0;
        private void timerTick(object sender, EventArgs e)
        {
            //end.AddSeconds(1);
            //sec++;
        }


        int[] mas = new int[8];
        List<Point> markedPoints = new List<Point>(); 
        Line ln = new Line();
        public void Hiding()
        {
            TBKv.Visibility = Visibility.Hidden;
            TBOstr.Visibility = Visibility.Hidden;
            TBPr.Visibility = Visibility.Hidden;
            TBPrTreg.Visibility = Visibility.Hidden;
            TBRavn.Visibility = Visibility.Hidden;
            TBRavnost.Visibility = Visibility.Hidden;
            TBTr.Visibility = Visibility.Hidden;
            TBTup.Visibility = Visibility.Hidden;

            GridError.Visibility = Visibility.Visible;
        }
        public void HidingError()
        {
            TBKv.Visibility = Visibility.Hidden;
            TBOstr.Visibility = Visibility.Hidden;
            TBPr.Visibility = Visibility.Hidden;
            TBPrTreg.Visibility = Visibility.Hidden;
            TBRavn.Visibility = Visibility.Hidden;
            TBRavnost.Visibility = Visibility.Hidden;
            TBTr.Visibility = Visibility.Hidden;
            TBTup.Visibility = Visibility.Hidden;
            GridError.Visibility = Visibility.Hidden;
        }
        Polygon MyPolygon = new Polygon();
        private void Canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Panel.SetZIndex(MyPolygon, -1);
            ln.X1 = ln.X2;
            ln.Y1 = ln.Y2;
           
            LB.Content = s;

            switch (s)
            {
                case "PrTreug":
                    if (markedPoints.Count == 3)
                    {
                       
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black;
                        MainGrid.Children.Add(MyPolygon);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));
                        

                        if (Math.Sqrt(a * a + b * b) + 3 >= c && Math.Sqrt(a * a + b * b) - 3 <= c || Math.Sqrt(a * a + c * c) + 3 >= b && Math.Sqrt(a * a + c * c) - 3 <= b || Math.Sqrt(c * c + b * b) + 3 >= a && Math.Sqrt(c * c + b * b) - 3 <= a)
                        {
                            if (a != 0 && b != 0 && c != 0)
                                {
                                
                                TregGridFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                                }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear(); MainGrid.Children.Remove(MyPolygon);
                                TBPrTreg.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBPrTreg.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "Trap":
                    if (markedPoints.Count == 4)
                    {
                        
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[3].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[3].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));
                        double d = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[3].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[3].Y, 2));
                        double[,] vect = new double[2, 4];
                        vect[0, 0] = markedPoints[1].X - markedPoints[0].X;
                        vect[1, 0] = markedPoints[1].Y - markedPoints[0].Y;
                        vect[0, 1] = markedPoints[3].X - markedPoints[2].X;
                        vect[1, 1] = markedPoints[3].Y - markedPoints[2].Y;

                        vect[0, 2] = markedPoints[2].X - markedPoints[1].X;
                        vect[1, 2] = markedPoints[2].Y - markedPoints[1].Y;
                        vect[0, 3] = markedPoints[3].X - markedPoints[0].X;
                        vect[1, 3] = markedPoints[3].Y - markedPoints[0].Y;
            
                        if ( (vect[0,0]/vect[0,1] + 1.3 >= vect[1,0]/vect[1,1] && vect[0, 0] / vect[0, 1] - 1.3 <= vect[1, 0] / vect[1, 1]) ||  (vect[0, 2] / vect[0, 3] + 1.3 >= vect[1, 2] / vect[1, 3] && vect[0, 2] / vect[0, 3] - 1.3 <= vect[1, 2] / vect[1, 3]))
                        {
                            if ((vect[0, 0] / vect[0, 1] + 1.3 >= vect[1, 0] / vect[1, 1] && vect[0, 0] / vect[0, 1] - 1.3 <= vect[1, 0] / vect[1, 1]))
                                {
                                double S = ((a + b) / 2) * Math.Sqrt(Math.Pow(vect[0, 0] - vect[0, 1], 2) + Math.Pow(vect[1, 0] - vect[1, 1], 2));
                      
                                if (!(vect[0, 0] / vect[0, 2] + 1.5 >= vect[1, 0] / vect[1, 2] && vect[0, 0] / vect[0, 2] - 1.5 <= vect[1, 0] / vect[1, 2]) && !(vect[0, 0] / vect[0, 3] + 1.5 >= vect[1, 0] / vect[1, 3] && vect[0, 0] / vect[0,3] - 1.5 <= vect[1, 0] / vect[1,3]))
                                {
                                    TrGridFinal.Visibility = Visibility.Visible;
                                    MyPolygon.Points.Clear();
                                    MainGrid.Children.Remove(MyPolygon);
                                }
                                else
                                {
                                    Hiding(); MyPolygon.Points.Clear(); MainGrid.Children.Remove(MyPolygon);
                                    TBTr.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            { if ((vect[0, 2] / vect[0, 3] + 1.3 >= vect[1, 2] / vect[1, 3] && vect[0, 2] / vect[0, 3] - 1.3 <= vect[1, 2] / vect[1, 3]))
                                {
                                    double S = ((c + d) / 2) * Math.Sqrt(Math.Pow(vect[0, 2] - vect[0, 3], 2) + Math.Pow(vect[1, 2] - vect[1, 3], 2));
                                  
                                    if (!(vect[0, 0] / vect[0, 2] + 1.5 >= vect[1, 0] / vect[1, 2] && vect[0, 0] / vect[0, 2] - 1.5 <= vect[1, 0] / vect[1, 2]) && !(vect[0, 0] / vect[0, 3] + 1.5 >= vect[1, 0] / vect[1, 3] && vect[0, 0] / vect[0, 3] - 1.5 <= vect[1, 0] / vect[1, 3]))
                                    {
                                        TrGridFinal.Visibility = Visibility.Visible;
                                        MyPolygon.Points.Clear();
                                        MainGrid.Children.Remove(MyPolygon);
                                    }
                                    else
                                    {
                                        Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                        TBTr.Visibility = Visibility.Visible;
                                    }
                                } else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBTr.Visibility = Visibility.Visible;
                            }
                            }

                        }
                        else
                        {
                            bool flag = false;
                           for(int i = 0; i < 2; i++)
                            {
                                for(int j = 0; j < 3; j+=2)
                                {
                                    if (vect[i, j] + 2 >= 0 && vect[i, j] - 2 <= 0)
                                    {
                                        if (vect[i, j + 1] + 2 >= 0 && vect[i, j +1] - 2 <= 0)
                                        {
                                            if (j == 0  && vect[i, 2] + 2 <= 0 && vect[i, 2] >= 0 && vect[i, 3] + 2 <= 0 && vect[i, 3] >= 0)
                                            {
                                                TrGridFinal.Visibility = Visibility.Visible;
                                                MyPolygon.Points.Clear();
                                                MainGrid.Children.Remove(MyPolygon);
                                                flag = true;
                                            }
                                            else
                                            {
                                                if(vect[i, 0] + 2 <= 0 && vect[i, 0] >= 0 && vect[i, 1] + 2 <= 0 && vect[i, 1] >= 0)
                                                {
                                                    TrGridFinal.Visibility = Visibility.Visible;
                                                    MyPolygon.Points.Clear();
                                                    MainGrid.Children.Remove(MyPolygon);
                                                    flag = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!flag)
                            {
                                 
                                    Hiding(); MyPolygon.Points.Clear(); MainGrid.Children.Remove(MyPolygon);
                                    TBTr.Visibility = Visibility.Visible;
                                
                            }
                        }
                    }
                    break;
                case "RavnTreug":
                    if (markedPoints.Count == 3)
                    {
                        
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon); Panel.SetZIndex(MyPolygon, -1);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));
                        

                        if ((a+3>=b&&a-3<=b && !(a+3>=c && a-3<=c))||(c+3>=b&&c-3<=b && !(c+3>=a && c-3<=a))||(a+3>=c&&a-3<=c && !(a+3>=b && a-3<=b)))
                        {
                            if (a != 0 && b != 0 && c != 0)
                                {
                                TregGridRavnobFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBRavn.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBRavn.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "RavnostTreug":
                    if (markedPoints.Count == 3)
                    {
                       
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));
                       

                        if ((a+4>=b&&a-4<=b && (a+4>=c && a-4<=c))||(c+4>=b&&c-4<=b && (c+4>=a && c-4<=a))||(a+4>=c&&a-4<=c && (a+4>=b && a-4<=b)))
                        {
                            if (a != 0 && b != 0 && c != 0)
                                {
                                
                                TregGridRavnostFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBRavnost.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBRavnost.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "TupTreug":
                    if (markedPoints.Count == 3)
                    {
                       
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));


                        if (a * a + c * c < b * b || a * a + b * b < c * c || b * b + c * c < a * a)
                        {
                            if (a != 0 && b != 0 && c != 0)
                            {
                                TregGridTupFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBTup.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBTup.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "OstrTreug":
                    if (markedPoints.Count == 3)
                    {
                       
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));


                        if (a * a + c * c > b * b && a * a + b * b > c * c && b * b + c * c > a * a)
                        {
                            if (a != 0 && b != 0 && c != 0)
                            {
                                TregGridOstrFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBOstr.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBOstr.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "Kvadrat":
                    if (markedPoints.Count == 4)
                    {
                     
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double d1 = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double d2 = Math.Sqrt(Math.Pow(markedPoints[1].X - markedPoints[3].X, 2) + Math.Pow(markedPoints[1].Y - markedPoints[3].Y, 2));
                        double a = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[1].Y, 2));
                        double b = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[3].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[3].Y, 2));
                        double c = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[1].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[1].Y, 2));
                        double d = Math.Sqrt(Math.Pow(markedPoints[2].X - markedPoints[3].X, 2) + Math.Pow(markedPoints[2].Y - markedPoints[3].Y, 2));


                      
                        if (d1+3>=d2&&d1-3<=d2)
                        {
                            if (d1 != 0 && a+3>=b &&a-3<=b&& a+3>=c &&a-3<=c&& a+3>=d &&a-3<=d)
                            {
                                KvGridFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBKv.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBKv.Visibility = Visibility.Visible;
                        }
                    }
                    break;
                case "Pryam":
                    if (markedPoints.Count == 4)
                    {
                 
                        MyPolygon.Points = new PointCollection(markedPoints);
                        MyPolygon.Stroke = Brushes.Black; MainGrid.Children.Add(MyPolygon);
                        double d1 = Math.Sqrt(Math.Pow(markedPoints[0].X - markedPoints[2].X, 2) + Math.Pow(markedPoints[0].Y - markedPoints[2].Y, 2));
                        double d2 = Math.Sqrt(Math.Pow(markedPoints[1].X - markedPoints[3].X, 2) + Math.Pow(markedPoints[1].Y - markedPoints[3].Y, 2));
                        if (d1+3>=d2&&d1-3<=d2)
                        {
                            if (d1 != 0 && d2 != 0)
                            {
                                PryamGridFinal.Visibility = Visibility.Visible;
                                MyPolygon.Points.Clear();
                                MainGrid.Children.Remove(MyPolygon);
                            }
                            else
                            {
                                Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                                TBPr.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            Hiding();   MyPolygon.Points.Clear();    MainGrid.Children.Remove(MyPolygon);
                            TBPr.Visibility = Visibility.Visible;
                        }
                    }
                    break;
            }
          


            //if (flag)
            //{
            //ln.X2 = e.GetPosition((Canvas)sender).X;
            //ln.Y2 = e.GetPosition((Canvas)sender).Y;
            //flag = false;
            //}
            //else
            //{

            //}
        }

        private void Canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            
                ln.X2 = e.GetPosition((Canvas)sender).X;
                ln.Y2 = e.GetPosition((Canvas)sender).Y;
            
           
        }
        public string s;
        
        private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
             ln.Visibility = Visibility.Visible;
            ln.X1 = ln.X2;
            ln.Y1 = ln.Y2;
            Point pt = e.GetPosition((Canvas)sender); // позиция относительно канваса 
            markedPoints.Add(pt);
            switch (markedPoints.Count)
            {
                case 1: Canvas.SetLeft(One, pt.X - 23); Canvas.SetTop(One, pt.Y - 23); One.Visibility = Visibility.Visible;  break;
                case 2: Canvas.SetLeft(Two, pt.X - 23); Canvas.SetTop(Two, pt.Y - 23); Two.Visibility = Visibility.Visible; break;
                case 3: Canvas.SetLeft(Three, pt.X - 23); Canvas.SetTop(Three, pt.Y - 23); Three.Visibility = Visibility.Visible; break;
                case 4: Canvas.SetLeft(Four, pt.X - 23); Canvas.SetTop(Four, pt.Y - 23); Four.Visibility = Visibility.Visible; break;
                case 5: Canvas.SetLeft(Five, pt.X - 23); Canvas.SetTop(Five, pt.Y - 23); Five.Visibility = Visibility.Visible; break;
            }
            ln.StrokeThickness = 2;
            ln.Stroke = Brushes.Green;
          
             
          
            ln.X1 = e.GetPosition((Canvas)sender).X;
            ln.Y1 = e.GetPosition((Canvas)sender).Y;
          switch (s)
            {
                case "PrTreug":
                    if (markedPoints.Count >= 3)
                    {
                        ln.Visibility = Visibility.Hidden;
                    }
                    break;
                case "Kvadrat":
                    if (markedPoints.Count >= 4)
                    {
                        ln.Visibility = Visibility.Hidden;
                    }
                    break;
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


        private void TregBut_Click(object sender, RoutedEventArgs e)
        {
         
            TregGrid.Visibility = Visibility.Hidden;
            TregGridTup.Visibility = Visibility.Hidden;
            TregGridOstr.Visibility = Visibility.Hidden;
            TregGridRavnob.Visibility = Visibility.Hidden;
            TregGridRavnost.Visibility = Visibility.Hidden;
            KvGrid.Visibility = Visibility.Hidden;
            TrGrid.Visibility = Visibility.Hidden;
            PryamGrid.Visibility = Visibility.Hidden;
        }
        int final = 1; List<string> Cor = new List<string>();
       List<string> UnCor = new List<string>();
        private void TregBut11_Click(object sender, RoutedEventArgs e)
        {
            if (GridError.Visibility == Visibility.Visible)
            {
                uncorr++;
                switch (s)
                {
                    case "PrTreug": UnCor.Add("Прямоугольный треугольник"); break;
                    case "OstrTreug": UnCor.Add("Остроугольный треугольник"); break;
                    case "TupTreug": UnCor.Add("Тупоугольный треугольник"); break;
                    case "RavnTreug": UnCor.Add("Равнобедренный треугольник"); break;
                    case "RavnostTreug": UnCor.Add("Равносторонний треугольник"); break;
                    case "Kvadrat": UnCor.Add("Квадрат"); break;
                    case "Pryam": UnCor.Add("Прямоугольник"); break;

                    case "Trap": UnCor.Add("Трапеция"); break;

                }
            }
            else
            {
                corr++;
                switch (s)
                {
                    case "PrTreug":  Cor.Add("Прямоугольный треугольник"); break;
                    case "OstrTreug": Cor.Add("Остроугольный треугольник"); break;
                    case "TupTreug": Cor.Add("Тупоугольный треугольник"); break;
                    case "RavnTreug": Cor.Add("Равнобедренный треугольник"); break;
                    case "RavnostTreug": Cor.Add("Равносторонний треугольник"); break;
                    case "Kvadrat": Cor.Add("Квадрат"); break;
                    case "Pryam": Cor.Add("Прямоугольник"); break;

                    case "Trap": Cor.Add("Трапеция"); break;

                }
            }
            final++;
            markedPoints.Clear();
            int g = 1; int k = 0;
            if (final == 9)
            {
                timer.Stop();
                GridAllFinal.Visibility = Visibility.Visible;
                //  TimeSpan ts = (DateTime.Now - now).ToString("hh':'mm':'ss");
               // string nows = Convert.ToString(DateTime.Now);
                TimeSpan times = (DateTime.Now - now);
             
                LBTIME.Content = "Время прохождения: " +  times.ToString("hh':'mm':'ss");
                Insert($"Insert into figures (CorrectAnswers, Date,Time,UnCorrectAnswers,ID_Account) VALUES ({corr},'{DateTime.Now.ToString("yyyyMMddHHmmss")}','{times}',{uncorr},{accid}) "); //вызов добавления
                pipChart.Series.Clear();
                Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                pipChart.Series.Add(new PieSeries { Title = "Неправильно", Fill = Brushes.Red,
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    StrokeThickness = 2, Values = new ChartValues<double> { uncorr } });
                pipChart.Series.Add(new PieSeries { Title = "Правильно", Fill = Brushes.Green,
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    StrokeThickness = 2, Values = new ChartValues<double> { corr } });

            }
            if (final <= 8)
            {
                do
                {
                    g = 1;

                    Random r = new Random();
                    k = r.Next(1, 9);

                    for (int i = 0; i < 8; i++)
                    {
                        if (k == mas[i]) g = 0;
                    }

                } while (g == 0);
            }
            else
            {
                return;
            }
            switch (k)
            {
                case 1: s = "PrTreug"; TregGrid.Visibility = Visibility.Visible; break;
                case 2: s = "OstrTreug"; TregGridOstr.Visibility = Visibility.Visible; break;
                case 3: s = "TupTreug"; TregGridTup.Visibility = Visibility.Visible; break;
                case 4: s = "RavnTreug"; TregGridRavnob.Visibility = Visibility.Visible; break;
                case 5: s = "RavnostTreug"; TregGridRavnost.Visibility = Visibility.Visible; break;
                case 6: s = "Kvadrat"; KvGrid.Visibility = Visibility.Visible; break;
                case 7: s = "Pryam"; PryamGrid.Visibility = Visibility.Visible; break;
               
                case 8: s = "Trap"; TrGrid.Visibility = Visibility.Visible; break;

            }
            for (int i = 0; i < 8; i++)
            {
                if (mas[i]==0)
                {
                    mas[i] = k;
                    break;
                }
            }
            ln.Visibility = Visibility.Hidden;
            One.Visibility = Visibility.Hidden;
            Two.Visibility = Visibility.Hidden;
            Three.Visibility = Visibility.Hidden;
            Four.Visibility = Visibility.Hidden;
            Five.Visibility = Visibility.Hidden;
            TregGridFinal.Visibility = Visibility.Hidden;
            TregGridTupFinal.Visibility = Visibility.Hidden;
            TregGridOstrFinal.Visibility = Visibility.Hidden;
            TregGridRavnobFinal.Visibility = Visibility.Hidden;
            TregGridRavnostFinal.Visibility = Visibility.Hidden;
            KvGridFinal.Visibility = Visibility.Hidden;
            TrGridFinal.Visibility = Visibility.Hidden;
            PryamGridFinal.Visibility = Visibility.Hidden;
            HidingError();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            markedPoints.Clear();
            MyPolygon.Points.Clear(); MainGrid.Children.Remove(MyPolygon);
            ln.Visibility = Visibility.Hidden;
            One.Visibility = Visibility.Hidden;
            Two.Visibility = Visibility.Hidden;
            Three.Visibility = Visibility.Hidden;
            Four.Visibility = Visibility.Hidden;
            Five.Visibility = Visibility.Hidden;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void pipChart_DataClick(object sender, ChartPoint chartPoint)
        {
            string s;
            s = "Правильные ответы: ";
            for(int i = 0; i < Cor.Count; i++)
            {
                s += "\n " + Cor[i];
            }
            s += "\n";
            s += "\nНеправильные ответы: ";
            for(int i = 0; i < UnCor.Count; i++)
            {
                s += "\n " + UnCor[i];
            }
            MessageBox.Show(s);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        //private void pipChart_DataClick(object sender, ChartPoint chartPoint)
        //{

        //}
    }
}
