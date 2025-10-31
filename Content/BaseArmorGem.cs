using ArmorGems.Content;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorGems.Content;

internal abstract class BaseArmorGem<T> : ModItem
{
    public abstract int HeadArmorID { get; }
    public abstract int BodyArmorID { get; }
    public abstract int LegsArmorID { get; }

    public abstract int HeadItemID { get; }
    public abstract int BodyItemID { get; }
    public abstract int LegsItemID { get; }

    public static string SetBonusKey;
    public override LocalizedText Tooltip => Language.GetText(SetBonusKey ?? "placeholder");

    public override LocalizedText DisplayName
    {
        get
        {
            string headName = Lang.GetItemNameValue(HeadItemID);
            string bodyName = Lang.GetItemNameValue(BodyItemID);
            string legsName = Lang.GetItemNameValue(LegsItemID);

            int minimumStringLength = Math.Min(headName.Length, Math.Min(bodyName.Length, legsName.Length));

            StringBuilder commonPrefix = new StringBuilder();

            for (int i = 0; i < minimumStringLength; i++)
            {
                if (headName[i] == bodyName[i] && headName[i] == legsName[i])
                {
                    commonPrefix.Append(headName[i]);
                }
                else
                {
                    break;
                }
            }

            return Mod.GetLocalization("Items.GemDisplayName").WithFormatArgs(commonPrefix);
        }
    }

    public override string Texture => "ArmorGems/Content/BaseArmorGem";

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
        player.head = HeadArmorID;
        int savedBody = player.body;
        player.body = BodyArmorID;
        int savedLegs = player.legs;
        player.legs = LegsArmorID;

        player.UpdateArmorSets(player.whoAmI);

        player.head = savedHead;
        player.body = savedBody;
        player.legs = savedLegs;
    }
}

internal sealed class ArmorGemAutoLoader : ModSystem
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
                dummyPlayer.head = (int)modItemType.GetProperty("HeadArmorID").GetValue(modItem);
                dummyPlayer.body = (int)modItemType.GetProperty("BodyArmorID").GetValue(modItem);
                dummyPlayer.legs = (int)modItemType.GetProperty("LegsArmorID").GetValue(modItem);

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