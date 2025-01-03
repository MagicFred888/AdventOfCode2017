using AdventOfCode2017.Extensions;
using AdventOfCode2017.Tools;
using System.Drawing;

namespace AdventOfCode2017.Solver
{
    internal partial class Day19 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "A Series of Tubes";

        public override string GetSolution1(bool isChallenge)
        {
            QuickMatrix map = new(_puzzleInput);
            return FollowPath(map).specialChars;
        }

        public override string GetSolution2(bool isChallenge)
        {
            QuickMatrix map = new(_puzzleInput);
            return FollowPath(map).nbrOfSteps.ToString();
        }

        private static (string specialChars, int nbrOfSteps) FollowPath(QuickMatrix map)
        {
            int nbrOfSteps = 0;
            string result = "";
            Point direction = new(0, 1);
            Point position = map.Rows[0].Find(c => c.StringVal == "|")?.Position ?? throw new InvalidDataException();
            do
            {
                // End of path?
                if (map.Cell(position).StringVal == " ")
                {
                    break;
                }

                // Special char for result ?
                if (!"+-|".Contains(map.Cell(position).StringVal[0]))
                {
                    result += map.Cell(position).StringVal;
                }

                // Try continue moving in same direction
                if (map.Cell(position).StringVal != "+")
                {
                    // Continue move in same direction
                    position = position.Add(direction);
                    nbrOfSteps++;
                    continue;
                }

                // Try to change direction Clockwise
                Point newDirection = direction.RotateClockwise();
                Point nextPos = position.Add(newDirection);
                if (map.Cell(nextPos).StringVal != " ")
                {
                    direction = newDirection;
                    position = nextPos;
                    nbrOfSteps++;
                    continue;
                }

                // Try to change direction Counterclockwise
                newDirection = direction.RotateCounterclockwise();
                nextPos = position.Add(newDirection);
                if (map.Cell(nextPos).StringVal != " ")
                {
                    direction = newDirection;
                    position = nextPos;
                    nbrOfSteps++;
                    continue;
                }

                // No way to go
                break;
            } while (true);

            // Done
            return (result, nbrOfSteps);
        }
    }
}