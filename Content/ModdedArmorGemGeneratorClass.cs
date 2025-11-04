using Microsoft.Build.Framework;
using MonoMod.RuntimeDetour;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
            List<int> helmets = new();
            List<int> chestplates = new();
            List<int> leggings = new();

            for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++)
            {
                ModItem item = ItemLoader.GetItem(i);
                AutoloadEquip equip = item.GetType().GetAttribute<AutoloadEquip>();
                if (equip != null)
                {
                    EquipType[] equipTypes = equip.equipTypes;
                    foreach (EquipType equipe in equipTypes)
                    {
                        if (equipe == EquipType.Head)
                        {
                            ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as helmet");
                            helmets.Add(item.Type);
                        }
                        if (equipe == EquipType.Body)
                        {
                            ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as chestplate");
                            chestplates.Add(item.Type);
                        }
                        if (equipe == EquipType.Legs)
                        {
                            ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as leggings");
                            leggings.Add(item.Type);
                        }
                    }
                }
            }
            ArmorGems.instance.Logger.Info("\n");

            for (int i = 0; i < helmets.Count; i++)
            {
                ModItem helmet = ItemLoader.GetItem(helmets[i]);
                for (int j = 0; j < chestplates.Count; j++)
                {
                    for (int k = 0; k < leggings.Count; k++)
                    {
                        if (helmet.IsArmorSet(helmet.Item, ItemLoader.GetItem(chestplates[j]).Item, ItemLoader.GetItem(leggings[k]).Item))
                        {
                            ArmorGems.instance.Logger.Info("Armor Set: " + helmet.Name + " " + ItemLoader.GetItem(chestplates[j]).Name + " " + ItemLoader.GetItem(leggings[k]).Name);
                            break;
                        }
                    }
                }
            }

            orig(unloading);
        }
    }
}
