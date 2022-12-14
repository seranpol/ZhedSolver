using System.Diagnostics;
using Zhed.Maps;

namespace Zhed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mapname = "Maps/Level_11.txt";
            var startTime = Stopwatch.GetTimestamp();
            var parser = new MapsParser();
            var board = parser.Parse(File.ReadAllText(mapname));
            Console.WriteLine($"Solving {mapname} : " + Environment.NewLine);
            Console.WriteLine(board.ToString());

            var result = Solver.Solve(board);

            var firstSolution = result.SolverResults.First().DebuggerDisplay;
            Console.WriteLine(firstSolution);
            Console.WriteLine();
            Console.WriteLine($"Solved in {Stopwatch.GetElapsedTime(startTime).TotalMilliseconds} ms");
        }
    }
}