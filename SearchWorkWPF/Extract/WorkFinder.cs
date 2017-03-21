using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using SearchWork.Job;

namespace SearchWork.Extract
{
    class WorkFinder
    {
        public List<JobInfo> GetJobLinksInMozaika()
        {
            return JobsInMozaika.GetJobList();
        }
        private static string ExtractSalary(HtmlDocument currentHTML)
        {
            HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
            string salary = priseNode.InnerHtml;
            string price = "Не указано";
            if (salary.Contains("Зарплата"))
            {
                price = priseNode.SelectSingleNode("font[@class=\"rabota-fieldsb\"]").InnerText;
            }

            return price;
        }

        class Jobs
        {
            protected static string ReadHTML(string site, Encoding encoding)
            {
                string result = "";
                // Создать объект запроса
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(site);

                // Получить ответ с сервера
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Получаем поток данных из ответа
                using (StreamReader stream = new StreamReader(
                     response.GetResponseStream(), encoding))
                {
                    // Выводим исходный код страницы
                    result = stream.ReadToEnd();
                }

                //
                // Получаем некоторые данные о сервере
                //string messageServer = "Целевой URL: \t" + 
                //                            request.RequestUri + 
                //                            "\nМетод запроса: \t" + 
                //                            request.Method +
                //                            "\nТип полученных данных: \t" + 
                //                            response.ContentType + 
                //                            "\nДлина ответа: \t" + 
                //                            response.ContentLength + 
                //                            "\nЗаголовки";
                //Console.WriteLine( messageServer );
                //

                return result;
            }
            protected static string ReadHTML(string site = "http://www.professorweb.ru")
            {
                return ReadHTML(site, Encoding.UTF8);
            }
        }

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

                foreach (var node in divNodes)
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

            private static string searchByTitle(string URL, string title, string xPath)
            {
                HtmlDocument curHtmlDocument = new HtmlDocument();
                string curHtml = ReadHTML(URL);
                curHtmlDocument.LoadHtml(curHtml);
                xPath = "//div[@class=\"rabota-allfull\"]/div[@class=\"rabota-fields\"]";
                HtmlNodeCollection divNodes = curHtmlDocument.DocumentNode.SelectNodes(xPath);
                foreach (var item in divNodes)
                {
                    bool isContain = item.InnerText.Contains(title);
                    string findedElement = item.InnerText;
                    if (isContain)
                    {
                        //findedElement = findedElement.Replace("&nbsp;", " ");
                        findedElement = findedElement.Replace(title, "");
                        return findedElement;
                    }
                }
                return "";
            }

            private static string ExtractSalary(HtmlDocument currentHTML)
            {
                HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
                string salary = priseNode.InnerHtml;
                string price = "Не указано";
                if (salary.Contains("Зарплата"))
                {
                    price = priseNode.SelectSingleNode("font[@class=\"rabota-fieldsb\"]").InnerText;
                }

                return price;
            }
        }
    }
}







