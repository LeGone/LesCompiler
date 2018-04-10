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
    class Variable : Main
    {
        public override string regex { get { return (@"(\$(?<Variable>[a-zA-Z0-9]+))"); } }
        LocalBuilder local = null;

        public string name;
        public Main value = null;

        public int parameter_index = -1;
        
        public Variable()
        {
            highlight_color = Colors.Red;
        }

        public override void set_value(Main visitor)
        {
            value = visitor;
        }

        public override void set_value(string name)
        {
            this.name = name;
        }

        public void assembler_declare(ref ILGenerator gen)
        {
            if (local != null)
                throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Variable " + name + " already declared", file_name, index);

            local = gen.DeclareLocal(typeof(string));

            if (parameter_index > -1)
            {
                gen.Emit(OpCodes.Ldarg, parameter_index);
                gen.Emit(OpCodes.Stloc, local);
            }
        }

        public override void assembler(ref ILGenerator gen)
        {
            if (local == null)
                throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Undeclared variable " + name, file_name, index);
            gen.Emit(OpCodes.Ldloc, local);
        }
    }
}