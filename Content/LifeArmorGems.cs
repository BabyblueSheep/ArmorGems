using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class PumpkinArmorGem : BaseArmorGem<PumpkinArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.PumpkinHelmet;
    public override int BodyArmorID => ArmorIDs.Body.PumpkinBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.PumpkinLeggings;

    public override int HeadItemID => ItemID.PumpkinHelmet;
    public override int BodyItemID => ItemID.PumpkinBreastplate;
    public override int? LegsItemID => ItemID.PumpkinLeggings;
}

internal sealed class BeeArmorGem : BaseArmorGem<BeeArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.BeeHeadgear;
    public override int BodyArmorID => ArmorIDs.Body.BeeBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.BeeGreaves;

    public override int HeadItemID => ItemID.BeeHeadgear;
    public override int BodyItemID => ItemID.BeeBreastplate;
    public override int? LegsItemID => ItemID.BeeGreaves;
}

internal sealed class SpiderArmorGem : BaseArmorGem<SpiderArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.SpiderMask;
    public override int BodyArmorID => ArmorIDs.Body.SpiderBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.SpiderGreaves;

    public override int HeadItemID => ItemID.SpiderMask;
    public override int BodyItemID => ItemID.SpiderBreastplate;
    public override int? LegsItemID => ItemID.SpiderGreaves;
}

internal sealed class CactusArmorGem : BaseArmorGem<CactusArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.CactusHelmet;
    public override int BodyArmorID => ArmorIDs.Body.CactusBreastplate;
    public override int? LegsArmorID => ArmorIDs.Legs.CactusLeggings;

    public override int HeadItemID => ItemID.CactusHelmet;
    public override int BodyItemID => ItemID.CactusBreastplate;
    public override int? LegsItemID => ItemID.CactusLeggings;
}

internal sealed class TurtleArmorGem : BaseArmorGem<TurtleArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.TurtleHelmet;
    public override int BodyArmorID => ArmorIDs.Body.TurtleScaleMail;
    public override int? LegsArmorID => ArmorIDs.Legs.TurtleLeggings;

    public override int HeadItemID => ItemID.TurtleHelmet;
    public override int BodyItemID => ItemID.TurtleScaleMail;
    public override int? LegsItemID => ItemID.TurtleLeggings;
}

internal sealed class BeetleArmorGem : BaseArmorGem<BeetleArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.BeetleHelmet;
    public override int BodyArmorID => ArmorIDs.Body.BeetleScaleMail;
    public override int? LegsArmorID => ArmorIDs.Legs.BeetleLeggings;

    public override int HeadItemID => ItemID.BeetleHelmet;
    public override int BodyItemID => ItemID.BeetleScaleMail;
    public override int? LegsItemID => ItemID.BeetleLeggings;
}
