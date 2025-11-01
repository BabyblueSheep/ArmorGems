using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class ShadowArmorGem : BaseArmorGem<ShadowArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ShadowHelmet;
    public override int BodyArmorID => ArmorIDs.Body.ShadowScalemail;
    public override int? LegsArmorID => ArmorIDs.Legs.ShadowGreaves;

    public override int HeadItemID => ItemID.ShadowHelmet;
    public override int BodyItemID => ItemID.ShadowScalemail;
    public override int? LegsItemID => ItemID.ShadowGreaves;
}

internal sealed class AncientShadowArmorGem : BaseArmorGem<AncientShadowArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.AncientShadowHelmet;
    public override int BodyArmorID => ArmorIDs.Body.AncientShadowScalemail;
    public override int? LegsArmorID => ArmorIDs.Legs.AncientShadowGreaves;

    public override int HeadItemID => ItemID.AncientShadowHelmet;
    public override int BodyItemID => ItemID.AncientShadowScalemail;
    public override int? LegsItemID => ItemID.AncientShadowGreaves;
}

internal sealed class NinjaArmorGem : BaseArmorGem<NinjaArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.NinjaHood;
    public override int BodyArmorID => ArmorIDs.Body.NinjaShirt;
    public override int? LegsArmorID => ArmorIDs.Legs.NinjaPants;

    public override int HeadItemID => ItemID.NinjaHood;
    public override int BodyItemID => ItemID.NinjaShirt;
    public override int? LegsItemID => ItemID.NinjaPants;
}

internal sealed class CrystalAssassinArmorGem : BaseArmorGem<CrystalAssassinArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.CrystalNinjaHelmet;
    public override int BodyArmorID => ArmorIDs.Body.CrystalNinjaChestplate;
    public override int? LegsArmorID => ArmorIDs.Legs.CrystalNinjaLeggings;

    public override int HeadItemID => ItemID.CrystalNinjaHelmet;
    public override int BodyItemID => ItemID.CrystalNinjaChestplate;
    public override int? LegsItemID => ItemID.CrystalNinjaLeggings;
}

internal sealed class SpookyArmorGem : BaseArmorGem<SpookyArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SpookyHelmet;
    public override int BodyArmorID => ArmorIDs.Body.SpookyBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.SpookyLeggings;

    public override int HeadItemID => ItemID.SpookyHelmet;
    public override int BodyItemID => ItemID.SpookyBreastplate;
    public override int? LegsItemID => ItemID.SpookyLeggings;
}

internal sealed class MonkArmorGem : BaseArmorGem<MonkArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.MonkBrows;
    public override int BodyArmorID => ArmorIDs.Body.MonkShirt;
    public override int? LegsArmorID => ArmorIDs.Legs.MonkPants;

    public override int HeadItemID => ItemID.MonkBrows;
    public override int BodyItemID => ItemID.MonkShirt;
    public override int? LegsItemID => ItemID.MonkPants;
}

internal sealed class ShinobiInfiltratorArmorGem : BaseArmorGem<ShinobiInfiltratorArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ShinobiInfiltrator;
    public override int BodyArmorID => ArmorIDs.Body.ShinobiInfiltrator;
    public override int? LegsArmorID => ArmorIDs.Legs.ShinobiInfiltrator;

    public override int HeadItemID => ItemID.MonkAltHead;
    public override int BodyItemID => ItemID.MonkAltShirt;
    public override int? LegsItemID => ItemID.MonkAltPants;
}

internal sealed class ApprenticeArmorGem : BaseArmorGem<ApprenticeArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ApprenticeHat;
    public override int BodyArmorID => ArmorIDs.Body.ApprenticeRobe;
    public override int? LegsArmorID => ArmorIDs.Legs.ApprenticeTrousers;

    public override int HeadItemID => ItemID.ApprenticeHat;
    public override int BodyItemID => ItemID.ApprenticeRobe;
    public override int? LegsItemID => ItemID.ApprenticeTrousers;
}

internal sealed class DarkArtistArmorGem : BaseArmorGem<DarkArtistArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ApprenticeDark;
    public override int BodyArmorID => ArmorIDs.Body.ApprenticeDark;
    public override int? LegsArmorID => ArmorIDs.Legs.ApprenticeDark;

    public override int HeadItemID => ItemID.ApprenticeAltHead;
    public override int BodyItemID => ItemID.ApprenticeAltShirt;
    public override int? LegsItemID => ItemID.ApprenticeAltPants;
}

internal sealed class NecroArmorGem : BaseArmorGem<NecroArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.NecroHelmet;
    public override int BodyArmorID => ArmorIDs.Body.NecroBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.NecroGreaves;

    public override int HeadItemID => ItemID.NecroHelmet;
    public override int BodyItemID => ItemID.NecroBreastplate;
    public override int? LegsItemID => ItemID.NecroGreaves;
}
