namespace GodotUtils.MultiMeshInstance;

using Godot;
using RidExtensions;

public class MultiMeshInstance
{
    private Rid instanceRid;
    private Rid multiMeshRid;
    
    public MultiMeshInstance(Mesh mesh, Vector3 position, List<Transform3D> instances, World3D scenario)
    {
        instanceRid = RenderingServer.InstanceCreate();
        multiMeshRid = RenderingServer.MultimeshCreate();
        
        RenderingServer.MultimeshSetMesh(multiMeshRid, mesh.GetRid());
        RenderingServer.MultimeshAllocateData(multiMeshRid, instances.Count, RenderingServer.MultimeshTransformFormat.Transform3D);
        RenderingServer.MultimeshSetVisibleInstances(multiMeshRid, instances.Count);
        
        for (var i = 0; i < instances.Count; i++)
        {
            RenderingServer.MultimeshInstanceSetTransform(multiMeshRid, i, instances[i]);
        }

        RenderingServer.InstanceSetBase(instanceRid, multiMeshRid);
        RenderingServer.InstanceSetScenario(instanceRid, scenario.Scenario);
        RenderingServer.InstanceSetTransform(instanceRid, new Transform3D(Basis.Identity, position));
    }

    public void Display()
    {
        if (!instanceRid.IsValid) return; // Can't find information if server checks for rid validity so this stays.
        RenderingServer.InstanceSetVisible(instanceRid, true);
    }
    
    public void Hide()
    {
        if (!instanceRid.IsValid) return; // Can't find information if server checks for rid validity so this stays.
        RenderingServer.InstanceSetVisible(instanceRid, false);
    }
    
    public void Dispose()
    {
        multiMeshRid.RenderingServerFreeRid();
        instanceRid.RenderingServerFreeRid();
    }
    
    ~MultiMeshInstance()
    {
        Dispose();
    }
}