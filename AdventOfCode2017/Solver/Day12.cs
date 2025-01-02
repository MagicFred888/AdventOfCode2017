using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day12 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Digital Plumber";

        private List<(string from, string to, long distance)> _allPair = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            QuickDijkstra quickDijkstra = new(_allPair);
            return quickDijkstra.GetNodesInNetwork("0").Count.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            int nbrOfGroups = 0;
            while (_allPair.Count > 0)
            {
                QuickDijkstra quickDijkstra = new(_allPair);
                List<string> nodes = quickDijkstra.GetNodesInNetwork(_allPair[0].from);
                _allPair = _allPair.FindAll(p => !nodes.Contains(p.from) && !nodes.Contains(p.to));
                nbrOfGroups++;
            }
            return nbrOfGroups.ToString();
        }

        private void ExtractData()
        {
            _allPair.Clear();

            // Extract data from _puzzleInput
            foreach (string line in _puzzleInput)
            {
                int[] parts = [.. line.Replace("<->", ",").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => int.Parse(s))];
                int from = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    _allPair.Add((Math.Min(from, parts[i]).ToString(), Math.Max(from, parts[i]).ToString(), 1));
                }
            }
            _allPair = _allPair.Distinct().ToList();
        }
    }
}