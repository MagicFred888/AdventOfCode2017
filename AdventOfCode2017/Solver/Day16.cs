namespace AdventOfCode2017.Solver
{
    internal partial class Day16 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Permutation Promenade";

        private readonly List<(char move, string a, string b)> _danceMoves = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            string iniPos = isChallenge ? "abcdefghijklmnop" : "abcde";
            return ExecuteDance(iniPos, 1);
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            string iniPos = "abcdefghijklmnop";
            return ExecuteDance(iniPos, 1000000000);
        }

        private string ExecuteDance(string iniPos, int nbrOfCycle)
        {
            char[] dancerPosition = iniPos.ToCharArray();
            for (int cycle = 1; cycle <= nbrOfCycle; cycle++)
            {
                foreach ((char move, string a, string b) in _danceMoves)
                {
                    switch (move)
                    {
                        case 's': // Spin
                            int spinDistance = int.Parse(a);
                            char[] temp = new char[dancerPosition.Length];
                            for (int i = 0; i < dancerPosition.Length; i++)
                            {
                                temp[(i + spinDistance) % dancerPosition.Length] = dancerPosition[i];
                            }
                            dancerPosition = temp;
                            break;

                        case 'x': // Exchange
                            int posA = int.Parse(a);
                            int posB = int.Parse(b);
                            (dancerPosition[posA], dancerPosition[posB]) = (dancerPosition[posB], dancerPosition[posA]);
                            break;

                        case 'p': //Partner
                            int indexA = Array.IndexOf(dancerPosition, a[0]);
                            int indexB = Array.IndexOf(dancerPosition, b[0]);
                            dancerPosition[indexA] = b[0];
                            dancerPosition[indexB] = a[0];
                            break;
                    }
                }
                if (new string(dancerPosition) == iniPos)
                {
                    // If cycle repaeat, we can save a lot of time ;-)
                    return ExecuteDance(iniPos, nbrOfCycle % cycle);
                }
            }

            // Dance completed
            return new string(dancerPosition);
        }

        private void ExtractData()
        {
            _danceMoves.Clear();
            foreach (string move in _puzzleInput[0].Split(','))
            {
                string action = move[..1];
                string[] parts = move[1..].Split('/');
                _danceMoves.Add((action[0], parts[0], parts.Length > 1 ? parts[1] : ""));
            }
        }
    }
}