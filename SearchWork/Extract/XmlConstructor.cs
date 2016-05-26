using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace SearchWork.Extract
{
    class XmlConstructor
    {
        string extension;

        string category;
        List<string> header;
        List<string> rusDescription;
        List<string> engDescription;
        List<string> typeFile;
        List<string> whatOpen;
        List<string> detailedDescription;
        List<string> infoHeaderFile;
        XmlTextWriter xmlWriter;

        public XmlConstructor()
        {
            xmlWriter = new XmlTextWriter( "ExtensionsBase.xml", null )
            {
                Formatting = Formatting.Indented,   // Включить форматирование документа (с отступом).
                IndentChar = '\t',                  // Для выделения уровня элемента использовать табуляцию.
                Indentation = 1,                    // использовать один символ табуляции.
                //QuoteChar = '\''                  // способ записи ковычек
            };
            if( !System.IO.File.Exists( "ExtensionsBase.xml" ) )
                xmlWriter.WriteStartDocument();         //<? xml version = "1.0" ?>

            xmlWriter.WriteComment( "Список всех расширений." );
            xmlWriter.WriteStartElement( "ListOfExtension" );
            //xmlWriter.WriteComment( "Кактегория файла(аудио, видео, архив)." );
            //xmlWriter.WriteStartElement( "Category" );


        }

        public void Add()
        {

            //xmlWriter.WriteStartElement( category );
            xmlWriter.WriteStartElement( extension );
            foreach( var item in header )
            {
                xmlWriter.WriteStartElement( "Header" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in rusDescription )
            {
                xmlWriter.WriteStartElement( "rusDescription" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in engDescription )
            {
                xmlWriter.WriteStartElement( "engDescription" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in typeFile )
            {
                xmlWriter.WriteStartElement( "typeFile" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in infoHeaderFile )
            {
                xmlWriter.WriteStartElement( "infoHeaderFile" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in detailedDescription )
            {
                xmlWriter.WriteStartElement( "detailedDescription" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }

            foreach( var item in whatOpen )
            {
                xmlWriter.WriteStartElement( "WhatOpen" );
                xmlWriter.WriteAttributeString( "Value", item );
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();


        }

        public void Close()
        {
            xmlWriter.Close();
        }

        public void Initialization( string category,
                                    List<string> headers,
                                    List<string> cells,
                                    List<string> WhatOpen
                                    )
        {
            rusDescription = new List<string>();
            engDescription = new List<string>();
            typeFile = new List<string>();
            detailedDescription = new List<string>();
            infoHeaderFile = new List<string>();
            Regex regex = new Regex( @"\.\S+" );
            var match = regex.Match( headers[0] );
            this.extension = match.Value;


            this.category = category;
            this.header = headers;
            this.whatOpen = new List<string>(); //WhatOpen;
            string ceurrentCase;
            for( int i = 0; i < cells.Count; i++ )
            {

                ceurrentCase = CorrectorWords( cells[i], extension );

                switch( ceurrentCase )//ceurrentCase
                {
                    case "Описание файла на русском":
                    case "Описани�� файла на русском":
                        this.rusDescription.Add( cells[i += 1] );
                        break;
                    case "Описание файла на английском":
                        this.engDescription.Add( cells[i += 1] );
                        break;
                    case "Информация о заголовке файла ":
                        this.infoHeaderFile.Add( cells[i += 1] );
                        regex = new Regex( "ASCII" );
                        if( i < cells.Count - 1 && regex.IsMatch( cells[i + 1] ) )
                        {
                            this.infoHeaderFile.Add( cells[i += 1] );
                            if( i < cells.Count - 1 && regex.IsMatch( cells[i + 1] ) )
                                this.infoHeaderFile.Add( cells[i += 1] );
                        }
                        break;
                    case "Тип файла":
                        this.typeFile.Add( cells[i += 1] );
                        break;
                    case "Подробное описание":
                        this.detailedDescription.Add( cells[i += 1] );
                        break;
                    case "Как, чем открыть файл":
                        //i++;
                        try
                        {
                            while( i < cells.Count - 1 )
                            {
                                if( !("Описание файла на русском" == ceurrentCase || "Описани�� файла на русском" == ceurrentCase) )
                                {
                                    whatOpen.Add( cells[i += 1] );
                                    if( i < cells.Count - 1 )
                                    {
                                        i++;
                                        ceurrentCase = CorrectorWords( cells[i], extension );
                                    }
                                } else
                                { i--; break; }
                            }
                        } catch( IndexOutOfRangeException ) { }
                        break;

                    default:
                        Console.WriteLine( "Нет совпадения со столбцом в таблице = " + "\"" + cells[i] + "\"\n" );//throw new Exception( "Нет совпадения со столбцом в таблице = " + ceurrentCase );
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine( "Расширение файла" + extension );
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static string CorrectorWords( string word, string extension )
        {
            string ceurrentCase = Regex.Replace( word, @"\.\S+ ", "" );
            ceurrentCase = Regex.Replace( ceurrentCase, @" \.\S+\?", "" );
            if(
                "Описание файла на русском" != ceurrentCase &&
                "Описание файла на английском" != ceurrentCase &&
                "Тип файла" != ceurrentCase &&
                "Как, чем открыть файл" != ceurrentCase &&
                "Информация о заголовке файла " != ceurrentCase &&
                "Подробное описание" != ceurrentCase
                )
            {
                //Описани�� файла .5xb на русском

                ceurrentCase = Regex.Replace( ceurrentCase, @"^\S+ ф\S+а на русском", "Описание файла на русском" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^\S+ие ф\S+а на английском", "Описание файла на английском" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Описание файла на английск��м", "Описание файла на английском" );

                ceurrentCase = Regex.Replace( ceurrentCase, @"^\S+п ф\S+", "Тип файла" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^Т\S+ \S+ла^", "Тип файла" );
                //ceurrentCase = Regex.Replace( ceurrentCase, @"\w+ файла^", "Тип файла" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^Тип .+", "Тип файла" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^Тип ��айла", "Тип файла" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Ти�� файла", "Тип файла" );

                ceurrentCase = Regex.Replace( ceurrentCase, @"^Ин\S+ о за\S+ ф\S+ ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^\S+я о за\S+ ф\S+ ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Информация о заголовке ��айла ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Информация �� заголовке файла ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Информация \S+ заголовке файла ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Информация о \S+ файла ", "Информация о заголовке файла " );
                ceurrentCase = Regex.Replace( ceurrentCase, @"Информация о заголовке \S+ ", "Информация о заголовке файла " );

                ceurrentCase = Regex.Replace( ceurrentCase, @"^П\S+ое оп\S+е", "Подробное описание" );
                ceurrentCase = Regex.Replace( ceurrentCase, @"^К\S+к, ч\S+ о\S+ь ф\S+", "Как, чем открыть файл" );
            }
            if(
                 "Описание файла на русском" != ceurrentCase &&
                 "Описание файла на английском" != ceurrentCase &&
                 "Тип файла" != ceurrentCase &&
                 "Как, чем открыть файл" != ceurrentCase &&
                 "Информация о заголовке файла " != ceurrentCase &&
                 "Подробное описание" != ceurrentCase
                 )
            {
                FileInfo fileLog = new FileInfo( "Log Errors.txt" );
                StreamWriter writer = fileLog.AppendText();
                writer.WriteLine( "Страница с расширением файла: " + extension );
                writer.WriteLine( "Слово которое не было распознано: " + word );
                writer.WriteLine();
                writer.Close();
            }
            return ceurrentCase;
        }
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
