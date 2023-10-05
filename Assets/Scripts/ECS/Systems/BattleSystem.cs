using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


//TODO: This is highly inefficient and not clean code, need to separate into different systems or Jobs
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

        foreach (var(transform, attackProperties, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<AttackProperties>, RefRO<AttackComponent>>()
            .WithAll<BlueTeamTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, attackProperties.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange
                && attackProperties.ValueRO.targetAcquired == true
                && attackProperties.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    
                    ecb.SetComponent(attackProperties.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints);
                }

                if(SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb.DestroyEntity(attackProperties.ValueRO.targetUnit);
                    ecb.SetComponent(entity, new AttackProperties()
                    {
                        targetPosition = transform.ValueRO.Position,
                        targetUnit = Entity.Null,
                        targetAcquired = false,
                    });
                }
            }
        }

        EntityCommandBuffer ecb2 = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged);

        foreach (var (transform, attackProperties, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<AttackProperties>, RefRO<AttackComponent>>()
            .WithAll<RedTeamTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, attackProperties.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange 
                && attackProperties.ValueRO.targetAcquired == true
                && attackProperties.ValueRO.targetUnit != Entity.Null)
            {
                if (timer >= calculationInterval)
                {
                    ecb2.SetComponent(attackProperties.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Source: " + entity.Index + "\nEnemy Health: " + SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints);
                }

                if (SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints <= 0)
                {
                    ecb2.DestroyEntity(attackProperties.ValueRO.targetUnit);
                    ecb2.SetComponent(entity, new AttackProperties()
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