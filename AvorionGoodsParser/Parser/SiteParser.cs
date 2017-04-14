using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using AvorionGoodsParser.Data;

namespace AvorionGoodsParser.Parser
{
    class SiteParser : HtmlString
    {
        public void BeginParse()
        {
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SiteParser";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void Start()
        {
            //OnCompleteConvert(GetJobList());
            GetGoodsList();
        }

        public List<GoodsInfo> GetGoodsList()
        {
            List<GoodsInfo> GoodsList = new List<GoodsInfo>();

            //string siteAddress = "http://avorion.gamepedia.com/index.php?title=Goods";
            string siteAddress = "Goods.txt";//"Goods - Official Avorion Wiki.htm";            
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

                string[] soldByArr = NodeToStringArr(tdNodes[3].SelectNodes("a"), out soldByArr);
                string[] boughtByArr = NodeToStringArr(tdNodes[4].SelectNodes("a"), out boughtByArr);
                if( tdNodes.Count < 7)
                {                    
                    continue;
                }
                
                GoodsList.Add(new GoodsInfo
                {
                    Name = tdNodes[0].InnerText.Replace("\r\n", ""),
                    Volume = tdNodes[1].InnerText.Replace("\r\n", ""),
                    Price = tdNodes[2].InnerText.Replace("\r\n", ""),
                    SoldBy = soldByArr,
                    BoughtBy = boughtByArr,
                    isIllegal = tdNodes[5].InnerText.Replace("\r\n", ""),
                    isDangerous = tdNodes[6].InnerText.Replace("\r\n", "")
                });                           
                OnChangeValue();
            }

            return GoodsList;
        }
        
        private static string[] NodeToStringArr(HtmlNodeCollection selectedCollection, out string[] soldByArr )
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
