#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class VehicleLockMap : ClassMap<VehicleLock>
    {
        public VehicleLockMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Owner, "owner");
            Map(x => x.Modified, "modified");
        }
    }
}