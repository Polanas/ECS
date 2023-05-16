namespace ECS;

static class Program
{

    public static void Main()
    {
        var world1 = new ECSWorld();
        var systems = new ECSSystems(world1);
        systems
            // .Add(new TestSystem())
            //   .Add(new TestSystem())
            .Inject()
            .Init();
        systems.Update();
        #region singletonTest
        /*
        world1.AddSingletonEvent<SingletonEvent>();
        ref var singleton = ref world1.GetSingletonEvent<SingletonEvent>();
        singleton.Instance = 10;
        Console.WriteLine(singleton.Instance);

        Console.WriteLine(world1.WasSingletonCreated<SingletonEvent>());
        world1.RemoveSingletonEvent<SingletonEvent>();
        Console.WriteLine(world1.WasSingletonCreated<SingletonEvent>());
        Console.ReadKey();*/
        #endregion

        #region PrefabWithChildrenTest
        /*
        var e = world1.AddEntity();
        e.Add<Velocity>();
        var filter1 = world1.Filter<Velocity>().Build();

        var bob = world1.AddPrefab("bob").Add<Position>();
        var bobson = world1.AddEntity().ChildOf(bob).Name("bobson");
        var bobdaughter = world1.AddEntity().ChildOf(bob).Name("bobd");
        var bobgrandson = world1.AddEntity().ChildOf(bobdaughter).Name("bobgs");

        Entity anotherbob = default;
        foreach (var entry in filter1)
        {
            anotherbob = world1.AddEntity().InstanceOf(bob);
        }
        foreach (var data in world1.Filter().All<ChildOf>(anotherbob).Build())
        {
            Console.WriteLine("bob has a child!");
            Console.WriteLine("their name is {0}!", data.entity.GetName());

            foreach (var data1 in world1.Filter().All<ChildOf>(data.entity).Build())
            {
                Console.WriteLine("bobs children have children!");
                Console.WriteLine("their name is {0}!", data1.entity.GetName());
            }
        }
        */
        Console.ReadLine();

        #endregion

        #region nameDelitionTest
        //var ann = world1.AddEntity("Ann");
        //var bob = world1.AddEntity("Bob");

        //bob.ChildOf(ann);

        //world1.RemoveEntity(ann);
        //Console.WriteLine(bob.IsAlive());
        //Console.ReadKey(); 
        #endregion

        #region CopyTest
        /*
        var a = world1.AddEntity()
            .Add(new Position(5, 1))
            .Add<Likes, Apples>();

        var aCopy = a.Copy();
        Console.WriteLine(aCopy.Get<Position>().x);
        Console.WriteLine(aCopy.Has<Likes, Apples>());
        Console.ReadKey();*/
        #endregion

        #region Prefab test
        /*
    var bobPrefab = world1.AddPrefab("bob");
    bobPrefab
        .Add<Position>()
        .Add<Likes>();

    var bobInstance = world1
        .AddEntity()
        .InstanceOf(world1.GetEntity("bob"));
    var annInstance = world1
        .AddEntity()
        .InstanceOf(world1.GetEntity("bob"));

    Console.WriteLine(bobInstance.Has<Likes>());
    bobInstance.Remove<Likes>();
    Console.WriteLine(bobInstance.Has<Likes>());

    Console.ReadKey();
    */
        #endregion

        #region ChildOf test
        /*
        var player = world1.AddEntity("player");
        var enemy = world1.AddEntity("enemy");
        var playerCamera = world1.AddEntity();
        var enemyCamera = world1.AddEntity();

        playerCamera.ChildOf(player);
        playerCamera.Name("camera");
        enemyCamera.ChildOf(enemy);
        enemyCamera.Name("camera");
        Console.Write(world1.IsEntityChildOf(playerCamera));

        enemyCamera.ChildOf(player);

        var playerCameraByName = world1.GetEntity("camera", player);
        var enemyCameraByName = world1.GetEntity("camera", enemy);
        */
        #endregion

        #region doing stuff in filter test
        //var isHuman = world1.AddEntity();

        //ann.All<Position>(new(10, 10));
        //ann.All<Velocity>();
        //ann.Add(isHuman);

        //var filter = world1.Filter().All(isHuman).Build();

        //foreach (var data in filter)
        //{
        //    var entity = data.entity;

        //    world1.RemoveEntity(isHuman);
        //    entity.None<Position>();
        //    entity.None<Velocity>();
        //}

        //Console.WriteLine(ann.IsAlive());
        #endregion

        #region data relationship test
        //ann.All<Begin, Position>(new Position(1,1));
        //bob.All<Begin, Position>(new Position(3,3));
        //ann.All<Eats, People>(new Eats { Amount = 10 });
        //bob.All<Eats, People>(new Eats { Amount = 25 });
        //var filter = world1.Filter<Pair1<Eats, People>, Pair2<Begin, Position>>().Build();

        //foreach (var _data in filter)
        //{
        //    ref var pos = ref _data.item2.GetValue();
        //    ref var eats = ref _data.item1.GetValue();
        //    eats.Amount++;

        //    pos.x++;
        //    pos.y++;

        //    Console.WriteLine(pos.x);
        //    Console.WriteLine(pos.y);
        //    Console.WriteLine(eats.Amount);
        //}
        #endregion
    }
}