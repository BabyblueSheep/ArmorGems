using Terraria.ID;

namespace ArmorGems.Content;

internal class NebulaArmorGem : BaseArmorGem<NebulaArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.NebulaHelmet;
    public override int BodyArmorID => ArmorIDs.Body.NebulaBreastplate;
    public override int LegsArmorID => ArmorIDs.Legs.NebulaLeggings;

    public override int HeadItemID => ItemID.NebulaHelmet;
    public override int BodyItemID => ItemID.NebulaBreastplate;
    public override int LegsItemID => ItemID.NebulaLeggings;
}
