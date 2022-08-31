using Unity.Entities;

namespace ConvertGo
{
    [GenerateAuthoringComponent]
    public struct MoveDirection : IComponentData
    {
        public float value;
    }
}
