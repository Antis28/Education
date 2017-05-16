using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AntisLib.Net
{
    /// <summary>
    /// Проверка доступности интернета
    /// </summary>
    public class InternetChecker
    {
        private static InternetConnectionState flags = 0;

        public static InternetConnectionState Flags
        {
            get
            {
                return flags;
            }

            set
            {
                flags = value;
            }
        }

        [DllImport("WININET", CharSet = CharSet.Auto)]
        static extern bool InternetGetConnectedState(
            ref InternetConnectionState lpdwFlags,
            int dwReserved );

        [Flags]
        public enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        /// <summary>
        /// Internet ONLINE ?
        /// </summary>
        /// <returns>true - ONLINE</returns>
        public static bool InternetGetConnectedState()
        {
            bool state = InternetGetConnectedState(ref flags, 0);
            return state;
        }

        static void Run()
        {
            InternetConnectionState flags = 0;

            Console.WriteLine(
                "InternetGetConnectedState : {0} - {1}",
                (InternetGetConnectedState(ref flags, 0) ? "ONLINE" : "OFFLINE"),
                flags
                );
            Console.ReadLine();
        }
    }
}