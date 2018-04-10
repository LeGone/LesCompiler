/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an visitor.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.AST.Visitor
{
    class Quoted_String : Main
    {
        public override string regex { get { return (@"(?<Quoted_String>""[^""\\]*(?:\\.[^""\\]*)*"")"); } }
        public override Type csharp_equivalent_type { get { return (typeof(string)); } }
        public string value = String.Empty;

        public Quoted_String()
        {
            highlight_color = Colors.Brown;
        }

        public override void set_value(string value)
        {
            value = value.Trim(new char[] { '"' });
            value = value.Replace(@"\""", @"""");

            this.value = value;
            has_value = true;
        }

        public override void assembler(ref ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldstr, value);
        }
    }
}