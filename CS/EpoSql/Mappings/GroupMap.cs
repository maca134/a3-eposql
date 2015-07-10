#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Level, "level");
            Map(x => x.GroupName, "groupname");
            Map(x => x.LeaderName, "leader");
            Map(x => x.Mods, "mods");
            Map(x => x.Members, "members");
            Map(x => x.Modified, "modified");
        }
    }
}