#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class StorageMap : ClassMap<Storage>
    {
        public StorageMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Class, "class");
            Map(x => x.Position, "position");
            Map(x => x.Damage, "damage");
            Map(x => x.Inventory, "inventory").CustomSqlType("TEXT");
            Map(x => x.Texture, "texture");
            Map(x => x.Owners, "owners");
            Map(x => x.Parent, "parent");
            Map(x => x.Modified, "modified");
        }
    }
}