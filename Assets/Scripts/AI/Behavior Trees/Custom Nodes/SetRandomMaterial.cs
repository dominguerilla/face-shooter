using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Basic/Renderer")]
[TaskDescription("Sets the material on the Renderer.")]
public class SetRandomMaterial : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The materials to choose from")]
    public SharedMaterial[] materials;

    // cache the renderer component
    private Renderer renderer;
    private GameObject prevGameObject;

    public override void OnStart()
    {
        var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        if (currentGameObject != prevGameObject)
        {
            renderer = currentGameObject.GetComponent<Renderer>();
            prevGameObject = currentGameObject;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (renderer == null)
        {
            Debug.LogWarning("Renderer is null");
            return TaskStatus.Failure;
        }
        int randomIndex = Random.Range(0, materials.Length);

        renderer.material = materials[randomIndex].Value;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        materials = null;
    }
}
