using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TransformMoveTowards : Action
{
    public SharedTransform Target;
    public float GoalDistance;
    public float MoveSpeed = 1.0f;
    
	public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
        if (Vector3.Distance(transform.position, Target.Value.position) > GoalDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.Value.position, MoveSpeed * Time.deltaTime);
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
	}
}