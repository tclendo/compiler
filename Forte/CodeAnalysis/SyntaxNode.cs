using System.Collections.Generic;

namespace Forte.CodeAnalysis
{
    abstract class SyntaxNode {

        /*
            Our syntax node class, containing an abstract kind
            and an abstract child.
        */
        
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}