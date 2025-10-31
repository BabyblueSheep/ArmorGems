using Terraria.ID;

namespace ArmorGems.Content;

internal class EbonwoodArmorGem : BaseArmorGem<EbonwoodArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.EbonwoodHelmet;
    public override int BodyArmorID => ArmorIDs.Body.EbonwoodBreastplate;
    public override int LegsArmorID => ArmorIDs.Legs.EbonwoodGreaves;

    public override int HeadItemID => ItemID.EbonwoodHelmet;
    public override int BodyItemID => ItemID.EbonwoodBreastplate;
    public override int LegsItemID => ItemID.EbonwoodGreaves;
}
