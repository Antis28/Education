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
using AvorionGoodsParser.Parser;
using AvorionGoodsParser.myData;
using AvorionGoodsParser.myXml;

namespace AvorionGoodsParser
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

        private void button_Click( object sender, RoutedEventArgs e )
        {
            comboBoxGoods.SelectionChanged += ComboBoxGoods_SelectionChanged;
            LoadBase();
        }

        private void LoadBase()
        {
            if( !XmlSrz.isFileExist )
            {
                SiteParser siteParser = new SiteParser();
                siteParser.CompleteConvertEvent += SiteParser_CompleteConvertEvent;                
                siteParser.BeginParse();
            }
            else
            {
                comboBoxGoods.ItemsSource = new myXml.XmlSrz().Load();
            }
        }

        private void ComboBoxGoods_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            listViewSold.ItemsSource = ((GoodsInfo)comboBoxGoods.SelectedItem).SoldBy;
            listViewBought.ItemsSource = ((GoodsInfo)comboBoxGoods.SelectedItem).BoughtBy;
        }

        private void SiteParser_CompleteConvertEvent( List<myData.GoodsInfo> obj )
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        comboBoxGoods.ItemsSource = obj;
                        new AvorionGoodsParser.myXml.XmlSrz().Save(obj);
                    });

        }
    }
}
