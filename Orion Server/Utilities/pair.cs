namespace OrionServer.Utilities
{
    public class Pair<T, U>
    {
        public T Item1 { get; set; }
        public U Item2 { get; set; }

        public Pair(T Item1, U Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }
}
