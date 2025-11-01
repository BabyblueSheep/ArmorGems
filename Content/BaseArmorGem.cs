using ArmorGems.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorGems.Content;

internal abstract class BaseArmorGem<T> : ModItem
{
    public abstract int HeadArmorID { get; }
    public abstract int BodyArmorID { get; }
    public abstract int? LegsArmorID { get; }

    public abstract int HeadItemID { get; }
    public abstract int BodyItemID { get; }
    public abstract int? LegsItemID { get; }

    public static string SetBonusKey = null;
    public static object SetBonusArg0 = null;
    public override LocalizedText Tooltip
    {
        get
        {
            LocalizedText tooltip = Language.GetText(SetBonusKey ?? "");

            if (SetBonusArg0 != null)
            {
                return tooltip.WithFormatArgs(SetBonusArg0);
            }
            return tooltip;
        }
    }

    public override LocalizedText DisplayName
    {
        // TODO: cache common substring somewhere
        get
        {
            string headName = Lang.GetItemNameValue(HeadItemID);
            string bodyName = Lang.GetItemNameValue(BodyItemID);
            string legsName = Lang.GetItemNameValue(LegsItemID ?? BodyItemID);

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

    public static Color TintColor;

    public static int Rarity;
    public static int Value;

    public override string Texture => "ArmorGems/Content/BaseArmorGem";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = Rarity;
        Item.value = Value;
        Item.color = TintColor;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        int savedHead = player.head;
        player.head = HeadArmorID;
        int savedBody = player.body;
        player.body = BodyArmorID;
        int savedLegs = player.legs;
        player.legs = LegsArmorID ?? -1;

        player.UpdateArmorSets(player.whoAmI);

        player.head = savedHead;
        player.body = savedBody;
        player.legs = savedLegs;
    }
}

internal sealed class ArmorGemAutoLoader : ModSystem
{
    private static string CapturedSetBonusKey;
    private static object CapturedSetBonusArg0;

    public override void SetStaticDefaults()
    {
        On_Language.GetTextValue_string += CaptureSetBonus;
        On_Language.GetTextValue_string_object += CaptureSetBonus;  

        Player dummyPlayer = new Player();
        Item dummyItem = new Item();

        Type baseArmorGemType = typeof(BaseArmorGem<int>).GetGenericTypeDefinition();

        foreach (ModItem modItem in ItemLoader.items)
        {
            Type modItemType = modItem.GetType();

            if (modItemType.BaseType.IsGenericType && modItemType.BaseType.GetGenericTypeDefinition() == baseArmorGemType)
            {
                dummyPlayer.head = (int)modItemType.GetProperty("HeadArmorID").GetValue(modItem);
                dummyPlayer.body = (int)modItemType.GetProperty("BodyArmorID").GetValue(modItem);
                dummyPlayer.legs = (int?)modItemType.GetProperty("LegsArmorID").GetValue(modItem) ?? dummyPlayer.body;
                dummyPlayer.setBonus = "";

                dummyPlayer.UpdateArmorSets(0);

                FieldInfo setBonusKeyField = modItemType.GetField("SetBonusKey", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                FieldInfo setBonusArg0Field = modItemType.GetField("SetBonusArg0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (dummyPlayer.setBonus == "")
                {
                    setBonusKeyField.SetValue(null, null);
                    setBonusArg0Field.SetValue(null, null);
                }
                else
                {
                    setBonusKeyField.SetValue(null, CapturedSetBonusKey);
                    setBonusArg0Field.SetValue(null, CapturedSetBonusArg0);
                }



                int itemID = (int)modItemType.GetProperty("BodyItemID").GetValue(modItem);
                dummyItem.SetDefaults(itemID);

                FieldInfo rarityField = modItemType.GetField("Rarity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                rarityField.SetValue(null, dummyItem.rare);
                FieldInfo valueField = modItemType.GetField("Value", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                valueField.SetValue(null, dummyItem.value);



                Main.instance.LoadItem(itemID);
                Asset<Texture2D> itemAsset = TextureAssets.Item[itemID];
                if (itemAsset != null)
                {
                    Main.QueueMainThreadAction(() =>
                    {
                        Texture2D itemTexture = itemAsset.Value;
                        Color[] textureData = new Color[itemTexture.Width * itemTexture.Height];
                        itemTexture.GetData(textureData);

                        float averageRed = 0;
                        float averageGreen = 0;
                        float averageBlue = 0;
                        int colorCount = 0;

                        foreach (Color color in textureData)
                        {
                            if (color.A > 0)
                            {
                                // Weird way to average colors.
                                // Calculating the average of square roots...
                                // ...and then use the square root of the average for a brighter result.
                                // Not the best way, but I'm not sure what else to do.
                                averageRed += MathF.Sqrt(color.R / 255f);
                                averageGreen += MathF.Sqrt(color.G / 255f);
                                averageBlue += MathF.Sqrt(color.B / 255f);
                                colorCount++;
                            }
                        }

                        averageRed /= colorCount;
                        averageGreen /= colorCount;
                        averageBlue /= colorCount;

                        Color averageColor = new Color(MathF.Sqrt(averageRed), MathF.Sqrt(averageGreen), MathF.Sqrt(averageBlue));

                        FieldInfo colorField = modItemType.GetField("TintColor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        colorField.SetValue(null, averageColor);
                    });
                }
            }
        }

        On_Language.GetTextValue_string -= CaptureSetBonus;
        On_Language.GetTextValue_string_object -= CaptureSetBonus;
    }

    private string CaptureSetBonus(On_Language.orig_GetTextValue_string orig, string key)
    {
        CapturedSetBonusKey = key;
        return orig(key);
    }

    private string CaptureSetBonus(On_Language.orig_GetTextValue_string_object orig, string key, object arg0)
    {
        CapturedSetBonusKey = key;
        CapturedSetBonusArg0 = arg0;
        return orig(key, arg0);
    }
}