#region usings

using EpoSql.Model;
using FluentNHibernate.Mapping;

#endregion

namespace EpoSql.Mappings
{
    internal class PlayerMap : ClassMap<Player>
    {
        public PlayerMap()
        {
            Id(x => x.Id, "playerid").GeneratedBy.Assigned();
            Map(x => x.Expiry, "expiry");
            Map(x => x.WorldSpace, "worldspace");
            Map(x => x.Medical, "medical");
            Map(x => x.Appearance, "appearance");
            Map(x => x.Vars, "vars");
            Map(x => x.Weapons, "weapons").CustomSqlType("TEXT");
            Map(x => x.AssignedItems, "assigneditems");
            Map(x => x.MagazinesAmmo, "magazinesammo").CustomSqlType("TEXT");
            Map(x => x.ItemsPlayer, "itemsplayer").CustomSqlType("TEXT");
            Map(x => x.Weaponsplayer, "weaponsplayer");
            Map(x => x.Group, "grp");
            Map(x => x.Revive, "revive");
            Map(x => x.Dead, "dead");
            Map(x => x.Modified, "modified");
        }
    }
}