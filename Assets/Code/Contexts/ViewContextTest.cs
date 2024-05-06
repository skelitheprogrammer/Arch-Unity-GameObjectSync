using System;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using Code.System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class ViewContextTest : MonoBehaviour
{
    private World _simulationWorld;
    private World _viewWorld;

    public CubeSpawnData SpawnData;
    public int Count;

    private CubeStartupSystem _cubeStartupSystem;
    private CubeSpawnSystem _cubeSpawnSystem;
    private CubeMoveSystem _moveSystem;

    private CubeViewSpawnSystem _cubeViewSpawnSystem;
    private CubeViewPositionSync _cubeViewPositionSync;

    private CommandBuffer _simulationBuffer;
    private CommandBuffer _viewBuffer;

    public GameObject Prefab;


    private void Awake()
    {
        _simulationWorld = World.Create();
        _viewWorld = World.Create();
        _moveSystem = new();
        _simulationBuffer = new CommandBuffer();
        _viewBuffer = new CommandBuffer();
    }

    private void Start()
    {
        CubeFactory cubeFactory = new();

        _cubeStartupSystem = new CubeStartupSystem(Count, SpawnData, cubeFactory);
        _cubeSpawnSystem = new CubeSpawnSystem(_simulationBuffer);

        _cubeViewSpawnSystem = new CubeViewSpawnSystem(_viewBuffer);
        _cubeViewPositionSync = new CubeViewPositionSync();

        _simulationWorld.Reserve(ComponentTypes.CubeViewRequests, Count);

        _cubeStartupSystem.Execute(_simulationWorld);
    }

    private void Update()
    {
        _cubeSpawnSystem.Execute(_simulationWorld);
        _cubeViewSpawnSystem.Execute(_viewWorld);

        _moveSystem.Execute(_simulationWorld);

        _cubeViewPositionSync.Execute(_viewWorld);

        if (_simulationBuffer.Size > 0)
        {
            _simulationBuffer.Playback(_simulationWorld);
        }

        if (_viewBuffer.Size > 0)
        {
            _viewBuffer.Playback(_viewWorld);
        }
    }
}

public static class ComponentTypes
{
    public static readonly ComponentType[] CubeViewRequests =
    {
        typeof(RequestView),
    };

    public static readonly ComponentType[] ViewArchetype =
    {
        typeof(ViewComponent)
    };

    public static readonly ComponentType[] CubeInitializerArchetype =
    {
        typeof(CubeInitializer)
    };

    public static readonly ComponentType[] CubeArchetype =
    {
        typeof(Position),
        typeof(Direction),
        typeof(MoveSpeed)
    };
}

public class CubeFactory
{
    public Entity Create(World world, in CubeSpawnData spawnData)
    {
        Entity entity = world.Create(ComponentTypes.CubeInitializerArchetype);
        entity.Set(new CubeInitializer
        {
            Position = Random.onUnitSphere,
            Direction = Random.insideUnitSphere,
            Speed = Random.Range(spawnData.MinSpeed, spawnData.MaxSpeed)
        });

        return entity;
    }
}

public class CubeStartupSystem : ISystem
{
    private readonly int _count;
    private readonly CubeSpawnData _spawnData;
    private readonly CubeFactory _factory;

    public CubeStartupSystem(int count, CubeSpawnData spawnData, CubeFactory factory)
    {
        _count = count;
        _spawnData = spawnData;
        _factory = factory;
    }

    public void Execute(World world)
    {
        world.Reserve(ComponentTypes.CubeArchetype, _count);

        for (int i = 0; i < _count; i++)
        {
            _factory.Create(world, _spawnData);
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

public class CubeViewSpawnSystem : ISystem
{
    private readonly CommandBuffer _buffer;
    private readonly QueryDescription _description = new QueryDescription().WithAll<RequestView>();

    public CubeViewSpawnSystem(CommandBuffer buffer)
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
        private readonly CommandBuffer _buffer;

        public Spawn(CommandBuffer buffer)
        {
            _buffer = buffer;
        }

        public void Update(Entity entity)
        {
            RequestView request = entity.Get<RequestView>();

            Entity viewEntity = _buffer.Create(ComponentTypes.ViewArchetype);

            _buffer.Set(viewEntity, new ViewComponent
            {
                Reference = request.Reference,
                View = Object.Instantiate(request.Prefab)
            });

            _buffer.Destroy(entity);
        }
    }
}

public class CubeViewPositionSync : ISystem
{
    private readonly QueryDescription _description = new QueryDescription().WithAll<ViewComponent>();

    public void Execute(World world)
    {
        world.InlineQuery<PositionSync>(_description);
    }

    private readonly struct PositionSync : IForEach
    {
        public void Update(Entity entity)
        {
            ViewComponent viewComponent = entity.Get<ViewComponent>();
            Position position = viewComponent.Reference.Entity.Get<Position>();

            viewComponent.View.transform.position = position.Value;
        }
    }
}

[Serializable]
public class CubeSpawnData
{
    public float MinSpeed;
    public float MaxSpeed;
}

public struct CubeInitializer
{
    public Vector3 Position;
    public Vector3 Direction;
    public float Speed;
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

public struct RequestView
{
    public EntityReference Reference;
    public GameObject Prefab;
}

public struct HasView
{
}

public struct ViewComponent
{
    public EntityReference Reference;
    public GameObject View;
}