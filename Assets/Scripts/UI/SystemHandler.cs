using Unity.Collections;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Controls ECS systems behavior via UI buttons
/// </summary>
public class SystemHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject _gamePanel;

    [SerializeField]
    public GameObject _landingPanel;

    private TargetSystem _targetSystem;
    private EntityManager _entityManager;

    public void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _targetSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<TargetSystem>();        
        _targetSystem.Enabled = false;
    }

    public void StartBattle()
    {
        // Update UI panels
        _gamePanel.SetActive(true);
        _landingPanel.SetActive(false);

        // Enable TargetSystem
        _targetSystem.Enabled = true;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<MovementSystem>().Enabled = true;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<BattleSystem>().Enabled = true;

        // Disable TeamSpawnerSystem
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<TeamSpawnerSystem>().Enabled = false;
    }

    public void StopBattle()
    {
        // Update UI panels
        _gamePanel.SetActive(false);
        _landingPanel.SetActive(true);

        // Disable TargetSystem
        _targetSystem.Enabled = false;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<MovementSystem>().Enabled = false;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<BattleSystem>().Enabled = false;
        

        // Remove leftover Units
        EntityQuery query = _entityManager.CreateEntityQuery(typeof(HealthComponent));

        NativeArray<Entity> entities = query.ToEntityArray(Allocator.TempJob);

        foreach (Entity entity in entities)
        {
            _entityManager.DestroyEntity(entity);
        }

        // Spawn teams
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<TeamSpawnerSystem>().Enabled = true;

    }
}
