namespace AdventOfCode2017.Solver
{
    internal partial class Day07 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Recursive Circus";

        private sealed class Node(string name, int weight)
        {
            public string Name { get; } = name;
            public int Weight { get; } = weight;
            public List<Node> Children { get; set; } = [];
            public Node? Parent { get; set; } = null;
            public bool IsBalanced => Children.Count <= 1 || Children.Select(c => c.FullWeight).Distinct().Count() == 1;
            public int FullWeight => Weight + Children.Sum(c => c.FullWeight);
        }

        private readonly Dictionary<string, Node> _allNodes = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return _allNodes.Values.ToList().Find(n => n.Parent == null)?.Name ?? throw new InvalidDataException();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            // Find an unbalanced node with as less weight as possible (to guarantee it's down the structure)
            Node? nodeOfInterest = _allNodes.Values.ToList().FindAll(n => !n.IsBalanced).MinBy(n => n.FullWeight) ?? throw new InvalidDataException();

            // List all children and find the one with weight differing from the others
            Node childToFix = nodeOfInterest.Children.ToDictionary(c => c, c => c.FullWeight).OrderByDescending(kvp => kvp.Value).First().Key;

            // find the target weight we need to balance the structure
            int weightDifference = childToFix.FullWeight - nodeOfInterest.Children.First(c => c != childToFix).FullWeight;

            // Return the difference
            return (childToFix.Weight - weightDifference).ToString();
        }

        private void ExtractData()
        {
            // Build structure in two pass
            _allNodes.Clear();
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Replace("(", "").Replace(")", "").Split(" ");
                _allNodes.Add(parts[0], new Node(parts[0], int.Parse(parts[1])));
            }

            // Attach nodes
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(" -> ");
                if (parts.Length < 2)
                {
                    continue;
                }
                string current = parts[0].Split(" ")[0];
                List<string> childrens = parts[1].Split(",").ToList().ConvertAll(s => s.Trim());
                foreach (string child in childrens)
                {
                    _allNodes[current].Children.Add(_allNodes[child]);
                    _allNodes[child].Parent = _allNodes[current];
                }
            }
        }
    }
}