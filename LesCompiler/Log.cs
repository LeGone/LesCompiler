/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class prints a message to the GUI.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LesCompiler
{
    class Log
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void add(string msg, bool print_info_about_caller = false, SolidColorBrush color = null)
        {
            string pre = String.Empty;

            if (print_info_about_caller)
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);

                pre = sf.GetMethod().DeclaringType.Name;
                pre += "::";
                pre += sf.GetMethod().Name;
                pre += "()->";
            }

            // Add new item
            ListBoxItem itm = new ListBoxItem();
            itm.Content = pre + msg;
            if (color != null)
                itm.Foreground = color;
            MainWindow.singleton.lb_log.Items.Add(itm);
        }
    }
}
