using System.Text;

namespace Zhed
{
    public class Board : IBoardBuilder, IBoard
    {
        private readonly Cell[,] _cells;

        private Board(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new Cell[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _cells[x, y] = new EmptyCell();
        }

        private Board(int width, int height, Cell[,] cells)
        {
            Width = width;
            Height = height;
            _cells = cells;
        }

        #region Board

        public int Width { get; }
        public int Height { get; }
        private bool IsFinished { get; set; }
        Cell this[Position position]
        {
            get => _cells[position.X, position.Y];
            set => _cells[position.X, position.Y] = value;
        }

        private void CheckCells(Action<Cell> predicate)
        {
            foreach (var cell in _cells)
            {
                predicate(cell);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var cells = (Cell[,])_cells.Clone();
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    var sign = cells[x, y] switch
                    {
                        GoalCell goalCell => 'X',
                        FullCell fullCell => 'f',
                        EmptyCell emptyCell => '-',
                        ValueCell valueCell => (char)(valueCell.Value + '0'),
                        _ => throw new NotImplementedException(),
                    };
                    sb.Append(sign);

                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion Board

        #region IBoard

        bool IBoard.IsFinished => IsFinished;

        IBoard IBoardBuilder.Build() => this;

        void IBoard.Move(Position position, Direction direction)
        {
            if (_cells[position.X, position.Y] is not ValueCell originalCell)
                throw new InvalidOperationException("can only move value cells");

            if (originalCell.Value <= 0)
                throw new InvalidOperationException("invalid value in value cell");

            Func<int, int> limitHorizontal = x => Math.Min(Width-1, Math.Max(x, 0));
            Func<int, int> limitVertical = y => Math.Min(Height-1, Math.Max(y, 0));

            Func<Position, Position> nextMovePositionGenerator = direction switch
            {
                Direction.Up => (Position p) => new Position(p.X, limitVertical(p.Y + 1)),
                Direction.Right => (Position p) => new Position(limitHorizontal(p.X + 1), p.Y),
                Direction.Down => (Position p) => new Position(p.X, limitVertical(p.Y - 1)),
                Direction.Left => (Position p) => new Position(limitHorizontal(p.X - 1), p.Y),
                _ => throw new NotImplementedException(),
            };

            var currentPosition = position;
            var currentCell = originalCell;
            do
            {
                var nextPosition = nextMovePositionGenerator.Invoke(currentPosition);
                if (nextPosition.Equals(currentPosition))
                    break;

                var nextCell = this[nextPosition];

                if (nextCell is FullCell fullCell)
                {
                    currentPosition = nextPosition;
                    continue;
                }

                if (nextCell is ValueCell valueCell)
                {
                    currentPosition = nextPosition;
                    continue;
                }

                if (nextCell is GoalCell goalCell)
                {
                    IsFinished = true;
                    break;
                }

                if (nextCell is EmptyCell emptyCell)
                {
                    currentCell.Decrement();
                    this[nextPosition] = new FullCell();
                    currentPosition = nextPosition;
                }
            }
            while (currentCell.Value > 0);
        }

        IBoard IBoard.Clone()
        {
            var cells = (Cell[,])_cells.Clone();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    cells[x, y] = _cells[x, y].Clone();

            return new Board(Width, Height, cells);
        }

        IEnumerable<(Cell, Position)> IBoard.GetCells()
        {
            var enumerator = _cells.GetEnumerator();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return (_cells[x, y], new Position(x, y));
                }
            }

        }

        #endregion IBoard

        #region IBoardBuilder

        public static IBoardBuilder CreateBuilder(int width, int height) => new Board(width: width, height: height);

        void IBoardBuilder.Add(int x, int y, Cell cell ) => ((IBoardBuilder)this).Add(new Position(x, y), cell);

        void IBoardBuilder.Add(Position position, Cell cell)
        {
            if (cell is GoalCell)
            {
                CheckCells(c =>
                {
                    if (c is GoalCell) throw new InvalidOperationException("only one goal cell");
                });
            }

            _cells[position.X, position.Y] = cell;
        }

        #endregion IBoardBuilder
    }

    public interface IBoardBuilder
    {
        void Add(Position position, Cell cell);

        void Add(int x, int y, Cell cell);

        IBoard Build();
    }

    public interface IBoard
    {
        void Move(Position position, Direction direction);

        bool IsFinished { get; }

        IBoard Clone();

        IEnumerable<(Cell cell, Position position)> GetCells();
    }
}
