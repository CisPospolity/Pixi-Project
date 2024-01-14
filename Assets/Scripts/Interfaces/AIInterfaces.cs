using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface ISpeedProvider
    {
        public float GetSpeed();
    }

    public interface ITargetTransformProvider
    {
        public Transform GetTargetTransform();
    }
}