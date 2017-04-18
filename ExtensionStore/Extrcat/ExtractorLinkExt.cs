﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExtensionStore
{
    /// <summary>
    /// Получает ссылки со страницы
    /// </summary>
    class ExtractorLinkExt : HtmlString
    {
        private string siteAddress;
        private string fileTempAddress;

        public Encoding encoding;

        public new event Action<Dictionary<string, string>> CompleteConvertEvent;
        protected void OnCompleteConvert( Dictionary<string, string> list )
        {
            CompleteConvertEvent(list);
        }

        public ExtractorLinkExt()
        {
            siteAddress = "http://open-file.ru/";
            fileTempAddress = "temp.txt";
            encoding = Encoding.GetEncoding(1251);
        }

        public ExtractorLinkExt( string siteAddress, Encoding encoding )
        {
            this.siteAddress = siteAddress;
            this.encoding = encoding;
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

        public Dictionary<string,string> GetList()
        {
            Dictionary<string, string> ExtList = new Dictionary<string, string>();

            //xPathQuery
            string xpq_allWorks = "//ul[@class=\"nav-list\"]"; //ul class="nav-list"

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


            var TableNodes = allHTML.DocumentNode.SelectSingleNode(xpq_allWorks);

            //OnMaxValue(TableNodes.Count());            
            foreach( var lineNode in TableNodes.ChildNodes )
            {
                if( lineNode.Name == "#text" )
                    continue;

                HtmlNode tdNodes = lineNode.SelectSingleNode("a");

                HtmlAttribute href = tdNodes.Attributes["href"];
                string name = tdNodes.InnerText;
                string link = href.Value;
                ExtList.Add(name,link);
                OnChangeValue();
            }

            return ExtList;
        }
    }
}