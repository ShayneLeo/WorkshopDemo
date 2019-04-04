using UnityEngine;
using Unity.Entities;

public class SystemControl : MonoBehaviour
{
    
    private void Awake()
    {
        World.Active.GetExistingManager<YU.ECS.T1.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T2.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T2.MoveCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T3.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T3.MoveCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T4.GenerateCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T4.MoveCubeSystem>().Enabled = false;
        World.Active.GetExistingManager<YU.ECS.T4.HealthSystem>().Enabled = false;
    }

}
