using Unity.Entities;

public struct AttackComponent : IComponentData
{
    public int attackDamage;
    public int attackSpeed;
    public float attackRange;
}
