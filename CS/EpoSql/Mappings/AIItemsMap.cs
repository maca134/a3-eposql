#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class AiItemsMap : ClassMap<AiItems>
    {
        public AiItemsMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Items, "items");
            Map(x => x.Count, "count");
            Map(x => x.Modified, "modified");
        }
    }
}