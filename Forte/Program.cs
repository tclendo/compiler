using System;
using System.Linq;

using Forte.CodeAnalysis;

namespace Forte
{
    class Program {

        /*
            Our program class

            This program runs our REPL for our Forte language compiler.
        */

        static void Main(string[] args) {

            bool showTree = false;

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

                } else if (line == "exit") {

                    return;
                
                } else if (string.IsNullOrWhiteSpace(line)) {

                    continue;
                } 

                var syntaxTree = SyntaxTree.ParseTree(line);

                if (showTree) {

                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    TreePrint(syntaxTree.Root);
                    Console.ForegroundColor = color;
                }
                
                if (!syntaxTree.Diagnostics.Any()) {

                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                    
                }

                else {

                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in syntaxTree.Diagnostics) {

                        Console.WriteLine(diagnostic);
                    }
                    Console.ForegroundColor = color;
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

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren()) {

                TreePrint(child, indent, child == lastChild);
            }
        }
    }
}
