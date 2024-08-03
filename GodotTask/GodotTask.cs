namespace GodotTask;

using Godot;

public class GodotTask
{
    private GodotTimingType timingType;
    private readonly Func<bool>? predicate;
    private readonly TaskCompletionSource taskCompletionSource;
    
    private static SceneTree? tree;

    private GodotTask(Func<bool> predicate, GodotTimingType timingType)
    {
        taskCompletionSource = new TaskCompletionSource();
        this.predicate = predicate ?? throw new NullReferenceException(
            "[GodotTask] Failed to create an object, predicate cannot be null!");
        this.timingType = timingType;
        RegisterPredicate();
    }

    /// <summary>
    /// Waits until the predicate returns true. It checks the predicate every frame in the main loop.
    /// </summary>
    /// <param name="predicate">Predicate to be checked.</param>
    /// <param name="timingType">Before which update call should the predicate be checked.</param>
    /// <returns>Task, which is complete when the predicate is true.</returns>
    public static Task WaitUntil(Func<bool> predicate, GodotTimingType timingType = GodotTimingType.Process)
    {
        GetSceneTree();

        var godotTask = new GodotTask(predicate, timingType);

        return godotTask.taskCompletionSource.Task;
    }

    private static void GetSceneTree()
    {
        if (tree != null) return;
        
        if (Engine.GetMainLoop() is not SceneTree parsedTree)
        {
            throw new NotSupportedException("[GodotTask] GodotTask only supports SceneTree as the main loop!");
        }

        tree = parsedTree;
    }

    private void CheckPredicate()
    {
        if (!predicate!.Invoke()) return;
        taskCompletionSource.SetResult();
        UnregisterPredicate();
    }
    
    private void RegisterPredicate()
    {
        switch (timingType)
        {
            case GodotTimingType.Process:
                tree!.ProcessFrame += CheckPredicate;
                break;
            case GodotTimingType.PhysicsProcess:
                tree!.PhysicsFrame += CheckPredicate;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(timingType), timingType, null);
        }
    }
    
    private void UnregisterPredicate()
    {
        switch (timingType)
        {
            case GodotTimingType.Process:
                tree!.ProcessFrame -= CheckPredicate;
                break;
            case GodotTimingType.PhysicsProcess:
                tree!.PhysicsFrame -= CheckPredicate;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(timingType), timingType, null);
        }
    }
}