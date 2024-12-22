class Path(Node _node)
{
    public Node Self = _node;
    public Path? Next;

    public static void CreateCopy(Path _source, out Path _target)
    {
        _target = new(_source.Self);

        while (_source.Next != null)
        {
            _source = _source.Next;
            _target.Insert(_source.Self);
        }
    }

    public bool Search(Node _node)
    {
        if (Self == _node) return true;
        if (Next != null) return Next.Search(_node);

        return false;
    }


    public Node GetLast()
    {
        if (Next != null) return Next.GetLast();
        return Self;
    }

    public float GetTotalCost(float _startValue = 0)
    {
        if(Next != null)
        {
            foreach (Edge edge in Self.Edges)
            {
                if(edge._node == Next.Self)
                    return Next.GetTotalCost(_startValue + edge.Cost);
            }
        }

        return _startValue;
    }

    public void Insert(Node _node)
    {

        if (Self == _node) return;

        if (Next == null)
        {
            Next = new(_node);
            return;
        }

        Next.Insert(_node);
    }

    public override string ToString()
    {
        string outStr = Self + "";

        if (Next == null) return outStr;

        CreateCopy(Next, out Path Next_cpy);

        while (true)
        {
            outStr += " " + Next_cpy.Self;
            if (Next_cpy.Next == null) break;
            CreateCopy(Next_cpy.Next, out Next_cpy);
        }

        return outStr;
    }
}
