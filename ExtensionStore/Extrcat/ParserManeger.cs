using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace ExtensionStore
{
    /// <summary>
    /// Управляет порядком парсинга
    /// </summary>
    class ParserManeger
    {
        MainWindow mainWindow;
        public ParserManeger( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
        }
        public void Parse()
        {
            Encoding codePage = Encoding.GetEncoding(1251);

            ExtractorLinkExt extractor = new ExtractorLinkExt("http://open-file.ru", codePage);

            //события для ссылок на категории форматов
            extractor.CompleteGenLinkParseEvent += Extractor_CompleteConvertEvent;
            extractor.ChangeValueGeneralEvent += Extractor_ChangeValueGeneralEvent;
            extractor.MaxValueGeneralEvent += Extractor_MaxValueGeneralEvent;

            //события для ссылок всех форматов
            extractor.MaxValueAllEvent += Extractor_MaxValueAllEvent;
            extractor.ChangeValueAllEvent += Extractor_ChangeValueAllEvent;
            extractor.CompleteAllLinkParseEvent += Extractor_CompleteAllLinkParseEvent;

            //события заполнения списка объектов на расширения
            extractor.MaxValueExtParseEvent += Extractor_MaxValueExtParseEvent;
            extractor.ChangeValueExtParseEvent += Extractor_ChangeValueExtParseEvent;
            extractor.CompleteExtParseEvent += Extractor_CompleteExtParseEvent;

            extractor.BeginParse();

        }

        public void ExtractExt()
        {
            Encoding codePage = Encoding.GetEncoding(1251);
            XmlExtractor xmlE = new XmlExtractor();
            xmlE.CompleteExtractEvent += XmlE_CompleteExtractEvent;
            xmlE.ExtractExt(mainWindow.textBox.Text);
        }
        ////////////////////////////////////////////////////////////////
        private void XmlE_CompleteExtractEvent( ExtInfo obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       ShowResult(obj);
                   });
        }
        void ShowResult( ExtInfo currentExt )
        {
            BindProp(mainWindow.tb_formatfile, currentExt, "Name");
            BindProp(mainWindow.tb_descripEng, currentExt, "EngDescription");
            BindProp(mainWindow.tb_descripRus, currentExt, "RusDescription");
            BindProp(mainWindow.lb_InfoHeaderFile, currentExt, "InfoHeaderFile");
            BindProp(mainWindow.lb_WhatOpenWindows, currentExt, "WhatOpenWindows");
            BindProp(mainWindow.lb_WhatOpenLinux, currentExt, "WhatOpenLinux");
            BindProp(mainWindow.lb_WhatOpenMac, currentExt, "WhatOpenMac");
        }
        void BindProp( TextBlock tb, ExtInfo currentExt, string property )
        {
            Binding bind = new Binding();
            bind.Source = currentExt;
            bind.Path = new PropertyPath(property);
            bind.Mode = BindingMode.OneWay;
            tb.SetBinding(TextBlock.TextProperty, bind);
        }
        void BindProp( ListBox lb, ExtInfo currentExt, string property )
        {
            Binding bind = new Binding();
            bind.Source = currentExt;
            bind.Path = new PropertyPath(property);
            bind.Mode = BindingMode.OneWay;
            lb.SetBinding(ListBox.ItemsSourceProperty, bind);
        }

        ////////////////////////////////////////////////////////////////

        private void Extractor_CompleteExtParseEvent( List<ExtInfo> extList )
        {
            XmlConstructor constructor = new XmlConstructor();
            foreach( var item in extList )
            {
                constructor.AddToCategory(item);
            }
            constructor.Close();
        }

        private void Extractor_ChangeValueExtParseEvent()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)delegate
                  {
                      mainWindow.pb_allExtension.Value += 1;
                      mainWindow.tb_allExtension.Text =
                                    "Форматов: "
                                    + mainWindow.pb_allExtension.Value
                                    + " из "
                                    + mainWindow.pb_allExtension.Maximum;
                  });
        }

        private void Extractor_MaxValueExtParseEvent( int obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       mainWindow.tb_allExtension.Text = "Форматов: " + obj;
                       mainWindow.pb_allExtension.Maximum = obj;
                       mainWindow.pb_allExtension.Value = 0;
                   });
        }

        private void Extractor_CompleteAllLinkParseEvent( Dictionary<string, List<string>> obj )
        {
            System.Windows.MessageBox.Show("Fine");
        }

        private void Extractor_ChangeValueAllEvent()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       mainWindow.pb_allLinks.Value += 1;
                       mainWindow.tb_allLinks.Text =
                                    "Ссылок на формат: "
                                    + mainWindow.pb_allLinks.Value
                                    + " из "
                                    + mainWindow.pb_allLinks.Maximum;
                   });
        }

        private void Extractor_MaxValueAllEvent( int obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       mainWindow.tb_allLinks.Text = "Ссылки на формат: " + obj;
                       mainWindow.pb_allLinks.Maximum = obj;
                       mainWindow.pb_allLinks.Value = 0;
                   });
        }

        private void Extractor_ChangeValueGeneralEvent()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       mainWindow.pb_genLinks.Value += 1;
                   });
        }
        private void Extractor_MaxValueGeneralEvent( int obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       mainWindow.tb_genLinks.Text = "Общие ссылки: " + obj;
                       mainWindow.pb_genLinks.Maximum = obj;
                   });
        }

        private void Extractor_CompleteConvertEvent( Dictionary<string, string> obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        foreach( KeyValuePair<string, string> item in obj )
                        {
                            mainWindow.listBox.Items.Add(item.Key);
                        }
                    });
        }

        private void Parser_CompleteConvertEvent( ExtInfo obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        //mainWindow.listBox.Items.Add(obj.Name);
                        //mainWindow.listBox.Items.Add(obj.Header);
                        //mainWindow.listBox.Items.Add(obj.TypeFile);

                        //mainWindow.listBox.Items.Add(obj.RusDescription);
                        //mainWindow.listBox.Items.Add(obj.EngDescription);

                        //mainWindow.listBox.Items.Add(obj.InfoHeaderFile);
                        //foreach( var item in obj.WhatOpenWindows )
                        //{
                        //    mainWindow.listBox.Items.Add(item);
                        //}

                    });
        }
    }
}
