using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Handles adding entities to a list which is then read by WorldSpaceUIScript to display unit health.
/// </summary>
public partial class HealthDisplaySystem : SystemBase
{
    private List<(int health, float3 position)> _entitiesToDisplayHealth = new List<(int health, float3 position)>();

    protected override void OnUpdate()
    {
        // Clear the list
        _entitiesToDisplayHealth.Clear();

        foreach(var(healthPoints, position, entity) in
            SystemAPI.Query<RefRO<HealthComponent>, RefRO<LocalTransform>>()
            .WithEntityAccess())
        {
            _entitiesToDisplayHealth.Add((healthPoints.ValueRO.healthPoints, position.ValueRO.Position));
        }
    }

    // ReadOnlyList of entities with health and position components
    public IReadOnlyList<(int health, float3 position)> GetEntitiesListDisplayHealth()
    {
        return _entitiesToDisplayHealth.AsReadOnly();
    }

}
