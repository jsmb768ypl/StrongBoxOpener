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
private HashSet<uint> _itemRequiresSleep = new HashSet<uint>() {
111598, // Golden Strong Box (Horde)
120354, // Gold Strong Box (Alliance)
111599, // Silver Strong Box (Horde)
120355, // Silver Strong Box (Alliance)
111600, // Bronze Strong Box (Horde)
120356 // Bronze Strong Box (Alliance)
};

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
