using Cartamentis.Domain.Topics;

namespace Cartamentis.Domain.MindMaps;

public sealed class MindMap
{
    public MindMapId Id { get; }
    public Topic Root { get; }

    private MindMap(MindMapId id, Topic root)
    {
        Id = id;
        Root = root;
    }

    public static MindMap Create(string rootContent)
    {
        var mindMapId = MindMapId.New();
        var root = Topic.CreateRoot(rootContent, mindMapId);
        return new MindMap(mindMapId, root);
    }

    public static MindMap CreateFromRoot(Topic root)
    {
        // For extracting a subtree, the root topic already has a mind map ID
        // We create a new mind map ID for the extracted map
        var mindMapId = MindMapId.New();
        return new MindMap(mindMapId, root);
    }
}
