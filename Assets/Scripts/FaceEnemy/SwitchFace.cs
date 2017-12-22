using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SwitchFace : Action
{
    public enum FaceType{
        SLEEPING,
        AWAKE,
        ATTACKING
    }

    public FaceType faceType;

    FaceEnemy face;

	public override void OnStart()
	{
        face = GetComponent<FaceEnemy>();
        if (!face)
            Debug.LogError("FaceEnemy component not found on " + gameObject.name);	
	}

	public override TaskStatus OnUpdate()
	{
        Material[] faces = GetFaceMaterials(faceType);
        int index = UnityEngine.Random.Range(0, faces.Length);
        face.SwitchMaterial(faces[index]);
		return TaskStatus.Success;
	}

    Material[] GetFaceMaterials(FaceType faceType)
    {
        switch (faceType)
        {
            case FaceType.SLEEPING:
                return face.spawningMats;
            case FaceType.AWAKE:
                return face.awakeMats;
            case FaceType.ATTACKING:
                return face.chargingMats;
            default:
                Debug.LogError("Couldn't find correct faceType: " + faceType);
                return face.spawningMats;
        }
    }
}