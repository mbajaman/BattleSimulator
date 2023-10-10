using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class RedTeamFormationHandler : MonoBehaviour
{
    private TeamSO _redTeam;

    // Start is called before the first frame update
    public void LoadTeam()
    {
        var teamName = gameObject.name;
        _redTeam = Resources.Load<TeamSO>("RedTeam/Teams/" + teamName);

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery query = entityManager.CreateEntityQuery(typeof(RedTag));

        NativeArray<Entity> entities = query.ToEntityArray(Allocator.TempJob);

        int idx = 0;
        foreach (Entity entity in entities)
        {
            Debug.Log("Entity ID: " + entity.Index);
            entityManager.SetComponentData(entity, new LocalTransform
            {
                Position = _redTeam.units[idx].position,
                Scale = 0.5f
            });
            idx++;
        }

    }

}
