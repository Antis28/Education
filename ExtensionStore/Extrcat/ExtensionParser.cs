using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExtensionStore
{
    /// <summary>
    /// получает данные с сайта и упаковывает в коллекцию
    /// </summary>
    class ExtParser : HtmlString
    {
        private string siteAddress;
        public Encoding encoding;
        public string fileTempAddress = "temp.txt";

        public ExtParser()
        {
            siteAddress = "http://open-file.ru/";
            encoding = Encoding.UTF8;
        }

        public ExtParser( string siteAddress, Encoding encoding )
        {
            this.siteAddress = siteAddress;
            this.encoding = encoding;
        }
        public event Action<ExtInfo> CompleteConvertEvent;
        protected void OnCompleteConvert( ExtInfo lJobs )
        {
            CompleteConvertEvent(lJobs);
        }

        public void BeginParse()
        {
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SiteParser";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void Start()
        {
            OnCompleteConvert(GetList());
            //GetGoodsList();
        }

        public ExtInfo GetList()
        {
            ExtInfo ext = new ExtInfo();
            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            if( File.Exists(fileTempAddress) )
            {
                allHTML.LoadHtml(ReadFileHTML(fileTempAddress, encoding));
            }
            else
            {
                allHTML.LoadHtml(ReadHTML(siteAddress, encoding));
            }
            //xPathQuery
            //table class="desc"
            string xpq_allWorks = "//table[@class=\"desc\"]/tbody/tr";// "//table[@class=\"desc\"]/tbody/tr"
            var TableNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);

            //OnMaxValue(TableNodes.Count());      

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach( var lineNode in TableNodes )
            {
                
                string key = null, val = null;

                HtmlNodeCollection tdNodes = lineNode.SelectNodes("td");
                if( tdNodes == null )
                {
                    tdNodes = lineNode.SelectNodes("th");
                    if( tdNodes.Count <= 2 )
                    {
                        key = tdNodes[0].InnerText.Replace("\n", "").Replace("\r", "");
                        MatchCollection m = Regex.Matches(key, @"([А-я]\w+\s*\w*\s*)(\.*[a-z]*)");//[А-я]\w+\s+\w+
                        ext.Header = m[0].Value;
                        key = m[1].Value;
                        m = Regex.Matches(tdNodes[0].InnerText, @"\.[a-z]*");//[А-я]\w+\s+\w+
                        val = m[0].Value;

                    }
                    //continue;
                }
                else
                {
                    if( tdNodes.Count == 2 )
                    {
                        key = tdNodes[0].InnerText;
                        val = tdNodes[1].InnerText;                        
                    }
                    else if( tdNodes.Count == 1 )
                    {
                        key = tdNodes[0].InnerText;
                        val = null;
                    }
                }
                switch( key )
                {
                    case "Основной формат":
                        ext.Name = val;
                        break;
                    case "Информация о заголовке файла ":
                        ext.InfoHeaderFile.Add(val);
                        break;
                    case "Тип файла":
                        ext.TypeFile = val;
                        break;
                }
                if( key.Contains("ASCII:") )
                {
                    ext.InfoHeaderFile.Add(key);
                }
                    
                if( key.Contains("на русском") )
                {
                    ext.RusDescription = val;
                }
                if( key.Contains("на английском") )
                {
                    ext.EngDescription = val;
                }
                if( key.Contains("Windows") )
                {
                    ext.WhatOpen = val;
                    HtmlNodeCollection li_s = tdNodes[1].SelectNodes("ul/li");
                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenWindows.Add(li.InnerText);
                    }
                }
                if( key.Contains("Linux") )
                {
                    ext.WhatOpen += val;
                    HtmlNodeCollection li_s = tdNodes[1].SelectNodes("ul/li");
                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenLinux.Add(li.InnerText);
                    }
                }
                if( key.Contains("MacOS") )
                {
                    ext.WhatOpen += val;
                    HtmlNodeCollection li_s = tdNodes[1].SelectNodes("ul/li");
                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenMac.Add(li.InnerText);
                    }
                }


                dic.Add(key, val);                
                
                OnChangeValue();
            }

            return ext;
        }

        private static string[] NodeToStringArr( HtmlNodeCollection selectedCollection, out string[] soldByArr )
        {
            if( selectedCollection != null )
            {
                soldByArr = new string[selectedCollection.Count];
                for( int i = 0; i < soldByArr.Length; i++ )
                {
                    soldByArr[i] = selectedCollection[i].InnerText;
                }
            }
            else
                soldByArr = new string[0];

            return soldByArr;
        }        
    }
}
