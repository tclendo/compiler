using System;
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
                var result = compilation.Evaluate();

                var diagnostics = result.Diagnostics;

                if (showTree) {

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    TreePrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any()) {

                    Console.WriteLine(result.Value);
                    
                }

                else {

                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in diagnostics) {

                        Console.WriteLine(diagnostic);
                    }
                    Console.ResetColor();
                }
                
            }
        }

        static void TreePrint(SyntaxNode node, string indent = "", bool isLast = true) {

            /*
            ├──
            │
            └──
            */

            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null) {

                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren()) {

                TreePrint(child, indent, child == lastChild);
            }
        }
    }
}
