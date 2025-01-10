namespace OCSPath
{
    internal delegate void ChildAction(in Edge edge);

    internal class Vertex
    {
        private List<Edge> _childrens;

        public Vertex(uint key)
        {
            Key = key;
            _childrens = [];
        }

        public uint Key { get; }

        internal void AddChild(in Edge edge)
        {
            var key = edge.Vertex.Key;

            if (!_childrens.Exists(x => x.Vertex.Key == key))
                _childrens.Add(edge);
        }

        internal void RemoveChild(uint to)
        {
            var edge = _childrens.Find(x => x.Vertex?.Key == to);
            if (edge.Vertex != null)
                _childrens.Remove(edge);
        }

        public void EachChild(ChildAction action)
        {
            foreach (var c in _childrens)
                action(in c);
        }

        public void SetAddtionalCost(uint to, int additionalCost)
        {
            var edge = _childrens.Find(x => x.Vertex?.Key == to);
            if (edge?.Vertex != null)
                edge.AddtionalWeight = additionalCost;
        }

    }
}