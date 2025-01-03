using AdventOfCode2017.Extensions;

namespace AdventOfCode2017.Solver
{
    internal partial class Day18 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Duet";

        private readonly List<(string cmd, string x, string y)> _commands = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();

            // Create one program and make it run
            Program program = new(0, _commands, false);
            program.StartOrContinue();

            // Return the program result
            return program.Result.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            // Create program 0 and 1 and link there Send/Receive buffer with each other
            Program program0 = new(0, _commands, true);
            Program program1 = new(1, _commands, true);
            program0.ReceiveBuffer = program1.SendBuffer;
            program1.ReceiveBuffer = program0.SendBuffer;

            // Run them until they both completed
            do
            {
                program0.StartOrContinue();
                program1.StartOrContinue();
            } while (program0.IsRunning || program1.IsRunning);

            // Return result from program 1
            return program1.Result.ToString();
        }

        private sealed class Program(long id, List<(string cmd, string x, string y)> commands, bool part2)
        {
            public long Id { get; init; } = id;
            public long Result { get; private set; } = 0;
            public List<long> ReceiveBuffer { get; set; } = [];
            public List<long> SendBuffer { get; set; } = [];
            public bool IsRunning { get; private set; } = false;

            private readonly List<(string cmd, string x, string y)> _commands = commands;
            private readonly bool _part2 = part2;
            private Dictionary<string, long> _memoryBuffers = [];
            private int _nbrFailReceived = 0;
            private int _position = 0;

            public void StartOrContinue()
            {
                if (!IsRunning)
                {
                    // Initialize a new run
                    Result = 0;
                    _position = 0;
                    _memoryBuffers = [];
                    _memoryBuffers.Add("p", Id);
                    IsRunning = true;
                }

                // Run until we need to wait for input or end
                Run();
            }

            private void Run()
            {
                while (_position < _commands.Count)
                {
                    (string cmd, string x, string y) = _commands[_position];

                    // Fix buffer
                    _memoryBuffers.TryAdd(x, 0);
                    if (!string.IsNullOrEmpty(y) && !y.IsNumeric())
                    {
                        _memoryBuffers.TryAdd(y, 0);
                    }

                    // Execute command
                    long valueOfX = x.IsNumeric() ? x.ToInt() : _memoryBuffers[x];
                    long valueOfY = 0;
                    if (y != "")
                    {
                        valueOfY = y.IsNumeric() ? y.ToInt() : _memoryBuffers[y];
                    }

                    switch (cmd)
                    {
                        case "snd":
                            if (_part2)
                            {
                                SendBuffer.Add(valueOfX);
                                Result++;
                            }
                            else
                            {
                                Result = valueOfX;
                            }
                            break;

                        case "rcv":
                            if (_part2)
                            {
                                if (ReceiveBuffer.Count == 0)
                                {
                                    _nbrFailReceived++;
                                    if (_nbrFailReceived >= 4)
                                    {
                                        IsRunning = false;
                                    }
                                    return;
                                }
                                _nbrFailReceived = 0;
                                _memoryBuffers[x] = ReceiveBuffer[0];
                                ReceiveBuffer.RemoveAt(0);
                            }
                            else
                            {
                                if (valueOfX != 0)
                                {
                                    // Done
                                    IsRunning = false;
                                    return;
                                }
                            }
                            break;

                        case "set":
                            _memoryBuffers[x] = valueOfY;
                            break;

                        case "add":
                            _memoryBuffers[x] += valueOfY;
                            break;

                        case "mul":
                            _memoryBuffers[x] *= valueOfY;
                            break;

                        case "mod":
                            _memoryBuffers[x] = valueOfX % valueOfY;
                            break;

                        case "jgz":
                            if (valueOfX > 0)
                            {
                                _position += (int)valueOfY - 1;
                            }
                            break;
                    }

                    // Next line
                    _position++;
                }
                throw new InvalidDataException();
            }
        }

        private void ExtractData()
        {
            _commands.Clear();
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(' ');
                _commands.Add((parts[0], parts[1], parts.Length == 3 ? parts[2] : ""));
            }
        }
    }
}