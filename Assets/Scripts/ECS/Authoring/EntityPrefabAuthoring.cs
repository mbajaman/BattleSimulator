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
            var blueUnitPrefabEntity = GetEntity(authoring.blueUnitPrefab, TransformUsageFlags.None);
            var redUnitPrefabEntity = GetEntity(authoring.redUnitPrefab, TransformUsageFlags.None);

            // Add a EntityPrefabComponent which contains the prefab Entities from above
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EntityPrefabComponent()
            {
                BlueUnit = blueUnitPrefabEntity,
                RedUnit = redUnitPrefabEntity
            });

        }
    }
}

