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

namespace ExtensionStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb_descripEngHeader.Visibility = Visibility.Collapsed;
            tb_descripEng.Visibility = Visibility.Collapsed;
        }

        private void button_Click( object sender, RoutedEventArgs e )
        {
            ParserManeger pm = new ParserManeger(this);
            pm.Parse();
        }

        private void button_extract_Click( object sender, RoutedEventArgs e )
        {
            ParserManeger pm = new ParserManeger(this);
            pm.ExtractExt();
        }
    }
}
