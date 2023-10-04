using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.UIElements;

public partial class BattleSystem : SystemBase
{
    private List<Entity> _enemyList;
    protected override void OnUpdate()
    {
        //Entities
        //    .WithName("Battle_Enemies")
        //    .WithAll<BlueTeamTag>()
        //    .ForEach(
        //        (ref Position position, ref AttackProperties attackProperties) =>
        //        {
                    
        //            position = new Position()
        //            {
        //                //
        //            }
        //        }
        //     ).Run();
    }
}