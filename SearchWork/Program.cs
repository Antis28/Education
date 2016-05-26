using System;
using SearchWork.Extract;

namespace SearchWork
{

    class Program
    {
        [STAThread]
        static void Main( string[] args )
        {
            //int ch;
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create( "http://mozaika.dn.ua/vakansy/" );
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //Stream stream = resp.GetResponseStream();
            //for( int i = 1; ; i++ )
            //{
            //    ch = stream.ReadByte();
            //    if( ch == -1 )
            //        break;
            //    Console.Write( (char)ch );

            //}
            //resp.Close();

            WorkFinder htd = new WorkFinder();
            // Console.WriteLine( htd.ReadHTML() );

            Console.WriteLine( htd.GetJobLinksInMozaika() );


            Console.ReadKey();
        }
    }



}
