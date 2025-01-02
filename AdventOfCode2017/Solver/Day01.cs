namespace AdventOfCode2017.Solver
{
    internal partial class Day01 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Inverse Captcha";

        public override string GetSolution1(bool isChallenge)
        {
            long result = 0;
            foreach (string sequence in _puzzleInput)
            {
                for (int i = 0; i < sequence.Length; i++)
                {
                    result += sequence[i] == sequence[(i + 1) % sequence.Length] ? long.Parse(sequence[i].ToString()) : 0;
                }
            }
            return result.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            long result = 0;
            foreach (string sequence in _puzzleInput)
            {
                for (int i = 0; i < sequence.Length; i++)
                {
                    result += sequence[i] == sequence[(i + sequence.Length / 2) % sequence.Length] ? long.Parse(sequence[i].ToString()) : 0;
                }
            }
            return result.ToString();
        }
    }
}