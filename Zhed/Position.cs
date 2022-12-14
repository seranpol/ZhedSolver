using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Zhed
{
    [DebuggerDisplay("x = {X} y = {Y}")]
    public class Position
    {
        public Position(int x, int y)
        {
            if(x < 0) throw new ArgumentOutOfRangeException("x"); 
            if(y < 0) throw new ArgumentOutOfRangeException("y");

            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override bool Equals(object? obj)
        {
            var other = obj as Position;
            return other != null && other.X==X&& other.Y==Y;
        }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();


        public override string ToString() => $"(x:{this.X.ToString()},y:{Y.ToString()})";
    }
}
