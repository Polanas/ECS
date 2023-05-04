using ECS;

namespace ECSTests;

struct Oranges { }

struct Hates { }

struct Apples { }

struct Begin { }

struct End { }

struct Owes { public int Amount; }

struct Eats { public int Amount; }

struct People { }

struct SingletonEvent : ISingletonEvent { public int value; }

struct Position
{
    public int x, y;
}

struct Velocity { public int x, y; }

struct Tag { }

struct Event : IEvent { public int value; }

struct TagEvent : IEvent { }

struct Likes { }

struct Size : IAutoReset<Size>
{
    public float amount;

    public void AutoReset(ref Size c, AutoResetState state)
    {
        c.amount = 1;
    }
}

public class TestSystem : IInitSystem, IUpdateSystem
{

    public int value;

    public void Update()
    {
        value++;
    }

    public void Init(ECSSystems systems)
    {
        value = 1;
    }
}
struct MyRelation { }

public class OnComponentSystemTest : OnComponentActionSystem
{
    public int value;

    public OnComponentSystemTest()
    {
        All<Position>().All<MyRelation, Wildcard>();
    }

    public override void OnComponentAdd(Entity entity)
    {
        value++;
    }

    public override void OnComponentRemove(Entity entity)
    {
        value++;
    }
}

public class TestInjectSystem : IUpdateSystem, IInitSystem
{

    public int value;

    private ECSWorld _world = null!;
    private readonly FilterInject<Position> _posFilter = new(new All<Velocity>(), new None<Tag>());

    public void Update()
    {
        var entityWithTag = _world.AddEntity().Add<Position>().Add<Velocity>().Add<Tag>();
        _world.AddEntity().Add<Position>().Add<Velocity>();

        foreach (var entry in _posFilter)
        {
            value++;
            entry.entity.Remove();
        }

        entityWithTag.Remove();
    }

    public void Init(ECSSystems systems)
    {
        _world = systems.World;
    }
}

public class ServiceTestSystem : IInitSystem, IUpdateSystem
{

    public int value;

    private readonly CustomInject<TestService> _testService;

    public void Update()
    {
        value = _testService.Instance.value;
    }

    public void Init(ECSSystems systems)
    {
        _testService.Instance.value = 1;
    }
}

public class GroupTestSystem1 : IUpdateSystem
{

    private SharedInject<TestSharedData> _sharedData;

    public void Update()
    {
        _sharedData.Instance.value++;
    }
}

public class GroupTestSystem2 : IUpdateSystem
{

    private SharedInject<TestSharedData> _sharedData;

    public void Update()
    {
        _sharedData.Instance.value++;
    }
}


public class TestService
{
    public int value;
}

public class TestSharedData
{
    public int value;
}

[TestClass]
public class UnitTests
{

    private static ECSWorld _world = null!;
    private static ECSSystems _testInjectSystems = null!;
    private static ECSSystems _testSystems = null!;
    private static ECSSystems _serviceSystems = null!;
    private static ECSSystems _testGroupSystems = null!;
    private static ECSSystems _testActionSystems = null!;

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        _world = new();
        _testSystems = new(_world);
        _testSystems
            .Add<TestSystem>()
            .Init();

        _testInjectSystems = new(_world);
        _testInjectSystems
            .Add<TestInjectSystem>()
            .Inject()
            .Init();

        _serviceSystems = new(_world);
        _serviceSystems
            .Add<ServiceTestSystem>()
            .Inject(new TestService())
            .Init();

        _testGroupSystems = new(_world, new TestSharedData());
        _testGroupSystems
            .AddGroup("TestGroup", false, new GroupTestSystem1(), new GroupTestSystem2())
            .Inject()
            .Init();

        _testActionSystems = new(_world);
        _testActionSystems
            .Add(new OnComponentSystemTest())
            .Init();
    }

    [TestMethod]
    public void CreatingAndDelitingEntitesTest()
    {
        bool condition;
        var entity = _world.AddEntity();
        condition = entity.IsAlive();

        entity.Remove();
        condition &= !entity.IsAlive();
        Assert.IsTrue(condition);
    }

    [TestMethod]
    public void AddingComponentTest()
    {
        bool condition;
        var entity = _world.AddEntity();

        entity.Add<Position>();
        condition = entity.Has<Position>();

        entity.Remove<Position>();
        condition &= !entity.Has<Position>();

        Assert.IsTrue(condition);
    }

    [TestMethod]
    public void GettingComponentValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 10;

        entity.Add(new Position { x = 1, y = 2 });
        entity.Add(new Velocity { x = 3, y = 4 });

        var position = entity.Get<Position>();
        var velocity = entity.Get<Velocity>();

        var actual = position.x + position.y + velocity.x + velocity.y;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ChangingComponentValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 6;

        entity.Add(new Position { x = 2, y = 2 });

        ref var position = ref entity.Get<Position>();
        position.x++;
        position.y++;

        var positionAfterChange = entity.Get<Position>();
        var actual = positionAfterChange.x + positionAfterChange.y;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddingEntityTagTest()
    {
        var entity = _world.AddEntity();
        var tagEntity = _world.AddEntity();

        entity.Add(tagEntity);
        var hasTagBeforeDelition = entity.Has(tagEntity);

        tagEntity.Remove();
        var hasTagAfterDelition = entity.Has(tagEntity);

        Assert.IsTrue(hasTagBeforeDelition && !hasTagAfterDelition);
    }

    [TestMethod]
    public void AddingTagTest()
    {
        var entity = _world.AddEntity().Add<Tag>();
        var hasTagBeforeDelition = entity.Has<Tag>();

        entity.Remove<Tag>();
        var hasTagAfterDelition = entity.Has<Tag>();
        Assert.IsTrue(hasTagBeforeDelition && !hasTagAfterDelition);
    }

    [TestMethod]
    public void AddingRelationshipTest1()
    {
        var entity = _world.AddEntity().Add<Likes, Apples>();

        var hasTagBeforeDelition = entity.Has<Likes, Apples>();

        entity.Remove<Likes, Apples>();
        var hasTagAfterDelition = entity.Has<Likes, Apples>();

        Assert.IsTrue(hasTagBeforeDelition && !hasTagAfterDelition);
    }

    [TestMethod]
    public void AddingRelationshipTest2()
    {
        var ann = _world.AddEntity();
        var bob = _world.AddEntity().Add<Likes>(ann);

        var hasTagBeforeDelition = bob.Has<Likes>(ann);

        bob.Remove<Likes>(ann);
        var hasTagAfterDelition = bob.Has<Likes>(ann);

        ann.Remove();

        Assert.IsTrue(hasTagBeforeDelition && !hasTagAfterDelition);
    }

    [TestMethod]
    public void AddingRelationshipTest3()
    {
        var likes = _world.AddEntity();
        var ann = _world.AddEntity();
        var bob = _world.AddEntity().Add(likes, ann);

        var hasTagBeforeDelition = bob.Has(likes, ann);

        bob.Remove(likes, ann);
        var hasTagAfterDelition = bob.Has(likes, ann);

        ann.Remove();
        likes.Remove();

        Assert.IsTrue(hasTagBeforeDelition && !hasTagAfterDelition);
    }

    [TestMethod]
    public void GettingRelationship1ValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 10;

        entity.Add<Begin, Position>(new Position { x = 1, y = 2 });
        entity.Add<End, Position>(new Position { x = 3, y = 4 });

        var positionBegin = entity.Get2<Begin, Position>();
        var positionEnd = entity.Get2<End, Position>();

        var actual = positionBegin.x + positionBegin.y + positionEnd.x + positionEnd.y;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GettingRelationship2ValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 6;

        entity.Add<Owes, Apples>(new Owes { Amount = 5 });
        entity.Add<Owes, Oranges>(new Owes { Amount = 1 });

        var owesAples = entity.Get1<Owes, Apples>();
        var owesOranges = entity.Get1<Owes, Oranges>();

        var actual = owesAples.Amount + owesOranges.Amount;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ChangingRelationship1ValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 6;

        entity.Add<Owes, Apples>(new Owes { Amount = 5 });

        ref var owesApples = ref entity.Get1<Owes, Apples>();
        owesApples.Amount++;

        var owesApplesAfterChange = entity.Get1<Owes, Apples>();
        var actual = owesApplesAfterChange.Amount;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ChangingRelationship2ValuesTest()
    {
        var entity = _world.AddEntity();
        float expected = 7;

        entity.Add<Begin, Position>(new Position { x = 2, y = 3 });

        ref var beignPosition = ref entity.Get2<Begin, Position>();
        beignPosition.x++;
        beignPosition.y++;

        var begiPositionAfterChange = entity.Get2<Begin, Position>();
        var actual = begiPositionAfterChange.x + begiPositionAfterChange.y;

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddingSingletonEventsTest()
    {
        float expected = 3;

        _world.AddSingletonEvent(new SingletonEvent { value = 1 });
        _world.AddSingletonEvent(new SingletonEvent { value = 2 });
        _world.GetSingletonEvent<SingletonEvent>().value++;

        float actual = _world.GetSingletonEvent<SingletonEvent>().value;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetChildrenTest()
    {
        int expected = 2;
        int actual = 0;

        var entity = _world.AddEntity();
        _world.AddEntity().ChildOf(entity);
        _world.AddEntity().ChildOf(entity);

        foreach (var entry in entity.GetChildren())
        {
            actual++;
        }

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddingEventsTest()
    {
        float expected = 6;
        float actual = 0;

        _world.AddEvent(new Event { value = 2 });
        _world.AddEvent(new Event { value = 4 });

        foreach (var entry in _world.GetEvents<Event>())
        {
            actual += entry.item.value;
        }

        _world.RemoveEvents<Event>();

        foreach (var entry in _world.GetEvents<Event>())
        {
            actual += entry.item.value;
        }

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void RemovingEventsTest()
    {
        float expected = 3;
        float actual = 0;

        _world.AddEvent<TagEvent>();
        _world.AddEvent<TagEvent>();
        _world.AddEvent<TagEvent>();

        foreach (var entry in _world.GetEvents<TagEvent>())
        {
            actual++;
        }

        _world.RemoveEvents<TagEvent>();

        foreach (var entry in _world.GetEvents<TagEvent>())
        {
            actual++;
        }

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddingPrefabsTest()
    {
        float expected = 12;

        var prefab = _world.AddPrefab();
        prefab.Add(new Position { x = 1, y = 1 });
        prefab.Add<Likes, Apples>();

        var instanceOne = _world.AddEntity().InstanceOf(prefab);
        var instanceTwo = _world.AddEntity().InstanceOf(prefab);
        _world.SetPrefabValue(prefab,
                              new Position { x = 2, y = 3 },
                              static (ref Position pos, Position temp) =>
                              {
                                  pos.x = temp.x + temp.y;
                              });

        var positionOne = instanceOne.Get<Position>();
        var positionTwo = instanceTwo.Get<Position>();

        var actual = positionOne.x + positionOne.y + positionTwo.x + positionTwo.y;

        instanceOne.Remove();
        instanceTwo.Remove();
        prefab.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void FilterAnyTest()
    {
        int extected = 3;
        int actual = 0;

        var filter = _world.Filter().Any<Likes, People>().Any<Hates, Wildcard>().Build();
        _world.AddEntity().Add<Likes, People>();
        _world.AddEntity().Add<Hates, People>();
        _world.AddEntity().Add<Hates, People>().Add<Likes, People>();
        var entity = _world.AddEntity().Add<MyRelation, Apples>();

        foreach (var entry in filter)
        {
            actual++;
            entry.entity.Remove();
        }

        entity.Remove();

        Assert.AreEqual(extected, actual);
    }

    [TestMethod]
    public void NamesTest()
    {
        bool condition;

        var entity = _world.AddEntity("funnyEntity");

        var entityByName = _world.GetEntity("funnyEntity");
        condition = entityByName == entity;

        var childEntity = _world.AddEntity("EvenFunnierEntity").ChildOf(entity);
        condition &= childEntity.GetName(entityByName) == "EvenFunnierEntity";

        entityByName.Remove();
        entityByName = _world.GetEntity("funnyEntity");
        condition &= !entityByName.IsAlive();

        Assert.IsTrue(condition);
    }

    [TestMethod]
    public void RenameTest()
    {
        string expected = "name2";
        string actual;

        var entity = _world.AddEntity().Name("name1").Name("name2");
        actual = entity.GetName();

        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void FilterNoneTest()
    {
        float expected = 1;
        float actual = 0;

        var tag = _world.AddEntity();
        var filter = _world.Filter<Position>().None(tag).Build();

        var entityWithTag = _world.AddEntity().Add<Position>().Add(tag);
        var entityWithoutTag = _world.AddEntity().Add<Position>();

        foreach (var entry in filter)
        {
            actual++;
        }

        entityWithoutTag.Remove();
        entityWithTag.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void FiltersTest()
    {
        float expected = 10;
        float actual = 0;

        var positionFilter = _world.Filter<Position>().Build();

        _world.AddEntity().Add(new Position { x = 1, y = 2 });
        _world.AddEntity().Add(new Position { x = 3, y = 4 });

        foreach (var entry in positionFilter)
        {
            actual += entry.item.x + entry.item.y;
            _world.RemoveEntity(entry.entity);
        }

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void PairFilterTest()
    {
        float excepted = 7;
        float actual = 0;

        var filter = _world.Filter<Pair1<Owes, Apples>,
                                   Pair1<Owes, Oranges>>()
                                   .Build();

        var entity = _world.AddEntity()
            .Add<Owes, Apples>(new Owes { Amount = 2 })
            .Add<Owes, Oranges>(new Owes { Amount = 3 });

        foreach (var entry in filter)
        {
            ref var owesApples = ref entry.item1.GetValue();
            ref var owesOranges = ref entry.item2.GetValue();

            owesApples.Amount++;
            owesOranges.Amount++;

            actual += entry.item1.GetValue().Amount;
            actual += entry.item2.GetValue().Amount;
        }

        entity.Remove();

        Assert.AreEqual(actual, excepted);
    }

    [TestMethod]
    public void AutoResetTest()
    {
        float expected = 1;
        float actual;

        var entity = _world.AddEntity().Add<Size>();

        actual = entity.Get<Size>().amount;

        Assert.AreEqual(expected, actual);
    }

#if DEBUG
    [TestMethod]
    public void AddingComponentTwiceTest()
    {
        var entity = _world.AddEntity();

        Assert.ThrowsException<Exception>(() =>
        {
            entity.Add<Position>().Add<Position>();
        });

        entity.Remove();
    }
#endif

    [TestMethod]
    public void SystemTest()
    {
        int expected = 1;

        _testInjectSystems.Update();
        var actual = _testInjectSystems.GetSystem<TestInjectSystem>().value;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void FilterInjectionTest()
    {
        int expected = 2;

        _testSystems.Update();
        var actual = _testSystems.GetSystem<TestSystem>().value;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SystemGroupTest()
    {
        int expected = 2;

        _testGroupSystems.Update();
        _testGroupSystems.SetGroupState("TestGroup", true);
        _testGroupSystems.Update();

        var actual = _testGroupSystems.GetShared<TestSharedData>().value;

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void ServiceInjectionTest()
    {
        int expected = 1;

        _serviceSystems.Update();
        var actual = _serviceSystems.GetSystem<ServiceTestSystem>().value;

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void AddingComponentsInFilterTest()
    {
        float expected = 6;

        var entity1 = _world.AddEntity().Add<Tag>();
        var entity2 = _world.AddEntity().Add<Tag>();
        var filter = _world.Filter().All<Tag>().Build();

        foreach (var entry in filter)
        {
            entry.entity.Add(new Position { x = 1, y = 2 });
        }

        var pos1 = entity1.Get<Position>();
        var pos2 = entity2.Get<Position>();

        var actual = pos1.x + pos1.y + pos2.x + pos2.y;

        entity1.Remove();
        entity2.Remove();

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void RemovingComponentsInFilterTest()
    {
        var entity1 = _world.AddEntity().Add<Position>();
        var entity2 = _world.AddEntity().Add<Position>();
        var filter = _world.Filter().All<Position>().Build();

        foreach (var entry in filter)
        {
            entry.entity.Remove<Position>();
        }

        var condition = !entity1.Has<Position>() && !entity2.Has<Position>();

        Assert.IsTrue(condition);
    }

    [TestMethod]
    public void AddingInstancesInFilterTest()
    {
        float expected = 3;

        var prefab = _world.AddPrefab()
            .Add(new Position { x = 1, y = 2 });

        var entity = _world.AddEntity().Add<Tag>();
        var filter = _world.Filter().All<Tag>().Build();

        foreach (var entry in filter)
        {
            entry.entity.InstanceOf(prefab);
        }

        var pos = entity.Get<Position>();

        float actual = pos.x + pos.y;

        prefab.Remove();
        entity.Remove();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void OnComponentSystemTest()
    {
        int expected = 2;

        _testActionSystems.Update();

        var entity = _world.AddEntity().Add<Position>().Add<MyRelation, Apples>();
        var entity1 = _world.AddEntity().Add<Position>();
        entity.Add<Velocity>().Remove<Velocity>();

        var actual = _testActionSystems.GetSystem<OnComponentSystemTest>().value;

        entity.Remove();
        entity1.Remove();

        Assert.AreEqual(expected, actual);
    }
}