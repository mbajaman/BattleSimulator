using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

/* Generic Methods are not support!!! */

//TODO: This class needs to be cleaned up and optimized if possible
public partial class TargetSystem : SystemBase
{
    private List<float3> _blueTeamPositions = new List<float3>();
    private List<float3> _redTeamPositions = new List<float3>();
    protected override void OnUpdate()
    {
        Entities.WithName("Get_BlueTeam_Positions")
            .WithAll<BlueTeamTag>()
            .ForEach(
                (ref LocalTransform transform) =>
                {
                    _blueTeamPositions.Add(transform.Position);
                }
            ).WithoutBurst().Run();

        Entities.WithName("Get_RedTeam_Positions")
            .WithAll<RedTeamTag>()
            .ForEach(
                (ref LocalTransform transform) =>
                {
                    _redTeamPositions.Add(transform.Position);
                }
            ).WithoutBurst().Run();

        Entities
            .WithAll<BlueTeamTag>()
            .ForEach(
                (ref AttackProperties attackProperties) =>
                {
                    if (attackProperties.targetAcquired == true)
                    {
                        attackProperties.targetPosition = _redTeamPositions[attackProperties.targetLocationIndex];
                        return;
                    }

                    var random = UnityEngine.Random.Range(0, _redTeamPositions.Count);
                    attackProperties.targetPosition = _redTeamPositions[random];
                    attackProperties.targetAcquired = true;
                    attackProperties.targetLocationIndex = random;
                }
            ).WithoutBurst().Run();

        Entities
            .WithAll<RedTeamTag>()
            .ForEach(
                (ref AttackProperties attackProperties) =>
                {
                    if (attackProperties.targetAcquired == true)
                    {
                        attackProperties.targetPosition = _blueTeamPositions[attackProperties.targetLocationIndex];
                        return;
                    }

                    var random = UnityEngine.Random.Range(0, _blueTeamPositions.Count);
                    attackProperties.targetPosition = _blueTeamPositions[random];
                    attackProperties.targetAcquired = true;
                    attackProperties.targetLocationIndex = random;
                }
            ).WithoutBurst().Run();

        _blueTeamPositions.Clear();
        _redTeamPositions.Clear();
    }

}