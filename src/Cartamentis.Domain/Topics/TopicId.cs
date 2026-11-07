namespace Cartamentis.Domain.Topics;

public readonly record struct TopicId
{
    private readonly Guid _value;

    public TopicId()
    {
        _value = Guid.NewGuid();
    }

    public TopicId(Guid value)
    {
        _value = value;
    }

    public Guid Value => _value;

    public static TopicId New() => new();

    public static TopicId NewUnique() => new(Guid.NewGuid());

    public override string ToString() => _value.ToString();
}
