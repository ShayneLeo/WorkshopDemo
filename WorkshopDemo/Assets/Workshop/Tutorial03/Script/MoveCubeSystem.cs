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
        struct JobProcess : IJobProcessComponentData<Position, CubeComponent>
        {
            public bool isForceOn;
            public float3 mousePosition;
            public float mouseMass;

            public void Execute(ref Position position, ref CubeComponent cube)
            {

                //add force on cube
                if (isForceOn)
                {
                    float3 forceDir = mousePosition - position.Value;
                    float d = math.length(forceDir);
                    //we use d in denominator,so we should clamp it first
                    d = math.clamp(d, 1, 25);
                    forceDir = math.normalize(forceDir);
                    // F = GMm / d^2
                    float strength = (G * mouseMass * cube.mass) / (d * d);
                    forceDir *= strength;
                    //F = ma
                    cube.acceration = cube.acceration + (forceDir / cube.mass);
                }

                //acceration
                cube.velocity += cube.acceration;

                //max speed limited
                if (math.length(cube.velocity) > cube.maxLength)
                {
                    cube.velocity = math.normalize(cube.velocity);
                    cube.velocity *= cube.maxLength;
                }

                //apply position to Position component
                position.Value = position.Value + cube.velocity;

                //reset cube acceration
                cube.acceration *= 0;
            }

        }


        //Gravity constant
        public const float G = 9.8f;
        //we assume mousebutton has a big Mass like sun
        public float Mass = 70f;

        bool _isForceOn = false;
        float3 _mousePos;
        
        //系统会每帧调用这个函数
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // ------- input handle ------
            if (Input.GetMouseButtonDown(0))
            {
                _isForceOn = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isForceOn = false;
            }
            _mousePos = Camera.main.ScreenToWorldPoint(new float3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            _mousePos.y = 0;
            // ------- end input handle ------


            //initial job
            var job = new JobProcess { isForceOn = _isForceOn, mousePosition = _mousePos , mouseMass = Mass};
    
            //schedule job
            return job.Schedule(this, inputDeps);
        }
    }
}