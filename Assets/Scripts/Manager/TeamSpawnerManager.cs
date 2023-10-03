using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Collections;

public class TeamSpawnerManager : MonoBehaviour
{
    [SerializeField] private EntityManager _entityManager;

    [SerializeField] private GameObject _blueUnitPrefab;

    [SerializeField] private GameObject _redUnitPrefab;

    private Entity _unitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
