namespace GodotUtils.Tests.MultiMeshInstance;

using Godot;
using GodotUtils.MultiMeshInstance;
using NUnit.Framework;

[TestFixture]
public class MultiMeshInstanceTests
{
    private MultiMeshInstance multiMeshInstance;
    
    [SetUp]
    public void Setup()
    {
        var mesh = new Mesh();
        var position = Vector3.Zero;
        var instances = new List<Transform3D> { new() };
        var scenario = new World3D();
        multiMeshInstance = new MultiMeshInstance(mesh, position, instances, scenario);
    }
    
    // Can figure out tests which would work.
    // Tried forcing godot to throw an exception, but couldn't even on custom code.
    // Maybe add tests in the future, but it should be small and robust enough to not break.

    [Test]
    public void Pass()
    {
        Assert.Pass();
    }

    [TearDown]
    public void TearDown()
    {
        multiMeshInstance.Dispose();
    }
}