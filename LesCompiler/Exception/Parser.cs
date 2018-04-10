/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an error coming from the parser.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.Exception
{
    class Parser : MainException
    {
        private string file;
        private int line;

        public Parser(MainException.Level level, string msg, string file, int line)
        {
            this.level = level;
            this.message = message;
            this.file = file;
            this.line = line;
        }

        public override void print()
        {
            message = file + "(" + line + "): " + message;
            base.print();
        }

        public override string to_string()
        {
            return (level.ToString() + " " + message + " " + file + " " + line);
        }
    }
}
