using System;
using System.Runtime.CompilerServices;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using Code.Arch.Arch.View;
using Code.System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ViewContextTest : MonoBehaviour
{
    private World _world;

    public CubeSpawnData SpawnData;
    public int Count;

    private IViewHandler<GameObject> _cubeViewHandler;

    private CubeSpawnSystem _cubeSpawnSystem;
    private CubeMoveSystem _moveSystem;

    private ViewPositionSync _viewPosSync;
    private CubeViewRotationSync _cubeViewRotSync;

    private CommandBuffer _buffer;

    private void Awake()
    {
        _world = World.Create();
        _buffer = new CommandBuffer();

        IPool<GameObject> cubeViewPool = new GameObjectPool(
            Instantiate,
            instance => instance.SetActive(true),
            instance => instance.SetActive(false),
            SpawnData.Prefab,
            Count);

        _cubeViewHandler = new CubeViewHandler(cubeViewPool.Rent, cubeViewPool.Return);

        _cubeSpawnSystem = new CubeSpawnSystem(_buffer);
        _cubeViewRotSync = new();
        _moveSystem = new();
        _viewPosSync = new();
    }

    private void Start()
    {
        new CubeStartupSystem(Count, SpawnData, _cubeViewHandler).Execute(_world);
    }

    private void Update()
    {
        _viewPosSync.Execute(_world);
        _cubeViewRotSync.Execute(_world);

        _cubeSpawnSystem.Execute(_world);
        _moveSystem.Execute(_world);

        if (_buffer.Size > 0)
        {
            _buffer.Playback(_world);
        }
    }
}

public static class ComponentTypes
{
    public static readonly ComponentType[] CubeInitializerArchetype =
    {
        typeof(CubeInitializer)
    };

    public static readonly ComponentType[] CubeArchetype =
    {
        typeof(Position),
        typeof(Direction),
        typeof(MoveSpeed),
        typeof(ViewReference)
    };
}


public class CubeStartupSystem : ISystem
{
    private readonly int _count;
    private readonly IViewHandler<GameObject> _viewHandler;
    private readonly CubeSpawnData _spawnData;

    public CubeStartupSystem(int count, CubeSpawnData spawnData, IViewHandler<GameObject> viewHandler)
    {
        _count = count;
        _spawnData = spawnData;
        _viewHandler = viewHandler;
    }

    public void Execute(World world)
    {
        world.Reserve(ComponentTypes.CubeInitializerArchetype, _count);
        world.Reserve(ComponentTypes.CubeArchetype, _count);

        for (int i = 0; i < _count; i++)
        {
            Entity entity = world.Create(ComponentTypes.CubeInitializerArchetype);

            entity.Set(new CubeInitializer
            {
                Position = Random.onUnitSphere,
                Direction = Random.insideUnitSphere,
                Speed = Random.Range(_spawnData.MinSpeed, _spawnData.MaxSpeed),
                Instance = _viewHandler.Get()
            });
        }
    }
}


public class CubeSpawnSystem : ISystem
{
    private readonly CommandBuffer _buffer;

    private readonly QueryDescription _description = new QueryDescription().WithAll<CubeInitializer>();

    public CubeSpawnSystem(CommandBuffer buffer)
    {
        _buffer = buffer;
    }

    public void Execute(World world)
    {
        Spawn spawn = new(_buffer);
        world.InlineQuery(_description, ref spawn);
    }

    private readonly struct Spawn : IForEach
    {
        public readonly CommandBuffer Buffer;

        public Spawn(CommandBuffer buffer)
        {
            Buffer = buffer;
        }

        public void Update(Entity entity)
        {
            CubeInitializer cubeInitializer = entity.Get<CubeInitializer>();

            Entity bufferEntity = Buffer.Create(ComponentTypes.CubeArchetype);

            Buffer.Set(bufferEntity, new Position
            {
                Value = cubeInitializer.Position
            });

            Buffer.Set(bufferEntity, new Direction
            {
                Value = cubeInitializer.Direction
            });

            Buffer.Set(bufferEntity, new MoveSpeed
            {
                Value = cubeInitializer.Speed
            });

            Buffer.Set(bufferEntity, new ViewReference
            {
                Value = cubeInitializer.Instance
            });

            Buffer.Destroy(entity);
        }
    }
}

public class CubeMoveSystem : ISystem
{
    private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<Direction>().WithAll<MoveSpeed>();

    public void Execute(World world)
    {
        world.InlineQuery<Move>(_description);
    }

    private readonly struct Move : IForEach
    {
        public void Update(Entity entity)
        {
            ref Position position = ref entity.Get<Position>();
            position.Value += entity.Get<Direction>().Value * (entity.Get<MoveSpeed>().Value * Time.deltaTime);
        }
    }
}

public class ViewPositionSync : ISystem
{
    private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Position>();

    public void Execute(World world)
    {
        world.InlineQuery<ViewSync>(_description);
    }

    private readonly struct ViewSync : IForEach
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(Entity entity)
        {
            ViewReference viewReference = entity.Get<ViewReference>();
            viewReference.Value.transform.position = entity.Get<Position>().Value;
        }
    }
}

public class CubeViewRotationSync : ISystem
{
    private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Direction>();

    public void Execute(World world)
    {
        world.InlineQuery<Sync>(_description);
    }


    private readonly struct Sync : IForEach
    {
        public void Update(Entity entity)
        {
            GameObject gameObject = entity.Get<ViewReference>().Value;
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, entity.Get<Direction>().Value);
        }
    }
}


[Serializable]
public class CubeSpawnData
{
    public float MinSpeed;
    public float MaxSpeed;
    public GameObject Prefab;
}

public struct CubeInitializer
{
    public Vector3 Position;
    public Vector3 Direction;
    public float Speed;
    public GameObject Instance;
}

public struct Position
{
    public Vector3 Value;
}

public struct Direction
{
    public Vector3 Value;
}

public struct MoveSpeed
{
    public float Value;
}

public struct ViewReference
{
    public GameObject Value;
}