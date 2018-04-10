using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LesCompiler.Parser
{
    class Lexer
    {
        public List<AST.Main> list_of_visitors = new List<AST.Main>();

        public Lexer(string file_name, string full_file)
        {
            file_name = System.IO.Path.GetFileName(file_name);

            Regex regexed_patteren = null;
            try
            {
                Type [] visitor_types = LesCompiler.Helper.get_all_classes_from_namespace("LesCompiler.AST.Visitor");

                string pattern = String.Empty;
                foreach (Type visitor_type in visitor_types)
                {
                    // Get the real vistor
                    object visitor = Activator.CreateInstance(visitor_type);
                    
                    // Get regex
                    pattern += ((AST.Main)visitor).regex + '|';
                }

                regexed_patteren = new Regex(pattern, RegexOptions.Singleline);
            }
            catch (System.Exception e)
            {
                throw new Exception.Lexer(Exception.MainException.Level.ERROR, e.ToString(), file_name, 0);
            }

            int line = 1;
            MatchCollection matches = regexed_patteren.Matches(full_file);
            foreach (Match match in matches)
            {
                int i = 0;
                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    bool success = group.Success;

                    string groupName = regexed_patteren.GroupNameFromNumber(i);
                    i++;

                    if (String.IsNullOrEmpty(matchValue) || LesCompiler.Helper.is_string_numeric(groupName))
                        continue;

                    if (groupName == "New_Line")
                    {
                        line++;
                    }
                    else if (groupName == "Include")
                    {
                        line = 1;
                        file_name = System.IO.Path.GetFileName(matchValue);
                        file_name = file_name.Replace("###", "");
                    }
                    else
                    {
                        Console.WriteLine("Line: " + groupName + ":" + matchValue);
                        object abstract_visitor = Activator.CreateInstance(Type.GetType("LesCompiler.AST.Visitor." + groupName + ", LesCompiler"));
                        AST.Main visitor = abstract_visitor as AST.Main;
                        visitor.set_value(matchValue);
                        visitor.index = line;
                        visitor.file_name = file_name;
                        list_of_visitors.Add(visitor);
                    }
                }
            }
        }
    }
}
