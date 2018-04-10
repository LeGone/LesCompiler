/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This file is the factory for building something like an AST.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.AST
{
    class Factory
    {
        public static List<Visitor.Function_Define> list_of_functions = new List<Visitor.Function_Define>();

        private static void build_function_defines(List<Main> list_of_visitors)
        {
            bool definition = false;
            int parameter_index = 0;
            list_of_functions = new List<Visitor.Function_Define>();
            Visitor.Function_Define current_function = null;
            foreach (Main visitor in list_of_visitors)
            {
                Visitor.Function_Define function = visitor as Visitor.Function_Define;
                if (function != null)
                {
                    definition = true;
                    parameter_index = 0;
                    
                    current_function = function;
                    list_of_functions.Add(function);
                }
                else
                {
                    if (definition)
                    {
                        Visitor.Braces_Right braces_right = visitor as Visitor.Braces_Right;
                        if (braces_right == null)
                        {
                            // Ignore left-braces
                            Visitor.Braces_Left braces_left = visitor as Visitor.Braces_Left;
                            if (braces_left == null)
                            {
                                // We only want variables as parameters
                                Visitor.Variable variable = visitor as Visitor.Variable;
                                if (variable != null)
                                {
                                    variable.parameter_index = parameter_index;
                                    parameter_index++;
                                    current_function.list_of_params.Add(variable);
                                }
                                else
                                {
                                    throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in function-head: " + visitor.GetType().Name + " should not be in the head.", visitor.file_name, visitor.index);
                                }
                            }
                        }
                        else
                        {
                            definition = false;
                        }
                    }
                    else
                    {
                        bool found = false;
                        Visitor.Variable variable = visitor as Visitor.Variable;
                        if (variable != null)
                        {
                            foreach (Visitor.Variable parameter in current_function.list_of_params)
                            {
                                if (variable.name == parameter.name)
                                {
                                    current_function.list_of_instructions.Add(parameter);
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (!found)
                            current_function.list_of_instructions.Add(visitor);
                    }
                }
            }
        }

        private static void build_function_calls()
        {
            bool definition = false;
            Visitor.Function_Call current_function_call = null;

            foreach (Visitor.Function_Define function in list_of_functions)
            {
                List<Main> new_list_of_instructions = new List<Main>();
                foreach (Main visitor in function.list_of_instructions)
                {
                    Visitor.Function_Call function_call = visitor as Visitor.Function_Call;
                    if (function_call != null)
                    {
                        definition = true;
                        current_function_call = function_call;
                    }
                    else
                    {
                        if (definition)
                        {
                            Visitor.Braces_Right braces_right = visitor as Visitor.Braces_Right;
                            if (braces_right == null)
                            {
                                if (visitor.has_value || visitor.GetType() == typeof(Visitor.Variable) || visitor.visitor_type == Main.Visitor_Type.OPERATOR)
                                {
                                    current_function_call.list_of_params.Add(visitor);
                                    continue;
                                }
                                else
                                {
                                    throw new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in function-call: " + visitor.GetType().Name + " does not have any value.", visitor.file_name, visitor.index);
                                }
                            }
                            else
                            {
                                definition = false;
                            }
                        }
                    }

                    new_list_of_instructions.Add(visitor);
                }
                function.list_of_instructions = new_list_of_instructions;
            }
        }

        private static void build_arithmetic()
        {
            foreach (Visitor.Function_Define function in list_of_functions)
            {
                for (int count_of_instructions = function.list_of_instructions.Count - 1; count_of_instructions > 0 ; count_of_instructions--)
                {
                    Main visitor = function.list_of_instructions[count_of_instructions - 1];
                    if (visitor.visitor_type == Main.Visitor_Type.OPERATOR)
                    {
                        Main value_1 = function.list_of_instructions[count_of_instructions];
                        Main value_2 = function.list_of_instructions[count_of_instructions - 2];
                        Main result = visitor.operate(value_2, value_1) as Main;

                        if (result != null)
                        {
                            function.list_of_instructions.RemoveRange(count_of_instructions - 2, 2);
                            function.list_of_instructions[count_of_instructions - 2] = result;
                            count_of_instructions--;
                        }
                    }
                }
            }
        }

        public static void build(List<Main> list_of_visitors)
        {
            build_function_defines(list_of_visitors);
            build_arithmetic();
            build_function_calls();
        }
    }
}
