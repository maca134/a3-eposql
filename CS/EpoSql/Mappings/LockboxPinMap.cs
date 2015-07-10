#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class LockboxPinMap : ClassMap<LockboxPin>
    {
        public LockboxPinMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Pin, "pin");
        }
    }
}