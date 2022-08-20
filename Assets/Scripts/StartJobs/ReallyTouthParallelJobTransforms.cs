using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace StartJobs
{
    [BurstCompile]
    public struct ReallyTouthParallelJobTransforms : IJobParallelForTransform
    {
        public NativeArray<float> speedArray;
        [ReadOnly] public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            transform.position += new Vector3(0, speedArray[index] * deltaTime, 0);
            if (transform.position.y > 5 || transform.position.y < -5)
                speedArray[index] *= -1;

            StartJobs.ReallyTouthTask();
        }
    }
}

