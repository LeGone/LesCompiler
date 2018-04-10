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
using System.Windows.Media;

namespace LesCompiler.AST.Visitor
{
    class Include : Main
    {
        public override string regex { get { return (@"(?<Include>###[a-zA-Z0-9.]+###)"); } }

        public Include()
        {
            highlight_color = Colors.Blue;
        }
    }
}