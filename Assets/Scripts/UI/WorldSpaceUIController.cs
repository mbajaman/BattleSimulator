using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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
