using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExtensionStore
{
    /// <summary>
    /// получает данные с сайта и упаковывает в коллекцию
    /// </summary>
    class SiteParser : HtmlString
    {
        private string siteAddress;

        public SiteParser()
        {            
            siteAddress = "psd.htm";//"Goods - Official Avorion Wiki.htm";
        }

        public SiteParser( string siteAddress )
        {
            this.siteAddress = siteAddress;
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
            OnCompleteConvert(GetGoodsList());
            //GetGoodsList();
        }

        public List<ExtInfo> GetGoodsList()
        {
            List<ExtInfo> ExtList = new List<ExtInfo>();

            //xPathQuery
            string xpq_allWorks = "//table[@class=\"wikitable sortable\"]/tr";          //<table class="wikitable sortable">   //<div id="dle-content">//class="rabota-all"

            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            //allHTML.LoadHtml(ReadHTML(siteAddress));
            allHTML.LoadHtml(ReadFileHTML(siteAddress));
            var TableNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);

            //OnMaxValue(TableNodes.Count());            
            foreach( var lineNode in TableNodes )
            {
                if( lineNode == TableNodes[0] )
                    continue;
                HtmlNodeCollection tdNodes = lineNode.SelectNodes("td");

                string[] soldByArr;  
                string[] boughtByArr;
                NodeToStringArr(tdNodes[3].SelectNodes("a"), out soldByArr);
                NodeToStringArr(tdNodes[4].SelectNodes("a"), out boughtByArr);
                if( tdNodes.Count < 7 )
                {
                    continue;
                }

                ExtList.Add(new ExtInfo
                {
                    Name = tdNodes[0].InnerText.Replace("\n", "").Replace("\r", ""),
                    Volume = tdNodes[1].InnerText.Replace("\n", "").Replace("\r", ""),
                    Price = tdNodes[2].InnerText.Replace("\n", "").Replace("\r", ""),
                    SoldBy = soldByArr,
                    BoughtBy = boughtByArr,
                    isIllegal = tdNodes[5].InnerText.Replace("\n", "").Replace("\r", ""),
                    isDangerous = tdNodes[6].InnerText.Replace("\n", "").Replace("\r", "")
                });
                OnChangeValue();
            }

            return ExtList;
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

        private string searchByTitle( string URL, string title, string xPath )
        {
            HtmlDocument curHtmlDocument = new HtmlDocument();
            string curHtml = ReadHTML(URL);
            curHtmlDocument.LoadHtml(curHtml);
            xPath = "//div[@class=\"rabota-allfull\"]/div[@class=\"rabota-fields\"]";
            HtmlNodeCollection divNodes = curHtmlDocument.DocumentNode.SelectNodes(xPath);
            foreach( var item in divNodes )
            {
                bool isContain = item.InnerText.Contains(title);
                string findedElement = item.InnerText;
                if( isContain )
                {
                    //findedElement = findedElement.Replace("&nbsp;", " ");
                    findedElement = findedElement.Replace(title, "");
                    return findedElement;
                }
            }
            return "";
        }

        private string ExtractSalary( HtmlDocument currentHTML )
        {
            HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
            string salary = priseNode.InnerHtml;
            string price = "Не указано";
            if( salary.Contains("Зарплата") )
            {
                price = priseNode.SelectSingleNode("font[@class=\"rabota-fieldsb\"]").InnerText;
            }

            return price;
        }
    }
}
