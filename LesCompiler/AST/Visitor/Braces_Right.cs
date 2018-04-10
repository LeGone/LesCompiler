/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an visitor.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.AST.Visitor
{
    class Braces_Right : Main
    {
        public override string regex { get { return (@"(?<Braces_Right>\))"); } }
    }
}