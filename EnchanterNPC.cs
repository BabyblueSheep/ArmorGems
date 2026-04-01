using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static ArmorGems.BaseArmorGem;

namespace ArmorGems;

[AutoloadHead]
internal sealed class EnchanterNPC : ModNPC
{
    internal sealed class EnchanterRespawnSystem : ModSystem
    {
        public static bool CanEnchanterRespawn { get; set; } = false;

        public override void ClearWorld()
        {
            CanEnchanterRespawn = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(CanEnchanterRespawn)] = CanEnchanterRespawn;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            CanEnchanterRespawn = tag.GetBool(nameof(CanEnchanterRespawn));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(CanEnchanterRespawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out bool canEnchanterRespawnCopy);
            CanEnchanterRespawn = canEnchanterRespawnCopy;
        }
    }
    
    internal sealed class CanOnlyEnchantOncePerDayModPlayer : ModPlayer
    {
        public bool EnchantedThisDay { get; set; } = false;

        public override void Load()
        {
            On_Main.UpdateTime_StartDay += ResetEnchantedThisDay;
        }

        public override void Unload()
        {
            On_Main.UpdateTime_StartDay -= ResetEnchantedThisDay;
        }

        private void ResetEnchantedThisDay(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
        {
            foreach (var player in Main.ActivePlayers)
            {
                player.GetModPlayer<CanOnlyEnchantOncePerDayModPlayer>().EnchantedThisDay = false;
            }

            orig(ref stopEvents);
        }

        public override void SaveData(TagCompound tag)
        {
            tag[nameof(EnchantedThisDay)] = EnchantedThisDay;
        }

        public override void LoadData(TagCompound tag)
        {
            EnchantedThisDay = tag.GetBool(nameof(EnchantedThisDay));
        }
    }

    private static Player _dummyPlayer;
    private static Item _dummyItem;
    
    public int NPCHeadShimmerTextureSlot { get; private set; }

    public override string Texture => "ArmorGems/Assets/EnchanterNPC";

    public override void Load()
    {
        NPCHeadShimmerTextureSlot = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");

        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Hat");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Hat_Party");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Arm");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Leg");
        
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Hat");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Hat_Party");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Arm");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Leg");

        On_AllPersonalitiesModifier.ModifyShopPrice_Relationships += MakeNPCHatePrincess;
    }

    public override void Unload()
    {
        On_AllPersonalitiesModifier.ModifyShopPrice_Relationships -= MakeNPCHatePrincess;
    }

    private void MakeNPCHatePrincess(On_AllPersonalitiesModifier.orig_ModifyShopPrice_Relationships orig, HelperInfo info, ShopHelper shopHelperInstance)
    {
        if (info.npc.type != ModContent.NPCType<EnchanterNPC>())
        {
            orig(info, shopHelperInstance);
            return;
        }
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 25;
        NPCID.Sets.ExtraFramesCount[Type] = 9;
        NPCID.Sets.AttackFrameCount[Type] = 4;

        NPCID.Sets.DangerDetectRange[Type] = 320;
        NPCID.Sets.PrettySafe[Type] = 100;
        NPCID.Sets.AttackType[Type] = 0;
        NPCID.Sets.AttackTime[Type] = 34;
        NPCID.Sets.AttackAverageChance[Type] = 30;

        NPCID.Sets.HatOffsetY[Type] = 2;

        NPCID.Sets.ShimmerTownTransform[Type] = true;

        //NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new()
        {
            Velocity = 1f,
        });

        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

        NPC.Happiness
            .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
            .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.Wizard, AffectionLevel.Love)
            .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
            .SetNPCAffection(NPCID.Angler, AffectionLevel.Hate)
            .SetNPCAffection(NPCID.Princess, AffectionLevel.Hate);

        ContentSamples.NpcBestiaryRarityStars[Type] = 3;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
			new FlavorTextBestiaryInfoElement("Mods.ArmorGems.NPCs.EnchanterNPC.Bestiary"),
        ]);
    }

    public override ITownNPCProfile TownNPCProfile()
    {
        return new Profiles.StackedNPCProfile(
            new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
            new Profiles.DefaultNPCProfile(Texture + "_Shimmer", NPCHeadShimmerTextureSlot, Texture + "_Shimmer_Party")
        );
    }

    public override List<string> SetNPCNameList()
    {
        return [
            this.GetLocalizedValue("Names.Misty"),
            this.GetLocalizedValue("Names.Carousel"),
            this.GetLocalizedValue("Names.Magicad"),
            this.GetLocalizedValue("Names.Glorks"),
            this.GetLocalizedValue("Names.Finellie"),
            this.GetLocalizedValue("Names.Jarlsova"),
            this.GetLocalizedValue("Names.Michelangela"),
        ];
    }

    public override string GetChat()
    {
        var chat = new WeightedRandom<string>();

        chat.Add(this.GetLocalizedValue("Chat.Normal1"));
        chat.Add(this.GetLocalizedValue("Chat.Normal2"));
        chat.Add(this.GetLocalizedValue("Chat.Normal3"));

        return chat;
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = this.GetLocalizedValue("EnchantButton");
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shop)
    {
        if (firstButton)
        {
            if (Main.LocalPlayer.GetModPlayer<CanOnlyEnchantOncePerDayModPlayer>().EnchantedThisDay)
            {
                Main.npcChatText = Main.rand.NextFromList
                (
                    this.GetLocalizedValue("Chat.EnchantTomorrow1"),
                    this.GetLocalizedValue("Chat.EnchantTomorrow2")
                );

                return;
            }

            _dummyPlayer ??= new Player();
            _dummyItem ??= new Item();

            var helmetItem = Main.LocalPlayer.armor[0];
            var chestplateItem = Main.LocalPlayer.armor[1];
            var leggingsItem = Main.LocalPlayer.armor[2];

            bool hasSetBonus;

            void CheckForSetBonus()
            {
                hasSetBonus = false;
                _dummyPlayer.head = _dummyPlayer.armor[0].headSlot;
                _dummyPlayer.body = _dummyPlayer.armor[1].bodySlot;
                _dummyPlayer.legs = _dummyPlayer.armor[2].legSlot;

                _dummyPlayer.UpdateArmorSets(0);

                if (_dummyPlayer.setBonus != "")
                    hasSetBonus = true;
            }

            void TryGiveArmorGem(bool checkHelmet, bool checkChestplate, bool checkLeggings)
            {
                _dummyPlayer.armor[0] = checkHelmet ? helmetItem : _dummyItem;
                _dummyPlayer.armor[1] = checkChestplate ? chestplateItem : _dummyItem;
                _dummyPlayer.armor[2] = checkLeggings ? leggingsItem : _dummyItem;
                CheckForSetBonus();
                if (hasSetBonus)
                {
                    int itemToSpawn = Item.NewItem
                    (
                        new EntitySource_ArmorGemEnchantment
                        (
                            NPC,
                            checkHelmet ? helmetItem : _dummyItem.Clone(),
                            checkChestplate ? chestplateItem : _dummyItem.Clone(),
                            checkLeggings ? leggingsItem : _dummyItem.Clone()
                        ),
                        Main.LocalPlayer.position, Main.LocalPlayer.Size,
                        ModContent.ItemType<BaseArmorGem>(),
                        noGrabDelay: true
                    );

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemToSpawn, 1f);

                    Main.npcChatText = Main.rand.NextFromList
                    (
                        this.GetLocalizedValue("Chat.EnchantSuccess1"),
                        this.GetLocalizedValue("Chat.EnchantSuccess2")
                    );
                    SoundEngine.PlaySound(SoundID.AchievementComplete, NPC.Center);

                    Main.LocalPlayer.GetModPlayer<CanOnlyEnchantOncePerDayModPlayer>().EnchantedThisDay = true;
                }
            }

            TryGiveArmorGem(true, true, false);
            if (hasSetBonus)
                return;
            TryGiveArmorGem(true, false, true);
            if (hasSetBonus)
                return;
            TryGiveArmorGem(false, true, true);
            if (hasSetBonus)
                return;
            TryGiveArmorGem(true, true, true);
            if (hasSetBonus)
                return;

            Main.npcChatText = Main.rand.NextFromList
            (
                this.GetLocalizedValue("Chat.EnchantFail1"),
                this.GetLocalizedValue("Chat.EnchantFail2"),
                this.GetLocalizedValue("Chat.EnchantFail3")
            );
        }
    }

    public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

    public override void SetDefaults()
    {
        (NPC.width, NPC.height) = (18, 40);

        NPC.lifeMax = 250;
        NPC.damage = 10;
        NPC.defense = 15;
        NPC.knockBackResist = 0.5f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.aiStyle = NPCAIStyleID.Passive;
        NPC.townNPC = true;
        NPC.friendly = true;

        AnimationType = NPCID.DyeTrader;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_SpawnNPC)
        {
            EnchanterRespawnSystem.CanEnchanterRespawn = true;
        }
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        if (EnchanterRespawnSystem.CanEnchanterRespawn)
            return true;

        var timId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCID.Tim];

        Main.NewText(Main.BestiaryTracker.Kills.GetKillCount(timId));

        return Main.BestiaryTracker.Kills.GetKillCount(timId) > 0;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (Main.netMode == NetmodeID.Server) return;

        if (NPC.life > 0)
        {
            for (int i = 0; i < hit.Damage / NPC.lifeMax * 100.0; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);

            return;
        }

        for (int i = 0; i < 50; i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f);

        var headGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Hat").Type;
        var headPartyGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Hat_Party").Type;
        var armGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Arm").Type;
        var legGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Leg").Type;

        var headShimmerGore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Hat").Type;
        var headPartyShimmerGore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Hat_Party").Type;
        var armShimmerGore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Arm").Type;
        var legShimmerGore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Leg").Type;

        var hatGore = NPC.GetPartyHatGore();
        if (hatGore > 0)
            Gore.NewGore(NPC.position, NPC.velocity, hatGore);
        Gore.NewGore(NPC.position, NPC.velocity, NPC.IsShimmerVariant ? (hatGore > 0 ? headPartyShimmerGore : headShimmerGore) : (hatGore > 0 ? headPartyGore : headGore));
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, NPC.IsShimmerVariant ? armShimmerGore : armGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, NPC.IsShimmerVariant ? armShimmerGore : armGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, NPC.IsShimmerVariant ? legShimmerGore : legGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, NPC.IsShimmerVariant ? legShimmerGore : legGore);
    }

    public override void FindFrame(int frameHeight)
    {
        base.FindFrame(frameHeight);
    }

    public override void TownNPCAttackStrength(ref int damage, ref float knockback)
    {
        damage = 18;
        knockback = 3f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
    {
        cooldown = 15;
        randExtraCooldown = 15;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
    {
        projType = ProjectileID.BallofFire;
        attackDelay = 15;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
    {
        multiplier = 6f;
        gravityCorrection = 20f;
    }
}