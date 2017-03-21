using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchWork.Job
{
    class JobInfo
    {
        public JobInfo()
        {
        }

        private string _price = string.Empty;

        public string Url { get; set; }

        public string Title { get; set; }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                if( _price != "з/п договорная" || _price != "" )
                {
                    int priceMoney;
                    if( int.TryParse( _price, out priceMoney ) )
                        PriceMoney = priceMoney;
                }
            }
        }
        public string Telephone { get; set; }
        public int PriceMoney { get; set; }

        public string Company { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Description { get; set; }


        public Education Education { get; set; }

        public Employment Employment { get; set; }

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

        public static Employment ParseEmployment( string employment )
        {
            switch( employment )
            {
                case "полная":
                    return Employment.Full;
                case "частичная":
                    return Employment.Partial;
                case "фриланс":
                    return Employment.Freelance;
                default:
                    throw new Exception();
            }
        }

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
        Freelance
    }
}
