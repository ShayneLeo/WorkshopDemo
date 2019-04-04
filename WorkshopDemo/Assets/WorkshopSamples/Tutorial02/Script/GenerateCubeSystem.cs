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

namespace YU.ECS.T2
{
    public class GenerateCubeSystem : ComponentSystem
    {
        //define what componentdata that a entity have
        public static EntityArchetype CubeArchetype;
        private static RenderMesh cubeRenderer;

        protected override void OnCreateManager()
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            CubeArchetype = entityManager.CreateArchetype(
                typeof(Position),
                typeof(RenderMesh)
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
                    //create entity
                    Entity cube = entityManager.CreateEntity(CubeArchetype);

                    //initial random position
                    float3 initialPosition = new float3(UnityEngine.Random.Range(-960f, 960f), 0f, UnityEngine.Random.Range(-540f, 540f));
;
                    //set componentData to entity
                    entityManager.SetComponentData(cube, new Position { Value = initialPosition });

                    //set sharedComponentData to entity
                    entityManager.SetSharedComponentData(cube, cubeRenderer);

                }
            }
        }
    }
}