﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SearchWork.Extract;
using SearchWorkWPF.Job;
using AntisLib.Net;

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

            //WorkFinder htd = new WorkFinder();           

            //List<JobInfo> j = htd.GetJobLinksInMozaika();
            //lstw.ItemsSource = j;
            MessageBox.Show(ComputerEnum.GetComEnum());
            if( InternetChecker.InternetGetConnectedState() )
            {
                // Подготовка вызова в другом потоке
                BeginGetJobInMozaika();
            }
            else
            {
                MessageBox.Show("Не могу подключится к интернету");
            }

        }

        private void BeginGetJobInMozaika()
        {
            JobsInMozaika jMozaika = new JobsInMozaika();
            // Добавляем обработчик события             
            jMozaika.MaxValueEvent += onInitialValue;
            jMozaika.ChangeValueEvent += onChangeIndicator;
            jMozaika.CompleteEvent += onCompleteConvert;
            jMozaika.CanceledEvent += onCanceledConvert;

            // Вызов в другом потоке
            jMozaika.BeginGetJobList();
        }

        private void TextBox_MouseDown( object sender, MouseButtonEventArgs e )
        {
            JobInfo ji = lstw.SelectedItem as JobInfo;
            Clipboard.SetText(ji.Url);
        }

        public bool isPercent = false;
        double maximum = 0, current;
        void onChangeIndicator()
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        current++;
                        string currentValue;
                        //if( isPercent )
                        //    currentValue = (int)(pb_loadJob.Value / pb_loadJob.Maximum * 100) + " %";
                        //else
                        //    currentValue = pb_loadJob.Value + " из " + pb_loadJob.Maximum;
                        if( isPercent )
                            currentValue = (int)(current / maximum * 100) + " %";
                        else
                            currentValue = current + " из " + maximum;

                        pb_loadJob.Value = Math.Round(current / maximum, 1);
                        tb_loadJob.Text = currentValue;//;
                    });

        }
        protected void onInitialValue( int maximum )
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
            {
                pb_loadJob.Value = current = 0;
                this.maximum = maximum;
                //pb_loadJob.Maximum = maximum;
                pb_loadJob.Maximum = 1;
            });

        }
        void onCanceledConvert()
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        //tb_percentConvert.Text = "Конвертировние отменено";
                        //btn_convert.Content = "Начать";
                    });

        }
        void onCompleteConvert( List<JobInfo> lJobs )
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        this.tb_loadJob.Text = "Загрузка завершена";
                        //isRunning = !isRunning;
                        //this.btn_convert.Content = "Начать";                        
                        lstw.ItemsSource = lJobs;
                    });

        }
    }
}
