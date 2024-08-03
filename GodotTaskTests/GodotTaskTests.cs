namespace GodotTaskTests;

using NUnit.Framework;
using System;
using System.Threading.Tasks;
using GodotTask;

[TestFixture]
public class GodotTaskTests
{
    [Test]
    public async Task WaitUntil_PredicateTrue_CompletesTaskInNextLoop()
    {
        var task = GodotTask.WaitUntil(() => true);
        Assert.That(task.IsCompleted, Is.False);
        await task;
        Assert.That(task.IsCompleted, Is.True);
    }

    [Test]
    public void WaitUntil_PredicateNull_ThrowsNullReferenceException()
    {
        Assert.Throws<NullReferenceException>(() => GodotTask.WaitUntil(null!));
    }

    [Test]
    public async Task WaitUntil_PredicateFalse_DoesNotCompleteTask()
    {
        var task = GodotTask.WaitUntil(() => false);
        await Task.Delay(100);
        Assert.That(task.IsCompleted, Is.False);
    }

    [Test]
    public async Task WaitUntil_PredicateTrueAfterDelay_CompletesTask()
    {
        var taskStartTime = DateTime.Now;
        const int delay = 1000;
        var task = GodotTask.WaitUntil(Predicate);
        
        await Task.Delay(delay);
        
        Assert.That(task.IsCompleted, Is.True);
        
        return;
        bool Predicate() => DateTime.Now.Second - taskStartTime.Second >= delay / 1000;
    }
}
