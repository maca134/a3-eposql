#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Id(x => x.Key, "skey").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Data, "sdata").CustomSqlType("TEXT");
            Map(x => x.Modified, "modified");
        }
    }
}