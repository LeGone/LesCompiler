/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an visitor.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.AST.Visitor
{
    class Decimal : Main
    {
        public override string regex { get { return (@"(?<Decimal>[-+]?\d*\.\d+([eE][-+]?\d+)?)"); } }
        public override Type csharp_equivalent_type { get { return (typeof(double)); } }
        public double value;

        public Decimal()
        {
            highlight_color = Colors.DarkGreen;
        }

        public override void set_value(string value)
        {
            has_value = true;
            Double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out this.value);
        }

        public void set_value(double value)
        {
            has_value = true;
            this.value = value;
        }

        public override void assembler(ref ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldc_R8, value);
        }
    }
}
