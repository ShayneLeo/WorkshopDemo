#region Author
///-----------------------------------------------------------------
///   Namespace:		YU.ECS
///   Class:			MoveCubeSystem
///   Author: 		    yutian
///-----------------------------------------------------------------
#endregion
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

namespace YU.ECS.T3
{
    public class MoveCubeSystem : JobComponentSystem
    {
        [BurstCompile]
        struct JobProcess : IJobProcessComponentData<Position, CubeComponent, ForceComponent>
        {
            public bool isForceOn;
            public float3 mousePosition;

            //Job的执行函数，作用是控制cube的运动
            public void Execute(ref Position position, ref CubeComponent cube, ref ForceComponent forcefield)
            {

                //加力
                if (isForceOn)
                {
                    float3 f = forcefield.CastForce(ref mousePosition, ref cube);
                    ApplyForce(ref cube, f);
                }

                //速度小于0.1f直接抛弃
                if (math.length(cube.velocity) >= 0.1f)
                { 
                    //应用摩擦力
                    ApplyForce(ref cube, CalculateFriction(forcefield.frictionCoe, ref cube));
                }
                else
                {
                    Stop(ref cube);
                }

                //加速度
                cube.velocity += cube.acceration;

                //最大速度限制
                if (math.length(cube.velocity) > cube.maxLength)
                {
                    cube.velocity = math.normalize(cube.velocity);
                    cube.velocity *= cube.maxLength;
                }

                //根据速度变化位置
                cube.position += cube.velocity;

                //应用cube位置到Position
                position.Value = cube.position;

                //重设加速度
                cube.acceration *= 0;
            }

            public void ApplyForce(ref CubeComponent b, float3 force)
            {
                //F = ma
                b.acceration = b.acceration + (force / b.mass);
            }

            public void Stop(ref CubeComponent b)
            {
                b.velocity *= 0;
            }

            float3 CalculateFriction(float coe, ref CubeComponent b)
            {
                float3 friction = b.velocity;
                friction *= -1;
                friction = math.normalize(friction);
                friction *= coe;

                return friction;
            }

        }

        bool _isForceOn = false;
        float3 mousePos;

        //系统会每帧调用这个函数
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // ------- input handle ------
            //left - pull
            if (Input.GetMouseButtonDown(0))
            {
                _isForceOn = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isForceOn = false;
            }

            mousePos = Camera.main.ScreenToWorldPoint(new float3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            mousePos.y = 0;
            // ------- end input handle ------


            //初始化一个job
            var job = new JobProcess { isForceOn = _isForceOn, mousePosition = mousePos };

            //开始job      
            return job.Schedule(this, inputDeps);
        }
    }
}