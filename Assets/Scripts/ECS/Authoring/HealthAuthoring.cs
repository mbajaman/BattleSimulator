using UnityEngine;
using Unity.Entities;

public class HealthAuthoring : MonoBehaviour
{
    [SerializeField]
    public int _currentHealth;

    [SerializeField]
    public int _maxHealth;

    // Baking components for when GameObject is converted into Entities
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
