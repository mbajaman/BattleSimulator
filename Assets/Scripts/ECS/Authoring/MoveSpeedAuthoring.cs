using UnityEngine;
using Unity.Entities;

public class MoveSpeedAuthoring : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    // Baking components for when GameObject is converted into Entities
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