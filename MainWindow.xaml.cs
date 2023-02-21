using EasyWord_DSSR.Models;
using EasyWord_DSSR.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace EasyWord_DSSR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WordsModel> wordsModel = new List<WordsModel>();
        MainViewModel viewModel = new MainViewModel();

        /// <summary>
        /// Inizialisiert das Hauptfenster und lädt die Daten aus der Datei "wordsList.bin"
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            if (File.Exists("Data/wordsLists.bin"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream("Data/wordsLists.bin", FileMode.Open))
                {
                    viewModel.MyList = (List<WordsModel>)formatter.Deserialize(stream);
                }
                viewModel.MyListReference = viewModel.MyList;
            }
            else
            {
                viewModel.MyList = new List<WordsModel>();
            }
            this.Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Speichert die Daten in der Datei "wordsList.bin"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            wordsModel = viewModel.MyList;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("Data/wordsLists.bin", FileMode.Create))
            {
                formatter.Serialize(stream, wordsModel);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version: 2.0 " + "\n" + "Release date: 31/01/23" + "\n" + "Creators: David, Raphael, Sasa, Simon", "EasyWord", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
