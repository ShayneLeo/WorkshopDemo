#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			HealthSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;

namespace YU.ECS.T4
{
    public class HealthSystem : JobComponentSystem
    {
        public class DestroyEntityBarrier : EndFrameBarrier
        { }

        [Inject] DestroyEntityBarrier barrier;

        //EntityCommandBuffer用burst有一个已知bug，不能用来add或set componentdata，但是DestroyEntity可以。
        [BurstCompile]
        struct JobProcess : IJobProcessComponentDataWithEntity<Position>
        {
            public EntityCommandBuffer.Concurrent entityCommandBuffer;

            public void Execute(Entity entity, int index, ref Position cubePos )
            {
                if (cubePos.Value.x >= 0)
                {
                    entityCommandBuffer.DestroyEntity(index, entity);
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            //Concurrent可以让ECB在job中使用。
            EntityCommandBuffer.Concurrent entityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent();

            var job = new JobProcess {
                entityCommandBuffer = entityCommandBuffer
            };

            return job.Schedule(this, inputDeps);
        }
    }
}