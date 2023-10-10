using Unity.Collections;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Controls ECS systems and UI panel behavior via UI buttons
/// </summary>
public class SystemHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _gamePanel;

    [SerializeField]
    private GameObject _landingPanel;

    [SerializeField]
    private GameObject _gameOverPanel;

    private TargetSystem _targetSystem;
    private EntityManager _entityManager;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _targetSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<TargetSystem>();        
        _targetSystem.Enabled = false;
    }

    private void Update()
    {
        // Query existing Red units
        EntityQuery r_query = _entityManager.CreateEntityQuery(typeof(RedTag));
        NativeArray<Entity> r_entities = r_query.ToEntityArray(Allocator.TempJob);

        // Query existing Blue units
        EntityQuery b_query = _entityManager.CreateEntityQuery(typeof(BlueTag));
        NativeArray<Entity> b_entities = b_query.ToEntityArray(Allocator.TempJob);

        // Only check if game is over when TargetSystem is running
        if(World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<TargetSystem>().Enabled == true)
        {
            // Check if player won or lost
            if (b_entities.Length > 0 && r_entities.Length == 0)
            {
                Victory();
            } 
            else if (b_entities.Length == 0)
            {
                Defeat();
            }
        }

    }

    public void StartBattle()
    {
        // Update UI panels
        _gamePanel.SetActive(true);
        _landingPanel.SetActive(false);

        // Start targeting, movement and battle/damage
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
        _gameOverPanel.SetActive(false);
        _landingPanel.SetActive(true);

        // Stop targeting, movement and battle / damage
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

    /// <summary>
    /// Display Victory Screen
    /// </summary>
    private void Victory()
    {
        GameOver();

        // Enable Win text
        Transform childWinText = _gameOverPanel.transform.Find("WinText");
        GameObject gameObject = childWinText.gameObject;
        gameObject.SetActive(true);

    }

    /// <summary>
    /// Display Defeat Screen
    /// </summary>
    private void Defeat()
    {
        GameOver();

        // Enable Lose text
        Transform childLoseText = _gameOverPanel.transform.Find("LoseText");
        GameObject gameObject = childLoseText.gameObject;
        gameObject.SetActive(true);

    }

    /// <summary>
    /// Updates UI panels and systems when game ends
    /// </summary>
    private void GameOver()
    {
        // Update UI panels
        _gamePanel.SetActive(false);
        _gameOverPanel.SetActive(true);

        // Stop targeting, movement and battle / damage
        _targetSystem.Enabled = false;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<MovementSystem>().Enabled = false;
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<BattleSystem>().Enabled = false;
    }
}
