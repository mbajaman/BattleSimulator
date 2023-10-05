
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(TargetSystem))]
[BurstCompile]
public partial struct MovementSystem : ISystem
{
    

    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (transform, moveComponent, attackProperties, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<MoveSpeedComponent>, RefRO<AttackProperties>, RefRO<AttackComponent>>()
            .WithAll<BlueTeamTag>()
            .WithEntityAccess())
        {
            transform.ValueRW.Position = MoveTowards(
                transform.ValueRW.Position,
                attackProperties.ValueRO.targetPosition,
                dt * moveComponent.ValueRO.moveSpeed,
                attackComponent.ValueRO.attackRange
            );
        }

        foreach (var (transform, moveComponent, attackProperties, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<MoveSpeedComponent>, RefRO<AttackProperties>, RefRO<AttackComponent>>()
            .WithAll<RedTeamTag>()
            .WithEntityAccess())
        {
            transform.ValueRW.Position = MoveTowards(
                transform.ValueRW.Position, 
                attackProperties.ValueRO.targetPosition, 
                dt * moveComponent.ValueRO.moveSpeed,
                attackComponent.ValueRO.attackRange
                );
        }
    }

    public static float3 MoveTowards(float3 current, float3 target, float step, float range)
    {
        float deltaX = target.x - current.x;
        float deltaY = target.y - current.y;
        float deltaZ = target.z - current.z;

        float sqdist = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;

        if (sqdist == 0 || sqdist <= range - 0.5f)
        {
            return current;
        }

        var dist = (float)System.Math.Sqrt(sqdist);

        return new float3(
            current.x + deltaX / dist * step,
            current.y + deltaY / dist * step,
            current.z + deltaZ / dist * step
            );
    }
}
