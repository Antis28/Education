using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using SearchWork.Extract;
using SearchWorkWPF.Job;

namespace SearchWorkWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGetInfoJob_Click( object sender, RoutedEventArgs e )
        {
            // привязка lstw к свойству коллекции
            //lstw.DataContext = "";
            //List<CarTable> lk = new List<CarTable>() {
            //    new CarTable(1), new CarTable(2),new CarTable(3)
            //};
            //lstw.ItemsSource = lk;
            WorkFinder htd = new WorkFinder();            
            List<JobInfo> j = htd.GetJobLinksInMozaika();
            lstw.ItemsSource = j;

        }

        class CarTable
        {
            private int v;

            public string ModelName { get; set; }
            public string ModelNumber { get; set; }
            public int? Cost { get; set; }
            public string Description { get; set; }

            public CarTable( int id )
            {

                this.v = id;
                ModelName = "My car with ModelName and id = " + id + ".";
                ModelNumber = "My car with ModelNumber and id = " + id + ".";
                //Cost = id * 1000;
            }
        }

        private void TextBox_MouseDown( object sender, MouseButtonEventArgs e )
        {
            JobInfo ji = lstw.SelectedItem as JobInfo;
            Clipboard.SetText( ji.Url);
        }
    }
}
