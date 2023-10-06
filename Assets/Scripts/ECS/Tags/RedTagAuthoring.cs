using UnityEngine;
using Unity.Entities;

public class RedTagAuthoring : MonoBehaviour
{
    class Baker : Baker<RedTagAuthoring>
    {
        public override void Bake(RedTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<RedTag>(entity);
        }
    }
}
public struct RedTag : IComponentData
{
    // Empty component used for tagging
}
