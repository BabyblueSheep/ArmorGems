using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArmorGems;

internal abstract class BaseArmorGem : ModItem
{
    public abstract string NameKey { get; }

    internal sealed class EntitySource_ArmorGemEnchantment : EntitySource_Parent
    {
        public Item HeadArmorItem { get; set; }
        public Item BodyArmorItem { get; set; }
        public Item LegsArmorItem { get; set; }

        public EntitySource_ArmorGemEnchantment(Entity entity, Item headArmorItem, Item bodyArmorItem, Item legsArmorItem, string? context = null) : base(entity, context)
        {
            HeadArmorItem = headArmorItem;
            BodyArmorItem = bodyArmorItem;
            LegsArmorItem = legsArmorItem;
        }
    }

    public Item HeadArmorItem { get; set; }
    public Item BodyArmorItem { get; set; }
    public Item LegsArmorItem { get; set; }

    private static readonly Player _dummyPlayer = new Player();

    private string _commonItemPrefix = "";
    private string _setBonusText = "";

    public override string Texture => "ArmorGems/Assets/BaseArmorGem";

    public override ModItem Clone(Item item)
    {
        var clone = (BaseArmorGem)base.Clone(item);
        clone.HeadArmorItem = HeadArmorItem?.Clone();
        clone.BodyArmorItem = BodyArmorItem?.Clone();
        clone.LegsArmorItem = LegsArmorItem?.Clone();
        clone.RefreshFieldsDerivedFromArmorSet();
        return clone;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_ArmorGemEnchantment armorGemSource)
        {
            HeadArmorItem = armorGemSource.HeadArmorItem.Clone();
            BodyArmorItem = armorGemSource.BodyArmorItem.Clone();
            LegsArmorItem = armorGemSource.LegsArmorItem.Clone();

            RefreshFieldsDerivedFromArmorSet();
        }
    }

    public void RefreshFieldsDerivedFromArmorSet()
    {
        Item.rare = BodyArmorItem.rare;
        Item.value = HeadArmorItem.value + BodyArmorItem.value + LegsArmorItem.value;

        #region Item name
        var headName = Lang.GetItemNameValue(HeadArmorItem.type);
        var bodyName = Lang.GetItemNameValue(BodyArmorItem.type);
        var legsName = Lang.GetItemNameValue(LegsArmorItem.type);

        var minimumStringLength = Math.Min(headName.Length, Math.Min(bodyName.Length, legsName.Length));

        var commonPrefix = new StringBuilder();

        for (int i = 0; i < minimumStringLength; i++)
        {
            if (headName[i] == bodyName[i] && headName[i] == legsName[i])
                commonPrefix.Append(headName[i]);
            else
                break;
        }
        _commonItemPrefix = commonPrefix.ToString().Trim();

        if (_commonItemPrefix.Length <= 2)
        {
            _commonItemPrefix = bodyName.Split(' ')[0];
        }

        Item.SetNameOverride(Mod.GetLocalization($"Items.BaseArmorGem.{NameKey}").WithFormatArgs(_commonItemPrefix).Value);
        #endregion

        #region Item description

        _dummyPlayer.armor[0] = HeadArmorItem;
        _dummyPlayer.armor[1] = BodyArmorItem;
        _dummyPlayer.armor[2] = LegsArmorItem;

        _dummyPlayer.head = _dummyPlayer.armor[0].headSlot;
        _dummyPlayer.body = _dummyPlayer.armor[1].bodySlot;
        _dummyPlayer.legs = _dummyPlayer.armor[2].legSlot;

        _dummyPlayer.UpdateArmorSets(0);

        _setBonusText = _dummyPlayer.setBonus;

        #endregion

        if (!Main.dedServ)
        {
            Main.QueueMainThreadAction(() =>
            {
                var bodyTextureAsset = TextureAssets.Item[BodyArmorItem.type];
                if (bodyTextureAsset == null)
                    return;
                var itemTexture = bodyTextureAsset.Value;
                var textureData = new Color[itemTexture.Width * itemTexture.Height];
                itemTexture.GetData(textureData);

                var averageRed = 0f;
                var averageGreen = 0f;
                var averageBlue = 0f;
                var colorCount = 0;

                foreach (Color color in textureData)
                {
                    if (color.A > 0)
                    {
                        averageRed += MathF.Sqrt(color.R / 255f);
                        averageGreen += MathF.Sqrt(color.G / 255f);
                        averageBlue += MathF.Sqrt(color.B / 255f);
                        colorCount++;
                    }
                }

                averageRed /= colorCount;
                averageGreen /= colorCount;
                averageBlue /= colorCount;

                var averageColorHsl = Main.rgbToHsl(new Color(averageRed, averageGreen, averageBlue));
                averageColorHsl.Y = float.Min(1f, averageColorHsl.Y + 0.5f);
                averageColorHsl.Z = float.Min(1f, averageColorHsl.Z + 0.1f);
                var averageColor = Main.hslToRgb(averageColorHsl);

                Item.color = averageColor;
            });
        }
    }

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 22;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var savedHeadItem = player.armor[0];
        var savedBodyItem = player.armor[1];
        var savedLegsItem = player.armor[2];

        player.armor[0] = HeadArmorItem;
        player.armor[1] = BodyArmorItem;
        player.armor[2] = LegsArmorItem;

        player.head = player.armor[0].headSlot;
        player.body = player.armor[1].bodySlot;
        player.legs = player.armor[2].legSlot;

        player.UpdateArmorSets(player.whoAmI);

        _setBonusText = player.setBonus;

        player.armor[0] = savedHeadItem;
        player.armor[1] = savedBodyItem;
        player.armor[2] = savedLegsItem;

        player.head = player.armor[0].headSlot;
        player.body = player.armor[1].bodySlot;
        player.legs = player.armor[2].legSlot;



        //See SpecificArmorFixes.cs for details.
        var isChlorophyte = (HeadArmorItem.type == ItemID.ChlorophyteMask || HeadArmorItem.type == ItemID.ChlorophyteHelmet || HeadArmorItem.type == ItemID.ChlorophyteHeadgear)
            && BodyArmorItem.type == ItemID.ChlorophytePlateMail && LegsArmorItem.type == ItemID.ChlorophyteGreaves;
        if (isChlorophyte)
        {
            player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasChlorophyte = true;
        }
        var isSolar = HeadArmorItem.type == ItemID.SolarFlareHelmet && BodyArmorItem.type == ItemID.SolarFlareBreastplate && LegsArmorItem.type == ItemID.SolarFlareLeggings;
        if (isSolar)
        {
            player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasSolar = true;
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "ArmorGemTooltip", _setBonusText));
    }

    public override void SaveData(TagCompound tag)
    {
        tag["HeadArmorItem"] = HeadArmorItem;
        tag["BodyArmorItem"] = BodyArmorItem;
        tag["LegsArmorItem"] = LegsArmorItem;
    }

    public override void LoadData(TagCompound tag)
    {
        HeadArmorItem = tag.Get<Item>("HeadArmorItem");
        BodyArmorItem = tag.Get<Item>("BodyArmorItem");
        LegsArmorItem = tag.Get<Item>("LegsArmorItem");

        RefreshFieldsDerivedFromArmorSet();
    }
}

internal sealed class AwkwardArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameAwkward";
}
internal sealed class MundaneArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameMundane";
}
internal sealed class ThickArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameThick";
}

internal sealed class CrackedArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameCracked";
}
internal sealed class ChippedArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameChipped";
}
internal sealed class PolishedArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNamePolished";
}
internal sealed class ShinyArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameShiny";
}
internal sealed class DullArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameDull";
}

internal sealed class PrehistoricArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNamePrehistoric";
}
internal sealed class AncientArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameAncient";
}
internal sealed class ModernArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameModern";
}
internal sealed class FuturisticArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameFuturistic";
}

internal sealed class CursedArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameCursed";
}
internal sealed class BlessedArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameBlessed";
}

internal sealed class GoodArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameGood";
}
internal sealed class NeutralArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameNeutral";
}
internal sealed class BadArmorGem : BaseArmorGem
{
    public override string NameKey => "DisplayNameBad";
}