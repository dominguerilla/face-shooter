﻿using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the mouse position.")]
    public class GetMousePosition : Action
    {
        [RequiredField]
        [Tooltip("The stored result")]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Input.mousePosition;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = Vector2.zero;
        }
    }
}