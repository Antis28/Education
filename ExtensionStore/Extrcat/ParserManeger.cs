using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtensionStore
{
    /// <summary>
    /// Управляет порядком парсинга
    /// </summary>
    class ParserManeger
    {
        public void Parse()
        {   
            List<string> linkCategoryList = new List<string>(); //Ссылки на категории расширений
            Dictionary<string, List<string>> linkAllExtensonDictionary;   //Имя катекории, список расширений      

            #region Create XML            
            ///////////////////////////////////////////////////////////////////////////////////// 
            //1)Вытащить ссылки на категории расширений из браузера
            //extract.ExtractHtmlAttributes(AllAddressExtenson, "http://open-file.ru/", "//div[@class=\"menu\"]/ul/li", "a", "href");

            //1)Вытащить ссылки на категории расширений из файла            
            //ExtractOnFile fileLoader = new ExtractOnFile();
            //linkCategoryList = fileLoader.ExtractCategory();

            //"Menu load!


            // 2) Вытащить ссылки для каждой категории из файла 
            ExtractOnFile fileLoader = new ExtractOnFile();
            linkAllExtensonDictionary = fileLoader.ExtractLinksExtension(linkCategoryList);
            Console.Write("All list extensions load!\n");

            // 4) Парсинг страниц и загрузка таблиц в xml
            Thread thread = new Thread(Parsing);
            thread.Start(linkAllExtensonDictionary);

            //foreach( KeyValuePair<string, List<string>> item in cNameFile )
            //{
            //    count += item.Value.Count;
            //}
            //Console.WriteLine( "Найдено записей = " + count );
            #endregion

            #region 5) Вытаскивает из Xml информацию о расширении файла
            ////Console.WriteLine( "Введите расширение файла:" );
            ////string s = Console.ReadLine();
            //XmlExtractor extractor = new XmlExtractor();
            //extractor.Extract();
            //foreach( var item in extractor.Header )
            //{
            //    Console.WriteLine( item );
            //}
            //foreach( var item in extractor.RusDescription )
            //{
            //    Console.WriteLine( item );
            //}
            //foreach( var item in extractor.EngDescription )
            //{
            //    Console.WriteLine( item );
            //}
            //foreach( var item in extractor.TypeFile )
            //{
            //    Console.WriteLine( item );
            //}
            //Console.WriteLine( "Чем открыть:" );
            //foreach( var item in extractor.WhatOpen )
            //{
            //    Console.WriteLine( item );
            //}
            #endregion


            Console.ReadKey();
        }

        private void Parsing( object box )
        {
            Dictionary<string, List<string>> linkAllExtensonDictionary = box as Dictionary<string, List<string>>;
            XmlConstructor xmlConstr = new XmlConstructor();
            ExtractHtml extract = new ExtractHtml();
            extract.xmlConstr = xmlConstr;
            // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
            //extract.ExtractHtmlAttributes(new List<string>(), "http://open-file.ru/", "//div[@class=\"menu\"]/ul/li", "a", "href" );
            // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
            Dictionary<string, string> ignorDuplicates = new Dictionary<string, string>();

            foreach( KeyValuePair<string, List<string>> pairKV in linkAllExtensonDictionary )
            {
                foreach( var item in pairKV.Value )
                {
                    try
                    {
                        ignorDuplicates.Add(item, pairKV.Key);
                    } catch( Exception  ) { }
                }
            }
            ;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Для добавления в базу
            string pathToXmlDocument = @"6000-ExtensionsBase.xml";
            var document = new System.Xml.XmlDocument();
            document.Load(pathToXmlDocument);
            System.Xml.XmlElement root = document.DocumentElement;
            Dictionary<string, string> nedostaushie = new Dictionary<string, string>();
            bool naydeno = false;
            string t;
            foreach( KeyValuePair<string, string> pairKV in ignorDuplicates )
            {
                if( pairKV.Key == "grp" )
                    continue;
                naydeno = false;
                t = pairKV.Key.Remove(0, 7);
                t = "RASHIRENIE" + t;
                foreach( System.Xml.XmlElement item in root )
                {
                    if( t == item.Name )
                    {
                        naydeno = true;
                        break;
                    }
                }
                if( naydeno )
                    continue;
                nedostaushie.Add(pairKV.Key, "");
            }
            
            ignorDuplicates = nedostaushie;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            //Заглушка для локального тестирования
            /////////////////////////////////////////////////////////////////////////////////////////////
            //Dictionary<string, string> ignorDuplicates = Test.Zapolnitel();
            /////////////////////////////////////////////////////////////////////////////////////////////

            List<string> keyList = new List<string>();
            foreach( KeyValuePair<string, string> pairKV in ignorDuplicates )
            {
                keyList.Add(pairKV.Key);
            }
            //int i = 0;
            //foreach( string key in keyList )
            //{
            //    i++;
            //    ignorDuplicates.Remove( key );
            //    if( i == 2853 + 900 + 53 + 2 )
            //        break;
            //}
            int count = 1;
            foreach( KeyValuePair<string, string> pairKV in ignorDuplicates )
            {
                //3)распарсить таблицу                    
                extract.ParsingTable(pairKV);
                //if( ++count > 5 )
                //break;

                //if( ++count > 5 )
                //break;
                Console.WriteLine("Страница № " + count++ + " из " + ignorDuplicates.Count);
            }
            xmlConstr.Close();
            extract.Close();
            Console.Write("XML build!\n");
        }
    }
}
