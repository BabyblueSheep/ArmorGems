using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArmorGems;

internal sealed class BaseArmorGem : ModItem
{
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

    private static Player _dummyPlayer = new Player();

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
            HeadArmorItem = armorGemSource.HeadArmorItem;
            BodyArmorItem = armorGemSource.BodyArmorItem;
            LegsArmorItem = armorGemSource.LegsArmorItem;

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

        Item.SetNameOverride(Mod.GetLocalization("Items.BaseArmorGem.DisplayNameWithSet").WithFormatArgs(_commonItemPrefix).Value);
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
                        //Weird way to average colors.
                        //Calculating the average of square roots...
                        //...and then use the square root of the average for a brighter result.
                        //Not the best way, but I'm not sure what else to do.
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
                averageColorHsl.Z = float.Min(1f, averageColorHsl.Z + 0.25f);
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
