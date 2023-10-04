using UnityEngine;
using Unity.Entities;

public class BlueTeamTagAuthoring : MonoBehaviour
{
    class Baker : Baker<BlueTeamTagAuthoring>
    {
        public override void Bake(BlueTeamTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<BlueTeamTag>(entity);
        }
    }
}
public struct BlueTeamTag : IComponentData
{
    // Empty component used for tagging
}
