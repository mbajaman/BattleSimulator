using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(TargetSystem))]
public partial struct BattleSystem : ISystem
{
    private float timer;

    public void OnCreate(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        float calculationInterval = 1.0f;
        float deltaTime = Time.deltaTime;
        // Update the timer every fram
        timer += deltaTime;

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged);

        foreach (var(transform, attackProperties, attackComponent, healthComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<AttackProperties>, RefRO<AttackComponent>, RefRW<HealthComponent>>()
            .WithAll<BlueTeamTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, attackProperties.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange && attackProperties.ValueRO.targetAcquired == true)
            {
                if (timer >= calculationInterval)
                {
                    timer = 0.0f;
                    ecb.SetComponent(attackProperties.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });

                    Debug.Log("Enemy Health: " + SystemAPI.GetComponent<HealthComponent>(attackProperties.ValueRO.targetUnit).healthPoints);
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
    }
}