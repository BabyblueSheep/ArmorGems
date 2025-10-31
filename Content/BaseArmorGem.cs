using ArmorGems.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorGems.Content;

internal abstract class BaseArmorGem<T> : ModItem
{
    public abstract int HeadID { get; }
    public abstract int BodyID { get; }
    public abstract int LegsID { get; }

    public static string SetBonusKey;
    public override LocalizedText Tooltip => Language.GetText(SetBonusKey ?? "placeholder");

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(0, 30, 0, 0);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        int savedHead = player.head;
        player.head = HeadID;
        int savedBody = player.body;
        player.body = BodyID;
        int savedLegs = player.legs;
        player.legs = LegsID;

        player.UpdateArmorSets(player.whoAmI);

        player.head = savedHead;
        player.body = savedBody;
        player.legs = savedLegs;
    }
}

internal sealed class ArmorGemTooltipLoader : ModSystem
{
    private static string CapturedSetBonusKey { get; set; }

    public override void SetStaticDefaults()
    {
        On_Language.GetTextValue_string += CaptureSetBonus;

        Player dummyPlayer = new Player();

        Type baseArmorGemType = typeof(BaseArmorGem<int>).GetGenericTypeDefinition();

        foreach (ModItem modItem in ItemLoader.items)
        {
            Type modItemType = modItem.GetType();

            if (modItemType.BaseType.IsGenericType && modItemType.BaseType.GetGenericTypeDefinition() == baseArmorGemType)
            {
                dummyPlayer.head = (int)modItemType.GetProperty("HeadID").GetValue(modItem);
                dummyPlayer.body = (int)modItemType.GetProperty("BodyID").GetValue(modItem);
                dummyPlayer.legs = (int)modItemType.GetProperty("LegsID").GetValue(modItem);

                dummyPlayer.UpdateArmorSets(0);

                FieldInfo field = modItemType.GetField("SetBonusKey", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                field.SetValue(null, CapturedSetBonusKey);
            }
        }

        On_Language.GetTextValue_string -= CaptureSetBonus;
    }

    private string CaptureSetBonus(On_Language.orig_GetTextValue_string orig, string key)
    {
        CapturedSetBonusKey = key;
        return orig(key);
    }
}