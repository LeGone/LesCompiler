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

namespace LesCompiler.AST.Visitor
{
    class Divide : Main
    {
        public override string regex { get { return (@"(?<Divide>\/)"); } }

        public Divide()
        {
            visitor_type = Visitor_Type.OPERATOR;
        }

        public Main operator_work(Int visitor_1, Int visitor_2)
        {
            if (visitor_2.value == 0)
            {
                new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in devide-arithmetic: division by 0!", file_name, index).print();
                return (null);
            }

            Int result_visitor = new Int();
            result_visitor.set_value(visitor_1.value / visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Decimal visitor_1, Decimal visitor_2)
        {
            if (visitor_2.value == 0)
            {
                new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in devide-arithmetic: division by 0!", file_name, index).print();
                return (null);
            }

            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value / visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Decimal visitor_1, Int visitor_2)
        {
            if (visitor_2.value == 0)
            {
                new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in devide-arithmetic: division by 0!", file_name, index).print();
                return (null);
            }

            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value / visitor_2.value);
            return (result_visitor);
        }

        public Main operator_work(Int visitor_1, Decimal visitor_2)
        {
            if (visitor_2.value == 0)
            {
                new Exception.Assembler(Exception.MainException.Level.ERROR, "Error in devide-arithmetic: division by 0!", file_name, index).print();
                return (null);
            }

            Decimal result_visitor = new Decimal();
            result_visitor.set_value(visitor_1.value / visitor_2.value);
            return (result_visitor);
        }
    }
}