using AdventOfCode2017.Tools;

namespace AdventOfCode2017.Solver
{
    internal partial class Day10 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Knot Hash";

        public override string GetSolution1(bool isChallenge)
        {
            // Get base data
            List<byte> loop = Enumerable.Range(0, isChallenge ? 256 : 5).ToList().ConvertAll(i => (byte)i);
            List<byte> sequence = QuickList.ListOfListByte(_puzzleInput, ",", true)[0];

            // Scramble
            loop = ScrambleLoop(loop, sequence, 1);

            // Result
            return (loop[0] * loop[1]).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // Get base data
            List<byte> loop = Enumerable.Range(0, isChallenge ? 256 : 5).ToList().ConvertAll(i => (byte)i);
            List<byte> sequence = _puzzleInput[0].ToCharArray().ToList().ConvertAll(c => (byte)c);
            sequence.AddRange([17, 31, 73, 47, 23]);

            // Scramble
            loop = ScrambleLoop(loop, sequence, 64);

            // Create dense hash
            List<byte> denseHash = [];
            for (int i = 0; i < 255; i += 16)
            {
                byte baseValue = loop[i];
                for (int j = 1; j < 16; j++)
                {
                    baseValue = (byte)(baseValue ^ loop[i + j]);
                }
                denseHash.Add(baseValue);
            }

            // Convert to hexa
            return denseHash.Aggregate("", (acc, b) => acc + b.ToString("x2"));
        }

        private static List<byte> ScrambleLoop(List<byte> loop, List<byte> sequence, int nbrOfScramble)
        {
            int pos = 0;
            int skipSize = 0;
            for (int tour = 0; tour < nbrOfScramble; tour++)
            {
                foreach (int move in sequence)
                {
                    // Invert string from index to index + move
                    List<byte> subList = [];
                    for (int i = 0; i < move; i++)
                    {
                        subList.Add(loop[(pos + i) % loop.Count]);
                    }
                    subList.Reverse();

                    // Put back in loop
                    for (int i = 0; i < move; i++)
                    {
                        loop[(pos + i) % loop.Count] = subList[i];
                    }

                    // Move pos
                    pos += move + skipSize;
                    skipSize++;
                }
            }
            return loop;
        }
    }
}