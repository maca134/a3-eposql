#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class BankMap : ClassMap<Bank>
    {
        public BankMap()
        {
            Id(x => x.Id, "playerid").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.Amount, "amount");
            Map(x => x.Modified, "modified");
        }
    }
}