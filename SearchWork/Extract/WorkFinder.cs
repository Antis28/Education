using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace SearchWork.Extract
{
    class WorkFinder
    {

        public static List<HabraJob> jobList;

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
            string messageServer = "Целевой URL: \t" + request.RequestUri + "\nМетод запроса: \t" + request.Method +
                 "\nТип полученных данных: \t" + response.ContentType + "\nДлина ответа: \t" + response.ContentLength + "\nЗаголовки";
            Console.WriteLine( messageServer );
            //

            return result;
        }


        public string GetJobLinksInMozaika()
        {
            string mozaika = "http://mozaika.dn.ua/vakansy/";
            //string mozaika = "http://html5-ap/mozaika.dn.ua.htm";            
            //xPathQuery
            string xpq_allWorks = "//div[@id=\"dle-content\"]/div[@class=\"rabota-all\"]";          //<div id="dle-content">//class="rabota-all"
            string xpq_currentWork = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-title\"]/a";  // rabota-data/rabota-title/a
            //string xpq_currentPrice = "/div[@class=\"rabota-data\"]/div[@class=\"rabota-fields12\"]";  ///font[@class=\"rabota-fieldsb\"]
            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            allHTML.LoadHtml( ReadHTML( mozaika ) );
            var divNodes = allHTML.DocumentNode.SelectNodes( xpq_allWorks );

            List<string> jobList = new List<string>();
            List<string> linkList = new List<string>();
            List<string> priceList = new List<string>();
            string title = "";
            string link = "";
            string descrition = "";
            string price = "";
            foreach( var node in divNodes )
            {
                currentHTML.LoadHtml( node.InnerHtml );

                title = currentHTML.DocumentNode.SelectSingleNode( xpq_currentWork ).InnerText;
                link = currentHTML.DocumentNode
                                        .SelectSingleNode( xpq_currentWork )
                                        .Attributes[0].Value;

                descrition = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[7].InnerText;
                HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
                string on = priseNode.InnerHtml;
                if( on.Contains( "Зарплата" ) )
                {
                    price = priseNode.SelectSingleNode( "font[@class=\"rabota-fieldsb\"]" ).InnerText;
                }
                jobList.Add( title );
                linkList.Add( link );
                priceList.Add( price );
                price = "";
            }

            return "";//divNodes.Count.ToString();
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