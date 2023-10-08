using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[CreateBefore(typeof(TargetSystem))]
public partial class TeamSpawnerSystem : SystemBase
{
    private TeamManagerSO blueTeamManagerSO;
    private TeamManagerSO redTeamManagerSO;

    protected override void OnCreate()
    {
        blueTeamManagerSO = Resources.Load<TeamManagerSO>("BlueTeam/BlueTeamManager");
        redTeamManagerSO = Resources.Load<TeamManagerSO>("RedTeam/RedTeamManager");
    }

    protected override void OnStartRunning()
    {
        var blueTeamUnits = blueTeamManagerSO.teams[0].units;
        var redTeamUnits = redTeamManagerSO.teams[0].units;

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        var PrefabAuthoringEntity = SystemAPI.GetSingleton<EntityPrefabComponent>();

        //TODO: This can probably be simplified into a function so there is less repeating code
        // Spawn Blue team Units
        for (int i = 0; i < blueTeamUnits.Length; i++)
        {
            var blueUnitEntity = ecb.Instantiate(PrefabAuthoringEntity.BlueUnit);

            // Set Unit LocalTransform
            ecb.SetComponent(blueUnitEntity, new LocalTransform
            {
                Position = blueTeamUnits[i].position,
                Scale = 0.5f
            });

            // Set Unit HealthComponent
            ecb.SetComponent(blueUnitEntity, new HealthComponent
            {
                healthPoints = blueTeamUnits[i].maxHealth,
                maxHealth = blueTeamUnits[i].maxHealth
            });

            // Set Unit MoveSpeedComponent
            ecb.SetComponent(blueUnitEntity, new MoveSpeedComponent
            {
                moveSpeed = blueTeamUnits[i].moveSpeed
            });

            // Set Unit AttackComponent
            ecb.SetComponent(blueUnitEntity, new AttackComponent
            {
                attackDamage = blueTeamUnits[i].attackDamage,
                attackRange = blueTeamUnits[i].attackRange,
                attackSpeed = blueTeamUnits[i].attackSpeed
            });
        }

        // Spawn Red team units
        for (int i = 0; i < redTeamUnits.Length; i++)
        {
            var redUnitEntity = ecb.Instantiate(PrefabAuthoringEntity.RedUnit);

            // Set Unit LocalTransform
            ecb.SetComponent(redUnitEntity, new LocalTransform
            {
                Position = redTeamUnits[i].position,
                Scale = 0.5f
            });

            // Set Unit HealthComponent
            ecb.SetComponent(redUnitEntity, new HealthComponent
            {
                healthPoints = redTeamUnits[i].maxHealth,
                maxHealth = redTeamUnits[i].maxHealth
            });

            // Set Unit MoveSpeedComponent
            ecb.SetComponent(redUnitEntity, new MoveSpeedComponent
            {
                moveSpeed = redTeamUnits[i].moveSpeed
            });

            // Set Unit AttackComponent
            ecb.SetComponent(redUnitEntity, new AttackComponent
            {
                attackDamage = redTeamUnits[i].attackDamage,
                attackRange = redTeamUnits[i].attackRange,
                attackSpeed = redTeamUnits[i].attackSpeed
            });
        }

    }

    protected override void OnUpdate()
    {
        
    }

}
