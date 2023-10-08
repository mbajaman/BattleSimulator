using Unity.Entities;
using UnityEngine;

public class EntityPrefabAuthoring : MonoBehaviour
{

    // Add prefabs here to bake as prefab Entities
    public GameObject blueUnitPrefab;
    public GameObject redUnitPrefab;

    class Baker : Baker<EntityPrefabAuthoring>
    {
        public override void Bake(EntityPrefabAuthoring authoring)
        {
            // Get Entities of the prefabs
            var blueUnitEntityPrefab = GetEntity(authoring.blueUnitPrefab, TransformUsageFlags.None);
            var redUnitEntityPrefab = GetEntity(authoring.redUnitPrefab, TransformUsageFlags.None);

            // Add a EntityPrefabComponent which contains the prefab Entities from above
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EntityPrefabComponent()
            {
                BlueUnit = blueUnitEntityPrefab,
                RedUnit = redUnitEntityPrefab
            });

        }
    }
}

