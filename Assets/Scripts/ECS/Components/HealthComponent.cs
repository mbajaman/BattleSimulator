using Unity.Entities;

public struct HealthComponent : IComponentData
{
    public int healthPoints;
    public int maxHealth;
}
