namespace Cartamentis.Domain.MindMaps;

public readonly record struct MindMapId
{
    private readonly Guid _value;

    public MindMapId()
    {
        _value = Guid.NewGuid();
    }

    public MindMapId(Guid value)
    {
        _value = value;
    }

    public static MindMapId New() => new();

    public override string ToString() => _value.ToString();
}
