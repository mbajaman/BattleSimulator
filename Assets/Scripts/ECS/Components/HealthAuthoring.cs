using UnityEngine;
using Unity.Entities;

public class HealthAuthoring : MonoBehaviour
{
    [SerializeField]
    private int _currentHealth;

    [SerializeField]
    private int _maxHealth;

    class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new HealthComponent
            {
                healthPoints = authoring._currentHealth,
                maxHealth = authoring._maxHealth,
            });
        }
    }
}

public struct HealthComponent : IComponentData
{
    public int healthPoints;
    public int maxHealth;
}