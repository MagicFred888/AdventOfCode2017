using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day04 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "High-Entropy Passphrases";

        public override string GetSolution1(bool isChallenge)
        {
            List<List<string>> passwods = QuickList.ListOfListString(_puzzleInput, [" "], true);
            return passwods.Count(l => l.Count == l.Distinct().Count()).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            List<List<string>> passwods = QuickList.ListOfListString(_puzzleInput, [" "], true);
            return passwods.FindAll(l => l.Count == l.Distinct().Count())
                .FindAll(l =>
                {
                    // Take each words and sort letters within it alphabetically
                    List<string> sortedWords = l.ConvertAll(w => new string([.. w.OrderBy(c => c)]));
                    return sortedWords.Count == sortedWords.Distinct().Count();
                })
                .Count.ToString();
        }
    }
}