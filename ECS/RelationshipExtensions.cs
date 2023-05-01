using ECS;

namespace ECS;

public static class RelationshipExtensions
{
    public static bool HasTarget<T>(this Relationship relationship) where T : struct =>
        relationship.world.RelationshipHasTarget<T>(relationship);

    public static bool HasTarget(this Relationship relationship, ulong target) =>
        relationship.world.RelationshipHasTarget(relationship, target);

    public static bool HasRelation<T>(this Relationship relationship) where T : struct =>
        relationship.world.RelationshipHasRelation<T>(relationship);

    public static bool HasRelation(this Relationship relationship, ulong target) =>
        relationship.world.RelationshipHasRelation(relationship, target);

    public static bool Is<T1, T2>(this Relationship relationship) where T1 : struct where T2 : struct =>
        relationship.world.RelationshipIs<T1, T2>(relationship);

    public static bool Is<T>(this Relationship relationship, ulong target) where T : struct =>
        relationship.world.RelationshipIs<T>(relationship, target);

    public static bool Is(this Relationship relationship, ulong relation, ulong target) =>
        relationship.world.RelationshipIs(relationship, relation, target);

    public static ulong GetTarget(this Relationship relationship) =>
        relationship.world.GetRelationshipTarget(relationship);

    public static ulong GetRelation(this Relationship relationship) =>
        relationship.world.GetRelationshipRelation(relationship);
}
