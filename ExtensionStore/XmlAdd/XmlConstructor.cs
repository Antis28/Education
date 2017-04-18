using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ExtensionStore
{
    class XmlConstructor
    {
        string fileName = "ExtensionsBase.xml";
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
            xmlWriter = new XmlTextWriter(fileName, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,   // Включить форматирование документа (с отступом).
                IndentChar = '\t',                  // Для выделения уровня элемента использовать табуляцию.
                Indentation = 1,                    // использовать один символ табуляции.
                //QuoteChar = '\''                  // способ записи ковычек
            };
            if( !System.IO.File.Exists("ExtensionsBase.xml") )
                xmlWriter.WriteStartDocument();         //<? xml version = "1.0" ?>

            xmlWriter.WriteComment("Список всех расширений.");
            xmlWriter.WriteStartElement("ListOfExtension");
            //xmlWriter.WriteComment( "Кактегория файла(аудио, видео, архив)." );
            //xmlWriter.WriteStartElement( "Category" );
        }

        public void Add()
        {
            //xmlWriter.WriteStartElement( category );
            xmlWriter.WriteStartElement(extension);
            foreach( var item in header )
            {
                xmlWriter.WriteStartElement("Header");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in rusDescription )
            {
                xmlWriter.WriteStartElement("rusDescription");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in engDescription )
            {
                xmlWriter.WriteStartElement("engDescription");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in typeFile )
            {
                xmlWriter.WriteStartElement("typeFile");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in infoHeaderFile )
            {
                xmlWriter.WriteStartElement("infoHeaderFile");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in detailedDescription )
            {
                xmlWriter.WriteStartElement("detailedDescription");
                xmlWriter.WriteAttributeString("Value", item);
                xmlWriter.WriteEndElement();
            }

            foreach( var item in whatOpen )
            {
                xmlWriter.WriteStartElement("WhatOpen");
                xmlWriter.WriteAttributeString("Value", item);
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
            Regex regex = new Regex(@"\.\S+");
            var match = regex.Match(headers[0]);
            this.extension = match.Value;


            this.category = category;
            this.header = headers;
            this.whatOpen = new List<string>(); //WhatOpen;
            string currentCase;
            for( int i = 0; i < cells.Count; i++ )
            {

                currentCase = CorrectorWords(cells[i], extension);

                switch( currentCase )//ceurrentCase
                {
                    case "Описание файла на русском":
                    case "Описани�� файла на русском":
                        this.rusDescription.Add(cells[i += 1]);
                        break;
                    case "Описание файла на английском":
                        this.engDescription.Add(cells[i += 1]);
                        break;
                    case "Информация о заголовке файла ":
                        this.infoHeaderFile.Add(cells[i += 1]);
                        regex = new Regex("ASCII");
                        if( i < cells.Count - 1 && regex.IsMatch(cells[i + 1]) )
                        {
                            this.infoHeaderFile.Add(cells[i += 1]);
                            if( i < cells.Count - 1 && regex.IsMatch(cells[i + 1]) )
                                this.infoHeaderFile.Add(cells[i += 1]);
                        }
                        break;
                    case "Тип файла":
                        this.typeFile.Add(cells[i += 1]);
                        break;
                    case "Подробное описание":
                        this.detailedDescription.Add(cells[i += 1]);
                        break;
                    case "Как, чем открыть файл":
                        //i++;
                        try
                        {
                            while( i < cells.Count - 1 )
                            {
                                if( !("Описание файла на русском" == currentCase || "Описани�� файла на русском" == currentCase) )
                                {
                                    whatOpen.Add(cells[i += 1]);
                                    if( i < cells.Count - 1 )
                                    {
                                        i++;
                                        currentCase = CorrectorWords(cells[i], extension);
                                    }
                                }
                                else
                                { i--; break; }
                            }
                        } catch( IndexOutOfRangeException ) { }
                        break;

                    default:
                        Console.WriteLine("Нет совпадения со столбцом в таблице = " + "\"" + cells[i] + "\"\n");//throw new Exception( "Нет совпадения со столбцом в таблице = " + ceurrentCase );
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Расширение файла" + extension);
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static string CorrectorWords( string word, string extension )
        {
            string descriptOnRussian = "Описание файла на русском";
            string descriptOnEnglish = "Описание файла на английском";
            string typeFile = "Тип файла";
            string infoHeader = "Информация о заголовке файла ";
            string fullDescript = "Подробное описание";
            string whatOpen = "Как, чем открыть файл";
            bool isWrongWords;

            string currentCase = Regex.Replace(word, @"\.\S+ ", "");
            currentCase = Regex.Replace(currentCase, @" \.\S+\?", "");

            isWrongWords = descriptOnRussian != currentCase &&
                                descriptOnEnglish != currentCase &&
                                typeFile != currentCase &&
                                whatOpen != currentCase &&
                                infoHeader != currentCase &&
                                fullDescript != currentCase;
            if( isWrongWords )
            {
                //Описани�� файла .5xb на русском              

                currentCase = Regex.Replace(currentCase, @"^\S+ ф\S+а на русском", descriptOnRussian);
                currentCase = Regex.Replace(currentCase, @"^\S+ие ф\S+а на английском", descriptOnEnglish);
                currentCase = Regex.Replace(currentCase, @"Описание файла на английск��м", descriptOnEnglish);

                currentCase = Regex.Replace(currentCase, @"^\S+п ф\S+", typeFile);
                currentCase = Regex.Replace(currentCase, @"^Т\S+ \S+ла^", typeFile);
                //ceurrentCase = Regex.Replace( ceurrentCase, @"\w+ файла^", "Тип файла" );
                currentCase = Regex.Replace(currentCase, @"^Тип .+", typeFile);
                currentCase = Regex.Replace(currentCase, @"^Тип ��айла", typeFile);
                currentCase = Regex.Replace(currentCase, @"Ти�� файла", typeFile);

                currentCase = Regex.Replace(currentCase, @"^Ин\S+ о за\S+ ф\S+ ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"^\S+я о за\S+ ф\S+ ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"Информация о заголовке ��айла ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"Информация �� заголовке файла ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"Информация \S+ заголовке файла ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"Информация о \S+ файла ", infoHeader);
                currentCase = Regex.Replace(currentCase, @"Информация о заголовке \S+ ", infoHeader);

                currentCase = Regex.Replace(currentCase, @"^П\S+ое оп\S+е", fullDescript);
                currentCase = Regex.Replace(currentCase, @"^К\S+к, ч\S+ о\S+ь ф\S+", whatOpen);

                isWrongWords = descriptOnRussian != currentCase &&
                                     descriptOnEnglish != currentCase &&
                                     typeFile != currentCase &&
                                     whatOpen != currentCase &&
                                     infoHeader != currentCase &&
                                     fullDescript != currentCase;

                if( isWrongWords )
                {
                    FileInfo fileLog = new FileInfo("Log Errors.txt");
                    StreamWriter writer = fileLog.AppendText();
                    writer.WriteLine("Страница с расширением файла: " + extension);
                    writer.WriteLine("Слово которое не было распознано: " + word);
                    writer.WriteLine();
                    writer.Close();
                }
            }            
            return currentCase;
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
