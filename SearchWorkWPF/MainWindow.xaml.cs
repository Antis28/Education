using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SearchWork.Extract;
using SearchWorkWPF.Job;
using AntisLib.Net;
using System.Windows.Media;
using System.Threading;
// To access MetroWindow, add the following reference
using MahApps.Metro.Controls;

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
        private Thread sercherJobs;
        private object cacheButtonContent;

        private void ResetLoadButton()
        {
            progressRing.IsActive = false;
            btnGetInfoJob.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            btnGetInfoJob.Content = cacheButtonContent;
        }
        private void ConfigLoadButton(string s)
        {
            cacheButtonContent = btnGetInfoJob.Content;
            System.Windows.Controls.TextBlock tb = new System.Windows.Controls.TextBlock();
            tb.Text = s;
            tb.Width = btnGetInfoJob.Width;
            tb.TextAlignment = TextAlignment.Center;

            btnGetInfoJob.Content = tb;
            btnGetInfoJob.Background = new SolidColorBrush(Colors.Gold);
        }

        private void btnGetInfoJob_Click( object sender, RoutedEventArgs e )
        {
            ConfigLoadButton("Отмена");
            if( sercherJobs != null && sercherJobs.IsAlive )
            {
                sercherJobs.Abort();
                ResetLoadButton();
                return;
            }

            // В системе есть подключение к интернету
            if( InternetChecker.InternetGetConnectedState() )
            {
                progressRing.IsActive = true;
                this.tb_loadJob.Text = "Загружаю";
                int.TryParse(tbxPageNumber.Text, out curPage);
                lbContentView.Items.Clear();

                // Подготовка вызова в другом потоке
                BeginGetJobInMozaika();
            }
            else
            {
                this.tb_loadJob.Text = "Не могу подключится к интернету";
                ResetLoadButton();
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
            sercherJobs = jMozaika.BeginGetJobList();
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
            //JobInfo ji = lstv.SelectedItem as JobInfo;
            //Clipboard.SetText(ji.Url);
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
            if( --curPage < 1 )
                curPage = 1;

            tbxPageNumber.Text = (curPage).ToString();
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
                progressRing.IsActive = false;
            });

        }
        void onCanceledConvert( string mesage )
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        this.tb_loadJob.Text = mesage;
                        ResetLoadButton();
                    });

        }
        void onCompleteConvert( List<JobInfo> lJobs )
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        this.tb_loadJob.Text = "Загрузка завершена";
                        ResetLoadButton();                    
                    });

        }
    }
}
