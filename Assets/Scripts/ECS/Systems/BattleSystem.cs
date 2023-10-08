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

        foreach (var(transform, targetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<BlueTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, targetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange
                && targetComponent.ValueRO.targetAcquired == true
                && targetComponent.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    
                    ecb.SetComponent(targetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints);
                }

                if(SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb.DestroyEntity(targetComponent.ValueRO.targetUnit);
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

        foreach (var (transform, targetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<RedTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, targetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange 
                && targetComponent.ValueRO.targetAcquired == true
                && targetComponent.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    ecb2.SetComponent(targetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints);
                }

                if (SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb2.DestroyEntity(targetComponent.ValueRO.targetUnit);
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