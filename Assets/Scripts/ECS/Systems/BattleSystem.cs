using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Controls damaging enemy units and resetting target component data on landing the killing blow
/// </summary>
//TODO: This is highly inefficient and not clean code, need to separate into different systems or Jobs
//TODO: Use IJobEntity to clean the below foreach idomatic expressions
//TODO: Implement IJobEntity to be able to use Parallel ECB to schedule jobs in parallel
//TODO: Event based system on when a target is dealt the killing blow (?)
[UpdateAfter(typeof(TargetSystem))]
public partial struct BattleSystem : ISystem
{
    // Timer for handling how frequently damage is dealt
    private float _timer;
    private float _calculationInterval;

    public void OnCreate(ref SystemState state)
    {
        _calculationInterval = 1.0f;
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = Time.deltaTime;
        _timer += deltaTime;

        // Setup Command Buffer
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged);

        // Query all Blue Units and deal damage to target if in range
        foreach (var(transform, targetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<BlueTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, targetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange
                && targetComponent.ValueRO.targetAcquired == true
                && state.EntityManager.Exists(targetComponent.ValueRO.targetUnit))
            {
                if (_timer >= _calculationInterval)
                {

                    ecb.SetComponent(targetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });
                }

                // Reset target information
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

        // Query all Red Units and deal damage to target if in range
        foreach (var (transform, targetComponent, attackComponent, entity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<TargetComponent>, RefRO<AttackComponent>>()
            .WithAll<RedTag>()
            .WithEntityAccess())
        {
            if (math.distance(transform.ValueRO.Position, targetComponent.ValueRO.targetPosition) < attackComponent.ValueRO.attackRange 
                && targetComponent.ValueRO.targetAcquired == true
                && state.EntityManager.Exists(targetComponent.ValueRO.targetUnit))
            {
                if (_timer >= _calculationInterval)
                {
                    ecb.SetComponent(targetComponent.ValueRO.targetUnit, new HealthComponent()
                    {
                        healthPoints = SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints - (attackComponent.ValueRO.attackDamage * attackComponent.ValueRO.attackSpeed)
                    });
                }

                // Reset target information
                if (SystemAPI.GetComponent<HealthComponent>(targetComponent.ValueRO.targetUnit).healthPoints <= 0)
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

        // Reset after 1 second has elapsed
        if (_timer >= _calculationInterval)
            _timer = 0.0f;
    }
}