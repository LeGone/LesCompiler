/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an error coming from the lexer.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.Exception
{
    class Lexer : MainException
    {
        private string file;
        private int line;

        public Lexer(MainException.Level level, string message, string file, int line)
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
