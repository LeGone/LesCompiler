/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This file contains a few helper-methods.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler
{
    class Helper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string get_current_method()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return (sf.GetMethod().Name);
            
            //return (System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public static Type[] get_all_classes_from_namespace(string the_namespace)
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, the_namespace, StringComparison.Ordinal)).ToArray();
        }

        public static bool is_string_numeric(string str)
        {
            int i = 0;
            return (int.TryParse(str, out i));
        }
    }
}
