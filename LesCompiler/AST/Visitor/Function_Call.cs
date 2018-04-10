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
    class Function_Call : Main
    {
        public override string regex { get { return (@"((?<Function_Call>[a-zA-Z0-9_]+)\()"); } }
        public string function_name = String.Empty;
        public List<Main> list_of_params = new List<Main>();

        public Function_Call()
        {
            highlight_color = Colors.Blue;
        }

        public override void set_value(string value)
        {
            function_name = value;
        }

        public override void assembler(ref ILGenerator gen)
        {
            int count_of_parameters = 0;
            MethodInfo function_definition = null;
            foreach (Visitor.Function_Define function in Factory.list_of_functions)
            {
                if (function.function_name == function_name)
                {
                    function_definition = function.function_definition;
                    count_of_parameters = function.list_of_params.Count;
                }
            }

            if (function_definition == null)
            {
                List<Type> param_types = new List<Type>();
                foreach (Main visitor in list_of_params)
                    if (visitor.csharp_equivalent_type != null)
                        param_types.Add(visitor.csharp_equivalent_type);
                    else
                        param_types.Add(typeof(string));

                function_definition = typeof(Console).GetMethod(function_name, param_types.ToArray());
                if (function_definition == null)
                {
                    string definition_as_string = function_name + '(';
                    foreach (Main visitor in list_of_params)
                        definition_as_string += visitor.GetType().Name + ",";

                    if (list_of_params.Count > 0)
                        definition_as_string = definition_as_string.TrimEnd(new char[] { ',' });
                    definition_as_string += ')';

                    throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Error finding build-in-function " + definition_as_string + ". Not found or wrong arguments.", file_name, index);
                }
                count_of_parameters = function_definition.GetParameters().Length;
            }

            if (count_of_parameters == list_of_params.Count)
            {
                foreach (Main visitor in list_of_params)
                {
                    visitor.assembler(ref gen);
                }
            }
            else
            {
                throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Number of parameters wrong.", file_name, index);
            }

            gen.EmitCall(OpCodes.Call, function_definition, null);
        }
    }
}