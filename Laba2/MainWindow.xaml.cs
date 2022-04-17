using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace Laba2
{
    /// <summary>
    /// Довести пагинацию до ума. Мб закинуть все методы в отдельный класс.
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private readonly string filePath = Environment.CurrentDirectory + "\\DB.xlsx";
        private int numberOfRecPerPage;
        static readonly Paging Pagination = new Paging();

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(filePath))
            {
                MessageBoxResult result = MessageBox.Show("Отсутсвует база данных.\nЗагрузить БД?", "My App", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        DownloadDB();
                        ReadDB(filePath);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                ReadDB(filePath);
            }

            Pagination.PageIndex = 0;
            int[] RecordsToShow = { 20, 50 };
            foreach (int i in RecordsToShow)
            {
                RowsToShow.Items.Add(i);
            }

            RowsToShow.SelectedItem = RecordsToShow[0];

        } //При старте чекает есть ли файл ДБ в папке(Папа)

        /*********************************************************************************************/

        public void ReadDB(string path)
        {
            UBI.CurrentUBIS.Clear(); // Очищаем массив на сулчай, если там есть старая таблица.
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            var tables = result.Tables;
            foreach (DataTable table in tables)
            {
                table.Rows.RemoveAt(0);
                table.Rows.RemoveAt(0);
                foreach (DataRow row in table.Rows)
                {
                    if (row[0].ToString() == "") // Если неправильное форматированеи дока.
                    {
                        continue;
                    }
                    UBI.CurrentUBIS.Add(Int32.Parse(row[0].ToString()),new UBI(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(),row[4].ToString(), 
                             row[5].ToString() == "1"? "Да" : "Нет", // Крутая тернарная операция
                             row[6].ToString() == "1" ? "Да" : "Нет", 
                             row[7].ToString() == "1" ? "Да" : "Нет"));
                }

            }
            stream.Dispose();
            reader.Dispose();
        } 

        private void ShowTable(List<UBI> UBISonLIST)
        {
            DataGridXAML.Items.Clear();
            foreach (var i in UBISonLIST)
            {
                DataGridXAML.Items.Add(i);
            }
            DisplayNumOfPages();
        }

        public void DownloadDB()
        {
            WebClient Client = new WebClient();
            try
            {
                Client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "DB.xlsx");
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 

        private void DisplayNumOfPages()
        {
            int RecAmount = numberOfRecPerPage * (Pagination.PageIndex + 1);
            int BeforeEndAmount = RecAmount - numberOfRecPerPage + 1;
            if (RecAmount > UBI.CurrentUBIS.Count)
            {
                RecAmount = UBI.CurrentUBIS.Count;
                BeforeEndAmount = UBI.CurrentUBIS.Count - (UBI.CurrentUBIS.Count % numberOfRecPerPage) + 1;// Считает сколько осталось до конца ДБ.
            }
            Label.Content = $"{BeforeEndAmount} - {RecAmount} of {UBI.CurrentUBIS.Count}";
        } 

        private void CompareTable()
        {
            Report report = new Report();
            Dictionary<int, UBI> OldUBIS = new Dictionary<int, UBI>(UBI.CurrentUBIS); // Сохраним старый Лист
            DownloadDB();
            ReadDB(filePath);
            Dictionary<int, UBI> NewUBIS = new Dictionary<int, UBI>(UBI.CurrentUBIS); // Сохраним новый Лист
            report.CompareTable(NewUBIS, ref OldUBIS);
            Pagination.PageIndex = 0; // обнулим индекс страницы, чтобы возвращать в самое начало.
            ShowTable(Pagination.SetPaging(UBI.CurrentUBIS.Values.ToList(), numberOfRecPerPage));
        }

        private void SaveDB(string path)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".xlsx",
                Filter = "XLSX documents (.xlsx)|*.xlsx"
            };

            Nullable<bool> result = dlg.ShowDialog();
            FileInfo fileInf = new FileInfo(path);

            if (result == true)
            {
                string filename = dlg.FileName;
                fileInf.CopyTo(filename,true);
            }
        }

        /*********************************************************************************************/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CompareTable();
        } // Должно быть обновеление ДБ

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            ShowTable(Pagination.Next(UBI.CurrentUBIS.Values.ToList(), numberOfRecPerPage));
        } // клик вперед !!!!!СДЕЛАТЬ НЕАКТИВНЫЕ КНОПКИ!!!!! UPD: Не буду!

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            ShowTable(Pagination.Previous(UBI.CurrentUBIS.Values.ToList(), numberOfRecPerPage));
        }

        private void RowsToShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberOfRecPerPage = Convert.ToInt32(RowsToShow.SelectedItem);

            if (numberOfRecPerPage == 50)
            {
                Pagination.PageIndex = Pagination.PageIndex * 20/50;
            }
            else
            {
                Pagination.PageIndex = Pagination.PageIndex * 50 / 20;
            }

            ShowTable(Pagination.SetPaging(UBI.CurrentUBIS.Values.ToList(), numberOfRecPerPage));
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridXAML_Moreinfo.Items.Clear();
            var row = (DataGridRow)sender;
            if (!(row.DataContext is UBI context)) return;
            UBI.CurrentUBIS.TryGetValue(context.ThreatID, out UBI B);
            DataGridXAML_Moreinfo.Items.Add(B);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveDB(filePath);
        }
    }
}
