using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/* Generic Methods are not support!!! */

//TODO: This class needs to be optimized by scheduling parallel jobs

/// <summary>
/// Handles targeting for each unit based on which team it is on.
/// </summary>
public partial class TargetSystem : SystemBase
{
    Unity.Mathematics.Random random;

    protected override void OnCreate()
    {
        base.OnCreate();

        // Initialize the random generator with a seed
        random = new Random((uint)UnityEngine.Random.Range(1, 10000));
    }

    protected override void OnUpdate()
    {
        // Make sure to use "random" as a parameter in the ForEach loop
        Random localRandom = random;

        // Create EntityQueries and NativeArray container for entities
        EntityQuery blueTeam;
        EntityQuery redTeam;
        NativeArray<Entity> redTeamEntityArray;
        NativeArray<Entity> blueTeamEntityArray;

        // Get all Entities with BlueTeamTag and update their target information if any
        blueTeam = GetEntityQuery(ComponentType.ReadOnly<BlueTeamTag>(), ComponentType.ReadOnly<LocalTransform>());
        if (!blueTeam.IsEmpty) 
        {
            blueTeamEntityArray = blueTeam.ToEntityArray(Allocator.TempJob); //Allocator needs to be a TempJob so it works inside Entities.ForEach()
            RedTeamTargetUpdate(blueTeamEntityArray, localRandom);
        }

        // Get all Entities with ReadTeamTag and update their target information if any
        redTeam = GetEntityQuery(ComponentType.ReadOnly<RedTeamTag>(), ComponentType.ReadOnly<LocalTransform>());
        if (!redTeam.IsEmpty)
        {
            redTeamEntityArray = redTeam.ToEntityArray(Allocator.TempJob);
            BlueTeamTargetUpdate(redTeamEntityArray, localRandom);
        }
    }

    /// <summary>
    /// Update all target information in all Entities with BlueTeamTag
    /// </summary>
    /// <param name="entities"></param>
    private void BlueTeamTargetUpdate(NativeArray<Entity> entities, Random localRandom)
    {
        if (entities.Length != 0)
        {
            Entities
                .WithAll<BlueTeamTag>()
                .ForEach(
                    (ref AttackProperties attackProperties) =>
                    {
                        var random = localRandom.NextInt(0, entities.Length);
                        if (attackProperties.targetAcquired == true)
                        {
                            attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(attackProperties.targetUnit).Position;
                            return;
                        }
                        attackProperties.targetUnit = entities[random];
                        attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(entities[random]).Position;
                        attackProperties.targetAcquired = true;
                    }
                ).Run();

            // Dispose of the Entitied Array
            entities.Dispose();
        }
    }

    /// <summary>
    /// Update all target information in all Entities with RedTeamTag
    /// </summary>
    /// <param name="entities" type="NativeArray"></param>
    private void RedTeamTargetUpdate(NativeArray<Entity> entities, Random localRandom)
    {
        if (entities.Length != 0)
        {
            Entities
                .WithAll<RedTeamTag>()
                .ForEach(
                    (ref AttackProperties attackProperties) =>
                    {
                        var random = localRandom.NextInt(0, entities.Length);
                        if (attackProperties.targetAcquired == true)
                        {
                            attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(attackProperties.targetUnit).Position;
                            return;
                        }
                        attackProperties.targetUnit = entities[random];
                        attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(entities[random]).Position;
                        attackProperties.targetAcquired = true;
                    }
                ).Run();

            // Dispose of the captured arrays and NativeArray when they are no longer needed
            entities.Dispose();
        }
    }

}