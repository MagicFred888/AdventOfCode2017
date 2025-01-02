using AdventOfCode2017.Extensions;
using System.Drawing;

namespace AdventOfCode2017.Solver
{
    internal partial class Day11 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Hex Ed";

        public override string GetSolution1(bool isChallenge)
        {
            return ComputeHexMove(_puzzleInput[0]).finalDistance.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            return ComputeHexMove(_puzzleInput[0]).maxDistance.ToString();
        }

        private static (int finalDistance, int maxDistance) ComputeHexMove(string moveSequence)
        {
            // we use special “double-height” horizontal layout coordinate system for hexagons
            // https://www.redblobgames.com/grids/hexagons/#coordinates

            Dictionary<string, Point> hexMove = new()
            {
                {"n", new(0, 2)},
                {"ne", new(1, 1)},
                {"se", new(1, -1)},
                {"s", new(0, -2)},
                {"sw", new(-1, -1)},
                {"nw", new(-1, 1)}
            };

            // Execute all moves
            int maxDistance = 0;
            Point position = new(0, 0);
            List<string> moves = [.. moveSequence.Split(',')];
            foreach (string m in moves)
            {
                position = position.Add(hexMove[m]);
                int currentDistance = ComputeDistance(position);
                maxDistance = Math.Max(maxDistance, currentDistance);
            }

            // Return result
            return (ComputeDistance(position), maxDistance);
        }

        private static int ComputeDistance(Point position)
        {
            int x = Math.Abs(position.X);
            int y = Math.Abs(position.Y);
            return Math.Min(x, y) + ((Math.Max(x, y) - Math.Min(x, y)) / 2);
        }
    }
}