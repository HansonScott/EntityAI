using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityAI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application, temporary for develpment and testing.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Entity E = new Entity();
            E.Run();
        }
    }
}
