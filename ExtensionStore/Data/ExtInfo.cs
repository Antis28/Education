using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionStore
{
    public class ExtInfo
    {
        public ExtInfo()
        {
            WhatOpenWindows = new List<string>();
            WhatOpenMac = new List<string>();
            WhatOpenLinux = new List<string>();
            InfoHeaderFile = new List<string>();

            Name = String.Empty;
            RusDescription = String.Empty;
            EngDescription = String.Empty;
            DetailedDescription = String.Empty;
            TypeFile = String.Empty;
        }
        public static ExtInfo operator +( ExtInfo left, ExtInfo right )
        {
            ExtInfo temp = new ExtInfo();
            temp.Name = left.Name;
            if( left.TypeFile != string.Empty  )
                temp.TypeFile = left.TypeFile + ", " + right.TypeFile;
            temp.InfoHeaderFile.AddRange(left.InfoHeaderFile);
            temp.InfoHeaderFile.AddRange(right.InfoHeaderFile);
            temp.EngDescription = left.EngDescription + "\n" + right.EngDescription;
            temp.RusDescription = left.RusDescription + "\n" + right.RusDescription;
            temp.DetailedDescription = left.DetailedDescription + "\n" + right.DetailedDescription;
            temp.WhatOpenWindows.AddRange(left.WhatOpenWindows);
            temp.WhatOpenWindows.AddRange(right.WhatOpenWindows);
            temp.WhatOpenLinux.AddRange(left.WhatOpenLinux);
            temp.WhatOpenLinux.AddRange(right.WhatOpenLinux);
            temp.WhatOpenMac.AddRange(left.WhatOpenMac);
            temp.WhatOpenMac.AddRange(right.WhatOpenMac);

            return temp;
        }

        public string Name { get; set; }
        public string Link { get; set; }

        public string TypeFile { get; set; }
        public string RusDescription { get; set; }
        public string EngDescription { get; set; }
        public string DetailedDescription { get; set; }

        public string WhatOpen { get; set; }
        public List<string> WhatOpenWindows { get; set; }
        public List<string> WhatOpenMac { get; set; }
        public List<string> WhatOpenLinux { get; set; }
        public List<string> InfoHeaderFile { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
