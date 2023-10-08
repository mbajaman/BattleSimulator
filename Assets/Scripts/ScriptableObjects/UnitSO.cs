using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/Unit")]
public class UnitSO : ScriptableObject
{
    public int index;
    public Vector3 position;

    public int attackDamage;
    public int attackSpeed;
    public int attackRange;

    public int maxHealth;
    public int moveSpeed;

}
