using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

/* Generic Methods are not support!!! */

//TODO: This class needs to be optimized by scheduling parallel jobs
public partial class TargetSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityQuery blueTeam;
        EntityQuery redTeam;
        NativeArray<Entity> redTeamEntityArray;
        NativeArray<Entity> blueTeamEntityArray;
        NativeArray<LocalTransform> redTeamLocalTransformArray;
        NativeArray<LocalTransform> blueTeamLocalTransformArray;

        blueTeam = GetEntityQuery(ComponentType.ReadOnly<BlueTeamTag>(), ComponentType.ReadOnly<LocalTransform>());
        if (!blueTeam.IsEmpty) 
        {
            blueTeamEntityArray = blueTeam.ToEntityArray(Allocator.TempJob);
            blueTeamLocalTransformArray = blueTeam.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            RedTeamTargetUpdate(blueTeamEntityArray, blueTeamLocalTransformArray);
        }

        redTeam = GetEntityQuery(ComponentType.ReadOnly<RedTeamTag>(), ComponentType.ReadOnly<LocalTransform>());
        if (!redTeam.IsEmpty)
        {
            redTeamEntityArray = redTeam.ToEntityArray(Allocator.TempJob);
            redTeamLocalTransformArray = redTeam.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            BlueTeamTargetUpdate(redTeamEntityArray, redTeamLocalTransformArray);
        }
    }

    private void BlueTeamTargetUpdate(NativeArray<Entity> entities, NativeArray<LocalTransform> localTransformArray)
    {
        var random = UnityEngine.Random.Range(0, entities.Length);

        if (entities.Length != 0)
        {
            Entities
                .WithAll<BlueTeamTag>()
                .ForEach(
                    (ref AttackProperties attackProperties) =>
                    {
                        if (attackProperties.targetAcquired == true)
                        {
                            attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(attackProperties.targetUnit).Position;
                            return;
                        }
                        attackProperties.targetUnit = entities[random];
                        attackProperties.targetPosition = localTransformArray[random].Position;
                        attackProperties.targetAcquired = true;
                    }
                ).Run();

            // Dispose of the captured arrays and NativeArray when they are no longer needed
            entities.Dispose();
            localTransformArray.Dispose();
        }
    }

    private void RedTeamTargetUpdate(NativeArray<Entity> entities, NativeArray<LocalTransform> localTransformArray)
    {
        var random = UnityEngine.Random.Range(0, entities.Length);

        if (entities.Length != 0)
        {
            Entities
                .WithAll<RedTeamTag>()
                .ForEach(
                    (ref AttackProperties attackProperties) =>
                    {
                        if (attackProperties.targetAcquired == true)
                        {
                            attackProperties.targetPosition = SystemAPI.GetComponent<LocalTransform>(attackProperties.targetUnit).Position;
                            return;
                        }
                        attackProperties.targetUnit = entities[random];
                        attackProperties.targetPosition = localTransformArray[random].Position;
                        attackProperties.targetAcquired = true;
                    }
                ).Run();

            // Dispose of the captured arrays and NativeArray when they are no longer needed
            entities.Dispose();
            localTransformArray.Dispose();
        }
    }

}