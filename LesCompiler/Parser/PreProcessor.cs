using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LesCompiler.Parser
{
    class PreProcessor
    {
        List<KeyValuePair<string, string>> macros = new List<KeyValuePair<string, string>>();

        public string file_name = String.Empty;
        public string full_file = String.Empty;
        int line;
        int index_of_last_element = 0;

        public PreProcessor(string full_file, string file_name = "")
        {
            this.file_name = file_name;
            this.full_file = full_file;
        }

        private PreProcessor(string file_name)
        {
            this.file_name = file_name;
            full_file = Helper.read_file(file_name);
        }

        public PreProcessor work()
        {
            // Define internal Macros
            macros.Add(new KeyValuePair<string, string>("__FILE__", System.IO.Path.GetFileName(file_name)));
            macros.Add(new KeyValuePair<string, string>("__DATE__", DateTime.Now.ToShortDateString()));
            macros.Add(new KeyValuePair<string, string>("__TIME__", DateTime.Now.ToShortTimeString()));

            replace_macros();

            for (line = 1; ; line++)
            {
                try
                {
                    int position_of_directive = full_file.IndexOf('#', index_of_last_element);
                    int position_of_eol = full_file.IndexOf("\r\n", index_of_last_element);

                    if (position_of_directive == -1)
                        break;

                    if (position_of_eol == -1)
                    {
                        new Exception.PreProcessor(Exception.PreProcessor.Level.WARNING, "No new line at the end of a line.", file_name, line).print();
                        break;
                    }

                    if (position_of_directive >= position_of_eol)
                    {
                        index_of_last_element = position_of_directive;
                        continue;
                    }

                    //position_of_eol++;

                    string directive = String.Empty;
                    string param = String.Empty;

                    // Get directive
                    int i;
                    int b = 0;
                    for (i = position_of_directive + 1; i < position_of_eol; i++)
                    {
                        b++;
                        char c = full_file[i];
                        if (Char.IsLetterOrDigit(c))
                            directive += c;
                        else
                            break;
                    }

                    // Get the args
                    for (; i < position_of_eol; i++)
                    {
                        b++;
                        param += full_file[i];
                    }

                    // Empty directive
                    if (String.IsNullOrEmpty(directive))
                    {
                        Log.add("Empty directive", false, Brushes.YellowGreen);
                    }

                    // To lower
                    directive = directive.ToLower();

                    remove_unnecessary_characters(ref param);
                    param = param.Trim();

                    // Remove directive with param
                    full_file = full_file.Remove(position_of_directive, b);
                    index_of_last_element = position_of_directive;

                    try
                    {
                        switch (directive)
                        {
                            case "include":
                                param = param.Substring(1, param.Length - 2);

                                if (!System.IO.File.Exists(Path.GetDirectoryName(file_name) + "/" + param))
                                    throw new Exception.PreProcessor(Exception.PreProcessor.Level.ERROR, "Unable to import file named \"" + param + "\"", file_name, line);

                                PreProcessor included_file = new PreProcessor(Path.GetDirectoryName(file_name) + "/" + param).work();
                                if (String.IsNullOrEmpty(included_file.full_file))
                                {
                                    new Exception.PreProcessor(Exception.PreProcessor.Level.WARNING, "File named \"" + param + "\" is empty!", file_name, line).print();
                                    break;
                                }

                                string include_file_text = included_file.full_file;

                                // Add macros to this file
                                macros.AddRange(included_file.macros);

                                // Insert the header
                                include_file_text = "###" + param + "###" + include_file_text + "###" + Path.GetFileName(file_name) + "###";

                                // Include the file
                                full_file = full_file.Insert(position_of_directive, include_file_text);

                                index_of_last_element += include_file_text.Length;
                                break;

                            case "message":
                                new Exception.PreProcessor(Exception.PreProcessor.Level.NOTICE, param, System.IO.Path.GetFileName(file_name), line).print();
                                break;

                            case "warning":
                                new Exception.PreProcessor(Exception.PreProcessor.Level.WARNING, param, System.IO.Path.GetFileName(file_name), line).print();
                                break;

                            case "error":
                                new Exception.PreProcessor(Exception.PreProcessor.Level.ERROR, param, System.IO.Path.GetFileName(file_name), line).print();
                                break;

                            case "define":
                                string[] splitted_param = param.Split(' ');
                                if (splitted_param.Length == 2)
                                    macros.Add(new KeyValuePair<string, string>(splitted_param[0], splitted_param[1]));
                                else
                                    new Exception.PreProcessor(Exception.PreProcessor.Level.ERROR, "Wrong formatted define: No whitespace.", file_name, line).print();
                                break;

                            case "line":
                                line = Convert.ToInt16(param);
                                break;

                            default:
                                new Exception.PreProcessor(Exception.PreProcessor.Level.WARNING, "Found unknown directive " + directive, file_name, line).print();
                                break;
                        }
                    }
                    catch (System.Exception e)
                    {
                        new Exception.PreProcessor(Exception.PreProcessor.Level.ERROR, e.ToString(), file_name, line).print();
                    }
                }
                catch (Exception.PreProcessor e)
                {
                    e.print();
                }
            }

            // Replace all macros
            replace_macros();
            //remove_unnecessary_characters(ref full_file);

            return (this);
        }

        public void remove_unnecessary_characters(ref string text)
        {
            bool literal_string_mode = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '"')
                {
                    literal_string_mode = !literal_string_mode;

                    //text = text.Remove(i, 1);
                    //i--;
                }
                else
                {
                    // Are we within a literal string: "..."?
                    if (!literal_string_mode)
                    {
                        if (c == '\r' || c == '\n' || c == '\t')
                        {
                            text = text.Remove(i, 1);
                            i--;
                        }
                    }
                }
            }

            // Are we still in the literal-string-mode? Throw an error.
            if (literal_string_mode)
                throw new Exception.PreProcessor(Exception.PreProcessor.Level.ERROR, "Literal-String not closed", file_name, line);
        }

        private void replace_macros()
        {
            foreach (KeyValuePair<string, string> macro in macros)
            {
                full_file = full_file.Replace(macro.Key, macro.Value);
            }
        }
    }
}
