using UnityEngine;
using Unity.Entities;

public class BlueTagAuthoring : MonoBehaviour
{
    class Baker : Baker<BlueTagAuthoring>
    {
        public override void Bake(BlueTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<BlueTag>(entity);
        }
    }
}
public struct BlueTag : IComponentData
{
    // Empty component used for tagging
}
