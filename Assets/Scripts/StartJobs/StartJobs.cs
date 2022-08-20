using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace StartJobs
{
    public class StartJobs : MonoBehaviour
    {
        [SerializeField]
        private bool useJobs;

        private void Update()
        {
            float startTime = Time.realtimeSinceStartup;

            if (useJobs)
            {
                NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(Allocator.Temp);

                for(int i = 0; i < 10; i++)
                    jobHandles.Add(ReallyTouthTaskJobs());

                JobHandle.CompleteAll(jobHandles);
                jobHandles.Dispose();
            }
            else
            {
                for (int i = 0; i < 10; i++)
                    ReallyTouthTask();
            }

            float endTime = Time.realtimeSinceStartup;

            Debug.Log( (endTime - startTime) * 1000 + "ms");
        }

        public static void ReallyTouthTask()
        {
            float value = 0;
            for (int i = 0; i < 10000; i++)
                value = math.exp(math.sqrt(value));
        }

        private JobHandle ReallyTouthTaskJobs()
        {
            ReallyTouthJob job = new ReallyTouthJob();
            return job.Schedule();
        }
    }
}
