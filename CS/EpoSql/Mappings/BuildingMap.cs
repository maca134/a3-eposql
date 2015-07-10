#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class BuildingMap : ClassMap<Building>
    {
        public BuildingMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Class, "class");
            Map(x => x.Position, "position");
            Map(x => x.StorageId, "storageid");
            Map(x => x.PlayerId, "playerid");
            Map(x => x.Texture, "texture");
            Map(x => x.Damage, "damage");
            Map(x => x.Modified, "modified");
        }
    }
}