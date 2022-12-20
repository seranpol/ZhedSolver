using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhed.Maps
{
    internal class MapsParser
    {
        public IBoard Parse(string data)
        {            
            var lines = data.Split(new []{ "\r\n", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray()!;
            if (lines.GroupBy(s => s.Length).Count() > 1) throw new InvalidOperationException();

            var builder = Board.CreateBuilder(lines[0].Length, lines.Count());
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    Cell cell = line[x] switch
                    {
                        '-' => new EmptyCell(),
                        'x' => new GoalCell(),
                        char c when int.TryParse(c.ToString(), out int num) => new ValueCell(num),
                        _ => throw new InvalidOperationException()
                    };
                    builder.Add(x, y, cell);
                }
            }

            return builder.Build();
        }
    }
}
