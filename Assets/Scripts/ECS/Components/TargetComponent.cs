using Unity.Entities;
using Unity.Mathematics;

public struct TargetComponent : IComponentData
{
    public float3 targetPosition;
    public Entity targetUnit;
    public bool targetAcquired;
}
