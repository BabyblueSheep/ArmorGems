using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class GoldArmorGem : BaseArmorGem<GoldArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.GoldHelmet;
    public override int BodyArmorID => ArmorIDs.Body.GoldChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.GoldGreaves;

    public override int HeadItemID => ItemID.GoldHelmet;
    public override int BodyItemID => ItemID.GoldChainmail;
    public override int? LegsItemID => ItemID.GoldGreaves;
}

internal sealed class PlatinumArmorGem : BaseArmorGem<PlatinumArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.PlatinumHelmet;
    public override int BodyArmorID => ArmorIDs.Body.PlatinumChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.PlatinumGreaves;

    public override int HeadItemID => ItemID.PlatinumHelmet;
    public override int BodyItemID => ItemID.PlatinumChainmail;
    public override int? LegsItemID => ItemID.PlatinumGreaves;
}

internal sealed class GladiatorArmorGem : BaseArmorGem<GladiatorArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.GladiatorHelmet;
    public override int BodyArmorID => ArmorIDs.Body.GladiatorBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.GladiatorLeggings;

    public override int HeadItemID => ItemID.GladiatorHelmet;
    public override int BodyItemID => ItemID.GladiatorBreastplate;
    public override int? LegsItemID => ItemID.GladiatorLeggings;
}

internal sealed class HuntressArmorGem : BaseArmorGem<HuntressArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.HuntressWig;
    public override int BodyArmorID => ArmorIDs.Body.HuntressJerkin;
    public override int? LegsArmorID => ArmorIDs.Legs.HuntressPantsMale;

    public override int HeadItemID => ItemID.HuntressWig;
    public override int BodyItemID => ItemID.HuntressJerkin;
    public override int? LegsItemID => ItemID.HuntressPants;
}

internal sealed class RedRidingArmorGem : BaseArmorGem<RedRidingArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.RedRidingHuntress;
    public override int BodyArmorID => ArmorIDs.Body.RedRidingHuntress;
    public override int? LegsArmorID => ArmorIDs.Legs.RedRidingHuntress;

    public override int HeadItemID => ItemID.HuntressAltHead;
    public override int BodyItemID => ItemID.HuntressAltShirt;
    public override int? LegsItemID => ItemID.HuntressAltPants;
}

internal sealed class SquireArmorGem : BaseArmorGem<SquireArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SquireGreatHelm;
    public override int BodyArmorID => ArmorIDs.Body.SquirePlating;
    public override int? LegsArmorID => ArmorIDs.Legs.SquireGreaves;

    public override int HeadItemID => ItemID.SquireGreatHelm;
    public override int BodyItemID => ItemID.SquirePlating;
    public override int? LegsItemID => ItemID.SquireGreaves;
}

internal sealed class ValhallaKnightArmorGem : BaseArmorGem<ValhallaKnightArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ValhallaKnight;
    public override int BodyArmorID => ArmorIDs.Body.ValhallaKnight;
    public override int? LegsArmorID => ArmorIDs.Legs.ValhallaKnight;

    public override int HeadItemID => ItemID.SquireAltHead;
    public override int BodyItemID => ItemID.SquireAltShirt;
    public override int? LegsItemID => ItemID.SquireAltPants;
}
