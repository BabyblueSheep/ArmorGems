using Terraria.ID;

namespace ArmorGems.Content;

internal class MeteorArmorGem : BaseArmorGem<MeteorArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.MeteorHelmet;
    public override int BodyArmorID => ArmorIDs.Body.MeteorSuit;
    public override int LegsArmorID => ArmorIDs.Legs.MeteorLeggings;

    public override int HeadItemID => ItemID.MeteorHelmet;
    public override int BodyItemID => ItemID.MeteorSuit;
    public override int LegsItemID => ItemID.MeteorLeggings;
}

internal class SolarArmorGem : BaseArmorGem<SolarArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SolarFlareHelmet;
    public override int BodyArmorID => ArmorIDs.Body.SolarFlareBreastplate;
    public override int LegsArmorID => ArmorIDs.Legs.SolarFlareLeggings;

    public override int HeadItemID => ItemID.SolarFlareHelmet;
    public override int BodyItemID => ItemID.SolarFlareBreastplate;
    public override int LegsItemID => ItemID.SolarFlareLeggings;
}

internal class VortexArmorGem : BaseArmorGem<VortexArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.VortexHelmet;
    public override int BodyArmorID => ArmorIDs.Body.VortexBreastplate;
    public override int LegsArmorID => ArmorIDs.Legs.VortexLeggings;

    public override int HeadItemID => ItemID.VortexHelmet;
    public override int BodyItemID => ItemID.VortexBreastplate;
    public override int LegsItemID => ItemID.VortexLeggings;
}

internal class NebulaArmorGem : BaseArmorGem<NebulaArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.NebulaHelmet;
    public override int BodyArmorID => ArmorIDs.Body.NebulaBreastplate;
    public override int LegsArmorID => ArmorIDs.Legs.NebulaLeggings;

    public override int HeadItemID => ItemID.NebulaHelmet;
    public override int BodyItemID => ItemID.NebulaBreastplate;
    public override int LegsItemID => ItemID.NebulaLeggings;
}

internal class StardustArmorGem : BaseArmorGem<StardustArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.StardustHelmet;
    public override int BodyArmorID => ArmorIDs.Body.StardustPlate;
    public override int LegsArmorID => ArmorIDs.Legs.StardustLeggings;

    public override int HeadItemID => ItemID.StardustHelmet;
    public override int BodyItemID => ItemID.StardustBreastplate;
    public override int LegsItemID => ItemID.StardustLeggings;
}
