using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Team", menuName = "ScriptableObjects/Team")]
public class TeamSO : ScriptableObject
{
    public UnitSO[] units;
}
