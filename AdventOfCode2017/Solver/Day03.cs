using AdventOfCode2017.Extensions;
using AdventOfCode2017.Tools;
using System.Drawing;

namespace AdventOfCode2017.Solver
{
    internal partial class Day03 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Spiral Memory";

        public override string GetSolution1(bool isChallenge)
        {
            // Get basic data
            int targetValue = int.Parse(_puzzleInput[0]);
            QuickGrid spiralGrid = GetSpiralGrid(targetValue, false);
            return spiralGrid.Cells.Find(c => c.Value == targetValue)?.Position.ManhattanDistance().ToString() ?? throw new InvalidDataException();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // Get basic data
            int targetValue = int.Parse(_puzzleInput[0]);
            QuickGrid spiralGrid = GetSpiralGrid(targetValue, true);
            return spiralGrid.Cells.FindAll(c => c.Value > targetValue).Min(c => c.Value).ToString();
        }

        private static QuickGrid GetSpiralGrid(int maxValue, bool sumAround)
        {
            List<Point> moveDirection =
            [
                new (0, 1),
                new (-1, 0),
                new (0, -1),
                new (1, 0)
            ];

            // Define grid size
            int squarreSize = (int)Math.Ceiling(Math.Sqrt(maxValue));
            int absMax = (squarreSize - 1) / 2;
            QuickGrid result = new(-absMax, absMax, -absMax, absMax, 0);

            // Fill the grid center
            result.Cell(0, 0)!.Value = 1;

            // Prepare loop for circle 1
            Point position = new(1, 0);
            int moveDirectionId = 0;
            int currentCircle = 1;
            long value = 1;

            // Fill the grid
            while (value < maxValue)
            {
                do
                {
                    if (sumAround)
                    {
                        value = result.TouchingCells(position, QuickGrid.TouchingMode.All).Sum(c => c.Value);
                    }
                    else
                    {
                        value++;
                    }
                    result.Cell(position)!.Value = value;
                    if (value > maxValue)
                    {
                        return result;
                    }
                    if (position.Add(moveDirection[moveDirectionId]).ManhattanDistance() > 2 * currentCircle)
                    {
                        moveDirectionId++;
                        if (moveDirectionId >= moveDirection.Count)
                        {
                            break;
                        }
                    }
                    position = position.Add(moveDirection[moveDirectionId]);
                } while (true);

                // Prepare next circle
                currentCircle++;
                position = new Point(position.X + 1, position.Y);
                moveDirectionId = 0;
            }

            // Done
            return result;
        }
    }
}