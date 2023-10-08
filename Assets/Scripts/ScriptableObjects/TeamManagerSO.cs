using UnityEngine;

[CreateAssetMenu(fileName = "TeamManager", menuName = "ScriptableObjects/TeamManager",order = 1)]
public class TeamManagerSO : ScriptableObject
{
    public TeamSO[] teams;
}
