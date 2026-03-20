using MonoMod.RuntimeDetour;
using ReLogic.Peripherals.RGB;
using ReLogic.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorGems;

internal sealed class ArmorSetDetector : ILoadable
{
    private enum ArmorPieceType
    {
        Helmet = 0,
        Chestplate = 1,
        Leggings = 2
    }

    private static MethodInfo _resizeArraysMethod = typeof(ModContent).GetMethod("ResizeArrays", BindingFlags.Static | BindingFlags.NonPublic);
    private delegate void orig_ResizeArrays(bool optional);

    private static Hook _detectArmorSetsHook;

    public void Load(Mod mod)
    {
        _detectArmorSetsHook = new Hook(_resizeArraysMethod, DetectArmorSets);
    }

    public void Unload()
    {
       
    }

    private void DetectArmorSets(orig_ResizeArrays orig, bool unloading)
    {
        if (unloading)
        {
            orig(unloading);
            return;
        }

        orig(true);

        var armorGemsMod = ModLoader.GetMod("ArmorGems");

        var helmets = new List<int>();
        var chestplates = new List<int>();
        var leggings = new List<int>();

        var dummyItem = new Item();

        for (int i = 1; i < ItemID.Count; i++)
        {
            dummyItem.SetDefaults(i);

            if (dummyItem.headSlot != -1)
                helmets.Add(i);
            if (dummyItem.bodySlot != -1)
                chestplates.Add(i);
            if (dummyItem.legSlot != -1)
                leggings.Add(i);
        }

        for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++)
        {
            ModItem item = ItemLoader.GetItem(i);
            AutoloadEquip equip = item.GetType().GetAttribute<AutoloadEquip>();
            if (equip != null)
            {
                EquipType[] equipTypes = equip.equipTypes;
                foreach (EquipType equipType in equipTypes)
                {
                    if (equipType == EquipType.Head)
                        helmets.Add(item.Type);
                    if (equipType == EquipType.Body)
                        chestplates.Add(item.Type);
                    if (equipType == EquipType.Legs)
                        leggings.Add(item.Type);
                }
            }
        }

        armorGemsMod.loading = true;

        var armorSets = new List<List<(int?, int?, int?)>>();
        var registeredItemPieces = new List<int>();

        var dummyPlayer = new Player();

        var dummyPrimaryPieceItem = new Item();
        var dummySecondaryPieceItem = new Item();
        var dummyRemainingPieceItem = new Item();

        void GenerateSetsWith2Pieces(ArmorPieceType priorityPiece, ArmorPieceType secondaryPiece, ArmorPieceType remainingPiece)
        {
            var priorityPieceInt = (int)priorityPiece;
            var secondaryPieceInt = (int)secondaryPiece;
            var remainingPieceInt = (int)remainingPiece;

            var priorityPieceList = priorityPiece switch
            {
                ArmorPieceType.Helmet => helmets,
                ArmorPieceType.Chestplate => chestplates,
                ArmorPieceType.Leggings => leggings,
                _ => null,
            };

            var secondaryPieceList = secondaryPiece switch
            {
                ArmorPieceType.Helmet => helmets,
                ArmorPieceType.Chestplate => chestplates,
                ArmorPieceType.Leggings => leggings,
                _ => null,
            };

            foreach (var priorityPieceId in priorityPieceList)
            {
                if (priorityPieceId < ItemID.Count)
                    dummyPrimaryPieceItem.SetDefaults(priorityPieceId);
                else
                    dummyPrimaryPieceItem.type = priorityPieceId;

                foreach (var secondaryPieceId in secondaryPieceList)
                {
                    if (secondaryPieceId < ItemID.Count)
                        dummySecondaryPieceItem.SetDefaults(secondaryPieceId);
                    else
                        dummySecondaryPieceItem.type = secondaryPieceId;

                    dummyPlayer.armor[priorityPieceInt] = dummyPrimaryPieceItem;
                    dummyPlayer.armor[secondaryPieceInt] = dummySecondaryPieceItem;
                    dummyPlayer.armor[remainingPieceInt] = dummyRemainingPieceItem;

                    if (priorityPieceId < ItemID.Count && secondaryPieceId < ItemID.Count)
                    {
                        dummyPlayer.head = dummyPlayer.armor[0].headSlot;
                        dummyPlayer.body = dummyPlayer.armor[1].bodySlot;
                        dummyPlayer.legs = dummyPlayer.armor[2].legSlot;

                        dummyPlayer.UpdateArmorSets(0); //The parameter is unused.

                        if (dummyPlayer.setBonus == "") continue;
                    }
                    else
                    {
                        var dummyHelmet = dummyPlayer.armor[0];
                        var dummyChestplate = dummyPlayer.armor[1];
                        var dummyLeggings = dummyPlayer.armor[2];

                        var isArmorSet = false;
                        if (ItemLoader.GetItem(dummyHelmet.type) != null && ItemLoader.GetItem(dummyHelmet.type).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;
                        if (ItemLoader.GetItem(dummyChestplate.type) != null && ItemLoader.GetItem(dummyChestplate.type).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;
                        if (ItemLoader.GetItem(dummyLeggings.type) != null && ItemLoader.GetItem(dummyLeggings.type).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;

                        if (!isArmorSet) continue;
                    }

                    int? helmetId = null;
                    int? chestplateId = null;
                    int? leggingsId = null;

                    switch (priorityPiece)
                    {
                        case ArmorPieceType.Helmet:
                            helmetId = priorityPieceId;
                            break;
                        case ArmorPieceType.Chestplate:
                            chestplateId = priorityPieceId;
                            break;
                        case ArmorPieceType.Leggings:
                            leggingsId = priorityPieceId;
                            break;
                    }
                    switch (secondaryPiece)
                    {
                        case ArmorPieceType.Helmet:
                            helmetId = secondaryPieceId;
                            break;
                        case ArmorPieceType.Chestplate:
                            chestplateId = secondaryPieceId;
                            break;
                        case ArmorPieceType.Leggings:
                            leggingsId = secondaryPieceId;
                            break;
                    }

                    registeredItemPieces.Add(priorityPieceId);
                    registeredItemPieces.Add(secondaryPieceId);

                    var setIsPartOfExistingCategory = false;
                    for (int i = 0; i < armorSets.Count; i++)
                    {
                        for (int j = 0; j < armorSets[i].Count; j++)
                        {
                            if (armorSets[i][j].Item1 == helmetId || armorSets[i][j].Item2 == chestplateId || armorSets[i][j].Item3 == leggingsId)
                            {
                                armorSets[i].Add((helmetId, chestplateId, leggingsId));
                                setIsPartOfExistingCategory = true;
                                break;
                            }
                        }
                        if (setIsPartOfExistingCategory)
                        {
                            break;
                        }
                    }
                    if (!setIsPartOfExistingCategory)
                    {
                        armorSets.Add([(helmetId, chestplateId, leggingsId)]);
                    }
                }
            }
        }

        GenerateSetsWith2Pieces(ArmorPieceType.Helmet, ArmorPieceType.Chestplate, ArmorPieceType.Leggings);
        GenerateSetsWith2Pieces(ArmorPieceType.Helmet, ArmorPieceType.Leggings, ArmorPieceType.Chestplate);
        GenerateSetsWith2Pieces(ArmorPieceType.Chestplate, ArmorPieceType.Leggings, ArmorPieceType.Helmet);

        var dummyHelmet = new Item();
        var dummyChestplate = new Item();
        var dummyLeggings = new Item();

        foreach (var helmetId in helmets)
        {
            if (helmetId < ItemID.Count)
                dummyHelmet.SetDefaults(helmetId);
            else
                dummyHelmet.type = helmetId;

            foreach (var chestplateId in chestplates)
            {
                if (chestplateId < ItemID.Count)
                    dummyChestplate.SetDefaults(chestplateId);
                else
                    dummyChestplate.type = chestplateId;

                foreach (var leggingsId in leggings)
                {
                    if (leggingsId < ItemID.Count)
                        dummyLeggings.SetDefaults(leggingsId);
                    else
                        dummyLeggings.type = leggingsId;

                    if (registeredItemPieces.Contains(helmetId))     continue;
                    if (registeredItemPieces.Contains(chestplateId)) continue;
                    if (registeredItemPieces.Contains(leggingsId))   continue;

                    if (helmetId < ItemID.Count && chestplateId < ItemID.Count && leggingsId < ItemID.Count)
                    {
                        dummyPlayer.armor[0] = dummyHelmet;
                        dummyPlayer.armor[1] = dummyChestplate;
                        dummyPlayer.armor[2] = dummyLeggings;

                        dummyPlayer.head = dummyPlayer.armor[0].headSlot;
                        dummyPlayer.body = dummyPlayer.armor[1].bodySlot;
                        dummyPlayer.legs = dummyPlayer.armor[2].legSlot;

                        dummyPlayer.UpdateArmorSets(0); //The parameter is unused.

                        if (dummyPlayer.setBonus == "") continue;
                    }
                    else
                    {
                        var isArmorSet = false;
                        if (ItemLoader.GetItem(helmetId) != null && ItemLoader.GetItem(helmetId).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;
                        if (ItemLoader.GetItem(chestplateId) != null && ItemLoader.GetItem(chestplateId).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;
                        if (ItemLoader.GetItem(leggingsId) != null && ItemLoader.GetItem(leggingsId).IsArmorSet(dummyHelmet, dummyChestplate, dummyLeggings))
                            isArmorSet = true;

                        if (!isArmorSet) continue;
                    }

                    var setIsPartOfExistingCategory = false;
                    for (int i = 0; i < armorSets.Count; i++)
                    {
                        for (int j = 0; j < armorSets[i].Count; j++)
                        {
                            var isSetShared = 0;
                            if (armorSets[i][j].Item1 == helmetId)     isSetShared++;
                            if (armorSets[i][j].Item2 == chestplateId) isSetShared++;
                            if (armorSets[i][j].Item3 == leggingsId)   isSetShared++;
                            if (isSetShared >= 2)
                            {
                                armorSets[i].Add((helmetId, chestplateId, leggingsId));
                                setIsPartOfExistingCategory = true;
                                break;
                            }
                        }
                        if (setIsPartOfExistingCategory)
                        {
                            break;
                        }
                    }
                    if (!setIsPartOfExistingCategory)
                    {
                        armorSets.Add([(helmetId, chestplateId, leggingsId)]);
                    }
                }
            }
        }

        armorGemsMod.Logger.Info($"{armorSets.Count}");
        foreach (var setCategory in armorSets)
        {
            foreach (var set in setCategory)
            {
                armorGemsMod.Logger.Info($"{set.Item1} {set.Item2} {set.Item3}");
            }
            armorGemsMod.Logger.Info(";");
        }

        armorGemsMod.loading = false;

        orig(unloading);
    }
}
