using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day06 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Memory Reallocation";

        public override string GetSolution1(bool isChallenge)
        {
            List<int> banks = QuickList.ListOfListInt(_puzzleInput, ["\t"], true)[0];
            (int nbrOfCycles, _) = PerformReallocationUntilDuplicate(banks);
            return nbrOfCycles.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            List<int> banks = QuickList.ListOfListInt(_puzzleInput, ["\t"], true)[0];
            (_, List<int> newBanksState) = PerformReallocationUntilDuplicate(banks);
            (int nbrOfCycles, _) = PerformReallocationUntilDuplicate(newBanksState);
            return nbrOfCycles.ToString();
        }

        private static (int nbrOfCycles, List<int> newBankState) PerformReallocationUntilDuplicate(List<int> banks)
        {
            int nbrOfCycles = 0;
            HashSet<string> seenStates = [];
            do
            {
                string newState = string.Join(",", banks);
                if (seenStates.Contains(newState))
                {
                    return (nbrOfCycles, banks);
                }
                seenStates.Add(newState);
                BalanceBanks(banks);
                nbrOfCycles++;
            } while (true);
        }

        private static void BalanceBanks(List<int> banks)
        {
            int startPos = banks.IndexOf(banks.Max());
            int blocks = banks[startPos];
            banks[startPos] = 0;
            for (int i = 0; i < blocks; i++)
            {
                banks[(startPos + i + 1) % banks.Count]++;
            }
        }
    }
}