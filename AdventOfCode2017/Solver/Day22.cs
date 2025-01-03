using AdventOfCode2017.Extensions;
using System.Drawing;

namespace AdventOfCode2017.Solver
{
    internal partial class Day22 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Sporifica Virus";

        private readonly Dictionary<Point, char> _infectedCells = [];

        public override string GetSolution1(bool isChallenge)
        {
            // Initialize  new scan
            ExtractData();
            int nbrInfected = 0;
            Point position = new();
            Point direction = new(0, 1);

            // Scan structure
            for (int i = 0; i < 10000; i++)
            {
                if (!_infectedCells.TryAdd(position, ' '))
                {
                    direction = direction.RotateClockwise();
                    _infectedCells.Remove(position);
                }
                else
                {
                    direction = direction.RotateCounterclockwise();
                    nbrInfected++;
                }
                position = position.Add(direction);
            }
            return nbrInfected.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // Initialize  new scan
            ExtractData();
            int nbrInfected = 0;
            Point position = new();
            Point direction = new(0, 1);

            // Scan structure
            for (int i = 0; i < 10000000; i++)
            {
                if (_infectedCells.TryGetValue(position, out char value))
                {
                    if (value == 'w')
                    {
                        nbrInfected++;
                        _infectedCells[position] = 'i';
                    }
                    else if (value == 'i')
                    {
                        direction = direction.RotateClockwise();
                        _infectedCells[position] = 'f';
                    }
                    else if (value == 'f')
                    {
                        direction = direction.Rotate180Degree();
                        _infectedCells.Remove(position);
                    }
                }
                else
                {
                    direction = direction.RotateCounterclockwise();
                    _infectedCells.Add(position, 'w');
                }
                position = position.Add(direction);
            }
            return nbrInfected.ToString();
        }

        private void ExtractData()
        {
            _infectedCells.Clear();
            int halfCol = _puzzleInput[0].Length / 2;
            int halfRow = _puzzleInput.Count / 2;
            for (int y = 0; y < _puzzleInput.Count; y++)
            {
                for (int x = 0; x < _puzzleInput[0].Length; x++)
                {
                    if (_puzzleInput[y][x] == '#')
                    {
                        _infectedCells.Add(new Point(x - halfCol, halfRow - y), 'i');
                    }
                }
            }
        }
    }
}