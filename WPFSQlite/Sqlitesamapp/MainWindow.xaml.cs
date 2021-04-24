using DemoLibrary;
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


namespace Sqlitesamapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PersonModel> people = new List<PersonModel>();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                SqliteDataAccess.CreateDbFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            LoadPeopleList();
        }

        private void LoadPeopleList()
        {
            try
            {
                for (int i = 0; i < 10; i++)
                    CreatePreopels();
                people = SqliteDataAccess.ReadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

            WireUpPeopleList();
        }

        private void CreatePreopels()
        {
            var p = new PersonModel { FirstName = "test", LastName = "mani", Id = new Random().Next() };
            SqliteDataAccess.InsertData(p);
        }

        private void WireUpPeopleList()
        {
            listPeopleListBox.ItemsSource = people;
        }
    }
}
