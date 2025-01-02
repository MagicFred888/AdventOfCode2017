using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day02 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Corruption Checksum";

        public override string GetSolution1(bool isChallenge)
        {
            int result = 0;
            foreach (List<int> row in QuickList.ListOfListInt(_puzzleInput, [" ", "\t"], true))
            {
                result += row.Max() - row.Min();
            }
            return result.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            int result = 0;
            foreach (List<int> row in QuickList.ListOfListInt(_puzzleInput, [" ", "\t"], true))
            {
                result += SmallTools.GenerateCombinations(row.Count, 2).Aggregate(0, (acc, c) =>
                {
                    int max = Math.Max(row[c[0]], row[c[1]]);
                    int min = Math.Min(row[c[0]], row[c[1]]);
                    return acc + (max % min == 0 ? max / min : 0);
                });
            }
            return result.ToString();
        }
    }
}