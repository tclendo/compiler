using System;
using System.Collections.Generic;
using System.Linq;

using Forte.CodeAnalysis;
using Forte.CodeAnalysis.Binding;
using Forte.CodeAnalysis.Syntax;

namespace Forte
{
    internal static class Program {

        /*
            Our program class

            This program runs our REPL for our Forte language compiler.
        */

        private static void Main() {

            var showTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            
            while (true) {
                Console.Write("$ ");
                var line = Console.ReadLine();

                if (line == "$showTree") {

                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                    
                } else if (line == "$cls") {

                    Console.Clear();
                    continue;

                } else if (string.IsNullOrWhiteSpace(line)) {

                    return;
                } 

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                var diagnostics = result.Diagnostics;

                if (showTree) {

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    syntaxTree.Root.WriteTo(Console.Out);
                    Console.ResetColor();
                }

                if (!diagnostics.Any()) {

                    Console.WriteLine(result.Value);
                    
                }

                else {

                    var text = syntaxTree.Text;

                    foreach (var diagnostic in diagnostics) {

                        var lineIndex = text.GetLineIndex(diagnostic.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var character = diagnostic.Span.Start - text.Lines[lineIndex].Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);

                        Console.WriteLine();
                    }

                    Console.WriteLine();

                }
            }
        }
    }
}
