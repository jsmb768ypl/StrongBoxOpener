/*
* 99.9% of the code taken from Tidy Bags v3.6.4.4 by LiquidAtoR
*/

namespace StrongBoxOpener
{
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Profiles;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
public class TidyBags3 : HBPlugin
{
public override string Name { get { return "StrongBoxOpener"; } }
public override string Author { get { return "jsmb768ypl"; } }
public override Version Version { get { return new Version(0.0.0.1); } }
public bool InventoryCheck = false;
private bool _init;
private void LootFinished(object sender, LuaEventArgs args)
{
if (InventoryCheck == false)
{
InventoryCheck = true;
}
}
private void MailboxFinished(object sender, LuaEventArgs args)
{
if (InventoryCheck == false)
{
InventoryCheck = true;
}
}
private HashSet<uint> _itemUseOnOne = new HashSet<uint>() {
3352, // Ooze-covered Bag
6351, // Dented Crate
6352, // Waterlogged Crate
6353, // Small Chest
6356, // Battered Chest
6357, // Sealed Crate
5523, // Small Barnacled Clam
5524, // Thick-shelled Clam
7973, // Big-mouth Clam
13874, // Heavy Crate
20708, // Tightly Sealed Trunk
20766, // Slimy Bag
20767, // Scum Covered Bag
20768, // Oozing Bag
21113, // Watertight Trunk
21150, // Iron Bound Trunk
21228, // Mithril Bound Trunk
21746, // Lucky Red Envelope (Lunar Festival item)
24476, // Jaggal Clam
24881, // Satchel of Helpful Goods (5-15 1st)
24889, // Satchel of Helpful Goods (5-15 others)
24882, // Satchel of Helpful Goods (15-25 1st)
24890, // Satchel of Helpful Goods (15-25 others)
27481, // Heavy Supply Crate
27511, // Inscribed Scrollcase
27513, // Curious Crate
32724, // Sludge Covered Object
35792, // Mage Hunter Personal Effects
35945, // Brilliant Glass (Daily Cooldown for Jewelcrafting - The Burning Crusade Edition)
36781, // Darkwater Clam
37586, // Handful of Treats (Hallow's End Event)
44475, // Reinforced Crate
44663, // Abandoned Adventurer's Satchel
44700, // Brooding Darkwater Clam
45072, // Brightly Colored Egg (Noble Garden Event)
45909, // Giant Darkwater Clam
51999, // Satchel of Helpful Goods (iLevel 25)
52000, // Satchel of Helpful Goods (31)
52001, // Satchel of Helpful Goods (41)
52002, // Satchel of Helpful Goods (50)
52003, // Satchel of Helpful Goods (57)
52004, // Satchel of Helpful Goods (62)
52005, // Satchel of Helpful Goods (66)
52340, // Abyssal Clam
54516, // Loot-Filled Pumpkin (Hallow's End Event)
57542, // Coldridge Mountaineer's Pouch
61387, // Hidden Stash
62242, // Icy Prism (Daily Cooldown for Jewelcrafting - Wrath Edition)
64657, // Canopic Jar (Archaeology Tol'vir relic)
67248, // Satchel of Helpful Goods (39)
67250, // Satchel of Helpful Goods (85)
67495, // Strange Bloated Stomach (Cataclysm Skinning)
67539, // Tiny Treasure Chest
67597, // Sealed Crate (level 85 version)
69903, // Satchel of Exotic Mysteries (LFD - Extra Reward)
72201, // Plump Intestines (MoP Skinning)
73478, // Fire Prism (Daily Cooldown for Jewelcrafting - Cataclysm Edition)
78890, // Crystalline Geode (Dragon Soul Raid - Normal 10/25 every bossloot)
78891, // Elementium-Coated Geode (Dragon Soul Raid - Normal 10/25 Deathwing Kill)
78892, // Perfect Geode (Dragon Soul Raid - Heroic Hardmode 10/25 Deathwing Kill)
78897, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78898, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78899, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78900, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78901, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78902, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78903, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
78905, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78906, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78907, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78908, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78909, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
78930, // Sealed Crate (around the Darkmoon Faire Island)
79896, // Pandaren Tea Set (Archaeology)
79897, // Pandaren Game Board (Archaeology)
79898, // Twin Stein Set (Archaeology)
79899, // Walking Cane (Archaeology)
79900, // Empty Keg (Archaeology)
79901, // Carved Bronze Mirror (Archaeology)
79902, // Gold-Inlaid Figurine (Archaeology)
79903, // Apothecary Tins (Archaeology)
79904, // Pearl of Yu'lon (Archaeology)
79905, // Standard of Niuzao (Archaeology)
79908, // Manacles of Rebellion (Archaeology)
79909, // Cracked Mogu Runestone (Archaeology)
79910, // Terracotta Arm (Archaeology)
79911, // Petrified Bone Whip (Archaeology)
79912, // Thunder King Insignia (Archaeology)
79913, // Edicts of the Thunder King (Archaeology)
79914, // Iron Amulet (Archaeology)
79915, // Warlord's Branding Iron (Archaeology)
79916, // Mogu Coin (Archaeology)
79917, // Worn Monument Ledger (Archaeology)
85224, // Basic Seed Pack
85225, // Basic Seed Pack
85226, // Basic Seed Pack
87391, // Plundered Treasure (Luck of the Lotus Buff)
88496, // Sealed Crate (MoP version)
89610, // Pandaria Herbs (Trade for Spirit of Harmony)
89613, // Cache of Treasures (Scenario Reward)
89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
90625, // Treasures of the Vale (Daily Quest Reward)
90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
90839, // Cache of Sha-Touched Gold (World Boss gold drop)
90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
92813, // Greater Cache of Treasures (Scenario Reward)
92960, // Silkworm Cocoon (Tailoring Imperial Silk)
93724, // Darkmoon Game Prize
94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
94296, // Cracked Primal Egg
94566, // Fortuitous Coffer (Loot Item IoTK)
95343, // Treasures of the Thunder King (LFR Loot)
95469, // Serpent's Heart (Daily Cooldown for Jewelcrafting - MoP Edition)
95601, // Shiny Pile of Refuse (World Boss drop)
95602, // Stormtouched Cache (World Boss drop)
95617, // Dividends of the Everlasting Spring (LFR Loot)
95618, // Cache of Mogu Riches (LFR Loot)
95619, // Amber Encased Treasure Pouch (LFR Loot)
98096, // Large Sack of Coins (Brawler Fight Reward)
98097, // Huge Sack of Coins (Brawler Fight Reward)
98098, // Bulging Sack of Coins (Brawler Fight Reward)
98099, // Giant Sack of Coins (Brawler Fight Reward)
98100, // Humongous Sack of Coins (Brawler Fight Reward)
98101, // Enormous Sack of Coins (Brawler Fight Reward)
98102, // Overflowing Sack of Coins (Brawler Fight Reward)
98103, // Gigantic Sack of Coins (Brawler Fight Reward)
98133, // Greater Cache of Treasures (Scenario Reward)
98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
98560, // Arcane Trove (Vendor Version Alliance)
98562, // Sunreaver Bounty (Vendor Version Horde)
103624, // Treasures of the Vale (Zone Loot)
104034, // Purse of Timeless Coins (Timeless Isle)
104035, // Giant Purse of Timeless Coins (Timeless Isle)
104271, // Coalesced Turmoil (SoO LFR Loot)
104272, // Celestial Treasure Box (Timeless Isle Loot)
104273, // Flame-Scarred Cache of Offerings (Timeless Isle Loot)
104275, // Twisted Treasures of the Vale (SoO LFR Loot)
105713, // Twisted Treasures of the Vale (SoO Flex Loot)
105714, // Coalesced Turmoil (SoO Flex Loot)
114634, // Icy Satchel of Helpful Goods Item Level 70
114641, // Icy Satchel of Helpful Goods Item Level 75
114648, // Scorched Satchel of Helpful Goods Item Level 80
114655, // Scorched Satchel of Helpful Goods Item Level 84
114662, // Tranquil Satchel of Helpful Goods Item Level 85
114669, // Tranquil Satchel of Helpful Goods Item Level 88
139776, // Banner of the Mantid Empire (Archaeology)
139779, // Ancient Sap Feeder (Archaeology)
139780, // The Praying Mantid (Archaeology)
139781, // Inert Sound Beacon (Archaeology)
139782, // Remains of a Paragon (Archaeology)
139783, // Mantid Lamp (Archaeology)
139784, // Pollen Collector (Archaeology)
139785 // Kypari sap Container (Archaeology)
};
private HashSet<uint> _itemUseOnThree = new HashSet<uint>() {

private HashSet<uint> _itemUseOnFive = new HashSet<uint>() {

private HashSet<uint> _itemUseOnTen = new HashSet<uint>() {

private HashSet<uint> _itemRequiresSleep = new HashSet<uint>() {
61387, // Hidden Stash
67495, // Strange Bloated Stomach (Cataclysm Skinning)
67539, // Tiny Treasure Chest
72201, // Plump Intestines (MoP Skinning)
87391, // Plundered Treasure (Luck of the Lotus Buff)
88496, // Sealed Crate (MoP version)
89610, // Pandaria Herbs (Trade for Spirit of Harmony)
89613, // Cache of Treasures (Scenario Reward)
89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
90625, // Treasures of the Vale (Daily Quest Reward)
90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
90839, // Cache of Sha-Touched Gold (World Boss gold drop)
90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
92813, // Greater Cache of Treasures (Scenario Reward)
92960, // Silkworm Cocoon (Tailoring Imperial Silk)
94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
94296, // Cracked Primal Egg
94566, // Fortuitous Coffer (Loot Item IoTK)
95343, // Treasures of the Thunder King (LFR Loot)
95601, // Shiny Pile of Refuse (World Boss drop)
95602, // Stormtouched Cache (World Boss drop)
95617, // Dividends of the Everlasting Spring (LFR Loot)
95618, // Cache of Mogu Riches (LFR Loot)
95619, // Amber Encased Treasure Pouch (LFR Loot)
98096, // Large Sack of Coins (Brawler Fight Reward)
98097, // Huge Sack of Coins (Brawler Fight Reward)
98098, // Bulging Sack of Coins (Brawler Fight Reward)
98099, // Giant Sack of Coins (Brawler Fight Reward)
98100, // Humongous Sack of Coins (Brawler Fight Reward)
98101, // Enormous Sack of Coins (Brawler Fight Reward)
98102, // Overflowing Sack of Coins (Brawler Fight Reward)
98103, // Gigantic Sack of Coins (Brawler Fight Reward)
98133, // Greater Cache of Treasures (Scenario Reward)
98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
98560, // Arcane Trove (Vendor Version Alliance)
98562 // Sunreaver Bounty (Vendor Version Horde)
};

private HashSet<uint> _destroyItems = new HashSet<uint>() {

public override void Pulse()
{
if (!_init) {
base.OnEnable();
Lua.DoString("SetCVar('AutoLootDefault','1')");
Lua.Events.AttachEvent("LOOT_CLOSED", LootFinished);
Lua.Events.AttachEvent("MAIL_CLOSED", MailboxFinished);
Logging.Write(LogLevel.Normal, Colors.DarkRed, "StrongBoxOpener ready for use...");
_init = true;
}
if (_init)
if (StyxWoW.Me.IsActuallyInCombat
|| StyxWoW.Me.Mounted
|| StyxWoW.Me.IsDead
|| StyxWoW.Me.IsGhost
) {
return;
}
if (InventoryCheck) { // Loot Event has Finished
foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>()) { // iterate over every item
if (item != null && item.BagSlot != -1 && StyxWoW.Me.FreeNormalBagSlots >= 2) { // check if item exists and is in bag and we have space
if (_itemUseOnOne.Contains(item.Entry)) { // stacks of 1
if (item.StackCount >= 1) {
this.useItem(item);
}
} 
 
}
}
InventoryCheck = false;
StyxWoW.SleepForLagDuration();
}
}
private void useItem(WoWItem item)
{
Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Using {2} we have {3}", this.Name, this.Version, item.Name, item.StackCount);
if (_itemRequiresSleep.Contains(item.Entry)) {
// some (soulbound) items require an additional sleep to prevent a loot bug
StyxWoW.SleepForLagDuration();
}
Lua.DoString("UseItemByName(\"" + item.Name + "\")");
StyxWoW.SleepForLagDuration();
}
}
}
