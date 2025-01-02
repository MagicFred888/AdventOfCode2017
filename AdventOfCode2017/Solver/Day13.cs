using AdventOfCode2017.Tools;
using static AdventOfCode2017.Tools.QuickMatrix;

namespace AdventOfCode2017.Solver
{
    internal partial class Day13 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Packet Scanners";

        private List<int> _scanners = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return ComputeSeverity(0, false).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            for (int startTime = 1; startTime < int.MaxValue; startTime++)
            {
                if (ComputeSeverity(startTime, true) == 0)
                {
                    return startTime.ToString();
                }
            }
            throw new InvalidDataException();
        }

        private long ComputeSeverity(int startTime, bool stopIfCaught)
        {
            long severityScore = 0;
            for (int cycleTime = 0; cycleTime < _scanners.Count; cycleTime++)
            {
                if (_scanners[cycleTime] == 0)
                {
                    // It's the start
                    continue;
                }
                int scannerCycleDuration = (_scanners[cycleTime] - 1) * 2; // Time for the scanner to go down and up again
                if ((startTime + cycleTime) % scannerCycleDuration == 0)
                {
                    // Scanner arrive at top position, we are caught...
                    if (stopIfCaught)
                    {
                        return int.MaxValue;
                    }
                    severityScore += cycleTime * _scanners[cycleTime];
                }
            }
            return severityScore;
        }

        private void ExtractData()
        {
            QuickMatrix qm = new(_puzzleInput, ":", true);
            _scanners = Enumerable.Repeat(0, qm.Cols[0].ConvertAll(c => int.Parse(c.StringVal)).Max() + 1).ToList();
            foreach (List<CellInfo> row in qm.Rows)
            {
                _scanners[int.Parse(row[0].StringVal)] = int.Parse(row[1].StringVal);
            }
        }
    }
}