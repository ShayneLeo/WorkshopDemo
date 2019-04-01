#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			HealthSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Rendering;
using Unity.Burst;
namespace YU.ECS
{
    public class HealthSystem : JobComponentSystem
    {
        public class DestroyEntityBarrier : EndFrameBarrier
        { }

        [Inject] DestroyEntityBarrier barrier;

        private static RenderMesh hurtRenderMesh;
        private static RenderMesh normalRenderMesh;

        //[BurstCompile]
        struct JobProcess : IJobProcessComponentDataWithEntity<HealthComponent>
        {
            public EntityCommandBuffer.Concurrent entityCommandBuffer;

            public void Execute(Entity entity, int index, ref HealthComponent health )
            {
                if (health.healthValue == 1 && health.currColor == 0)
                {
                    entityCommandBuffer.SetSharedComponent<RenderMesh>(index, entity, hurtRenderMesh);
                    health = new HealthComponent { healthValue = 1, currColor = 1 };
                }
                else if (health.healthValue == 2 && health.currColor == 1)
                {
                    entityCommandBuffer.SetSharedComponent<RenderMesh>(index, entity, normalRenderMesh);
                    health = new HealthComponent { healthValue = 2, currColor = 0 };
                }
                if (health.healthValue == 0)
                {
                    //entityCommandBuffer.DestroyEntity(index, entity);
                }

            }
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene()
        {
            var proto = GameObject.Find("HurtCubePrototype");
            hurtRenderMesh = proto.GetComponent<RenderMeshProxy>().Value;
            var proto2 = GameObject.Find("CubePrototype");
            normalRenderMesh = proto2.GetComponent<RenderMeshProxy>().Value;
            
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            //EntityCommandBuffer不支持burst，所以这里没有burst compiler
            EntityCommandBuffer.Concurrent entityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent();

            var job = new JobProcess {
                entityCommandBuffer = entityCommandBuffer
            };

            return job.Schedule(this, inputDeps);
        }
    }
}