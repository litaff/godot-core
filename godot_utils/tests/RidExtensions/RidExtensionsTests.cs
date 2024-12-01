namespace GodotUtils.Tests.RidExtensions;

using Godot;
using GodotUtils.RidExtensions;
using NUnit.Framework;

[TestFixture]
public class RidExtensionsTests
{
    [Test]
    public void FreeRid_WithValidRid_PerformsFreeingAction()
    {
        // RenderingServer is used as a dummy.
        var rid = RenderingServer.InstanceCreate();
        var actionCalled = false;

        rid.FreeRid(FreeingAction);
        
        Assert.That(actionCalled, Is.True);
        
        return;

        void FreeingAction(Rid r)
        {
            RenderingServer.FreeRid(r);
            actionCalled = true;
        }
    }
    
    [Test]
    public void FreeRid_WithValidRid_MakesRidInvalid()
    {
        // RenderingServer is used as a dummy.
        var rid = RenderingServer.InstanceCreate();

        rid.FreeRid(FreeingAction);
        
        Assert.That(rid.IsValid, Is.False);
        
        return;

        void FreeingAction(Rid r)
        {
            RenderingServer.FreeRid(r);
        }
    }
}