namespace OCSPath
{
    internal class VertexComparer : IComparer<uint>
    {
        private readonly IDictionary<uint, int> _distance;

        public VertexComparer(IDictionary<uint, int> distance)
        {
            _distance = distance;
        }

        public int Compare(uint x, uint y)
        {
            int xDistance = _distance.ContainsKey(x) ? _distance[x] : int.MaxValue;
            int yDistance = _distance.ContainsKey(y) ? _distance[y] : int.MaxValue;

            int comparer = xDistance.CompareTo(yDistance);

            if (comparer == 0)
            {
                return x.CompareTo(y);
            }

            return comparer;
        }
    }
}