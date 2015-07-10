#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class PlayerDataMap : ClassMap<PlayerData>
    {
        public PlayerDataMap()
        {
            Id(x => x.Id, "playerid").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Name, "name");
            Map(x => x.Modified, "modified");
        }
    }
}