using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


//TODO: This is highly inefficient and not clean code, need to separate into different systems or Jobs
//TODO: Use IJobEntity to clean the below foreach idomatic expressions
[UpdateAfter(typeof(TargetSystem))]
public partial struct BattleSystem : ISystem
{
    private float timer;

    public void OnUpdate(ref SystemState state)
    {
        float calculationInterval = 1.0f;
        float deltaTime = Time.deltaTime;
        // Update the timer every frame
        timer += deltaTime;

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged);

        foreach (var(transform, TargetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<BlueTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, TargetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange
                && TargetComponent.ValueRO.targetAcquired == true
                && TargetComponent.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    
                    ecb.SetComponent(TargetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints);
                }

                if(SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb.DestroyEntity(TargetComponent.ValueRO.targetUnit);
                    ecb.SetComponent(entity, new TargetComponent()
                    {
                        targetPosition = transform.ValueRO.Position,
                        targetUnit = Entity.Null,
                        targetAcquired = false,
                    });
                }
            }
        }

        EntityCommandBuffer ecb2 = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged);

        foreach (var (transform, TargetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<RedTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, TargetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange 
                && TargetComponent.ValueRO.targetAcquired == true
                && TargetComponent.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    ecb2.SetComponent(TargetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints);
                }

                if (SystemAPI.GetComponent<HealthComponent>(TargetComponent.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb2.DestroyEntity(TargetComponent.ValueRO.targetUnit);
                    ecb2.SetComponent(entity, new TargetComponent()
                    {
                        targetPosition = transform.ValueRO.Position,
                        targetUnit = Entity.Null,
                        targetAcquired = false,
                    });
                }
            }
        }

        if (timer >= calculationInterval)
            timer = 0.0f;
    }
}