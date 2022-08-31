using Unity.Entities;
using Unity.Transforms;

namespace ConvertGo
{
    [AlwaysSynchronizeSystem]
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Entities.ForEach(
                            (ref Translation translation, in MoveDirection moveDirection) =>
                            translation.Value.x += moveDirection.value * deltaTime
                        )
                .Run();
                // .Schedule();
        }
    }
}
