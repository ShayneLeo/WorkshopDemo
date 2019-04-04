#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			CubeComponent
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Entities;
using Unity.Mathematics;

namespace YU.ECS.T4
{
    public struct CubeComponent : IComponentData
    {
        public float3 velocity;
        public float3 acceration;
        public float mass;
        //Max speed length limitation
        public float maxLength;

    }
}