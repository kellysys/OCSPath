namespace OCSPath
{
    internal class Graph
    {
        private IDictionary<uint, Vertex> vertices = new Dictionary<uint, Vertex>();

        public void AddVertex(uint key)
        {
            if (vertices.ContainsKey(key))
                throw new Exception("Vertex have to be unique.");

            vertices.Add(key, new Vertex(key));
        }

        private bool Exist(uint from, uint to) => !vertices.ContainsKey(from) || !vertices.ContainsKey(to);

        public bool Connect(uint from, uint to, int cost)
        {
            if (Exist(from, to)) return false;

            Vertex vertexFrom = vertices[from];
            Vertex vertexTo = vertices[to];

            vertexFrom.AddChild(new Edge(vertexTo, cost));

            return true;
        }

        public bool Disconnect(uint from, uint to)
        {
            if (Exist(from, to)) return false;

            Vertex vertexFrom = vertices[from];
            vertexFrom.RemoveChild(to);
            return true;
        }

        public bool SetAddtionalCost(uint from, uint to, int additionalCost)
        {
            if (Exist(from, to)) return false;

            Vertex vertexFrom = vertices[from];
            vertexFrom.SetAddtionalCost(to, additionalCost);
            return true;
        }

        public int GetDistance(uint from, uint to, int depth = int.MaxValue)
        {
            var distance = new Dictionary<uint, int> { [from] = 0 };
            var q = new SortedSet<uint>(new[] { from }, new VertexComparer(distance));
            var current = new HashSet<uint>();

            int Distance(uint key) => distance.ContainsKey(key) ? distance[key] : int.MaxValue;

            do
            {
                uint u = q.Deque();

                if (u == to)
                    return distance[u];

                current.Remove(u);

                vertices[u].EachChild((in Edge e) =>
                {
                    if (Distance(e.Vertex.Key) > Distance(u) + e.Weight + e.AddtionalWeight)
                    {
                        if (current.Contains(e.Vertex.Key))
                            q.Remove(e.Vertex.Key);

                        distance[e.Vertex.Key] = Distance(u) + e.Weight + e.AddtionalWeight;
                        q.Add(e.Vertex.Key);
                        current.Add(e.Vertex.Key);
                    }
                });
            } while (q.Count > 0);

            return int.MaxValue;
        }

        public SearchResult Search(uint from, uint to, int depth = int.MaxValue)
        {
            var path = new Dictionary<uint, uint>();
            var distance = new Dictionary<uint, int> { [from] = 0 };
            var d = new Dictionary<uint, int> { [from] = 0 };
            var q = new SortedSet<uint>(new[] { from }, new VertexComparer(distance));
            var current = new HashSet<uint>();

            int Distance(uint key) => distance.ContainsKey(key) ? distance[key] : int.MaxValue;

            do
            {
                uint u = q.Deque();

                if (u == to)
                    return new SearchResult(from, to, distance[u], path);

                current.Remove(u);

                if (depth == d[u])
                    continue;

                vertices[u].EachChild((in Edge e) =>
                {
                    if (Distance(e.Vertex.Key) > Distance(u) + e.Weight + e.AddtionalWeight)
                    {
                        if (current.Contains(e.Vertex.Key))
                            q.Remove(e.Vertex.Key);

                        distance[e.Vertex.Key] = Distance(u) + e.Weight + e.AddtionalWeight;
                        q.Add(e.Vertex.Key);
                        current.Add(e.Vertex.Key);
                        path[e.Vertex.Key] = u;
                        d[e.Vertex.Key] = d[u] + 1;
                    }
                });
            } while (q.Count > 0 && depth > 0);

            return new SearchResult(from, to);
        }

        public (uint vertex, int weight) MultiSearch(uint from, HashSet<uint> toList, int minCost = 0)
        {
            var distance = new Dictionary<uint, int> { [from] = 0 };
            var q = new SortedSet<uint>(new[] { from }, new VertexComparer(distance));
            var current = new HashSet<uint>();
            int Distance(uint key) => distance.ContainsKey(key) ? distance[key] : Int32.MaxValue;

            if (toList.Count() == 0)
                return (0, 0);

            do
            {
                uint u = q.Deque();

                if (distance[u] >= minCost && toList.Contains(u))
                    return (u, distance[u]);

                current.Remove(u);

                vertices[u].EachChild((in Edge e) =>
                {
                    if (Distance(e.Vertex.Key) > Distance(u) + e.Weight + e.AddtionalWeight)
                    {
                        if (current.Contains(e.Vertex.Key))
                            q.Remove(e.Vertex.Key);

                        distance[e.Vertex.Key] = Distance(u) + e.Weight + e.AddtionalWeight;
                        q.Add(e.Vertex.Key);
                        current.Add(e.Vertex.Key);
                    }
                });
            } while (q.Count > 0);

            return (0, 0);
        }
    }
}