/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an error coming an unspecifig place.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.Exception
{
    class Unspecific : MainException
    {
        public Unspecific(Level level, string message)
        {
            this.level = level;
            this.message = message;
        }
    }
}
