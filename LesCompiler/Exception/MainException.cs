/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class is the main-abstract-exception.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.Exception
{
    public class MainException : System.Exception
    {
        public enum Level
        {
            NOTICE,
            WARNING,
            ERROR
        };

        public Level level;
        public string message;

        public virtual void print()
        {
            SolidColorBrush color = null;

            switch (level)
            {
                case Level.WARNING:
                    color = Brushes.Yellow;
                    MainWindow.singleton.number_of_warnings++;
                    break;

                case Level.ERROR:
                    color = Brushes.Red;
                    MainWindow.singleton.number_of_errors++;
                    break;
            }

            Log.add(message, false, color);
        }

        public virtual string to_string()
        {
            return (level.ToString() + " " + message);
        }
    }
}
