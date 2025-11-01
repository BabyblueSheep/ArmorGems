using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class ForbiddenArmorGem : BaseArmorGem<ForbiddenArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.AncientBattleArmor;
    public override int BodyArmorID => ArmorIDs.Body.AncientBattleArmor;
    public override int? LegsArmorID => ArmorIDs.Legs.AncientBattleArmor;

    public override int HeadItemID => ItemID.AncientBattleArmorHat;
    public override int BodyItemID => ItemID.AncientBattleArmorShirt;
    public override int? LegsItemID => ItemID.AncientBattleArmorPants;
}

internal sealed class HallowedArmorGem : BaseArmorGem<HallowedArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.HallowedHelmet;
    public override int BodyArmorID => ArmorIDs.Body.HallowedPlateMail;
    public override int? LegsArmorID => ArmorIDs.Legs.HallowedGreaves;

    public override int HeadItemID => ItemID.HallowedHelmet;
    public override int BodyItemID => ItemID.HallowedPlateMail;
    public override int? LegsItemID => ItemID.HallowedGreaves;
}

internal sealed class AncientHallowedArmorGem : BaseArmorGem<AncientHallowedArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.AncientHallowedHelmet;
    public override int BodyArmorID => ArmorIDs.Body.AncientHallowedPlateMail;
    public override int? LegsArmorID => ArmorIDs.Legs.AncientHallowedGreaves;

    public override int HeadItemID => ItemID.AncientHallowedHelmet;
    public override int BodyItemID => ItemID.AncientHallowedPlateMail;
    public override int? LegsItemID => ItemID.AncientHallowedGreaves;
}

internal sealed class TikiArmorGem : BaseArmorGem<TikiArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.TikiMask;
    public override int BodyArmorID => ArmorIDs.Body.TikiShirt;
    public override int? LegsArmorID => ArmorIDs.Legs.TikiPants;

    public override int HeadItemID => ItemID.TikiMask;
    public override int BodyItemID => ItemID.TikiShirt;
    public override int? LegsItemID => ItemID.TikiPants;
}

internal sealed class FossilArmorGem : BaseArmorGem<FossilArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.FossilHelmet;
    public override int BodyArmorID => ArmorIDs.Body.FossilPlate;
    public override int? LegsArmorID => ArmorIDs.Legs.FossilGreaves;

    public override int HeadItemID => ItemID.FossilHelm;
    public override int BodyItemID => ItemID.FossilShirt;
    public override int? LegsItemID => ItemID.FossilPants;
}


internal sealed class SpectreArmorGem : BaseArmorGem<SpectreArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SpectreHood;
    public override int BodyArmorID => ArmorIDs.Body.SpectreRobe;
    public override int? LegsArmorID => ArmorIDs.Legs.SpectrePants;

    public override int HeadItemID => ItemID.SpectreHood;
    public override int BodyItemID => ItemID.SpectreRobe;
    public override int? LegsItemID => ItemID.SpectrePants;
}
