#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Class, "class");
            Map(x => x.Position, "position");
            Map(x => x.Damage, "damage");
            Map(x => x.HitPointDamage, "hpdamage").CustomSqlType("TEXT");
            Map(x => x.Fuel, "fuel");
            Map(x => x.Inventory, "inventory").CustomSqlType("TEXT");
            Map(x => x.MagazinesAmmo, "magazinesammo").CustomSqlType("TEXT");
            Map(x => x.Texture, "texture");
            Map(x => x.Modified, "modified");
        }
    }
}