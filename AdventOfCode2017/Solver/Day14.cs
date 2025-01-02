using AdventOfCode2017.Tools;
using System.Text;
using static AdventOfCode2017.Tools.QuickMatrix;

namespace AdventOfCode2017.Solver
{
    internal partial class Day14 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Disk Defragmentation";

        public override string GetSolution1(bool isChallenge)
        {
            string key = _puzzleInput[0];
            QuickMatrix map = GenerateMap(key);
            return map.Cells.Count(c => c.StringVal == "1").ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            string key = _puzzleInput[0];
            QuickMatrix map = GenerateMap(key);
            int nbrOfZone = 0;
            while (map.Cells.Any(c => c.StringVal == "1"))
            {
                nbrOfZone++;
                List<CellInfo> area = map.GetTouchingCellsWithValue(map.Cells.First(c => c.StringVal == "1").Position, TouchingMode.HorizontalAndVertical);
                area.ForEach(c => c.StringVal = "0");
            }
            return nbrOfZone.ToString();
        }

        private static QuickMatrix GenerateMap(string key)
        {
            List<string> allLines = [];
            for (int i = 0; i < 128; i++)
            {
                List<byte> sequence = $"{key}-{i}".ToCharArray().ToList().ConvertAll(c => (byte)c);
                string hash = Day10.CreateDenseHash(sequence);

                // Convert hexa to binary
                List<byte> hashBytes = [];
                foreach (char c in hash)
                {
                    hashBytes.Add(Convert.ToByte(c.ToString(), 16));
                }

                // count bit at 1
                StringBuilder stringBuilder = new();
                foreach (byte b in hashBytes)
                {
                    string dataBlock = ("0000" + Convert.ToString(b, 2))[^4..];
                    stringBuilder.Append(dataBlock);
                }
                allLines.Add(stringBuilder.ToString());
            }
            return new(allLines);
        }
    }
}