using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldSpaceUIController : MonoBehaviour
{

    [SerializeField] private GameObject _currentHealthPrefab;
    [SerializeField] private Vector3 healthDisplayOffset;
    private Transform _mainCameraTransform;
    private List<GameObject> _instantiatedHealthUI = new List<GameObject>();
    HealthDisplaySystem healthDisplaySystem;

    // Start is called before the first frame update
    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;
        healthDisplaySystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<HealthDisplaySystem>();
    }

    private void OnEnable()
    {
        
        // healthDisplaySystem.HealthDisplay += DisplayHealth;
    }

    private void OnDisable()
    {
        if (World.DefaultGameObjectInjectionWorld == null) return;
    }

    private void DisplayHealth(int currentHealth, float3 position)
    {
        var directionToCamera = (Vector3) position - _mainCameraTransform.position;
        var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
        var newDisplay = Instantiate(_currentHealthPrefab, position, rotationToCamera, transform);
        var newDisplayText = newDisplay.GetComponent<TextMeshProUGUI>();
        newDisplayText.text = currentHealth.ToString();
        Destroy(newDisplay);
    }

    private void Update()
    {
        IReadOnlyList<(int health, float3 position)> entitiesToDisplay = healthDisplaySystem.GetEntitiesListDisplayHealth();

        foreach(var UIElement in _instantiatedHealthUI)
        {
            Destroy(UIElement);
        }

        _instantiatedHealthUI.Clear();

        foreach(var entity in entitiesToDisplay)
        {
            var directionToCamera = (Vector3)entity.position - _mainCameraTransform.position;
            var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
            GameObject healthUI = Instantiate(_currentHealthPrefab, entity.position, rotationToCamera, transform);
            healthUI.transform.position += healthDisplayOffset;
            healthUI.GetComponent<TextMeshProUGUI>().text = entity.health.ToString();
            _instantiatedHealthUI.Add(healthUI);
        }
    }
}
