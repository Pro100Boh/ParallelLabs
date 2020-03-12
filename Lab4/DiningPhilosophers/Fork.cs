namespace Lab4.DiningPhilosophers
{
    public class Fork
	{
        private static int _count = 0;
        public string Name { get; private set; }

        public Fork()
        {
            Name = $"Fork {_count++}";
        }
    }
}
