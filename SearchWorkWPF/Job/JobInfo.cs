using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchWorkWPF.Job
{
    class JobInfo
    {
        public JobInfo()
        {
        }

        private string _salary = string.Empty;

        public string Date { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                _salary = value;
                if( _salary != "з/п договорная" || _salary != ""  || _salary != "Сдельная" )
                {
                    int priceMoney;
                    if( int.TryParse( _salary, out priceMoney ) )
                        PriceMoney = priceMoney;
                }
            }
        }
        public string Telephone { get; set; }
        public string ContFace { get; set; }
        public string Email { get; set; }

        public int PriceMoney { get; set; }

        public string Company { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Description { get; set; }


        public Education Education { get; set; }

        public string Employment { get; set; }
        public string Schedule { get; set; }

        public static Education ParseEducation( string education )
        {
            switch( education )
            {
                case "Высшее":
                    return Education.Higher;
                case "Неполное высшее":
                    return Education.IncompleteHigher;
                case "Среднее специальное":
                    return Education.SecondaryVocational;
                case "Среднее":
                    return Education.Secondary;
                case "Учащийся":
                    return Education.Pupil;
                case "Не имеет значения":
                    return Education.None;
                default:
                    throw new Exception();
            }
        }

        //public static Employment ParseEmployment( string employment )
        //{
        //    switch( employment )
        //    {
        //        case "полная":
        //            return Employment.Full;
        //        case "частичная":
        //            return Employment.Partial;
        //        case "фриланс":
        //            return Employment.Freelance;
        //        case "свободный график":
        //            return Employment.FreeSchedule;
        //        default:
        //            throw new Exception();
        //    }
        //}
        //public static string ParseEmployment( Employment employment )
        //{
        //    switch( employment )
        //    {
        //        case Employment.Full:
        //            return "полная";
        //        case Employment.Partial:
        //            return "частичная";
        //        case Employment.Freelance:
        //            return "фриланс";
        //        case Employment.FreeSchedule:
        //            return "свободный график";
        //        default:
        //            throw new Exception();
        //    }
        //}

    }
    public enum Education
    {
        None,
        Higher,
        IncompleteHigher,
        SecondaryVocational,
        Secondary,
        Pupil
    }
    public enum Employment
    {
        Full,
        Partial,
        Freelance,
        FreeSchedule
    }
}
