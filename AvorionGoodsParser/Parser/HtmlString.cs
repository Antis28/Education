using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AvorionGoodsParser.Data;

namespace AvorionGoodsParser.Parser
{
    class HtmlString
    {
        // Объявляем событие для индикатора
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action<List<GoodsInfo>> CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        // Используем метод для запуска события
        protected void OnChangeValue()
        {
            if( ChangeValueEvent != null )
                ChangeValueEvent();
        }
        protected void OnMaxValue( int maxValue )
        {
            if( MaxValueEvent != null )
                MaxValueEvent(maxValue);
        }
        protected void OnCompleteConvert( List<GoodsInfo> lJobs )
        {
            CompleteConvertEvent(lJobs);
        }
        protected void OnCanceledConvert()
        {
            if( CanceledConvertEvent != null )
                CanceledConvertEvent();
        }

        protected void ClearEvents()
        {
            ChangeValueEvent = null;
            MaxValueEvent = null;
            //CompleteConvertEvent = null;
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
            using( StreamWriter streawr = new StreamWriter(
                Environment.CurrentDirectory + "\\" + "Goods.txt"
                , false
                , encoding) )
            {
                streawr.WriteLine(result);
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
        protected string ReadFileHTML( string file, Encoding encoding = null )
        {
            string result = "";

            if( encoding == null )
                encoding = Encoding.UTF8;

            // Получаем поток данных из ответа
            using( StreamReader stream = new StreamReader(
                Environment.CurrentDirectory + "\\" + file
                 , encoding) )
            {
                // Выводим исходный код страницы
                result = stream.ReadToEnd();
            }
            return result;
        }
    }
}
