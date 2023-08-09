using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

using System.Management;

namespace Kaya_Mobilya_ve_Dekorasyon
{

    internal static class Program
    {
        
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new girisForm());
            
        }
    }
}
