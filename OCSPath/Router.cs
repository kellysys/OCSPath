namespace OCSPath
{
    public class Router
    {
        public bool Initialized { get; private set; }
        private ReaderWriterLockSlim _lock = new();

        private Graph _graph = new();
        private Graph _graphReverse = new();

        private List<uint> _vertices = new();
        private List<(uint f, uint t, int w)> _edges = new();

        public void AddVertices(List<uint> vertices)
        {
            try
            {
                _lock.EnterWriteLock();

                _vertices.AddRange(vertices);

                foreach (var vertex in _vertices)
                {
                    _graph.AddVertex(vertex);
                    _graphReverse.AddVertex(vertex);
                }
            }
            finally { _lock.ExitWriteLock(); }
        }

        public void AddEdges(List<(uint f, uint t, int w)> edges)
        {
            _edges.AddRange(edges);

            foreach (var edge in _edges)
            {
                Connect(edge.f, edge.t, edge.w);
            }

            Initialized = true;
        }

        public void Connect(uint from, uint to, int weight)
        {
            try
            {
                _lock.EnterWriteLock();
                _graph.Connect(from, to, weight);
                _graphReverse.Connect(to, from, weight);
            }
            finally { _lock.ExitWriteLock(); }
        }

        public void Disconnect(uint from, uint to)
        {
            try
            {
                _lock.EnterWriteLock();
                _graph.Disconnect(from, to);
                _graphReverse.Disconnect(to, from);
            }
            finally { _lock.ExitWriteLock(); }
        }

        public void SetAdditionalWeight(uint from, uint to, int cost)
        {
            try
            {
                _lock.EnterWriteLock();
                _graph.SetAddtionalCost(from, to, cost);
                _graphReverse.SetAddtionalCost(to, from, cost);
            }
            finally { _lock.ExitWriteLock(); }
        }

        public int GetWeight(uint from, uint to)
        {
            try
            {
                _lock.EnterReadLock();
                return _graph.GetDistance(from, to);
            }
            finally { _lock.ExitReadLock(); }
        }

        /// <summary>
        /// Basic shortest path
        /// </summary>
        public SearchResult? Search(uint from, uint to)
        {
            if (!Initialized) return null;

            try
            {
                _lock.EnterReadLock();
                return _graph.Search(from, to);
            }
            finally { _lock.ExitReadLock(); }
        }

        /// <summary>
        /// ex) Vehicle find nearest waiting position
        /// </summary>
        /// <param name="from">Vehicle position</param>
        /// <param name="toList">The positions of multiple targets</param>
        public (uint vertex, int weight) Search(uint from, HashSet<uint> toList)
        {
            if (!Initialized) return (0, 0);

            try
            {
                _lock.EnterReadLock();
                return _graph.MultiSearch(from, toList, 0);
            }
            finally { _lock.ExitReadLock(); }
        }

        /// <summary>
        /// ex) Order find nearest vehicle
        /// </summary>
        /// <param name="fromList">The positions of multiple vehicles</param>
        /// <param name="to">Target port position</param>
        public (uint vertex, int weight) Search(HashSet<uint> fromList, uint to)
        {
            if (!Initialized) return (0, 0);
            try
            {
                _lock.EnterReadLock();
                return _graphReverse.MultiSearch(to, fromList);
            }
            finally { _lock.ExitReadLock(); }
        }
    }
}
