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

namespace LesCompiler.AST.Visitor
{
    class Plus : Main
    {
        public override string regex { get { return (@"(?<Plus>\+)"); } }

        public Plus()
        {
            visitor_type = Visitor_Type.OPERATOR;
        }

        public Main operator_work(Int visitor_1, Int visitor_2)
        {
            Int result_visitor = new Int();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Decimal visitor_1, Decimal visitor_2)
        {
            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Quoted_String visitor_1, Quoted_String visitor_2)
        {
            Quoted_String result_visitor = new Quoted_String();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Quoted_String visitor_1, Int visitor_2)
        {
            Quoted_String result_visitor = new Quoted_String();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Quoted_String visitor_1, Decimal visitor_2)
        {
            Quoted_String result_visitor = new Quoted_String();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Variable visitor_1, Quoted_String visitor_2)
        {
            return (null);
        }

        public Main operator_work(Quoted_String visitor_1, Variable visitor_2)
        {
            return (null);
        }

        public Main operator_work(Decimal visitor_1, Int visitor_2)
        {
            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Int visitor_1, Decimal visitor_2)
        {
            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value + visitor_2.value);
            return (result_visitor);
        }

        public override void assembler(ref ILGenerator gen)
        {
            gen.Emit(OpCodes.Add);
        }
    }
}