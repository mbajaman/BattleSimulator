using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class AttackAuthoring : MonoBehaviour
{
    [SerializeField]
    private int _attackDamage;

    [SerializeField]
    private int _attackSpeed;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private float3 _targetPosition;

    [SerializeField]
    private bool _targetAcquired;

    [SerializeField]
    private int unitIndex;

    // Baking components for when GameObject is converted into Entities
    class Baker : Baker<AttackAuthoring>
    {
        public override void Bake(AttackAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new AttackComponent
            {
                attackDamage = authoring._attackDamage,
                attackSpeed = authoring._attackSpeed,
                attackRange = authoring._attackRange
            });

            AddComponent(entity, new TargetComponent
            {
                targetPosition = authoring._targetPosition,
                targetAcquired = authoring._targetAcquired,
            });
        }
    }
}

