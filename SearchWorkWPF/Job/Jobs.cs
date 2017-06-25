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
        public event Action<JobInfo> NextStepEvent;
        public event Action<int> MaxValueEvent;
        public event Action<List<JobInfo>> CompleteEvent;
        public event Action CanceledEvent;

        // Используем метод для запуска события
        protected void OnChangeValue()
        {
            ChangeValueEvent();
        }
        protected void OnNextStepEvent( JobInfo job )
        {
            NextStepEvent(job);
        }
        protected void OnMaxValue( int maxValue )
        {
            MaxValueEvent(maxValue);
        }
        protected void OnComplete( List<JobInfo> lJobs )
        {
            CompleteEvent(lJobs);
        }
        protected void OnCanceled()
        {
            CanceledEvent();
        }

        protected void ClearEvents()
        {
            ChangeValueEvent = null;
            MaxValueEvent = null;
            CompleteEvent = null;
            CanceledEvent = null;
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
            return result;
        }
        protected static string ReadHTML( string site = "http://www.professorweb.ru" )
        {
            return ReadHTML(site, Encoding.UTF8);
        }
    }
}
