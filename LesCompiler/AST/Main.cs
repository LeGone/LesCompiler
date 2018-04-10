/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This class is the abstract visitor.
 * All the other visitors are deriving from this class.
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace LesCompiler.AST
{
    class Main
    {
        public enum Visitor_Type
        {
            NONE,
            OPERATOR
        }

        public Visitor_Type visitor_type = Visitor_Type.NONE;
        public Color highlight_color = Colors.Black;

        public virtual string regex { get; protected set; }
        public virtual Type csharp_equivalent_type { get; protected set; }

        public string file_name = String.Empty;
        public int line = 0;
        public int index = 0;
        public bool has_value = false;

        public virtual void set_value(string value)
        {
        }

        public virtual void set_value(Main value)
        {
        }

        public Object operate(Object visitor_1, Object visitor_2)
        {
            Type type_of_this = GetType();
            try
            {
                return (type_of_this.InvokeMember("operator_work", System.Reflection.BindingFlags.InvokeMethod, null, this, new Object[] { visitor_1, visitor_2 }));
            }
            catch (System.Exception)
            {
                throw new Exception.Lexer(Exception.MainException.Level.ERROR, type_of_this.Name + " has no arithmetic for type " + visitor_1.GetType().Name + " and " + visitor_2.GetType().Name, file_name, line);
            }
        }

        public virtual void assembler(ref ILGenerator gen)
        {
        }

        public string to_string()
        {
            return (GetType().Name);
        }
    }
}
