using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LesCompiler.Parser
{
    class Assembler
    {
        public Assembler(List<AST.Visitor.Function_Define> ast, string file_name)
        {
            Type pointType = null;

            string only_name = System.IO.Path.GetFileName(file_name);
            string path = System.IO.Path.GetDirectoryName(file_name);

            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "Main";

            AssemblyBuilder myAsmBldr = Thread.GetDomain().DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave, path);
            ModuleBuilder myModuleBldr = myAsmBldr.DefineDynamicModule(only_name, only_name);
            TypeBuilder type_builder = myModuleBldr.DefineType("Main");

            foreach (AST.Visitor.Function_Define function in ast)
            {
                function.assembler(ref type_builder);

                if (function.function_name.ToLower() == "main")
                    myAsmBldr.SetEntryPoint(function.function_definition);
            }

            if (myAsmBldr.EntryPoint == null)
                throw new Exception.Assembler(Exception.MainException.Level.ERROR, "No entrypoint(function main) found.", "", 0);

            pointType = type_builder.CreateType();

            myAsmBldr.Save(only_name);
        }
    }
}
