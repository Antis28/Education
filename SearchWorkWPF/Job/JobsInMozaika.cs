using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;

namespace SearchWorkWPF.Job
{
    class JobsInMozaika : Jobs
    {
        public void BeginGetJobList()
        {
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "Вторичный";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
        public List<JobInfo> GetJobList()
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

            OnMaxValue(divNodes.Count());

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

                OnChangeValue();

            }

            return MozaikaJobList;
        }

        private void Start()
        {
            try
            {
                OnCompleteConvert(GetJobList());
            } catch
            {
                System.Windows.MessageBox.Show("Exception: не могу подключится к интернету");
            }
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
