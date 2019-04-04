#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			GenerateCubeSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;

namespace YU.ECS.T3
{
    public class GenerateCubeSystem : ComponentSystem
    {
        
        public static EntityArchetype CubeArchetype;
        private static RenderMesh cubeRenderer;

        protected override void OnCreateManager()
        {

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            CubeArchetype = entityManager.CreateArchetype(
                typeof(Position),
                typeof(RenderMesh),
                typeof(CubeComponent)
            );
        }
        protected override void OnStartRunning()
        {
            cubeRenderer = GetLookFromPrototype("CubePrototype");
        }

        public static RenderMesh GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<RenderMeshProxy>().Value;
            return result;
        }

        protected override void OnUpdate()
        {

            //for test
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var entityManager = World.Active.GetOrCreateManager<EntityManager>();
                for (int i = 0; i <10000; i++)
                {
                    Entity cube = entityManager.CreateEntity(CubeArchetype);

                    //初始化随机点
                    float3 initialPosition = new float3(UnityEngine.Random.Range(-960f, 960f), 0f, UnityEngine.Random.Range(-540f, 540f));
;
                    //设置给实体
                    entityManager.SetComponentData(cube, new Position { Value = initialPosition });

                    //cube属性
                    CubeComponent c = new CubeComponent
                    {
                        mass = 1,
                        maxLength = 20,
                        velocity = Vector3.zero,
                        acceration = Vector3.zero
                    };

                    entityManager.SetComponentData(cube,c);

                    entityManager.SetSharedComponentData(cube, cubeRenderer);

                }
            }
        }
    }
}