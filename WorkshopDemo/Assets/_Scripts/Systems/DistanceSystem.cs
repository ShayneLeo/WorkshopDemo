#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			DistanceSystem
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
    public class DistanceSystem : JobComponentSystem
    {
        private ComponentGroup m_jobInput;

        private ComponentGroup m_enemyGroup;
        private ComponentGroup m_laserGroup;
        private ComponentGroup m_cubeGroup;

        protected override void OnCreateManager()
        {
            m_jobInput = GetComponentGroup(ComponentType.ReadOnly<Position>(), typeof(CubeComponent) , typeof(HealthComponent));

            m_enemyGroup = GetComponentGroup(typeof(EnemyComponent));
            m_laserGroup = GetComponentGroup(typeof(LaserComponent));
            m_cubeGroup = GetComponentGroup(typeof(CubeComponent));
        }

        [BurstCompile]
        struct JobProcess : IJobChunk
        {
            [ReadOnly] public ArchetypeChunkComponentType<Position> PositionType;
            public ArchetypeChunkComponentType<EnemyComponent> enemyType;
            [ReadOnly]public ArchetypeChunkComponentType<LaserComponent> laserType;
            public ArchetypeChunkComponentType<CubeComponent> cubeType;
            public ArchetypeChunkComponentType<HealthComponent> healthType;

            // Conclusion: pass a NativeArray<ArchetypeChunk> instead of NativeArray<SomeComponent> to manipulate the ComponentData
            [DeallocateOnJobCompletion]
            public NativeArray<ArchetypeChunk> EnemyEntityArchetypeChunkArray;
            [DeallocateOnJobCompletion]
            public NativeArray<ArchetypeChunk> LaserEntityArchetypeChunkArray;


            private int isOutEnemy;


            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {

                NativeArray<CubeComponent> _cubes = chunk.GetNativeArray(cubeType);
                NativeArray<Position> _positions = chunk.GetNativeArray(PositionType);
                NativeArray<HealthComponent> _healths = chunk.GetNativeArray(healthType);


                for (int i = 0; i < chunk.Count; i++)
                {
                    var position = _positions[i];
                    var cube = _cubes[i];
                    var health = _healths[i];

                    isOutEnemy = 1;


                    for (int enemyChunkIdx = 0; enemyChunkIdx < EnemyEntityArchetypeChunkArray.Length; enemyChunkIdx++)
                    {
                        var enemyComponentDataArray = EnemyEntityArchetypeChunkArray[enemyChunkIdx].GetNativeArray(enemyType);
                        var enemyInstanceCount = EnemyEntityArchetypeChunkArray[enemyChunkIdx].Count;

                        for (int ii = 0; ii < enemyInstanceCount; ii++)
                        {
                            if (enemyComponentDataArray[ii].enemyType == 0)
                            {
                                //判断是否在敌人范围内，是就扣血
                                if (math.abs(enemyComponentDataArray[ii].position.x - position.Value.x) < 25f &&
                                    math.abs(enemyComponentDataArray[ii].position.z - position.Value.z) < 25f)
                                {
                                    if (_cubes[i].isInEnemy == 0)
                                    {
                                    
                                        enemyComponentDataArray[ii] = new EnemyComponent { enemyType = 0 , health = enemyComponentDataArray[ii].health - 1, position = enemyComponentDataArray[ii].position };
                                        _healths[i] = new HealthComponent {
                                            healthValue = health.healthValue - 1,
                                            currColor = health.currColor,
                                        };
                                    }
                                    isOutEnemy = 0;
                                }
                            }
                            else if (enemyComponentDataArray[ii].enemyType == 1)
                            {
                                //判断是否在敌人范围内，是就扣血
                                if (math.abs(enemyComponentDataArray[ii].position.x - position.Value.x) < 7.5f &&
                                    math.abs(enemyComponentDataArray[ii].position.z - position.Value.z) < 7.5f)
                                {
                                    if (cube.isInEnemy == 0)
                                    {
                                        enemyComponentDataArray[ii] = new EnemyComponent { enemyType = 1, health = enemyComponentDataArray[ii].health - 1, position = enemyComponentDataArray[ii].position };
                                        _healths[i] = new HealthComponent
                                        {
                                            healthValue = health.healthValue - 1,
                                            currColor = health.currColor,
                                        };
                                    }
                                    isOutEnemy = 0;
                                }
                            }

                        }
                    }
                    if (isOutEnemy == 1)
                    {   //如果不在任何敌人内部，才是不在敌人内部
                        _cubes[i] = new CubeComponent
                        {
                            acceration = cube.acceration,
                            isInEnemy = 0,
                            mass = cube.mass,
                            maxLength = cube.maxLength,
                            position = cube.position,
                            radius = cube.radius,
                            velocity = cube.velocity,
                        };
                    }

                    if (math.length(position.Value) <= 40)
                    {
                        //回家回血
                        _healths[i] = new HealthComponent
                        {
                            healthValue = 2,
                            currColor = health.currColor,
                        };
                    }

                    for (int laserChunkIdx = 0; laserChunkIdx < LaserEntityArchetypeChunkArray.Length; laserChunkIdx++)
                    {
                        var laserDataArray = LaserEntityArchetypeChunkArray[laserChunkIdx].GetNativeArray(laserType);
                        var laserInstanceCount = LaserEntityArchetypeChunkArray[laserChunkIdx].Count;
                        for (int jj = 0; jj < laserInstanceCount; jj++)
                        {
                            //激光伤害
                            if (laserDataArray[jj].lifeTime <= 0.2f
                                && math.length(math.cross(laserDataArray[jj].direction, position.Value - laserDataArray[jj].startPoint)) <= 50f)
                            {
                                _healths[i] = new HealthComponent
                                {
                                    healthValue = 0,
                                    currColor = health.currColor,
                                };
                            }
                        }
                    }
                }
            }

        }


        //系统会每帧调用这个函数
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            //Cube计数
            var cubeDataArray = m_cubeGroup.ToComponentDataArray<CubeComponent>(Allocator.TempJob);
            //UIManager.Instance.ChangeCubeNumText(cubeDataArray.Length);
            cubeDataArray.Dispose();

            var positionType = GetArchetypeChunkComponentType<Position>(true);
            var enemyType = GetArchetypeChunkComponentType<EnemyComponent>(false);
            var cubeType = GetArchetypeChunkComponentType<CubeComponent>(false);
            var laserType = GetArchetypeChunkComponentType<LaserComponent>(true);
            var healthType = GetArchetypeChunkComponentType<HealthComponent>(false);


            var enemyEntityArray = m_enemyGroup.CreateArchetypeChunkArray(Allocator.TempJob, out JobHandle enemyCollectHandle);
            var laserEntityArray = m_laserGroup.CreateArchetypeChunkArray(Allocator.TempJob, out JobHandle laserCollectHandle);
            

            //初始化一个job
            var job = new JobProcess
            {
                cubeType = cubeType,
                enemyType = enemyType,
                PositionType = positionType,
                laserType = laserType,
                healthType = healthType,

                EnemyEntityArchetypeChunkArray = enemyEntityArray,
                LaserEntityArchetypeChunkArray = laserEntityArray,

            }.Schedule(m_jobInput,JobHandle.CombineDependencies(inputDeps,enemyCollectHandle,laserCollectHandle));

            //开始job      
            return job;
        }
    }
}