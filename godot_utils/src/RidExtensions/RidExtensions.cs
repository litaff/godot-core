namespace GodotUtils.RidExtensions;

using Godot;

public static class RidExtensions
{
    /// <summary>
    /// Frees the rid and make it invalid, so it can be checked later for validity.
    /// If freed with a wrong action, the rid is lost.
    /// </summary>
    /// <param name="rid">Rid being processed.</param>
    /// <param name="freeingAction">Action which will be performed, should be a server rid freeing action.</param>
    public static void FreeRid(this ref Rid rid, Action<Rid> freeingAction)
    {
        if (!rid.IsValid)
        {
            rid = new Rid(); // TODO: Investigate this call, !IsValid mean Rid == 0 and new Rid() should make a 0 Rid.
            return;
        }
        freeingAction.Invoke(rid);
        rid = new Rid();
    }

    public static void RenderingServerFreeRid(this ref Rid rid)
    {
        rid.FreeRid(RenderingServer.FreeRid);
    }
}