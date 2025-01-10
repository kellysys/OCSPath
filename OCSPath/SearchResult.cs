namespace OCSPath
{
    public readonly struct SearchResult
    {
        public List<uint>? Path { get; }
        public int Distance { get; }
        public uint From { get; }
        public uint To { get; }
        public bool IsFounded => Distance != int.MaxValue;

        internal SearchResult(uint from, uint to, int distance = int.MaxValue, IDictionary<uint, uint>? path = null)
        {
            From = from;
            To = to;
            Distance = distance;

            if (path != null)
            {
                Path = [];
                List<uint> result = [];

                uint p = To;
                result.Add(p);

                while (p != From)
                {
                    result.Add(path[p]);
                    p = path[p];
                }

                result.Reverse();
                Path = result;
            }
        }
    }
}