using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SearchWorkWPF.Job
{
    class Jobs
    {
        protected static string ReadHTML( string site, Encoding encoding )
        {
            string result = "";
            // Создать объект запроса
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(site);

            // Получить ответ с сервера
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Получаем поток данных из ответа
            using( StreamReader stream = new StreamReader(
                 response.GetResponseStream(), encoding) )
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
        protected static string ReadHTML( string site = "http://www.professorweb.ru" )
        {
            return ReadHTML(site, Encoding.UTF8);
        }
    }
}
