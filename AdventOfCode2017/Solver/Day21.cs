using AdventOfCode2017.Extensions;
using AdventOfCode2017.Tools;
using System.Drawing;

namespace AdventOfCode2017.Solver
{
    internal partial class Day21 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Fractal Art";

        private static readonly string[] s_sourceArray = [".#.", "..#", "###"];

        private readonly List<(QuickMatrix from, QuickMatrix to)> _designBook = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            QuickMatrix _pattern = new([.. s_sourceArray]);
            for (int step = 0; step < (isChallenge ? 5 : 2); step++)
            {
                _pattern = ComputeNextMove(_pattern);
            }
            return _pattern.Cells.Count(c => c.StringVal == "#").ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            QuickMatrix _pattern = new([.. s_sourceArray]);
            for (int step = 0; step < 18; step++)
            {
                _pattern = ComputeNextMove(_pattern);
            }
            return _pattern.Cells.Count(c => c.StringVal == "#").ToString();
        }

        private QuickMatrix ComputeNextMove(QuickMatrix pattern)
        {
            int stepSize = pattern.ColCount % 2 == 0 ? 2 : 3;
            int nbrOfStep = pattern.ColCount / stepSize;
            QuickMatrix result = new((pattern.RowCount / stepSize) * (stepSize + 1), (pattern.ColCount / stepSize) * (stepSize + 1), ".");
            for (int x = 0; x < nbrOfStep; x++)
            {
                for (int y = 0; y < nbrOfStep; y++)
                {
                    QuickMatrix subPattern = FindMatching(pattern, new Point(x * stepSize, y * stepSize), stepSize);
                    result.SetCells(subPattern, new(x * (stepSize + 1), y * (stepSize + 1)));
                }
            }
            return result;
        }

        private QuickMatrix FindMatching(QuickMatrix reference, Point startPos, int step)
        {
            QuickMatrix subReference = reference.GetSubMatrix(startPos, startPos.Add(step - 1, step - 1));
            foreach ((QuickMatrix from, QuickMatrix to) in _designBook)
            {
                if (from.ColCount != subReference.ColCount)
                {
                    continue;
                }
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;

                subReference.FlipHorizontal();
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;
                subReference.RotateClockwise();
                if (from.ToString() == subReference.ToString()) return to;
            }
            throw new InvalidOperationException();
        }

        private void ExtractData()
        {
            _designBook.Clear();
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(" => ");
                QuickMatrix from = new(parts[0].Split("/").Aggregate(new List<string>(), (acc, val) =>
                {
                    acc.Add(val);
                    return acc;
                }));
                QuickMatrix to = new(parts[1].Split("/").Aggregate(new List<string>(), (acc, val) =>
                {
                    acc.Add(val);
                    return acc;
                }));
                _designBook.Add((from, to));
            }
        }
    }
}