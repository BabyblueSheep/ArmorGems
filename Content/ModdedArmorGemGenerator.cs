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
    internal class ModdedArmorGemGenerator : ModSystem
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
            FieldInfo modLoading = typeof(Mod).GetField("loading", BindingFlags.Instance | BindingFlags.NonPublic);
            if (modLoading != null)
            {
                modLoading.SetValue(ModLoader.GetMod("ArmorGems"), true);
                List<int> helmets = new();
                List<int> chestplates = new();
                List<int> leggings = new();
                List<ArmorSet> armorSets = new();

                // collect lists of helmets, chestplates, leggings
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
                                //ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as helmet");
                                helmets.Add(item.Type);
                            }
                            if (equipe == EquipType.Body)
                            {
                                //ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as chestplate");
                                chestplates.Add(item.Type);
                            }
                            if (equipe == EquipType.Legs)
                            {
                                //ArmorGems.instance.Logger.Info("item loaded: " + item.Name + " as leggings");
                                leggings.Add(item.Type);
                            }
                        }
                    }
                }

                // compare armor pieces to each other and get list of armor sets
                for (int i = 0; i < helmets.Count; i++)
                {
                    ModItem helmet = ItemLoader.GetItem(helmets[i]);
                    for (int j = 0; j < chestplates.Count; j++)
                    {
                        for (int k = 0; k < leggings.Count; k++)
                        {
                            if (helmet.IsArmorSet(helmet.Item, ItemLoader.GetItem(chestplates[j]).Item, ItemLoader.GetItem(leggings[k]).Item))
                            {
                                // cull sets that share 2 pieces into one
                                bool addSet = true;
                                foreach (ArmorSet set in armorSets)
                                {
                                    if (set.helmets[0] != helmet && set.chestplates[0] == ItemLoader.GetItem(chestplates[j]) && set.leggings[0] == ItemLoader.GetItem(leggings[k]))
                                    {
                                        set.helmets.Add(helmet);
                                        addSet = false;
                                    }
                                    else if (set.helmets[0] == helmet && set.chestplates[0] != ItemLoader.GetItem(chestplates[j]) && set.leggings[0] == ItemLoader.GetItem(leggings[k]))
                                    {
                                        set.chestplates.Add(ItemLoader.GetItem(chestplates[j]));
                                        addSet = false;
                                    }
                                    else if (set.helmets[0] == helmet && set.chestplates[0] == ItemLoader.GetItem(chestplates[j]) && set.leggings[0] != ItemLoader.GetItem(leggings[k]))
                                    {
                                        set.leggings.Add(ItemLoader.GetItem(leggings[k]));
                                        addSet = false;
                                    }
                                }

                                if (addSet)
                                {
                                    //ArmorGems.instance.Logger.Info("Armor Set: " + helmet.Name + " " + ItemLoader.GetItem(chestplates[j]).Name + " " + ItemLoader.GetItem(leggings[k]).Name);
                                    armorSets.Add(new ArmorSet(helmet, ItemLoader.GetItem(chestplates[j]), ItemLoader.GetItem(leggings[k])));
                                }
                                break;
                            }
                        }
                    }
                }

                // add items to game
                foreach (ArmorSet set in armorSets)
                {
                    ModdedArmorGem gem = new ModdedArmorGem(set, set.helmets[0].Name);
                    ModContent.GetInstance<ArmorGems>().AddContent(gem);
                }

                modLoading.SetValue(ModLoader.GetMod("ArmorGems"), true);
            }
            orig(unloading);
        }
    }

    public struct ArmorSet
    {
        public List<ModItem> helmets;
        public List<ModItem> chestplates;
        public List<ModItem> leggings;

        #region Constructors
        public ArmorSet(List<ModItem> helmets, List<ModItem> chestplates, List<ModItem> leggings)
        {
            this.helmets = helmets;
            this.chestplates = chestplates;
            this.leggings = leggings;
        }

        public ArmorSet(ModItem helmets, ModItem chestplates, ModItem leggings)
        {
            this.helmets = new List<ModItem> { helmets };
            this.chestplates = new List<ModItem> { chestplates };
            this.leggings = new List<ModItem> { leggings };
        }
        #endregion
    }
}
