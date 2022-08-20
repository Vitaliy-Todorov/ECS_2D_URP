using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace StartJobs
{
    [BurstCompile]
    public struct ReallyTouthParallelJob : IJobParallelFor
    {
        public NativeArray<float3> positionArray;
        public NativeArray<float> speedArray;
        [ReadOnly] public float deltaTime;

        public void Execute(int index)
        {
            positionArray[index] += new float3(0, speedArray[index] * deltaTime, 0);
            if (positionArray[index].y > 5 || positionArray[index].y < -5)
                speedArray[index] *= -1;

            StartJobs.ReallyTouthTask();
        }
    }
}

