An Entity Component System library based on [RelECS](https://github.com/Byteron/RelEcs), with major inspiration from [ecslite](https://github.com/Leopotam/ecs) and [flecs](https://github.com/SanderMertens/flecs).

> Note: don't forget to build in RELEASE if you don't need additional checks and helpful exceptions.

## Usage overview
### World
A world stores all the data related to the ECS, including entites, their components, etc.
> Note: multiple worlds are not supported as for now.
```cs
//creating a world
ECSWorld world = new ECSWorld();

//destroying the world
world.Destroy();
```

### Entities
Each entity is a unique ID, which is associated with a set of components.
> Note: IDs of deleted entities will be reused, so if you save entities for later use, it's recommended to ensure they are alive before doing so.
```cs
//creating an entity
Entity entity = world.AddEntity();

//checking if the entity is alive
bool alive = entity.IsAlive();

//removing an entity
entity.Remove();
```
### Components
Components can be added to entites. They are represented as structs.
> Note: if all components are deleted from an entity, the entity itself will be deleted.
```cs
struct Velocity { public float x, y; }
//if a component doesn't contain any data, it's considered to be a tag, which makes it faster to add/remove
struct IsDead { }
...
var entity = world.AddEntity();

//adding a component to an entity
entity.Add(new Velocity { x = 5, y = 5 });

//getting a component by reference
var velocity = entity.Get<Velocity>();
velocity.Value.x++;

//does this entity have Velocity?
bool hasVelocity = entity.Has<Velocity>();

//deleting a component from an entity
entity.Remove<Velocity>();

//now this entity is dead :(
entity.Add<Dead>();
```
#### Auto Reset
AutoReset(ref T, ResetState) method (from IAutoReset<T> interface) is called when a component is added or removed.
```cs
struct AnimationComponent : IAutoReset<AnimationComponent>
{
    public List<Animation> animations;

    public void AutoReset(ref AnimationComponent c, AutoResetState state)
    {
       //make sure list isn't allocated multiple times
       animations ??= new();
       animations.Clear();
    }
}
```
### Relationships
Relationships are special components which are composed of two things (could be (Entity, Entity), (Entity, Component) or (Component, Component).
```cs
struct Likes { }

var ann = world.AddEntity();
var bob = world.AddEntity();

//Ann likes bob
ann.Add<Likes>(bob);

//does bob like Ann? (Apparently, no)
bool likesAnn = bob.Has<Likes>(ann);

//Ann no longer likes bob
ann.Remove<Likes>(bob);
``` 
#### Data Relationships
Relationships may or may not contain a component with data (it could be any part of a relationships, but only *one*).
```cs
struct Position { public float x, y; }
struct Begin { }
struct End { }
...

var animation = world.AddEntity();
//as (Begin, Position) and (End, Position) have different IDs, one can store multiple instances of the same data type
animation.Add<Begin, Position>();
animation.Add<End, Position>(new Position { x = 20, y = 40 });

//Position is the second part of the relationship, so Get1 is required 
var beginPosition = animation.Get2<Begin, Position>();
```
#### ChildOf Relationship
(ChildOf, Entity) is a built-in relationship that helps create hierarchies of entities.
```cs
var solarSystem = world.AddEntity();
//earth is a child of the solar system
var earth = world.AddEntity().ChildOf(solarSystem);
//moon is a child of the earth
var moon = world.AddEntity().ChildOf(earth);

//gets all the children of an entity
foreach (var child in solarSystem.GetChildren())
{
    ...
}
```
### Filters
Filters, well, *filter* entities by their components (or absence of them) and help accessing their values.
To be able to read/write to a component, specify it as genetic argument of ECSWorld.Filter<..>() methods.
```cs
//creating a filter of entities with position
var positionFilter = world.Filter<Position>().Build();

//entries contain an entity and ref fields of specified components, if there are any
foreach (var entry in filter)
{
    var entity = entry.entity;
    ref var position = ref entry.item;
}
//creating a filter of entities with velocity and position, which don't like apples and hate something, which also have dogs or cats (or both)
//make as complex filters as you'd like! :)
var complexFilter = world.Filter<Velocity>().All<Position>().None<Likes, Apples>().All<Hates, Wildcard>().Any<Has, Dogs>().Any<Has, Cats>().Build();
```
#### Wildcard
Filtering by relationships is cool and all, but sometimes it isn't flexible enough. *Wildcards* can replace any part of a relationships to include all relationships which match the specified pattern.
```cs
//finds all children who have some relation to apples
var allChildrenFilter = world.Filter().All<ChildOf, Wildcard>().All<Wildcard, Apples>().Build();
//finds all entities which relate to player in some way
var relationWithPlayerFilter = world.Filter().All<Wildcard>(world.GetEntity("player")).Build();
```
#### Getting Data From Relationships
```cs
//use TermN() to specify which relationships correspond to each value
var filter = world.Filter<Position, Owes>()
    .Term1().First<Begin>()
    .Term2().Second<Apples>()
    .Build();

foreach (var entry in filter)
{
    ref var beginPosition = ref entry.item1.GetValue();
    ref var owesApples = ref entry.item2.GetValue();
}
```
#### Optional Components
components can be marked optional, which will include entites with and without these components.

```cs
var filter = world.Flter<Position, Speed>
    //mark Speed as optional
    .Term2().Optional()
    .Build();

foreach (var entry in filter)
{
    Speed speed = default;

    //check if current entity has Speed
    if (!entry.IsNull2())
        speed = entry.item2;

    entry.item1 += speed;
}

```
### Systems
Systems run all the logic; they are represented as classes which can implement a bunch of interfaces (IUpdateSystem, IPostUpdateSystem, IInitSystem, IPreInitSystem, IDestroySystem).
```cs
class MoveSystem : IInitSystem, IUpdateSystem
{
    private Filter<Position, Speed> _movableFilter;

    public void Init(ECSSystems systems)
    {
        //it's recommended to cache filters instead of creating them every frame
        _movableFilter = systems.World.Filter<Position, Speed>
    }

    public void Update()
    {
        foreach (var entry in _movableFilter)
        {
            //adding speed to position
            entry.item1.value += entry.item2.value;
        }
    }
}
...

//systems are stored in a ECSSystems instance
//multiple ECSSystems can be created for a single world
var systems = new ECSSystems(world);

systems
    .Add<MoveSystem>()
     //adding other systems...
    .Init(); //preInit and init are called here
...

//systems are executed in the order they were added
systems.Update(); //PreUpdate and Update are called here
```
#### System groups
Systems can be grouped; groups are assigned with a name, which can later be used to toggle groups on or off.
```cs
var systems = new ECSSystems(world);
systems
    .Add("gameLoop",
        new UpdateUISystem(),
        new UpdateGameSystem(),
        defaultState: false) //this group will be toggled of by default
                             //adding other systems...
     .Init();
...

//now gameLoop group is active
systems.SetGroupState("gameLoop", true);
```
#### OnComponentAction Systems
These systems provide a callback when addition and removal of certain components happen.
```cs
class OnComponentSystemTest : OnComponentActionSystem
{
    public OnComponentSystemTest()
    {
        //one can also use None() and Any() here
        All<Position>();
    }

    public override void OnComponentAdd(Entity entity)
    {
        Console.WriteLine($"Position component was added to {entity}!");
    }

    public override void OnComponentRemove(Entity entity)
    {
        Console.WriteLine($"Position component was removed from {entity}!");
    }
}
```

### Entity Names
An entity can be associated with a unique name.
```cs
//now it's a player
var player = world. AddEntity("player");

//oh wait, it's actually player1
player.Name("player1");

//and this is a camera
var camera = world.AddEntity("camera");

//every parent has it's own "namespace", so that entities can share names as long as they have different parents
//(as for the global names, all the entities with them are associated with a single fake parent)
var camera = world.AddEntity().ChildOf(player).Name("camera");
```
### Prefabs
Sometimes it's useful to have some kind of a base entity, which other entities can inherit components from — that's what prefabs are. They can also be used to change the value of all inherited components of all prefab instances.
```cs
var enemyPrefab = _world.AddPrefab()
    .Add<Position>()
    .Add<Renderable>()
//other components/relationships
    .Add<Collider>();

//enemy inherited all the specified components   
var enemy = _world.AddEntity().InstanceOf(enemyPrefab);
//now all enemies have a position of (10,10) (for some reason)
_world.SetPrefabValue(enemyPrefab, new Position { x = 10, y = 10 });

var yellow = 5;
//If you want to replace some fields and keep others, also pass a delegate. second argument is used to store a temporary value
_world.SetPrefabValue(enemyPrefab,
                      new Renderable { color = yellow },
                      static (ref Renderable c, Renderable temp) =>
                      {
                          c.color = temp.color;
                      });
```
### Dependency Injection
This feature lets one inject classes and filters in systems without having to initialize them manually.
```cs

class LevelService
{
   ...
}

class LevelSystem : IInitSystem
{
    private readonly CustomInject<LevelService> _levelService;
    //use Any, None or All objects to add additional arguments to a filter
    private readonly FilterInject<Sprite, Transform> _sprites = new(new None<InvisibleSprite>());

    public void Init(ECSSystems systems)
    {
       //LevelService is ready to use
       var levelService = _levelService.Instance;
       foreach (var entry in _sprites)
       {
           ...
       }
       ...
    }
}
...
systems
    .Add<LevelSystem>()
    //injecting a service
    //Inject() must be called before Init()
    .Inject(new LevelService())
    .Init();
```
### Events
Sometimes it's useful to treat components like events, which multiple systems can subscribe to.
```cs
struct OnCollisionEvent { Entity collided, collidedWith; }
...

//regular events can be added multiple times
world.AddEvent(new OnCollisionEvent {...});
world.AddEvent(new OnCollisionEvent {...});
...

foreach (var entry in world.GetEvents<OnCollisionEvent>())
{
    var collisionEvent = entity.item;

    if (!collisionEvent.collided.IsAlive() || !collisionEvent.collidedWith.IsAlive())
        //removes specific event
        world.RemoveEntity(entry.entity);
}
...

//removes all events
world.RemoveEvents<OnCollisionEvent>();
```
#### Singleton Events
Singleton events, as the name suggests, are always single instanced.
```cs
struct PlayerStateEvent : ISingletonEvent
{
    public PlayerState playerState;
}
...

//adding a singleton
_world.AddSingletonEvent(new PlayerStateEvent { });

//and getting it
ref var playerStateEvent = ref _world.GetSingletonEvent<PlayerStateEvent>();

//removing it
_world.RemoveSingletonEvent<PlayerStateEvent>();

```
### DeleteHere
DeleteHere is a helper system that deletes a specified component for all entites.
```cs
systems
    ...
    // all components (or events) of this type will be deleted
    .DeleteHere<PLayerDiedEvent>()
    .Init();
```

### Other Features
#### Shared Data
A single instance of an object can be stored in a world to be accessed later.
```cs
class SharedData
{
    public readonly Window gameWindow;
    public float deltaTime;
}
...

var systems = new ECSSystems(world, new SharedData { ... });

var sharedData = systems.GetShared<SharedData>();
```

#### Entity deactivation
Any entity can be deactivated, in which case it will be excluded from all filters. All other properties stay the same though.
> Note: Deactivation/Reactivation also recursively affects children of an entity, children's children an so on.
```cs
var myEntity = world.AddEntity().Deactivate();
bool IsActive = myEntity.IsActive();
entity.Activate();
```
