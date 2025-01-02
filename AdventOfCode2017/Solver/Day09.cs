using System.Text;

namespace AdventOfCode2017.Solver
{
    internal partial class Day09 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Stream Processing";

        public override string GetSolution1(bool isChallenge)
        {
            StringBuilder result = new();
            foreach (string stream in _puzzleInput)
            {
                result.Append('+');
                result.Append(ProcessStream(stream, 1).score);
            }
            return result.ToString().Trim('+');
        }

        public override string GetSolution2(bool isChallenge)
        {
            StringBuilder result = new();
            foreach (string stream in _puzzleInput)
            {
                result.Append('+');
                result.Append(ProcessStream(stream, 1).removed);
            }
            return result.ToString().Trim('+');
        }

        private static (string processedStream, int score, int removed) ProcessStream(string stream, int depth)
        {
            int totalScore = 1 * depth;
            int removed = 0;
            stream = stream[1..];
            do
            {
                switch (stream[..1])
                {
                    case "}":
                        return (stream[1..], totalScore, removed);

                    case ",":
                        stream = stream[1..];
                        break;

                    case "<":
                        (string stream, int nbrRemoved) cleanned = RemoveGarbageAtStart(stream);
                        removed += cleanned.nbrRemoved;
                        stream = cleanned.stream;
                        break;

                    case "{":
                        (string processedStream, int score, int removed) tmpResult = ProcessStream(stream, depth + 1);
                        totalScore += tmpResult.score;
                        removed += tmpResult.removed;
                        stream = tmpResult.processedStream;
                        break;
                }
            } while (true);
        }

        private static (string stream, int nbrRemoved) RemoveGarbageAtStart(string stream)
        {
            // Remove garbage and count removal
            int i = 1;
            char[] streamChars = [.. stream];
            for (i = 1; i < streamChars.Length; i++)
            {
                if (streamChars[i] == '!')
                {
                    streamChars[i + 1] = '@';
                }
                else if (streamChars[i] == '>' && streamChars[i - 1] != '!')
                {
                    //Convert char array as string and remove the garbage
                    string removedSection = new string(streamChars)[1..i];
                    int count = removedSection.Count(c => c != '!' && c != '@');
                    return (stream[(i + 1)..], count);
                }
            }
            return (stream, 0);
        }
    }
}