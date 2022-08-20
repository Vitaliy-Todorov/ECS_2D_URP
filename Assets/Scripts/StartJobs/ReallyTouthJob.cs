using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace StartJobs
{
    [BurstCompile]
    public struct ReallyTouthJob : IJob
    {
        public void Execute()
        {
            float value = 0;
            for (int i = 0; i < 50000; i++)
                value = math.exp(math.sqrt(value));
        }
    }
}
