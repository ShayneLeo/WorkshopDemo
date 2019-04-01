#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			IsDeadComponent
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Entities;

namespace YU.ECS
{
    public struct IsDeadComponent : ISharedComponentData
    {
        /// <summary>
        /// 0 = false
        /// 1 = true
        /// </summary>
        public int isDead;
    }
}
