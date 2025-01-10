namespace OCSPath
{
    internal class Edge : IEquatable<Edge>
    {
        public Vertex Vertex { get; }
        public int Weight { get; }
        public int AddtionalWeight { get; set; }

        public Edge(Vertex vertex, int weight, int additionalCost = 0)
        {
            Vertex = vertex;
            Weight = weight;
            AddtionalWeight = additionalCost;
        }

        public bool Equals(Edge other)
        {
            return Vertex.Key == other.Vertex.Key
                && Weight == other.Weight
                && AddtionalWeight == other.AddtionalWeight;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = hash * 7 + Weight;
            hash = hash * 7 + AddtionalWeight;
            hash = hash * 7 + (int)Vertex.Key;
            return hash;
        }

        public override bool Equals(object obj)
        {   
            var other = obj as Edge;

            if (other == null)
                return false;

            return Equals(other);
        }
    }
}