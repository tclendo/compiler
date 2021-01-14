using System.Collections.Generic;

namespace Forte.CodeAnalysis
{
    abstract class SyntaxNode {

        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}