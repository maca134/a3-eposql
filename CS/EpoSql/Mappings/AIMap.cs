#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class AiMap : ClassMap<Ai>
    {
        public AiMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Class, "class");
            Map(x => x.Home, "home");
            Map(x => x.Work, "work");
            Map(x => x.Modified, "modified");
        }
    }
}