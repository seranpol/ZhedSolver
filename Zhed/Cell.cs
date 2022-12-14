using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhed
{
    public abstract class Cell
    {
        public abstract Cell Clone();

        public abstract string OnToString();

        public override string ToString() => OnToString();
    }

    public class EmptyCell : Cell 
    {
        public const string CellString = "-";
        public override string OnToString() => CellString;
        public override Cell Clone() => new EmptyCell();
    }

    public class GoalCell : Cell
    {
        public const string CellString = "X";
        public override string OnToString() => CellString;
        public override Cell Clone() => new GoalCell(); 
    }
    
    public class FullCell : Cell 
    {
        public const string CellString = "#";
        public override string OnToString() => CellString;
        public override Cell Clone() => new FullCell(); }

    [DebuggerDisplay("ValueCell '{Value}'")]
    public class ValueCell : Cell 
    {
        public override string OnToString() => Convert.ToString(Value);

        public ValueCell(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }

        public override Cell Clone()
        {
            return new ValueCell(Value);
        }

        public void Decrement() => Value -= 1;
    }

}
