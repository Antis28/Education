using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;
// Определено внутри System.Xml.dll
using System.Xml.Serialization;

using AvorionGoodsParser.myData;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace AvorionGoodsParser.myXml
{
    /// <summary>
    /// Создает или переписывает xml
    /// в соответсвии с данными
    /// </summary>
    class XmlSrz
    {
        private static readonly string fileName = "GoodsBase.xml";

        public static bool isFileExist
        {
            get
            {
                return File.Exists(fileName);
            }
        }

        public void Save( List<myData.GoodsInfo>  listGI )
        {
            //SaveBinaryFormat(gi, "GoodsInfo.dat");
            //LoadFromBinaryFile("GoodsInfo.dat");

            SaveInXmlFormat(listGI.ToArray(), fileName);
        }
        void SaveBinaryFormat( object objGraph, string fileName )
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using( Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None) )
            {
                binFormat.Serialize(fStream, objGraph);
            }
        }
        void LoadFromBinaryFile( string fileName )
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            using( Stream fStream = File.OpenRead(fileName) )
            {
                GoodsInfo fromDisk = (GoodsInfo)binFormat.Deserialize(fStream);
            }
        }

        internal GoodsInfo[] Load()
        {
            return LoadFromXmlFormat(fileName);
        }
        GoodsInfo[] LoadFromXmlFormat( string fileName )
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(GoodsInfo[]));
            GoodsInfo[] fromDisk;
            using( Stream fStream = File.OpenRead(fileName) )
            {
                fromDisk = (GoodsInfo[])xmlFormat.Deserialize(fStream);                
            }
            return fromDisk;
        }

        void SaveInXmlFormat( object objGraph, string fileName )
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(GoodsInfo[]));
            using( Stream fStream = new FileStream(fileName,
                FileMode.Create, FileAccess.Write, FileShare.None) )
            {
                xmlFormat.Serialize(fStream, objGraph);
            }
        }
        /*
        /// <summary>
        /// Добавляет новый элемент в xml
        /// </summary>
        /// <param name="addedItem"></param>
        /// <param name="xmlTWriter"></param>
        
        */
        /*
        /// <summary>
        /// Проверяет xml аттрибут с данными в объекте,
        /// если отличаются то добавляются новые данные
        /// </summary>
        /// <param name="itemExt">объект с данными</param>
        /// <param name="element">элемент с именем как в объекте</param>
        void AddInfoExtension( GoodsInfo addedItem, XElement element )
        {
            XAttribute attr;
            bool coincidence;

            attr = element.Attribute(XmlStruct.eDetailedDescription);
            coincidence = attr.Value == itemExt.DetailedDescription;
            if( !coincidence )
            {
                attr.Value = itemExt.DetailedDescription;
            }
        }
        */
    }
}

/*

МЕТАСИМВОЛЫ - это символы для составления Шаблона поиска.

\d    Определяет символы цифр. 
\D 	Определяет любой символ, который не является цифрой. 
\w 	Определяет любой символ цифры, буквы или подчеркивания. 
\W    Определяет любой символ, который не является цифрой, буквой или подчеркиванием. 
\s 	Определяет любой непечатный символ, включая пробел. 
\S 	Определяет любой символ, кроме символов табуляции, новой строки и возврата каретки.
.    Определяет любой символ кроме символа новой строки. 
\.    Определяет символ точки.

*/
