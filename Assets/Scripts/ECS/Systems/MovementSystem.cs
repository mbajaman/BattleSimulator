using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct MovementSystem : ISystem
{
    //private List<Entity> _enemyEntities;

    public void OnCreate(ref SystemState state)
    {
        foreach (var (transform, entity) in 
            SystemAPI.Query<RefRW<BlueTeamTag>>()
            .WithEntityAccess())
        {
            //_enemyEntities.Add(entity);
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (transform, moveSpeed, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<MoveSpeedComponent>>()
            .WithAll<BlueTeamTag>()
            .WithEntityAccess())
        {
            var dir = float3.zero;

            transform.ValueRW.Position += new float3(1,0,0) * dt * moveSpeed.ValueRO.moveSpeed;
        }
    }

    //private Entity RandomEnemy()
    //{
    //    int randomIndex = UnityEngine.Random.Range(0, _enemyEntities.Count);
    //    Entity enemyEntity = _enemyEntities[randomIndex];

    //    return enemyEntity;
    //}
}
