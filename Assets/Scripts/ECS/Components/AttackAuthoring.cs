using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class AttackAuthoring : MonoBehaviour
{
    [SerializeField]
    private int _attackDamage;

    [SerializeField]
    private float _attackSpeed;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private float3 _targetPosition;

    [SerializeField]
    private bool _targetAcquired;

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

            AddComponent(entity, new AttackProperties
            {
                targetPosition = authoring._targetPosition,
                targetAcquired = authoring._targetAcquired,
                targetLocationIndex = -1
            });
        }
    }
}

public struct AttackComponent : IComponentData
{
    public int attackDamage;
    public float attackSpeed;
    public float attackRange;
}

public struct AttackProperties : IComponentData
{
    public float3 targetPosition;
    public bool targetAcquired;
    public int targetLocationIndex;
}