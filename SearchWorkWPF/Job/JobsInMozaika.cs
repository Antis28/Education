using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SearchWorkWPF.Job
{
    class JobsInMozaika : Jobs
    {
        public static List<JobInfo> GetJobList()
        {
            List<JobInfo> MozaikaJobList = new List<JobInfo>();

            string mozaika = "http://mozaika.dn.ua/vakansy/";
            //string mozaika = "http://html5-ap/mozaika.dn.ua.htm";            
            //xPathQuery
            string xpq_allWorks = "//div[@id=\"dle-content\"]/div[@class=\"rabota-all\"]";          //<div id="dle-content">//class="rabota-all"
            string xpq_currentWork = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-title\"]/a";  // rabota-data/rabota-title/a
            string xpq_phone = "//div[@class=\"rabota-allfull\"]/div[@class=\"rabota-fields\"]/span[@class=\"masha_index masha_index9\"]";

            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            allHTML.LoadHtml(ReadHTML(mozaika));
            var divNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);

            foreach( var node in divNodes )
            {
                currentHTML.LoadHtml(node.InnerHtml);

                HtmlNode currentWork = currentHTML
                                        .DocumentNode
                                        .SelectSingleNode(xpq_currentWork);

                string curUrl = currentHTML
                                    .DocumentNode
                                    .SelectSingleNode(xpq_currentWork)
                                    .Attributes[0].Value;

                //Зайти на страницу вакансии и получить телефон                    
                string curPhone = searchByTitle(curUrl, "Телефон:&nbsp; ", xpq_phone);


                MozaikaJobList.Add(new JobInfo
                {
                    Title = currentHTML
                                .DocumentNode
                                .SelectSingleNode(xpq_currentWork)
                                .InnerText,
                    Price = ExtractSalary(currentHTML),
                    Description = currentHTML
                                    .DocumentNode
                                    .ChildNodes[1]
                                    .ChildNodes[7]
                                    .InnerText,
                    Url = curUrl,
                    Telephone = curPhone

                });



            }

            return MozaikaJobList;
        }

        private static string searchByTitle( string URL, string title, string xPath )
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

        private static string ExtractSalary( HtmlDocument currentHTML )
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
