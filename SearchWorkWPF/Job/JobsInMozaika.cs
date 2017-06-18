using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;

namespace SearchWorkWPF.Job
{
    class JobsInMozaika : Jobs
    {
        private int pageNumber = 1;

        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                if( value < 1 || value > 61 )
                    pageNumber = 1;
                else
                    pageNumber = value;
            }
        }

        private HtmlNodeCollection contactFieldsNodes = null;

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

            string mozaika = "http://mozaika.dn.ua/vakansy/page/"+ pageNumber + "/";
            //string mozaika = "http://html5-ap/mozaika.dn.ua.htm";            
            //xPathQuery
            string xpq_allWorks = "//div[@id=\"dle-content\"]/div[@class=\"rabota-all\"]";          //<div id="dle-content">//class="rabota-all"


            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            allHTML.LoadHtml(ReadHTML(mozaika));

            var divNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);

            OnMaxValue(divNodes.Count());

            foreach( var node in divNodes )
            {
                currentHTML.LoadHtml(node.InnerHtml);
                JobInfo newJob = ExtractInfoJob(currentHTML);

                MozaikaJobList.Add(newJob);

                OnChangeValue();
                OnNextStepEvent(newJob);
            }

            return MozaikaJobList;
        }

        private JobInfo ExtractInfoJob( HtmlDocument currentHTML )
        {
            string xpq_currentWork = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-title\"]/a";  // rabota-data/rabota-title/a           
            string xpq_date = "/div[@class=\"rabota-data\"]/ul[@class=\"info-doska\"]/li[1]";

            HtmlNode currentWork = currentHTML
                                    .DocumentNode
                                    .SelectSingleNode(xpq_currentWork);

            string Title = currentHTML
                        .DocumentNode
                        .SelectSingleNode(xpq_currentWork)
                        .InnerText;
            string Date = currentHTML
                        .DocumentNode
                        .SelectSingleNode(xpq_date)
                        ?.FirstChild.InnerText;
            string Salary;//= ExtractSalary(currentHTML);
            string Schedule;
            string City;
            ExtractSchedSalCity(currentHTML, out Schedule, out Salary, out City);            
            string Description = currentHTML
                            .DocumentNode
                            .ChildNodes?[1]
                            .ChildNodes?[7]
                            ?.InnerText;
            string curUrl = currentHTML
                                .DocumentNode
                                .SelectSingleNode(xpq_currentWork)
                                ?.Attributes?[0].Value;

            //Зайти на страницу вакансии и получить телефон                    
            string curPhone = searchByTitle(curUrl, "Телефон:&nbsp; ");
            string curContFace = searchByTitle(curUrl, "Контактное лицо:&nbsp; ", true);
            string curEmail = searchByTitle(curUrl, "Емайл, ICQ:&nbsp; ", true);
            string curExperience = searchByTitle(curUrl, " Стаж:&nbsp; ", true);
            string curEducation = searchByTitle(curUrl, " Образование:&nbsp; ", true);
            
            return new JobInfo()
            {
                Title = currentHTML
                     .DocumentNode
                     .SelectSingleNode(xpq_currentWork)
                     .InnerText,
                Salary = Salary,
                Date = Date,
                Schedule = Schedule,
                City = City,
                Description = Description,
                Url = curUrl,
                Telephone = curPhone,
                ContFace = curContFace,
                Email = curEmail,
                Experience = curExperience,
                Education = curEducation,
            };
        }

        private void Start()
        {
            try
            {
                OnCompleteConvert(GetJobList());
            } catch( Exception ex )
            {
                System.Windows.MessageBox.Show("Exception: не могу подключится к интернету");
            }
        }

        private string searchByTitle( string URL, string title, bool isLoaded = false )
        {
            if( !isLoaded || contactFieldsNodes == null )
            {
                HtmlDocument curHtmlDocument = new HtmlDocument();
                string curHtml = ReadHTML(URL);
                curHtmlDocument.LoadHtml(curHtml);
                string xPath = "//div[@class=\"rabota-allfull\"]/div[@class=\"rabota-fields\"]";
                contactFieldsNodes = curHtmlDocument.DocumentNode.SelectNodes(xPath);
            }
            
            foreach( var item in contactFieldsNodes )
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
            return "Не указано";
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
        private string ExtractSchedSalCity( HtmlDocument currentHTML, out string scheduleValue, out string salaryValue, out string cityValue )
        {
            string xpq_schedule = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-fields12\"]";
            scheduleValue = "Не указано";
            salaryValue = "Не указано";
            cityValue = "Не указано";

            HtmlNode scheduleNode = currentHTML
                        .DocumentNode
                        .SelectSingleNode(xpq_schedule);

            if( scheduleNode == null )
                return scheduleValue;

            foreach( var item in scheduleNode.ChildNodes.Where(x => x.Name == "#text") )
            {
                if( item.InnerText.Contains("Зарплата") )
                {
                    salaryValue = item.NextSibling.InnerText;
                    continue;
                }
                if( item.InnerText.Contains("График") )
                {
                    scheduleValue = item.NextSibling.InnerText;
                    continue;
                }
                if( item.InnerText.Contains("Город") )
                {
                    cityValue = item.NextSibling.InnerText;
                    continue;
                }
            }


            return scheduleValue;
        }
    }
}
