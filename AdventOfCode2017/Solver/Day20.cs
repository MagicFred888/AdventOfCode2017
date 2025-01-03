namespace AdventOfCode2017.Solver
{
    internal partial class Day20 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Particle Swarm";

        private sealed class Point3D(long x, long y, long z)
        {
            public long X { get; } = x;
            public long Y { get; } = y;
            public long Z { get; } = z;

            public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

            public static bool operator ==(Point3D? a, Point3D? b)
            {
                if (ReferenceEquals(a, b)) return true;
                if (a is null || b is null) return false;
                return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
            }

            public static bool operator !=(Point3D? a, Point3D? b) => !(a == b);

            public override int GetHashCode() => HashCode.Combine(X, Y, Z);

            public override bool Equals(object? obj) => obj is Point3D p && this == p;

            public override string ToString() => $"({X},{Y},{Z})";
        }

        private sealed class Particle
        {
            public Point3D Position;
            public Point3D Velocity;
            public Point3D Acceleration;

            public Particle(string rawInfo)
            {
                string[] parts = rawInfo.Replace("<", ",").Replace(">", ",").Split(",");
                Position = new(long.Parse(parts[1]), long.Parse(parts[2]), long.Parse(parts[3]));
                Velocity = new(long.Parse(parts[6]), long.Parse(parts[7]), long.Parse(parts[8]));
                Acceleration = new(long.Parse(parts[11]), long.Parse(parts[12]), long.Parse(parts[13]));
            }

            public object ManhattanDistance => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
        }

        private List<Particle> _particles = [];

        public override string GetSolution1(bool isChallenge)
        {
            // Run simulation for 1000 iterations and find particle closest to 0,0,0
            _particles = _puzzleInput.Select(p => new Particle(p)).ToList();
            RunSimulation(1000, false);
            return _particles.IndexOf(_particles.MinBy(p => p.ManhattanDistance)!).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // Run simulation for 1000 iterations and count remaining particles
            _particles = _puzzleInput.Select(p => new Particle(p)).ToList();
            RunSimulation(1000, true);
            return _particles.Count.ToString();
        }

        private void RunSimulation(int nbrIteration, bool removeCollidingParticles)
        {
            for (int i = 0; i < nbrIteration; i++)
            {
                // Move particles
                foreach (Particle particle in _particles)
                {
                    particle.Velocity += particle.Acceleration;
                    particle.Position += particle.Velocity;
                }

                // Remove colliding particles
                if (removeCollidingParticles)
                {
                    // Group particles by position and save groups in intermediate variable
                    _particles = _particles.GroupBy(p => p.Position).Where(g => g.Count() == 1).Select(g => g.First()).ToList();
                }
            }
        }
    }
}