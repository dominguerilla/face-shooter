using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveTo : Action {

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The object to move. If it has a FaceEnemy component, the move speed and stopping distance will be automatically set.")]
    public SharedGameObject movingGameObject;
    public SharedGameObject destinationGameObject;

    public SharedFloat moveSpeed;
    public SharedFloat stoppingDistance;

    // FaceEnemy support
    private FaceEnemy face;
    private Transform target;

    public override void OnStart()
    {
        face = movingGameObject.Value.GetComponent<FaceEnemy>();
        target = GetDefaultGameObject(destinationGameObject.Value).transform;
        if (face != null)
        {
            moveSpeed = face.chargeSpeed;
            stoppingDistance = face.stoppingDistance;
        }
    }

    public override TaskStatus OnUpdate()
    {
        while (Vector3.Distance(face.transform.position, target.position) > face.stoppingDistance)
        {
            face.transform.position = Vector3.MoveTowards(face.transform.position, target.position, face.chargeSpeed * Time.deltaTime);
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }
}
