using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class CrimsonArmorGem : BaseArmorGem<CrimsonArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.CrimsonHelmet;
    public override int BodyArmorID => ArmorIDs.Body.CrimsonScalemail;
    public override int? LegsArmorID => ArmorIDs.Legs.CrimsonGreaves;

    public override int HeadItemID => ItemID.CrimsonHelmet;
    public override int BodyItemID => ItemID.CrimsonScalemail;
    public override int? LegsItemID => ItemID.CrimsonGreaves;
}

internal sealed class MoltenArmorGem : BaseArmorGem<MoltenArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.MoltenHelmet;
    public override int BodyArmorID => ArmorIDs.Body.MoltenBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.MoltenGreaves;

    public override int HeadItemID => ItemID.MoltenHelmet;
    public override int BodyItemID => ItemID.MoltenBreastplate;
    public override int? LegsItemID => ItemID.MoltenGreaves;
}

internal sealed class RainArmorGem : BaseArmorGem<RainArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.RainHat;
    public override int BodyArmorID => ArmorIDs.Body.RainCoat;
    public override int? LegsArmorID => null;

    public override int HeadItemID => ItemID.RainHat;
    public override int BodyItemID => ItemID.RainCoat;
    public override int? LegsItemID => null;
}

internal sealed class SnowArmorGem : BaseArmorGem<SnowArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.EskimoHood;
    public override int BodyArmorID => ArmorIDs.Body.EskimoCoat;
    public override int? LegsArmorID => ArmorIDs.Legs.EskimoPants;

    public override int HeadItemID => ItemID.EskimoHood;
    public override int BodyItemID => ItemID.EskimoCoat;
    public override int? LegsItemID => ItemID.EskimoPants;
}

internal sealed class PinkSnowArmorGem : BaseArmorGem<PinkSnowArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.PinkEskimoHood;
    public override int BodyArmorID => ArmorIDs.Body.PinkEskimoCoat;
    public override int? LegsArmorID => ArmorIDs.Legs.PinkEskimoPants;

    public override int HeadItemID => ItemID.PinkEskimoHood;
    public override int BodyItemID => ItemID.PinkEskimoCoat;
    public override int? LegsItemID => ItemID.PinkEskimoPants;
}

internal sealed class FrostArmorGem : BaseArmorGem<FrostArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.FrostHelmet;
    public override int BodyArmorID => ArmorIDs.Body.FrostBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.FrostLeggings;

    public override int HeadItemID => ItemID.FrostHelmet;
    public override int BodyItemID => ItemID.FrostBreastplate;
    public override int? LegsItemID => ItemID.FrostLeggings;
}

internal sealed class JungleArmorGem : BaseArmorGem<JungleArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.JungleHat;
    public override int BodyArmorID => ArmorIDs.Body.JungleShirt;
    public override int? LegsArmorID => ArmorIDs.Legs.JunglePants;

    public override int HeadItemID => ItemID.JungleHat;
    public override int BodyItemID => ItemID.JungleShirt;
    public override int? LegsItemID => ItemID.JunglePants;
}

internal sealed class ChlorophyteArmorGem : BaseArmorGem<ChlorophyteArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ChlorophyteHelmet;
    public override int BodyArmorID => ArmorIDs.Body.ChlorophytePlateMail;
    public override int? LegsArmorID => ArmorIDs.Legs.ChlorophyteGreaves;

    public override int HeadItemID => ItemID.ChlorophyteHelmet;
    public override int BodyItemID => ItemID.ChlorophytePlateMail;
    public override int? LegsItemID => ItemID.ChlorophyteGreaves;
}

internal sealed class ShroomiteArmorGem : BaseArmorGem<ShroomiteArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ShroomiteHelmet;
    public override int BodyArmorID => ArmorIDs.Body.ShroomiteBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.ShroomiteLeggings;

    public override int HeadItemID => ItemID.ShroomiteHelmet;
    public override int BodyItemID => ItemID.ShroomiteBreastplate;
    public override int? LegsItemID => ItemID.ShroomiteLeggings;
}
