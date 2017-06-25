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
        public string Category { get; internal set; }
        
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
                //if( _salary != "з/п договорная" || _salary != ""  || _salary != "Сдельная" )
                //{
                //    int priceMoney;
                //    if( int.TryParse( _salary, out priceMoney ) )
                //        PriceValue = priceMoney;
                //}
            }
        }
        public string City { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Schedule { get; set; }

        public string ContFace { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public string Url { get; set; }
        public string Description { get; set; }

        //public string Employment { get; set; }
        //public string Company { get; set; }
        //public string Country { get; set; }
        //public string Region { get; set; }
        //public int PriceValue { get; set; }
    }   
}
