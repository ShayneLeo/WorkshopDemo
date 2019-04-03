using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

using YU.ECS;

public class SystemControl : MonoBehaviour
{
    
    private void Awake()
    {
        World.Active.GetExistingManager<DistanceSystem>().Enabled = false;
        World.Active.GetExistingManager<EnemyDistanceSystem>().Enabled = false;
        World.Active.GetExistingManager<EnemyHealthSystem>().Enabled = false;
        World.Active.GetExistingManager<GameControlSystem>().Enabled = false;
        World.Active.GetExistingManager<GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<HealthSystem>().Enabled = false;
        World.Active.GetExistingManager<HomeManagerSystem>().Enabled = false;
        World.Active.GetExistingManager<LaserHealthSystem>().Enabled = false;
        World.Active.GetExistingManager<MoveCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<MoveEnemySystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T1.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T2.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T2.MoveCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T3.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T3.MoveCubeSystem>().Enabled = false;
    }

}
