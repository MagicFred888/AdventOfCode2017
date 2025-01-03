namespace AdventOfCode2017.Solver
{
    internal partial class Day15 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Dueling Generators";

        private sealed class Generator(long initialValue, long factor, long multiple)
        {
            private long _previousValue = initialValue;

            public long Next()
            {
                do
                {
                    _previousValue = (_previousValue * factor) % 2147483647;
                } while (_previousValue % multiple != 0);
                return _previousValue;
            }
        }

        private readonly Dictionary<string, int> _iniValues = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            Generator generatorA = new(_iniValues["A"], 16807, 1);
            Generator generatorB = new(_iniValues["B"], 48271, 1);
            return CountMatch(generatorA, generatorB, isChallenge ? 40000000 : 5).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            Generator generatorA = new(_iniValues["A"], 16807, 4);
            Generator generatorB = new(_iniValues["B"], 48271, 8);
            return CountMatch(generatorA, generatorB, 5000000).ToString();
        }

        private static long CountMatch(Generator generatorA, Generator generatorB, int nbrOfCycle)
        {
            int nbrMatch = 0;
            for (int i = 0; i < nbrOfCycle; i++)
            {
                nbrMatch += generatorA.Next() << 48 == generatorB.Next() << 48 ? 1 : 0;
            }
            return nbrMatch;
        }

        private void ExtractData()
        {
            _iniValues.Clear();
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(' ');
                _iniValues[parts[1]] = int.Parse(parts[^1]);
            }
        }
    }
}