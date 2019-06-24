using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;
public class SystemControl : MonoBehaviour
{
    
    private void Awake()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Tutorial01":
                World.Active.GetExistingManager<YU.ECS.T2.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T2.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.HealthSystem>().Enabled = false;
                break;
            case "Tutorial02":
                World.Active.GetExistingManager<YU.ECS.T1.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.HealthSystem>().Enabled = false;
                break;
            case "Tutorial03":
                World.Active.GetExistingManager<YU.ECS.T1.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T2.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T2.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T4.HealthSystem>().Enabled = false;
                break;
            case "Tutorial04":
                World.Active.GetExistingManager<YU.ECS.T1.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T2.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T2.MoveCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.GenerateCubeSystem>().Enabled = false;
                World.Active.GetExistingManager<YU.ECS.T3.MoveCubeSystem>().Enabled = false;
                break;
        }
    }

}
