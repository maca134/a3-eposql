#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    public class PlotpoleMap : ClassMap<Plotpole>
    {
        public PlotpoleMap()
        {
            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Names, "name");
            Map(x => x.Pids, "pids");
            Map(x => x.Modified, "modified");
        }
    }
}