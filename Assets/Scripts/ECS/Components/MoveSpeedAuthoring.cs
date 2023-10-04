using UnityEngine;
using Unity.Entities;

public class MoveSpeedAuthoring : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    class Baker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MoveSpeedComponent
            {
                moveSpeed = authoring._moveSpeed,
            });
        }
    }
}

public struct MoveSpeedComponent : IComponentData
{
    public float moveSpeed;
}