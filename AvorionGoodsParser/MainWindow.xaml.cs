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

        private void button1_Click( object sender, RoutedEventArgs e )
        {
            double price1, price2, deposit;
            Double.TryParse(tbx_bought.Text, out price1);
            Double.TryParse(tbx_sell.Text, out price2);
            Double.TryParse(tbx_deposit.Text, out deposit);

            double resultPrice1, resultPrice2, count;
            string result;

            resultPrice1 = price2 - price1;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");
            ci.NumberFormat.CurrencyDecimalDigits = 0;
            ci.NumberFormat.CurrencyGroupSeparator = " ";
            result = string.Format("Прибыль за 1 товар: {0}", resultPrice1.ToString("C", ci));
            if( deposit > 0 )
            {
                count = Math.Round(deposit / price1);
                resultPrice2 = resultPrice1 * count;
                result += string.Format("\nПрибыль за {0}: {1}", count, resultPrice2.ToString("C", ci));
            }
            tb_result.Text = result;
        }
    }
}
