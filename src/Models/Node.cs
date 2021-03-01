using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calc24Blazor.Models
{
    public class Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Node(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
    }
}
