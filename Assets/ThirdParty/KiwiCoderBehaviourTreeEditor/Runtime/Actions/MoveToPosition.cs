using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace TheKiwiCoder {

    [System.Serializable]
    public class MoveToPosition : ActionNode {

        [Tooltip("How fast to move")]
        public NodeProperty<float> speed = new NodeProperty<float> { defaultValue = 5.0f };

        [Tooltip("Stop within this distance of the target")]
        public NodeProperty<float> stoppingDistance = new NodeProperty<float> { defaultValue = 0.1f };

        [Tooltip("Updates the agents rotation along the path")]
        public NodeProperty<bool> updateRotation = new NodeProperty<bool> {defaultValue = true};
        
        [Tooltip("Maximum acceleration when following the path")] 
        public NodeProperty<float> acceleration = new NodeProperty<float> { defaultValue = 40.0f };

        [Tooltip("Returns success when the remaining distance is less than this amount")]
        public NodeProperty<float> tolerance = new NodeProperty<float> { defaultValue = 1.0f };
        
        [Tooltip("Target Position")] 
        public NodeProperty<Vector3> targetPosition = new NodeProperty<Vector3> { defaultValue = Vector3.zero };

        protected override void OnStart() {

        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            return State.Running;
        }
    }
}
