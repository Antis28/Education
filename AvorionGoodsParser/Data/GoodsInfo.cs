﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvorionGoodsParser.myData
{
    [Serializable]
    public class GoodsInfo    
    {
        // необходим для сериализации
        public GoodsInfo()
        {
        }

        public string Name { get; set; }
        public string Price { get; set; }
        public string Volume { get; set; }
        public string isIllegal { get; set; }
        public string isDangerous { get; set; }

        public string[] SoldBy  { get; set; }
        public string[] BoughtBy  { get; set; }
        
        void dsds()
        {
            SoldBy.Contains("d");
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
