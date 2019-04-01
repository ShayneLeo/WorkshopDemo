#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			MoveCubeSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

namespace YU.ECS.T2
{
    public class MoveCubeSystem : JobComponentSystem
    {
        [BurstCompile]
        struct JobProcess : IJobProcessComponentData<Position>
        {
            public Random random;

            public void Execute(ref Position position)
            {
                var pos = position.Value;

                position.Value = pos + random.NextFloat3(new float3(5,0,0));
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var random = new Random(1);

            var job = new JobProcess { random = random, };
 
            return job.Schedule(this, inputDeps);
        }
    }
}