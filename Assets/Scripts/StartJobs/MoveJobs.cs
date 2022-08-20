using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace StartJobs
{
    public partial class MoveJobs : MonoBehaviour
    {
        [SerializeField]
        private bool _useJobsFloat3;
        [SerializeField]
        private bool _useJobsTransform;
        [SerializeField]
        private Transform _pfObject;
        private List<MovingObject> _movingObjects = new List<MovingObject>();

        private void Start()
        {
            for(int i = 0; i < 1000; i++)
            {
                Transform objTransform = Instantiate(_pfObject, new Vector3(UnityEngine.Random.Range(-7.9f, 7.9f), UnityEngine.Random.Range(-4.9f, 4.9f), 0), Quaternion.identity);
                MovingObject movingObject = new MovingObject
                {
                    transform = objTransform,
                    moveSpeed = UnityEngine.Random.Range(1f, 3f)
                };

                _movingObjects.Add(movingObject);
            }
        }

        private void Update()
        {
            float startTime = Time.realtimeSinceStartup;
            if (_useJobsFloat3)
                UseJobsFloat3();
            else if(_useJobsTransform)
                UseJobsTransform();
            else
                foreach (MovingObject movingObject in _movingObjects)
                {
                    movingObject.transform.position += new Vector3(0, movingObject.moveSpeed * Time.deltaTime, 0);
                    if (movingObject.transform.position.y > 5 || movingObject.transform.position.y < -5)
                        movingObject.moveSpeed *= -1;

                    StartJobs.ReallyTouthTask();
                }

            float endTime = Time.realtimeSinceStartup;
            Debug.Log((endTime - startTime) * 1000f + " ms");
        }

        private void UseJobsFloat3()
        {
            NativeArray<float3> positionArrey = new NativeArray<float3>(_movingObjects.Count, Allocator.TempJob);
            NativeArray<float> speedArray = new NativeArray<float>(_movingObjects.Count, Allocator.TempJob);

            for (int i = 0; i < _movingObjects.Count; i++)
            {
                positionArrey[i] = _movingObjects[i].transform.position;
                speedArray[i] = _movingObjects[i].moveSpeed;
            }

            ReallyTouthParallelJob reallyTouthParallelJob = new ReallyTouthParallelJob
            {
                positionArray = positionArrey,
                speedArray = speedArray,
                deltaTime = Time.deltaTime
            };

            JobHandle jobHandle = reallyTouthParallelJob.Schedule(_movingObjects.Count, 120);
            jobHandle.Complete();

            for (int i = 0; i < _movingObjects.Count; i++)
            {
                _movingObjects[i].transform.position = positionArrey[i];
                _movingObjects[i].moveSpeed = speedArray[i];
            }

            positionArrey.Dispose();
            speedArray.Dispose();
        }

        private void UseJobsTransform()
        {
            TransformAccessArray transformArrey = new TransformAccessArray(_movingObjects.Count);
            NativeArray<float> speedArray = new NativeArray<float>(_movingObjects.Count, Allocator.TempJob);

            for (int i = 0; i < _movingObjects.Count; i++)
            {
                transformArrey.Add(_movingObjects[i].transform);
                speedArray[i] = _movingObjects[i].moveSpeed;
            }

            ReallyTouthParallelJobTransforms reallyTouthParallelJob = new ReallyTouthParallelJobTransforms
            {
                speedArray = speedArray,
                deltaTime = Time.deltaTime
            };

            JobHandle jobHandle = reallyTouthParallelJob.Schedule(transformArrey);
            jobHandle.Complete();

            for (int i = 0; i < _movingObjects.Count; i++)
                _movingObjects[i].moveSpeed = speedArray[i];

            transformArrey.Dispose();
            speedArray.Dispose();
        }
    }
}

