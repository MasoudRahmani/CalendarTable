using SQLCalendarTable.Logic;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SQLCalendarTable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _server;
        private string _db;
        private string _table;

        public MainWindow()
        {
            InitializeComponent();
        }


        #region WPFEvent

        private void ServerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _server = ServerTextBox.Text;
        }
        private void DatabaseSelected(object sender, SelectionChangedEventArgs e)
        {
            _db = Databases.SelectedValue.ToString();
        }
        private void TableName_Changed(object sender, EventArgs e)
        {
            _table = TableNameTextBox.Text.ToString();
        }

        private void Fill_databases(object sender, RoutedEventArgs e)
        {
            var _helper = new SQLConnectionHelper(_server, (bool)Integrated.IsChecked);
            Databases.ItemsSource = _helper.GetDatabases();
        }

        #endregion


        private void StartToCreate(object sender, RoutedEventArgs e)
        {
            UpdateLayoutToInformStartofJob();

            DateCreator bl = new DateCreator(_server, _db, _table, (bool)Integrated.IsChecked);

            CalculateDates(bl);

        }

        private void UpdateLayoutToInformStartofJob()
        {
            Progressbar.IsIndeterminate = true;
            MainGrid.IsEnabled = false;
        }

        private async void CalculateDates(DateCreator dc)
        {
            var start = (DateTime)StartDatePicker.SelectedDate;
            var end = (DateTime)EndDatePicker.SelectedDate;
            var result = Task.Factory.StartNew(() => { dc.Start(start, end); });
            await result;
            UpdateLayoutToInfromJobDone();

        }
        private void UpdateLayoutToInfromJobDone()
        {
            MainGrid.IsEnabled = true;
            Progressbar.IsIndeterminate = false;
            Progressbar.Maximum = 1;
            Progressbar.Value = 1;
            MessageBox.Show("Done.");
        }
    }
}
