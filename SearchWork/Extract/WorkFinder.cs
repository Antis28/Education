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
        string ReadHTML( string site = "http://www.professorweb.ru" )
        {
            return ReadHTML( site, Encoding.UTF8 );
        }

        string ReadHTML( string site, Encoding encoding )
        {
            string result = "";
            // Создать объект запроса
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( site );

            // Получить ответ с сервера
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Получаем поток данных из ответа
            using( StreamReader stream = new StreamReader(
                 response.GetResponseStream(), encoding ) )
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


        public List<JobInfo> GetJobLinksInMozaika()
        {
            List<JobInfo> MozaikaJobList = new List<JobInfo>();
            string mozaika = "http://mozaika.dn.ua/vakansy/";
            //string mozaika = "http://html5-ap/mozaika.dn.ua.htm";            
            //xPathQuery
            string xpq_allWorks = "//div[@id=\"dle-content\"]/div[@class=\"rabota-all\"]";          //<div id="dle-content">//class="rabota-all"
            string xpq_currentWork = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-title\"]/a";  // rabota-data/rabota-title/a
           
            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            allHTML.LoadHtml( ReadHTML( mozaika ) );
            var divNodes = allHTML.DocumentNode.SelectNodes( xpq_allWorks );

            foreach( var node in divNodes )
            {                         
                currentHTML.LoadHtml( node.InnerHtml );
                
                HtmlNode currentWork = currentHTML
                                        .DocumentNode
                                        .SelectSingleNode( xpq_currentWork );
                MozaikaJobList.Add( new JobInfo
                {
                    Title = currentHTML
                                .DocumentNode
                                .SelectSingleNode( xpq_currentWork )
                                .InnerText,
                    Price = ExtractSalary( currentHTML ),
                    Descrition = currentHTML
                                    .DocumentNode
                                    .ChildNodes[1]
                                    .ChildNodes[7]
                                    .InnerText,
                    Url = currentHTML
                                    .DocumentNode
                                    .SelectSingleNode( xpq_currentWork )
                                    .Attributes[0].Value
                } );
            }
            return MozaikaJobList;
        }

        private static string ExtractSalary( HtmlDocument currentHTML )
        {
            HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
            string salary = priseNode.InnerHtml;
            string price = "Не указано";
            if( salary.Contains( "Зарплата" ) )
            {
                price = priseNode.SelectSingleNode( "font[@class=\"rabota-fieldsb\"]" ).InnerText;
            }

            return price;
        }
    }
}





public enum Education
{
    None,
    Higher,
    IncompleteHigher,
    SecondaryVocational,
    Secondary,
    Pupil
}

public enum Employment
{
    Full,
    Partial,
    Freelance
}

class HabraJob
{
    public HabraJob()
    {
    }

    private string _price = string.Empty;

    public string Url { get; set; }

    public string Title { get; set; }

    public string Price
    {
        get
        {
            return _price;
        }
        set
        {
            _price = value;
            if( _price != "з/п договорная" )
            {
                PriceMoney = int.Parse( value.Substring( 3 ).Replace( " ", "" ) );
            }
        }
    }
    public int PriceMoney { get; set; }

    public string Company { get; set; }

    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }

    public Education Education { get; set; }

    public Employment Employment { get; set; }

    public static Education ParseEducation( string education )
    {
        switch( education )
        {
            case "Высшее":
                return Education.Higher;
            case "Неполное высшее":
                return Education.IncompleteHigher;
            case "Среднее специальное":
                return Education.SecondaryVocational;
            case "Среднее":
                return Education.Secondary;
            case "Учащийся":
                return Education.Pupil;
            case "Не имеет значения":
                return Education.None;
            default:
                throw new Exception();
        }
    }

    public static Employment ParseEmployment( string employment )
    {
        switch( employment )
        {
            case "полная":
                return Employment.Full;
            case "частичная":
                return Employment.Partial;
            case "фриланс":
                return Employment.Freelance;
            default:
                throw new Exception();
        }
    }
}