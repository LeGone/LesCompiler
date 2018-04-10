using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesCompiler.Parser
{
    class Parser
    {
        string full_file = String.Empty;
        string file_name = String.Empty;

        public Lexer lexer;

        public Parser(string file_name)
        {
            this.file_name = file_name;
        }

        public void work()
        {
            full_file = Helper.read_file(file_name);
            if (full_file == String.Empty)
                throw new Exception.Unspecific(Exception.Parser.Level.ERROR, "Unable to read file named \"" + file_name + "\"");

            PreProcessor pre_processor = new PreProcessor(full_file, file_name);
            full_file = pre_processor.work().full_file;

            Helper.write_file(file_name + ".lesc", full_file);

            lexer = new Lexer(file_name, full_file);
        }
    }
}
