using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.CubeLayer.Components;
using Code.MovableLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CubeSineWaveDirectionUpdateSystem : ISystem
    {
        private readonly QueryDescription _updateSineWaveDescription = new QueryDescription().WithAll<Cube>().WithAll<SineWave>();
        private readonly QueryDescription _moveDescription = new QueryDescription().WithAll<Cube>().WithAll<MoveDirection>().WithAll<SineWave>();

        public void Execute(World world)
        {
            world.InlineQuery<UpdateSineWave>(_updateSineWaveDescription);
            world.InlineQuery<UpdatePosition>(_moveDescription);
        }

        private readonly struct UpdateSineWave : IForEach
        {
            public void Update(Entity entity)
            {
                ref SineWave sineWave = ref entity.Get<SineWave>();
                float sin = Mathf.Sin(Time.time * sineWave.Frequency);
                sineWave.Value = sin * sineWave.Amplitude;
            }
        }

        private readonly struct UpdatePosition : IForEach
        {
            public void Update(Entity entity)
            {
                SineWave sineWave = entity.Get<SineWave>();
                ref Position position = ref entity.Get<Position>();
                Vector3 pos = Vector3.Cross(entity.Get<MoveDirection>().Value, sineWave.Axis).normalized * sineWave.Value;

                position.Value += pos * Time.deltaTime;
            }
        }
    }
}