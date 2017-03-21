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
        // Объявляем событие для индикатора
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action<List<JobInfo>> CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        // Используем метод для запуска события
        protected void OnChangeValue()
        {
            ChangeValueEvent();
        }
        protected void OnMaxValue( int maxValue )
        {
            MaxValueEvent(maxValue);
        }
        protected void OnCompleteConvert( List<JobInfo> lJobs )
        {
            CompleteConvertEvent(lJobs);
        }
        protected void OnCanceledConvert()
        {
            CanceledConvertEvent();
        }

        protected void ClearEvents()
        {
            ChangeValueEvent = null;
            MaxValueEvent = null;
            CompleteConvertEvent = null;
            CanceledConvertEvent = null;
        }

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
