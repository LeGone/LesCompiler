/*****************************************************************
 * Compilerbau WS14/15
 * Raffael Holz
 * 
 * This file contains the Main-Gui
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LesCompiler
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow singleton = null;
        public int number_of_warnings = 0;
        public int number_of_errors = 0;
        Regex regexed_patteren = null;
        string path;

        public MainWindow()
        {
            InitializeComponent();

            // Create singleton
            singleton = this;

            // Create Regex for syntax-highlighting
            try
            {
                Type[] visitor_types = LesCompiler.Helper.get_all_classes_from_namespace("LesCompiler.AST.Visitor");

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
            catch (System.Exception err)
            {
                throw new Exception.Lexer(Exception.MainException.Level.ERROR, err.ToString(), path, 0);
            }
        }

        private static TextPointer get_position_at_char_offset(TextPointer start, int numbertOfChars)
        {
            var offset = start;
            int i = 0;
            string stringSoFar = "";
            while (stringSoFar.Length < numbertOfChars)
            {
                i++;
                TextPointer offsetCandidate = start.GetPositionAtOffset(i, LogicalDirection.Forward);

                if (offsetCandidate == null)
                    return offset; // ups.. we are to far

                offset = offsetCandidate;
                stringSoFar = new TextRange(start, offset).Text;
            }

            return offset;
        }

        private static TextRange get_text_range(TextPointer start, int startIndex, int length)
        {
            var rangeStart = get_position_at_char_offset(start, startIndex);
            var rangeEnd = get_position_at_char_offset(rangeStart, length);
            return new TextRange(rangeStart, rangeEnd);
        }

        private void on_highlight_clicked(object sender, EventArgs e)
        {
            TextSelection old_selection = the_editor.Selection;

            TextRange text_range = new TextRange(the_editor.Document.ContentStart, the_editor.Document.ContentEnd);
            MatchCollection matches = regexed_patteren.Matches(text_range.Text);
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
                    
                    object abstract_visitor = Activator.CreateInstance(Type.GetType("LesCompiler.AST.Visitor." + groupName + ", LesCompiler"));
                    AST.Main visitor = abstract_visitor as AST.Main;
                    
                    var range = get_text_range(the_editor.Document.ContentStart, match.Index, match.Index + match.Length);
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(visitor.highlight_color));
                }
            }

            the_editor.Selection.Select(old_selection.Start, old_selection.End); // Go to the end.
        }

        private void on_file_new_clicked(object sender, RoutedEventArgs e)
        {
            path = String.Empty;
            the_editor.Document.Blocks.Clear();
        }

        private void on_file_open_clicked(object sender, RoutedEventArgs e)
        {
            lb_log.Items.Clear();

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".les";
            dlg.Filter = "LES-File (*.les)|*.les";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                path = dlg.FileName;
                the_editor.Document.Blocks.Clear();
                the_editor.AppendText(Parser.Helper.read_file(path));
            }
        }

        private void on_file_save_clicked(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(path))
            {
                TextRange text_range = new TextRange(the_editor.Document.ContentStart, the_editor.Document.ContentEnd);
                System.IO.File.WriteAllText(path, text_range.Text);
            }
        }

        private void compile(string file_name)
        {
            lb_log.Items.Clear();

            number_of_errors = 0;
            number_of_warnings = 0;

            try
            {
                Parser.Parser parser = new Parser.Parser(file_name);
                parser.work();
                AST.Factory.build(parser.lexer.list_of_visitors);

                file_name = file_name.Replace(".les", ".exe");
                Parser.Assembler assembler = new Parser.Assembler(AST.Factory.list_of_functions, file_name);
            }
            catch (Exception.MainException err)
            {
                err.print();
                number_of_errors++;
            }

            if (number_of_errors == 0)
                Log.add("Successfully compiled! (0 errors, " + number_of_warnings + " warnings)", false, Brushes.GreenYellow);
            else
                Log.add("Failed to compile! (" + number_of_errors + " errors, " + number_of_warnings + " warnings)", false, Brushes.Red);
        }

        private void on_compile_clicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(path))
                path = Environment.CurrentDirectory + "/temp.les";
            TextRange text_range = new TextRange(the_editor.Document.ContentStart, the_editor.Document.ContentEnd);
            System.IO.File.WriteAllText(path, text_range.Text);

            compile(path);
        }

        private void on_compile_file_clicked(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".les";
            dlg.Filter = "LES-File (*.les)|*.les";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string file_name = dlg.FileName;

                compile(file_name);
            }
        }

        private void on_start_clicked(object sender, RoutedEventArgs e)
        {
            on_compile_clicked(sender, e);

            if (number_of_errors == 0)
                Process.Start(path.Replace(".les", ".exe"));
        }

        private void the_editor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
