﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SearchWork.Extract;
using SearchWorkWPF.Job;
using AntisLib.Net;
using System.Windows.Media;

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

        private int curPage;


        private void btnGetInfoJob_Click( object sender, RoutedEventArgs e )
        {
            int.TryParse(tbxPageNumber.Text, out curPage);
            btnGetInfoJob.Background = new SolidColorBrush(Colors.Gold);
            lbContentView.Items.Clear();
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
            jMozaika.PageNumber = curPage;
            // Добавляем обработчик события             
            jMozaika.MaxValueEvent += onInitialValue;
            jMozaika.ChangeValueEvent += onChangeIndicator;
            jMozaika.NextStepEvent += JMozaika_NextStepEvent;
            jMozaika.CompleteEvent += onCompleteConvert;
            jMozaika.CanceledEvent += onCanceledConvert;

            // Вызов в другом потоке
            jMozaika.BeginGetJobList();
        }

        private void JMozaika_NextStepEvent( JobInfo obj )
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       lbContentView.Items.Add(obj);
                   });
        }

        private void TextBox_MouseDown( object sender, MouseButtonEventArgs e )
        {
            JobInfo ji = lstv.SelectedItem as JobInfo;
            Clipboard.SetText(ji.Url);
        }

        private void btnNextInfoJob_Click( object sender, RoutedEventArgs e )
        {
            int.TryParse(tbxPageNumber.Text, out curPage);
            tbxPageNumber.Text = (++curPage).ToString();
            btnGetInfoJob_Click(sender, e);
        }
        private void btnPrevInfoJob_Click( object sender, RoutedEventArgs e )
        {
            int.TryParse(tbxPageNumber.Text, out curPage);
            tbxPageNumber.Text = (--curPage).ToString();
            btnGetInfoJob_Click(sender, e);
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
                        lstv.ItemsSource = lJobs;

                        btnGetInfoJob.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
                    });

        }
    }
}
