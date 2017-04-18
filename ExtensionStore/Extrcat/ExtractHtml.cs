using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionStore
{
    class ExtractHtml
    {
        //FireFox browser = null;
        public XmlConstructor xmlConstr = null;

        public ExtractHtml()
        {
            #region ExtractHtml
            //browser = new FireFox();
            //browser.SizeWindow(200, 100);
            //Settings.CloseExistingFireFoxInstances = true;
            //Settings.WaitForCompleteTimeOut = 999999999;//увеличиваем тайм-аут на всякий случай
            #endregion
        }
        public void GoTo( string uri )
        {
            #region GoTo
            //try
            //{
            //    browser.GoTo(uri);
            //} catch( WatiN.Core.Exceptions.TimeoutException e ) { }
            ////string s = browser.Body.OuterHtml;
            #endregion
        }

        public void ExtractHtmlInnerText( List<string> resultCollection,
                                         string nodesToForeach,
                                        string selectNode, bool isAll = false )
        {
            #region ExtractHtmlInnerText
            //var doc = new HtmlDocument();       //инсталляция объекта парсера HTML Agility Pack
            //if( "\r\n<body></body>" == browser.Body.OuterHtml )
            //    browser.Refresh();

            //doc.LoadHtml(browser.Body.OuterHtml); //помещаем в парсер полученный html с страницы yvison.kz 

            ////получаем в переменную все блоки из rootToDocument
            //var dataBlock = doc.DocumentNode.SelectSingleNode(nodesToForeach);

            ////в цикле проходим по каждому nodesToForeach    

            //var Collection = dataBlock.SelectNodes(nodesToForeach);
            //foreach( var item in Collection )
            //{
            //    try
            //    {
            //        resultCollection.Add(item.SelectSingleNode(selectNode).InnerText);
            //    } catch( NullReferenceException e ) { if( isAll ) resultCollection.Add(item.InnerText); }
            //}

            ////browser.Close();
            # endregion
        }
        public void ExtractHtmlAttributes( List<string> AllAddressExtenson, string uri,
                                         string nodesToForeach,
                                        string selectNode, string attributes )
        {
            #region ExtractHtmlAttributes
            //using( var browser = new FireFox(uri) )
            //{
            //    Settings.WaitForCompleteTimeOut = 999999999;//увеличиваем тайм-аут на всякий случай
            //    var doc = new HtmlDocument();       //инсталляция объекта парсера HTML Agility Pack
            //    doc.LoadHtml(browser.Body.OuterHtml); //помещаем в парсер полученный html с страницы yvison.kz 

            //    //получаем в переменную все блоки из rootToDocument
            //    var dataBlock = doc.DocumentNode.SelectSingleNode(nodesToForeach);

            //    //в цикле проходим по каждому nodesToForeach    
            //    try
            //    {
            //        var Collection = dataBlock.SelectNodes(nodesToForeach);
            //        foreach( var item in Collection )
            //        {
            //            //Console.Write( "Link item: " + item.SelectSingleNode( "//div[@class=\"info\"]/div[@class=\"photo\"]/div[@class=\"hold\"]/a" ).Attributes["href"].Value + "\n" );
            //            AllAddressExtenson.Add(item.SelectSingleNode(selectNode).Attributes[attributes].Value);
            //        }
            //    } catch( NullReferenceException e ) { }
            //    //browser.Close();
            //}
            #endregion
        }
        public void Close()
        {
            //browser.Close();
            //browser.Dispose();
        }

        #region кусок разметки для ссылок на подробное описание     
        //< tbody >
        //< tr >
        //    < td class="ex">.
        //        <a href="/types/2sf">2sf</a>
        //    </td>
        //    <td>
        //        <a href="/types/2sf">Аудио-файл Nintendo DS</a>
        //    </td>
        //</tr>
        #endregion        
        public void ExtractLinkForCategory( List<string> AllAddressExtenson )
        {
            #region 2) Вытащить ссылки для каждой категории используя браузер

            //int RazdelNumber = 4;
            //for( int i = 0; i < AllAddressExtenson.Count; i++ )
            //{
            //    Console.WriteLine( i );
            //    //Вытаскиваю все ссылки на расширения со страницы
            //    listExtenson.Add( new List<string>() );
            //    System.Threading.Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
            //    ExtractHTML( listExtenson[i], "http://open-file.ru" + AllAddressExtenson[RazdelNumber], "//td[@class=\"ex\"]", "a", "href" );

            //    //////////////////////////////////////////////////////// /types/audio/
            //    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex( @"\w+?-\w+|\w+" );
            //    var match = regex.Matches( AllAddressExtenson[RazdelNumber] );
            //    string nameFile;
            //    if( match.Count > 2 )
            //    {
            //        nameFile = match[1].Value + match[2].Value;
            //    } else
            //        nameFile = match[1].Value;

            //    file = new FileStream( nameFile + ".txt", FileMode.OpenOrCreate, FileAccess.Write );
            //    writer = new StreamWriter( file, Encoding.GetEncoding( 1251 ) );
            //    try
            //    {
            //        foreach( var item in listExtenson[i] )
            //        {
            //            writer.WriteLine( item.ToString() );
            //        }
            //    } catch( Exception e ) { } finally
            //    {
            //        writer.Close();
            //        file.Close();
            //    }
            //    RazdelNumber++;
            //    /////////////////////////////////////
            //}

            #endregion
        }

        #region кусок разметки для таблицы подробного описания

        //<table class="desc">
        //    <tbody>
        //        <tr>
        //            <th colspan="3">Формат файла .ais #1 (наиболее вероятный)</th>
        //        </tr>
        //        <tr>
        //            <td class="over" width="120">Описание файла .ais на русском</td>
        //            <td width="380">Последовательность рисунков ACDSee</td>
        //        </tr>
        //        <tr
        //            ><td class="over">Описание файла .ais на английском</td>
        //            <td>ACDSee Image Sequence</td>
        //        </tr>
        //        <tr>
        //            <td class="over">Тип файла</td>
        //            <td><a href="http://open-file.ru/types/pictures/">Рисунки, изображения</a></td>
        //        </tr>
        //        <tr>
        //            <td class="over">Как, чем открыть файл .ais?</td>
        //            <td width="350">
        //                <ul>
        //                    <li>
        //                        <img style="border:0;display:inline;margin:1px 4px -3px 0" src="%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%20%D1%84%D0%B0%D0%B9%D0%BB%D0%B0%20.ais%20-%20%D0%BE%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5%20%D1%80%D0%B0%D1%81%D1%88%D0%B8%D1%80%D0%B5%D0%BD%D0%B8%D1%8F,%20%D1%87%D0%B5%D0%BC%20%D0%BE%D1%82%D0%BA%D1%80%D1%8B%D1%82%D1%8C%20%D1%84%D0%B0%D0%B9%D0%BB%20ais_files/acdsee_photo_editor.png" width="16">
        //                        <a href="http://open-file.ru/go?http://send.onenetworkdirect.net/z/12066/CD94176/" rel="nofollow" target="_blank">ACDSee Photo Editor 6</a> 
        //                        <img style="border:0;display:inline;margin:1px 4px -1px 1px" src="%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%20%D1%84%D0%B0%D0%B9%D0%BB%D0%B0%20.ais%20-%20%D0%BE%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5%20%D1%80%D0%B0%D1%81%D1%88%D0%B8%D1%80%D0%B5%D0%BD%D0%B8%D1%8F,%20%D1%87%D0%B5%D0%BC%20%D0%BE%D1%82%D0%BA%D1%80%D1%8B%D1%82%D1%8C%20%D1%84%D0%B0%D0%B9%D0%BB%20ais_files/ext.png" width="12">
        //                    </li>
        //                </ul>
        //            </td>
        //        </tr>
        //    </tbody>
        // </table>

        #endregion
        public void ParsingTable( KeyValuePair<string, string> pairKV )
        {
            #region 3)распарсить таблицу
            List<string> myTableHeader = new List<string>();
            List<string> myTableCell = new List<string>();
            List<string> myTableWhatOpen = new List<string>();
            ExtractHtml extract = this;
            string item = pairKV.Key;
            string mainSite = "http://open-file.ru";

            /////////////////////////////////////////////////////////////////////////////////////////////
            //Заглушка для локального тестирования
            //string mainSite = @"D:\Win_Doc_APP\Desktop\РАСШИРЕНИЯ,ФОРМАТ файлов\Подробнее\";  // Для теста
            /////////////////////////////////////////////////////////////////////////////////////////////

            string currentSite = mainSite + item;
            extract.GoTo(@currentSite);
            try
            {
                extract.ExtractHtmlInnerText(myTableHeader, "//table[@class=\"desc\"]/tbody/tr", "th");
                extract.ExtractHtmlInnerText(myTableCell, "//table[@class=\"desc\"]/tbody/tr/td", "td", true);
                extract.ExtractHtmlInnerText(myTableWhatOpen, "//table[@class=\"desc\"]/tbody/tr/td/ul/li", "a");
                try
                {
                    xmlConstr.Initialization(pairKV.Value, myTableHeader, myTableCell, myTableWhatOpen);
                } catch( Exception ex ) { Console.WriteLine(ex.Message); }
                xmlConstr.Add();
            } catch( NullReferenceException  ) { }
            #endregion
            //xmlConstr.Close();
            //extract.Close();
        }
    }
}
