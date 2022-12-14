using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zhed
{
    public record MoveInfo(Position Position, Direction Direction, ValueCell Cell);

    [DebuggerDisplay("{DebuggerDisplay}")]
    public record SolverResultItem(IBoard OriginalBoard, IBoard ResultingBoard, MoveInfo[] MoveInfos) { public string DebuggerDisplay => string.Join(Environment.NewLine, MoveInfos.Select(moveinfo => 
        moveinfo.Position.ToString() + "/" + "val:(" + moveinfo.Cell.ToString() + ") -> " + moveinfo.Direction.GetDisplayName())); }

    [DebuggerDisplay("{DebuggerDisplay}")]
    public record SolverResult(SolverResultItem[] SolverResults) { public string DebuggerDisplay => string.Join(Environment.NewLine, SolverResults.Select(solverResultItem => solverResultItem.DebuggerDisplay)); }

    public static class Solver
    {
        public static SolverResult Solve(IBoard board)
        {
            var viableCells = board.GetCells().Where(a => a.cell is ValueCell).ToArray();
            var cellDirPairGroups = viableCells.Select(cell => new[] { (Direction.Up, cell), (Direction.Right, cell), (Direction.Down, cell), (Direction.Left, cell), });
            var cartesian = CartesianProduct(cellDirPairGroups).ToArray();

            var permutes = cartesian.SelectMany(x => GetPermutations(x, x.Count())).ToArray();            
            var viableMoves = permutes.Select(x => x.Select(y => new MoveInfo(y.cell.position, y.Item1, (ValueCell)y.cell.cell))).ToArray();
            var resultItems = new List<SolverResultItem>();

            foreach (var item in viableMoves)
            {
                var clone = board.Clone();
                var success = SolveInstance(clone, item);
                if (success) resultItems.Add(new SolverResultItem(board, clone, item.ToArray()));
            }

            return new SolverResult(resultItems.ToArray());
        }

        private static bool SolveInstance(IBoard board, IEnumerable<MoveInfo> moveInfos)
        {
            foreach (var moveInfo in moveInfos) board.Move(moveInfo.Position, moveInfo.Direction);
            return board.IsFinished;
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            // from some stackowerflow post
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            // https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(emptyProduct, (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }

        public static string GetDisplayName(this Enum enumValue) => enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName()!;

    }
}
