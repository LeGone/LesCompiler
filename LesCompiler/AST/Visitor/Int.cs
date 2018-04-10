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
using System.Windows.Media;

namespace LesCompiler.AST.Visitor
{
    class Int : Main
    {
        public override string regex { get { return (@"(?<Int>[-+]?\d+)"); } }
        public override Type csharp_equivalent_type { get { return (typeof(int)); } }
        public int value;

        public Int()
        {
            highlight_color = Colors.DarkGreen;
        }

        public override void set_value(string value)
        {
            has_value = true;
            this.value = Convert.ToInt32(value);
        }

        public void set_value(int value)
        {
            has_value = true;
            this.value = value;
        }

        public override void assembler(ref ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldc_I4, value);
        }
    }
}
