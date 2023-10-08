using UnityEngine;
using Unity.Entities;

public class BlueTagAuthoring : MonoBehaviour
{

    // Baking components for when GameObject is converted into Entities
    class Baker : Baker<BlueTagAuthoring>
    {
        public override void Bake(BlueTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<BlueTag>(entity);
        }
    }
}

