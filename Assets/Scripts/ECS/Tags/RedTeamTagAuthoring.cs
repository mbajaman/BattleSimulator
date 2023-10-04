using UnityEngine;
using Unity.Entities;

public class RedTeamTagAuthoring : MonoBehaviour
{
    class Baker : Baker<RedTeamTagAuthoring>
    {
        public override void Bake(RedTeamTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<RedTeamTag>(entity);
        }
    }
}
public struct RedTeamTag : IComponentData
{
    // Empty component used for tagging
}
