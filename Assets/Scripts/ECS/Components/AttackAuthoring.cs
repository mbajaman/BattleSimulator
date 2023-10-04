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
        }
    }
}

public struct AttackProperties : IComponentData
{
    public float3 TargetPosition;
    public Entity TargetEnemy;
    public Entity OriginCharacter;
}

public struct AttackComponent : IComponentData
{
    public int attackDamage;
    public float attackSpeed;
    public float attackRange;
}