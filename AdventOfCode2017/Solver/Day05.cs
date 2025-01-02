using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day05 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "A Maze of Twisty Trampolines, All Alike";

        public override string GetSolution1(bool isChallenge)
        {
            return RunCode(QuickList.ListOfInt(_puzzleInput), false).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            return RunCode(QuickList.ListOfInt(_puzzleInput), true).ToString();
        }

        private static long RunCode(List<int> instructions, bool reduceIfThreeOrMore)
        {
            int pos = 0;
            int nbrOfSteps = 0;
            while (pos >= 0 && pos < instructions.Count)
            {
                nbrOfSteps++;
                int offset = instructions[pos];
                instructions[pos] += offset >= 3 && reduceIfThreeOrMore ? -1 : 1;
                pos += offset;
            }
            return nbrOfSteps;
        }
    }
}