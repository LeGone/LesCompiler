/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class represents an visitor.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.AST.Visitor
{
    class Function_Define : Main
    {
        public override string regex { get { return (@"(function\s(?<Function_Define>[a-zA-Z0-9_]+))"); } }
        public string function_name = String.Empty;

        public MethodBuilder function_definition;
        public List<Variable> list_of_params = new List<Variable>();
        public List<Main> list_of_instructions = new List<Main>();

        public Function_Define()
        {
            highlight_color = Colors.BlueViolet;
        }

        public override void set_value(string value)
        {
            function_name = value;
        }

        public void assembler(ref TypeBuilder type_builder)
        {
            List<Type> param_types = new List<Type>();
            foreach (AST.Visitor.Variable variable in list_of_params)
            {
                param_types.Add(typeof(string));
            }

            function_definition = type_builder.DefineMethod(function_name, MethodAttributes.Public | MethodAttributes.Static, typeof(void), param_types.ToArray());
            ILGenerator generator = function_definition.GetILGenerator();

            // Declares - Head
            foreach (Variable variable in list_of_params)
            {
                variable.assembler_declare(ref generator);
            }

            // Declares - Content
            foreach (Main visitor in list_of_instructions)
            {
                AST.Visitor.Variable variable = visitor as Variable;
                if (variable != null)
                    variable.assembler_declare(ref generator);
            }

            foreach (Main visitor in list_of_instructions)
                visitor.assembler(ref generator);

            generator.Emit(OpCodes.Ret);
        }
    }
}