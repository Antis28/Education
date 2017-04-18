﻿using System;
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
using AvorionGoodsParser.Data;

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
            SiteParser siteParser = new SiteParser();
            siteParser.CompleteConvertEvent += SiteParser_CompleteConvertEvent;
            siteParser.BeginParse();
            comboBoxGoods.SelectionChanged += ComboBoxGoods_SelectionChanged;
            
        }

        private void ComboBoxGoods_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            listViewSold.ItemsSource = ((GoodsInfo)comboBoxGoods.SelectedItem).SoldBy;
            listViewBought.ItemsSource = ((GoodsInfo)comboBoxGoods.SelectedItem).BoughtBy;
        }

        private void SiteParser_CompleteConvertEvent( List<Data.GoodsInfo> obj )
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        comboBoxGoods.ItemsSource = obj;
                    });

        }
    }
}