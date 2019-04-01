#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			EnemyDistanceSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

namespace YU.ECS
{
    public class EnemyDistanceSystem : JobComponentSystem
    {
        private ComponentGroup m_jobInput;
        private ComponentGroup m_homeGroup;

        protected override void OnCreateManager()
        {
            m_jobInput = GetComponentGroup(typeof(EnemyComponent));
            m_homeGroup = GetComponentGroup(typeof(HomeComponent));
        }

        [BurstCompile]
        struct JobProcess : IJobChunk
        {
            public ArchetypeChunkComponentType<EnemyComponent> enemyType;
            public ArchetypeChunkComponentType<HomeComponent> homeType;
            [DeallocateOnJobCompletion]
            public NativeArray<ArchetypeChunk> HomeEntityArchetypeChunkArray;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<EnemyComponent> _enemys = chunk.GetNativeArray(enemyType);

                var homeComponentDataArray = HomeEntityArchetypeChunkArray[0].GetNativeArray(homeType);

                for (int i = 0; i < chunk.Count; i++)
                {
                    var enemy = _enemys[i];
                    if (math.length(_enemys[i].position) < 40)
                    {
                        homeComponentDataArray[0] = new HomeComponent { health = homeComponentDataArray[0].health - 1 };
                        _enemys[i] = new EnemyComponent { health = 0, position = enemy.position, enemyType = enemy.enemyType, velocity = enemy.velocity };
                    }

                }
            }
        }


        //系统会每帧调用这个函数
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var homeDataArray = m_homeGroup.ToComponentDataArray<HomeComponent>(Allocator.TempJob);
            //UIManager.Instance.ChangeHomeHPText(homeDataArray[0].health);
            homeDataArray.Dispose();

            var homeType = GetArchetypeChunkComponentType<HomeComponent>(false);
            var enemyType = GetArchetypeChunkComponentType<EnemyComponent>(false);

            var homeEntityArray = m_homeGroup.CreateArchetypeChunkArray(Allocator.TempJob, out JobHandle homeCollectHandle);

            //初始化一个job
            var job = new JobProcess {
                enemyType = enemyType,
                homeType = homeType,
                HomeEntityArchetypeChunkArray = homeEntityArray
            }.Schedule(m_jobInput, JobHandle.CombineDependencies(inputDeps, homeCollectHandle));

            //开始job      
            return job;
        }
    }
}