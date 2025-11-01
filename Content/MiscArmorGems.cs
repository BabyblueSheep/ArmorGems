using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArmorGems.Content;

internal sealed class MiningArmorGem : BaseArmorGem<MiningArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.MiningHelmet;
    public override int BodyArmorID => ArmorIDs.Body.MiningShirt;
    public override int? LegsArmorID => ArmorIDs.Legs.MiningPants;

    public override int HeadItemID => ItemID.MiningHelmet;
    public override int BodyItemID => ItemID.MiningShirt;
    public override int? LegsItemID => ItemID.MiningPants;
}

internal sealed class AnglerArmorGem : BaseArmorGem<AnglerArmorGem>
{
    public override int HeadArmorID => ArmorIDs.Head.AnglerHat;
    public override int BodyArmorID => ArmorIDs.Body.AnglerVest;
    public override int? LegsArmorID => ArmorIDs.Legs.AnglerPants;

    public override int HeadItemID => ItemID.AnglerHat;
    public override int BodyItemID => ItemID.AnglerVest;
    public override int? LegsItemID => ItemID.AnglerPants;
}
