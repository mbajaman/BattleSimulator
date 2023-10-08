using UnityEngine;
using Unity.Entities;

public class RedTagAuthoring : MonoBehaviour
{
    // Baking components for when GameObject is converted into Entities
    class Baker : Baker<RedTagAuthoring>
    {
        public override void Bake(RedTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<RedTag>(entity);
        }
    }
}
