#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    public class LogMap : ClassMap<Log>
    {
        public LogMap()
        {
            Id(x => x.Id, "id");
            Map(x => x.Type, "type");
            Map(x => x.Entry, "entry");
            Map(x => x.Modified, "modified");
        }
    }
}