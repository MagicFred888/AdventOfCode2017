namespace AdventOfCode2017.Solver
{
    internal partial class Day08 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "I Heard You Like Registers";

        private readonly List<(string target, string action, int value, string toCheck, string checkOperation, int checkValue)> _allCommands = [];
        private readonly Dictionary<string, int> _registers = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            _ = ExecuteProgram();
            return _registers.Values.Max().ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return ExecuteProgram().ToString();
        }

        private int ExecuteProgram()
        {
            int maxValueInAnyRegister = 0;
            int position = 0;
            while (position >= 0 && position < _allCommands.Count)
            {
                (string target, string action, int value, string toCheck, string checkOperation, int checkValue) = _allCommands[position];

                // Check condition
                bool checkPassed = checkOperation switch
                {
                    "==" => _registers[toCheck] == checkValue,
                    "!=" => _registers[toCheck] != checkValue,
                    ">" => _registers[toCheck] > checkValue,
                    "<" => _registers[toCheck] < checkValue,
                    ">=" => _registers[toCheck] >= checkValue,
                    "<=" => _registers[toCheck] <= checkValue,
                    _ => throw new NotImplementedException()
                };
                position++;
                if (!checkPassed)
                {
                    continue;
                }

                // Execute command
                _registers[target] = action switch
                {
                    "inc" => _registers[target] + value,
                    "dec" => _registers[target] - value,
                    _ => throw new NotImplementedException()
                };

                // Check register
                maxValueInAnyRegister = Math.Max(maxValueInAnyRegister, _registers.Values.Max());
            }

            // Done
            return maxValueInAnyRegister;
        }

        private void ExtractData()
        {
            // Extract data from _puzzleInput here...
            _allCommands.Clear();
            _registers.Clear();
            foreach (string command in _puzzleInput)
            {
                string[] parts = command.Replace("if", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                _registers.TryAdd(parts[0], 0);
                _registers.TryAdd(parts[3], 0);
                _allCommands.Add((parts[0], parts[1], int.Parse(parts[2]), parts[3], parts[4], int.Parse(parts[5])));
            }
        }
    }
}