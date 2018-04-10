using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.Parser
{
    class Helper
    {
        public static string read_file(string file_name)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file_name))
                {
                    return (sr.ReadToEnd());
                }
            }
            catch (System.Exception e)
            {
                Log.add(e.Message, true, Brushes.Red);
            }

            return (String.Empty);
        }

        public static void write_file(string file_name, string data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file_name))
                {
                    sw.Write(data);
                }
            }
            catch (System.Exception e)
            {
                Log.add(e.Message, true, Brushes.Red);
            }
        }
    }
}
