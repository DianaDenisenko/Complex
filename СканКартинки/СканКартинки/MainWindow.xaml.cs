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
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;

namespace СканКартинки
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        DataTable dt_user;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(int ID,bool x)
        {
            InitializeComponent();
            redact = x;
            if (x)
            {
                ID_Test = ID;
                LoadTest();
            }
            else
            {
                ID_Catalog = ID;
            }
            
        }
        int ID_Test;
        int ID_Catalog;

        bool redact;


        int zind = -1;
        List<Image> img_list = new List<Image>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {   zind--;
            try
            {
                Image ImageContainer = new Image();
                Panel.SetZIndex(ImageContainer, zind);
                Canvas1.Children.Add(ImageContainer);
                ImageContainer.MouseLeftButtonDown += new MouseButtonEventHandler(StartDrag);
                RenderOptions.SetBitmapScalingMode(ImageContainer, BitmapScalingMode.Fant);
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Выберите картинку для добавления в тест.";
                ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofd.ShowDialog();
                BitmapImage BI = new BitmapImage();
                BI.BeginInit();
                BI.UriSource = new Uri(ofd.FileName, UriKind.RelativeOrAbsolute);
                BI.EndInit();
                ImageContainer.Width = BI.Width;
                ImageContainer.Height = BI.Height;
                ImageContainer.Source = BI;

                img_list.Add(ImageContainer);
            }
            catch { }
        }

        

        private void Image1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (RB_V3.Visibility == Visibility.Hidden) { RB_V3.Visibility = Visibility.Visible; return; }
            if (RB_V4.Visibility == Visibility.Hidden) { RB_V4.Visibility = Visibility.Visible; return; }
            if (RB_V5.Visibility == Visibility.Hidden) { RB_V5.Visibility = Visibility.Visible; return; }
            
        }

        private void Image2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (RB_V5.Visibility == Visibility.Visible) { RB_V5.Visibility = Visibility.Hidden; return; }
            if (RB_V4.Visibility == Visibility.Visible) { RB_V4.Visibility = Visibility.Hidden; return; }
            if (RB_V3.Visibility == Visibility.Visible) { RB_V3.Visibility = Visibility.Hidden; return; }
        }

        Vector relativeMousePos;
        FrameworkElement draggedObject;
        Image img = new Image();
        void StartDrag(object sender, MouseButtonEventArgs e)
        {
            img = sender as Image;
            draggedObject = (FrameworkElement)sender;
            relativeMousePos = e.GetPosition(draggedObject) - new Point();
            draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(Canvas1);
            var newPos = point - relativeMousePos;
            Canvas.SetLeft(draggedObject, newPos.X);
            Canvas.SetTop(draggedObject, newPos.Y);
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            draggedObject.MouseMove -= OnDragMove;
            draggedObject.LostMouseCapture -= OnLostCapture;
            draggedObject.MouseUp -= OnMouseUp;
            UpdatePosition(e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(img.Name != "")
            {
                try
                {
                    int ID_Png = int.Parse(img.Name.Split('I')[1]);
                    Update($"update пнг set Статус = '(Удалён)' where ID_Пнг = {ID_Png}");
                }
                catch { MessageBox.Show("Ошибка проведения операции."); }
            }
            img.Visibility = Visibility.Hidden;
            img_list.Remove(img);
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
        public string ToStr(byte[] a)
        {
            string s = "";
            foreach(var b in a)
            {
                s += b.ToString();
            }
            return s;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (redact == true)
            {
                string[] answer = new string[5];
                if (RB_V1.Visibility == Visibility.Visible) { answer[0] = TB_V1.Text; } else { answer[0] = ""; }
                if (RB_V2.Visibility == Visibility.Visible) { answer[1] = TB_V2.Text; } else { answer[1] = ""; }
                if (RB_V3.Visibility == Visibility.Visible) { answer[2] = TB_V3.Text; } else { answer[2] = ""; }
                if (RB_V4.Visibility == Visibility.Visible) { answer[3] = TB_V4.Text; } else { answer[3] = ""; }
                if (RB_V5.Visibility == Visibility.Visible) { answer[4] = TB_V5.Text; } else { answer[4] = ""; }

                string good_ans = "";
                if (RB_V1.IsChecked == true) { good_ans += TB_V1.Text + "|"; }
                if (RB_V2.IsChecked == true) { good_ans += TB_V2.Text + "|"; }
                if (RB_V3.IsChecked == true) { good_ans += TB_V3.Text + "|"; }
                if (RB_V4.IsChecked == true) { good_ans += TB_V4.Text + "|"; }
                if (RB_V5.IsChecked == true) { good_ans += TB_V5.Text + "|"; }
                good_ans = good_ans.Remove(good_ans.Length-1, 1);

Update($"UPDATE тесты set Заголовок = '{TB_text.Text}',Вариант1 = '{answer[0]}',Вариант2 = '{answer[1]}',Вариант3 = '{answer[2]}',Вариант4 = '{answer[3]}',Вариант5 = '{answer[4]}',Ответы = '{good_ans}' WHERE ID_Теста = {ID_Test}");
                for (int i = 0; i < img_list.Count; i++)
                {
                    if (img_list[i].Name == "")
                    {
                        try
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
                            sqlCmd.CommandText = $"insert into пнг(ImageData,ID_Теста,PX,PY) values(@am,{ID_Test},{Canvas.GetLeft(img_list[i])},{Canvas.GetTop(img_list[i])})";
                            sqlCmd.Parameters.Add("@am", MySqlDbType.VarBinary, 60000);
                            sqlCmd.Parameters["@am"].Value = ConvertImageToByte(img_list[i]);
                            sqlConnection.Open();
                            sqlCmd.ExecuteNonQuery();
                            sqlConnection.Close();
                        }
                        catch { MessageBox.Show("Ошибка проведения операции."); }
                    }
                    else
                    {
                        int id_png = int.Parse(img_list[i].Name.Split('I')[1]);
                        Update($"update пнг set PX = {Canvas.GetLeft(img_list[i])}, PY = {Canvas.GetTop(img_list[i])} where ID_Пнг = {id_png}");
                    }
                }
                MessageBox.Show("Редактирование успешно сохранено.");
                this.Close();
            }
            else // Добавление теста
            {
                string[] answer = new string[5];
                if (RB_V1.Visibility == Visibility.Visible) { answer[0] = TB_V1.Text; } else { answer[0] = ""; }
                if (RB_V2.Visibility == Visibility.Visible) { answer[1] = TB_V2.Text; } else { answer[1] = ""; }
                if (RB_V3.Visibility == Visibility.Visible) { answer[2] = TB_V3.Text; } else { answer[2] = ""; }
                if (RB_V4.Visibility == Visibility.Visible) { answer[3] = TB_V4.Text; } else { answer[3] = ""; }
                if (RB_V5.Visibility == Visibility.Visible) { answer[4] = TB_V5.Text; } else { answer[4] = ""; }
                if (TB_text.Text == "") { MessageBox.Show("Ошибка создания теста."); return; }
                string good_ans = "";
                if (RB_V1.IsChecked == true) { good_ans += TB_V1.Text + "|"; }
                if (RB_V2.IsChecked == true) { good_ans += TB_V2.Text + "|"; }
                if (RB_V3.IsChecked == true) { good_ans += TB_V3.Text + "|"; }
                if (RB_V4.IsChecked == true) { good_ans += TB_V4.Text + "|"; }
                if (RB_V5.IsChecked == true) { good_ans += TB_V5.Text + "|"; }
                good_ans = good_ans.Remove(good_ans.Length - 1, 1);

                Insert($"Insert into тесты(Заголовок,Вариант1,Вариант2,Вариант3,Вариант4,Вариант5,Ответы,ID_Каталог) values('{TB_text.Text}','{answer[0]}','{answer[1]}','{answer[2]}','{answer[3]}','{answer[4]}','{good_ans}',{ID_Catalog})");
                for (int i = 0; i < img_list.Count; i++)
                {
                        try
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
                            sqlCmd.CommandText = $"insert into пнг(ImageData,ID_Теста,PX,PY) values(@am,{ID_Test},{Canvas.GetLeft(img_list[i])},{Canvas.GetTop(img_list[i])})";
                            sqlCmd.Parameters.Add("@am", MySqlDbType.VarBinary, 60000);
                            sqlCmd.Parameters["@am"].Value = ConvertImageToByte(img_list[i]);
                            sqlConnection.Open();
                            sqlCmd.ExecuteNonQuery();
                            sqlConnection.Close();
                        }
                        catch { MessageBox.Show("Ошибка проведения операции."); return; }
                }
                MessageBox.Show("Тест успешно добавлен.");
                this.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("" +
                "Добавление изображений:\nДля того, чтобы добавить изображение, нажмите кнопку \"Добавить изображение\".\n" +
                "После добавления, изображение можно перемещать, при этом задавая позицию изображения при последующем отображении теста\n" +
                "При необходимости удалить изображение кликните по изображению и нажмите кнопку \"Удалить изображение\"." +
                "После занесения информации, необходимо нажать на копку \"Создать тест\".");
        }

        void LoadTest()
        {
            B1.Content = "Сохранить";
            DataTable Test = new DataTable();
            for (int i_for = 0; i_for < 3; i_for++)
            {
                if (i_for == 2) { MessageBox.Show("Проверьте подключение к Интернету.");this.Close(); return;}
                try
                {
                    Test = Select($"SELECT ID_Теста,Заголовок,Вариант1,Вариант2,Вариант3,Вариант4,Вариант5,Ответы,ID_Каталог,Статус FROM тесты WHERE ID_Теста = {ID_Test}");
                    break;
                }
                catch { }
            }
            TB_text.Text = Test.Rows[0][1].ToString();
            string[] answer = new string[] { Test.Rows[0][2].ToString(), Test.Rows[0][3].ToString(),Test.Rows[0][4].ToString(),Test.Rows[0][5].ToString(),Test.Rows[0][6].ToString()};
            if (answer[0] != "") { RB_V1.Visibility = Visibility.Visible; TB_V1.Text = answer[0]; }
            if (answer[1] != "") { RB_V2.Visibility = Visibility.Visible; TB_V2.Text = answer[1]; }
            if (answer[2] != "") { RB_V3.Visibility = Visibility.Visible; TB_V3.Text = answer[2]; }
            if (answer[3] != "") { RB_V4.Visibility = Visibility.Visible; TB_V4.Text = answer[3]; }
            if (answer[4] != "") { RB_V5.Visibility = Visibility.Visible; TB_V5.Text = answer[4]; }
            string[] good_ans = Test.Rows[0][7].ToString().Split('|');
            for(int i = 0; i < good_ans.Length; i++)
            {
                if (good_ans[i] == TB_V1.Text) { RB_V1.IsChecked = true; }
                if (good_ans[i] == TB_V2.Text) { RB_V2.IsChecked = true; }
                if (good_ans[i] == TB_V3.Text) { RB_V3.IsChecked = true; }
                if (good_ans[i] == TB_V4.Text) { RB_V4.IsChecked = true; }
                if (good_ans[i] == TB_V5.Text) { RB_V5.IsChecked = true; }
            }
            LoadPNG();
        }
        public void LoadPNG()
        {
            DataTable png = Select($"SELECT ID_Пнг,ImageData,ID_Теста,PX,PY FROM пнг WHERE Статус is null and ID_Теста = {ID_Test}");
            if (png.Rows.Count == 0) return;
            for (int i = 0; i < png.Rows.Count; i++)
            {
                byte[] byte_img = (byte[])png.Rows[i][1];
                using (MemoryStream ms = new MemoryStream(byte_img, 0, byte_img.Length))
                {
                    ms.Write(byte_img, 0, byte_img.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();

                    Image img = new Image();
                    img.Source = bi;
                    img.Name = "I" + (int)png.Rows[i][0];
                    img_list.Add(img);
                    Canvas.SetLeft(img, Convert.ToDouble(png.Rows[i][3].ToString()));
                    Canvas.SetTop(img, Convert.ToDouble(png.Rows[i][4].ToString()));
                    Canvas1.Children.Add(img);
                    img.MouseLeftButtonDown += new MouseButtonEventHandler(StartDrag);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
