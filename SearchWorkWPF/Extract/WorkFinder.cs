using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using SearchWorkWPF.Job;

namespace SearchWork.Extract
{
    class WorkFinder
    {
        public List<JobInfo> GetJobLinksInMozaika()
        {
            return JobsInMozaika.GetJobList();
        }
        private static string ExtractSalary(HtmlDocument currentHTML)
        {
            HtmlNode priseNode = currentHTML.DocumentNode.ChildNodes[1].ChildNodes[5];
            string salary = priseNode.InnerHtml;
            string price = "Не указано";
            if (salary.Contains("Зарплата"))
            {
                price = priseNode.SelectSingleNode("font[@class=\"rabota-fieldsb\"]").InnerText;
            }

            return price;
        }
    }
}







