namespace AdventOfCode2017.Solver
{
    internal partial class Day17 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Spinlock ";

        private sealed class ChainedNode
        {
            public int Value { get; init; }
            public ChainedNode Next;

            public ChainedNode(int value)
            {
                Value = value;
                Next = this;
            }
        }

        public override string GetSolution1(bool isChallenge)
        {
            int finalBufferSize = 2017;
            int steps = int.Parse(_puzzleInput[0]);

            ChainedNode baseNode = new(0);
            ChainedNode finalNode = PerformTheLoops(baseNode, steps, finalBufferSize);
            return finalNode.Next.Value.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            int finalBufferSize = 50000000;
            int steps = int.Parse(_puzzleInput[0]);

            ChainedNode baseNode = new(0);
            _ = PerformTheLoops(baseNode, steps, finalBufferSize);
            return baseNode.Next.Value.ToString();
        }

        private static ChainedNode PerformTheLoops(ChainedNode baseNode, int steps, int finalBufferSize)
        {
            ChainedNode currentPosition = baseNode;
            for (int virtualBufferSize = 1; virtualBufferSize <= finalBufferSize; virtualBufferSize++)
            {
                // Move to the next node
                for (int i = 0; i < steps; i++)
                {
                    currentPosition = currentPosition.Next;
                }

                // Insert the new node
                ChainedNode newNode = new(virtualBufferSize)
                {
                    Next = currentPosition.Next
                };
                currentPosition.Next = newNode;
                currentPosition = newNode;
            }
            return currentPosition;
        }
    }
}