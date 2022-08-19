using Unity.Entities;
using UnityEngine;

namespace Sripts.ECS
{
    public partial class TestingECS
    {
        public class LevelUpSystem : ComponentSystem
        {
            protected override void OnUpdate()
            {
                Entities.ForEach(
                    (ref LevelComponent levelComponent) => 
                    {
                        levelComponent.level += 1f * Time.DeltaTime;
                        // Debug.Log(levelComponent + " = " + levelComponent.level);
                    });
            }
        }
    }
}
