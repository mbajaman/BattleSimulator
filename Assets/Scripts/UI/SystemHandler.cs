using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Controls ECS systems behavior via UI buttons
/// </summary>
public class SystemHandler : MonoBehaviour
{
    private TargetSystem _targetSystem;

    [SerializeField]
    public GameObject _gamePanel;

    [SerializeField]
    public GameObject _landingPanel;

    public void Start()
    {
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
    }
}
