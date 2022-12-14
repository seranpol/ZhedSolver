using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using Xunit;
using Zhed;

namespace ZhedTest
{
    public class UnitTest1
    {
        [Fact]
        public void SomethingWorks()
        {
            var positionGoal = new Position(3, 2);
            var positionNorm = new Position(1, 2);

            var builder = Board.CreateBuilder(10, 10);
            builder.Add(positionGoal, new GoalCell());
            builder.Add(positionNorm, new ValueCell(2));

            var board = builder.Build();
            board.Move(positionNorm, Direction.Right);

            Assert.True(board.IsFinished);
        }

        [Fact]
        public void SolveSimple()
        {
            var positionGoal = new Position(3, 2);
            var positionNorm = new Position(1, 2);

            var builder = Board.CreateBuilder(10, 10);
            builder.Add(positionGoal, new GoalCell());
            builder.Add(positionNorm, new ValueCell(2));
            var board = builder.Build();

            var result = Solver.Solve(board);
            var solverResultItem = Assert.Single(result.SolverResults);
        }

        [Fact]
        public void SolveMedium()
        {
            var positionGoal = new Position(4, 5);
            var positionNorm1 = new Position(4, 2);
            var positionNorm2 = new Position(3, 3);

            var builder = Board.CreateBuilder(10, 10);
            builder.Add(positionGoal, new GoalCell());
            builder.Add(positionNorm1, new ValueCell(2));
            builder.Add(positionNorm2, new ValueCell(1));
            var board = builder.Build();

            var result = Solver.Solve(board);
            var solverResultItem = Assert.Single(result.SolverResults);
        }

        [Fact]
        public void SolveAdvanced()
        {
            var positionGoal = new Position(7, 6);
            var positionNorm1 = new Position(3, 3);
            var positionNorm2 = new Position(2, 4);
            var positionNorm3 = new Position(4, 6);
            var positionNorm4 = new Position(5, 3);

            var builder = Board.CreateBuilder(10, 10);
            builder.Add(positionGoal, new GoalCell());
            builder.Add(positionNorm1, new ValueCell(1));
            builder.Add(positionNorm2, new ValueCell(2));
            builder.Add(positionNorm3, new ValueCell(2));
            builder.Add(positionNorm4, new ValueCell(2));

            var board = builder.Build();
            var result = Solver.Solve(board);
            var solverResultItem = Assert.Single(result.SolverResults);
        }

        [Fact(Skip = "will probally run until the universe implodes ..")]
        public void Level_39()
        {
            var builder = Board.CreateBuilder(10, 10);
            builder.Add(8, 5, new GoalCell());
            builder.Add(3, 2, new ValueCell(2));
            builder.Add(5, 3, new ValueCell(2));
            builder.Add(4, 4, new ValueCell(3));
            builder.Add(2, 5, new ValueCell(1));
            builder.Add(6, 5, new ValueCell(1));
            builder.Add(9, 6, new ValueCell(5));
            builder.Add(2, 7, new ValueCell(3));
            builder.Add(7, 8, new ValueCell(1));

            var board = builder.Build();
            var result = Solver.Solve(board);
            var solverResultItem = Assert.Single(result.SolverResults);
        }
    }
}