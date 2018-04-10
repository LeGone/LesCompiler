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
    class Comment : Main
    {
        public override string regex { get { return (@"(?<Comment>((/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//(.*?)\r?\n)))"); } }

        public Comment()
        {
            highlight_color = Colors.Green;
        }
    }
}