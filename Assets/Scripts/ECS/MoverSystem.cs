using Unity.Entities;
using Unity.Transforms;

namespace Sripts.ECS
{
    public class MoverSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach(
                    ( ref Translation translation, ref MoveSpeedComponent moveSpeedCom ) =>
                    {
                        translation.Value.y += moveSpeedCom.moveSpeed * Time.DeltaTime;
                        if (translation.Value.y > 5f || translation.Value.y < -5f)
                            moveSpeedCom.moveSpeed *= -1;
                    }
                );
        }
    }
}
