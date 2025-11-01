using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class CopperArmorGem : BaseArmorGem<CopperArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.CopperHelmet;
    public override int BodyArmorID => ArmorIDs.Body.CopperChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.CopperGreaves;

    public override int HeadItemID => ItemID.CopperHelmet;
    public override int BodyItemID => ItemID.CopperChainmail;
    public override int? LegsItemID => ItemID.CopperGreaves;
}

internal sealed class TinArmorGem : BaseArmorGem<TinArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.TinHelmet;
    public override int BodyArmorID => ArmorIDs.Body.TinChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.TinGreaves;

    public override int HeadItemID => ItemID.TinHelmet;
    public override int BodyItemID => ItemID.TinChainmail;
    public override int? LegsItemID => ItemID.TinGreaves;
}

internal sealed class IronArmorGem : BaseArmorGem<IronArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.IronHelmet;
    public override int BodyArmorID => ArmorIDs.Body.IronChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.IronGreaves;

    public override int HeadItemID => ItemID.IronHelmet;
    public override int BodyItemID => ItemID.IronChainmail;
    public override int? LegsItemID => ItemID.IronGreaves;
}

internal sealed class LeadArmorGem : BaseArmorGem<LeadArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.LeadHelmet;
    public override int BodyArmorID => ArmorIDs.Body.LeadChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.LeadGreaves;

    public override int HeadItemID => ItemID.LeadHelmet;
    public override int BodyItemID => ItemID.LeadChainmail;
    public override int? LegsItemID => ItemID.LeadGreaves;
}

internal sealed class SilverArmorGem : BaseArmorGem<SilverArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SilverHelmet;
    public override int BodyArmorID => ArmorIDs.Body.SilverChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.SilverGreaves;

    public override int HeadItemID => ItemID.SilverHelmet;
    public override int BodyItemID => ItemID.SilverChainmail;
    public override int? LegsItemID => ItemID.SilverGreaves;
}

internal sealed class TungstenArmorGem : BaseArmorGem<TungstenArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.TungstenHelmet;
    public override int BodyArmorID => ArmorIDs.Body.TungstenChainmail;
    public override int? LegsArmorID => ArmorIDs.Legs.TungstenGreaves;

    public override int HeadItemID => ItemID.TungstenHelmet;
    public override int BodyItemID => ItemID.TungstenChainmail;
    public override int? LegsItemID => ItemID.TungstenGreaves;
}

internal sealed class AshWoodArmorGem : BaseArmorGem<AshWoodArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.AshWoodHelmet;
    public override int BodyArmorID => ArmorIDs.Body.AshWoodBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.AshWoodGreaves;

    public override int HeadItemID => ItemID.AshWoodHelmet;
    public override int BodyItemID => ItemID.AshWoodBreastplate;
    public override int? LegsItemID => ItemID.AshWoodGreaves;
}


internal sealed class ObsidianArmorGem : BaseArmorGem<ObsidianArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.ObsidianOutlawHat;
    public override int BodyArmorID => ArmorIDs.Body.ObsidianLongcoat;
    public override int? LegsArmorID => ArmorIDs.Legs.ObsidianPants;

    public override int HeadItemID => ItemID.ObsidianHelm;
    public override int BodyItemID => ItemID.ObsidianShirt;
    public override int? LegsItemID => ItemID.ObsidianPants;
}
