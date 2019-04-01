#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			HealthComponent
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using Unity.Entities;
using Unity.Mathematics;

namespace YU.ECS
{
    public struct HealthComponent : IComponentData
    {
        //生命值组件，用在cube身上
        public int healthValue;


        /// <summary>
        /// 0 = normal
        /// 1 = red
        /// using for record current color,in case change color even not necessary
        /// </summary>
        public int currColor;

    }
}