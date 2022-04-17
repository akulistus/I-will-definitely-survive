using System;
using System.Collections;
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

namespace Laba2
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        Dictionary<int, UBI> NewChekedUBIS = new Dictionary<int, UBI>();
        int updateCount = 0;
        int deleteCount = 0;
        int addCount = 0;

        public Report()
        {
            InitializeComponent();
        }

        internal void CompareTable(Dictionary<int, UBI> New, ref Dictionary<int, UBI> Old)
        {
            foreach (var newRec in New)// Если измененные поля
            {

                if (Old.ContainsKey(newRec.Key)) // Чекает изменения
                {
                    bool checkaction = false;
                    Old.TryGetValue(newRec.Key,out UBI oldRec);

                    if (oldRec.ThreatName != newRec.Value.ThreatName)
                    {
                            newRec.Value.ThreatName = $"{oldRec.ThreatName}\n->\n{newRec.Value.ThreatName}";
                            checkaction = true;
                    }
                    if (oldRec.ThreatDescription != newRec.Value.ThreatDescription)
                    {
                            newRec.Value.ThreatDescription = $"{oldRec.ThreatDescription}\n->\n{newRec.Value.ThreatDescription}";
                            checkaction = true;
                    }
                    if (oldRec.ThreatSource != newRec.Value.ThreatSource)
                    {
                            newRec.Value.ThreatSource = $"{oldRec.ThreatSource}\n->\n{newRec.Value.ThreatSource}";
                            checkaction = true;
                    }
                    if (oldRec.ThreatObject != newRec.Value.ThreatObject)
                    {
                            newRec.Value.ThreatObject = $"{oldRec.ThreatObject}\n->\n{newRec.Value.ThreatObject}";
                            checkaction = true;
                    }
                    if (oldRec.ConfViolation != newRec.Value.ConfViolation)
                    {
                            newRec.Value.ConfViolation = $"{oldRec.ConfViolation}\n->\n{newRec.Value.ConfViolation}";
                            checkaction = true;
                    }
                    if (oldRec.IntegrityViolation != newRec.Value.IntegrityViolation)
                    {
                            newRec.Value.IntegrityViolation = $"{oldRec.IntegrityViolation}\n->\n{newRec.Value.IntegrityViolation}";
                            checkaction = true;
                    }
                    if (oldRec.AccessViolation != newRec.Value.AccessViolation)
                    {
                            newRec.Value.AccessViolation = $"{oldRec.AccessViolation}\n->\n{newRec.Value.AccessViolation}";
                            checkaction = true;
                    }

                    if (checkaction) // Если есть добавленные поля
                    {
                        newRec.Value.Status = "Обнавлен";
                        ChangeReport.Items.Add(newRec.Value);
                        updateCount++;
                    }

                }
                else
                {
                    newRec.Value.Status = "Добавлен";
                    ChangeReport.Items.Add(newRec.Value);
                    addCount++;
                }
                
            }

            foreach (var item in Old.Keys.Except(New.Keys)) // Если есть удаленные поля
            {
                Old.TryGetValue(item, out UBI B);
                B.Status = "Удален";
                ChangeReport.Items.Add(B);
                deleteCount++;
            }

            NewChekedUBIS = New;//Для просмотра изменений в записи

            MessageBox.Show($"Обновление прошло успешно!" +
                $"\nОбновлено записей: {updateCount}" +
                $"\nДобавлено записей: {addCount}" +
                $"\nУдалено записей: {deleteCount}" +
                $"\nВсего изменений: {addCount + deleteCount + updateCount}");

            if (addCount + deleteCount + updateCount != 0)
            {
                Show();
            }
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            if (!(row.DataContext is UBI context)) return;
            ChangeReport_Moreinfo.Items.Clear();
            if (context.Status == "Удален")
            {
                UBI.OldUBIS.TryGetValue(context.ThreatID, out UBI B);
                ChangeReport_Moreinfo.Items.Add(B);
            }
            else
            {
                NewChekedUBIS.TryGetValue(context.ThreatID, out UBI B);
                ChangeReport_Moreinfo.Items.Add(B);
            }
        }
    }
}
