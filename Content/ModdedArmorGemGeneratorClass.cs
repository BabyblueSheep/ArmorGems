using Microsoft.Build.Framework;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorGems.Content
{
    internal class ModdedArmorGemGeneratorClass : ModSystem
    {
        public static MethodInfo resizeMethod = typeof(ModContent).GetMethod("ResizeArrays", BindingFlags.Static | BindingFlags.NonPublic);
        public delegate void orig_ResizeArrays(bool optional);

        public static Hook collectArmorSetsHook;

        public override void Load()
        {
            collectArmorSetsHook = new Hook(resizeMethod, ResizeArraysAndGetArmorSetShit);
        }
        
        public static void ResizeArraysAndGetArmorSetShit(orig_ResizeArrays orig, bool unloading)
        { 
            for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++)
            {
                ModItem item1 = ItemLoader.GetItem(i);
                Item bodySlot = new();
                Item legSlot = new();
                bool isSet = false;

                for (int j = ItemID.Count; j < ItemLoader.ItemCount; j++)
                {
                    ModItem item2 = ItemLoader.GetItem(j);
                    for (int k = ItemID.Count; k < ItemLoader.ItemCount; k++)
                    {
                        ModItem item3 = ItemLoader.GetItem(k);

                        if (item1.IsArmorSet(item1.Item, item2.Item, item3.Item))
                        {
                            bodySlot = item2.Item;
                            legSlot = item3.Item;
                            isSet = true;
                        }
                    }
                }

                if (isSet)
                {
                    ArmorGems.instance.Logger.Info("Armor Set: " + item1.Name + " " + bodySlot.ModItem.Name + " " + legSlot.ModItem.Name + "\n");
                }
            }

            orig(unloading);
        }
    }
}
