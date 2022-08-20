using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine.Rendering;

namespace Sripts.ECS
{
    public class StartECS : MonoBehaviour
    {
        [SerializeField]
        Mesh _mesh;
        [SerializeField]
        Material _material;

        private void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Entity entity = entityManager.CreateEntity(typeof(LevelComponent));

            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(LevelComponent),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(MoveSpeedComponent)
                );
            // Entity entity = entityManager.CreateEntity(entityArchetype);

            NativeArray<Entity> entityArray = new NativeArray<Entity>(10000, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, entityArray);

            // entityManager.SetComponentData(entity, new LevelComponent() { level = 10 });

            //_mesh.bounds.ToAABB();

            for (int i = 0; i < entityArray.Length; i++)
            {
                Entity entity = entityArray[i];
                entityManager.SetComponentData(entity, new LevelComponent { level = UnityEngine.Random.Range(10, 20) });
                entityManager.SetComponentData(entity, new MoveSpeedComponent { moveSpeed = UnityEngine.Random.Range(1, 3) });
                entityManager.SetComponentData(entity, new Translation {
                    Value = new float3(UnityEngine.Random.Range(-7.9f, 7.9f), UnityEngine.Random.Range(-4.9f, 4.9f), 0)
                });

                RenderMeshUtility.AddComponents(entity, entityManager, new RenderMeshDescription(_mesh, _material));
            }

            entityArray.Dispose();

            // Render();
        }

        public void Render()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            RenderMeshDescription desc = new RenderMeshDescription(
                _mesh,
                _material,
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: false);

            Entity entity = entityManager.CreateEntity();
            RenderMeshUtility.AddComponents(
                entity,
                entityManager,
                desc);

            entityManager.AddComponentData(entity, new MaterialColor());

            float4x4 transform = float4x4.TRS(
                new float3(0, 0, 0),
                quaternion.identity,
                new float3(4));

            entityManager.AddComponentData(entity, new LocalToWorld { Value = transform });
        }
    }
}
