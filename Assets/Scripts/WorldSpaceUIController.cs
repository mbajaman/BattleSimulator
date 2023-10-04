using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class WorldSpaceUIController : MonoBehaviour
{

    [SerializeField] private GameObject _damageIconPrefab;

    private Transform _mainCameraTransform;
    private EntityManager _entityManager;


    private void OnEnable()
    {
    }

    private void HealthDisplayUI(int health, float3 pos)
    {
        //Display current Health
    }

    private void ShowHealth()
    {
        //var curUnitHealth = _entityManager.GetComponentData<HealtPonits>;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
