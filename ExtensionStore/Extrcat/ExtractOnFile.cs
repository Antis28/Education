using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionStore
{
    class ExtractOnFile
    {
        string fileName;
        FileStream file;
        StreamReader reader;

        public ExtractOnFile()
        {
            fileName = AddTXT("GeneralRef");
        }
        public ExtractOnFile( string name )
        {
            fileName = AddTXT(name);
        }

        string AddTXT( string name )
        {
            return name + ".txt";
        }

        public List<string> ExtractCategory()
        {
            List<string> categoryExtension = new List<string>();

            file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file, Encoding.GetEncoding(1251));
            try
            {
                while( !reader.EndOfStream )
                {
                    categoryExtension.Add(reader.ReadLine());
                }

            } catch/*( Exception e )*/
            {
            } finally
            {
                reader.Close();
                file.Close();
            }
            return categoryExtension;
        }

        public Dictionary<string, List<string>> ExtractLinksExtension( List<string> linkCategoryList )
        {
            #region 2) Вытащить ссылки для каждой категории из файла           

            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();//Имя катекории, список расширений 

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+?-\w+|\w+");
            for( int i = 0; i < linkCategoryList.Count; i++ )
            {
                var matchs = regex.Matches(linkCategoryList[i]);
                //Console.WriteLine( i + 1 + ") " + matchs[1] );


                file = new FileStream(matchs[1] + ".txt", FileMode.Open, FileAccess.Read);
                reader = new StreamReader(file, Encoding.GetEncoding(1251));

                result.Add(matchs[1].ToString(), new List<string>());
                string currentLine;
                while( !reader.EndOfStream )
                {
                    currentLine = reader.ReadLine();

                    result[matchs[1].ToString()].Add(currentLine);
                }
            }
            return result;
            #endregion
        }
    }
}
