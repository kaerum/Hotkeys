﻿using Harmony;
using Verse;
using System.Reflection;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using Verse.Sound;
using RimWorld.Planet;

namespace Hotkeys
{
    public class Hotkeys : Mod
    {
        public override string SettingsCategory() => "Hotkeys";
        private HotkeySettings settings;

        public Hotkeys(ModContentPack content) : base(content)
        {
            // Initialize Harmony
            var harmonyHotkeys = HarmonyInstance.Create("Hotkeys");
            harmonyHotkeys.PatchAll(Assembly.GetExecutingAssembly());

            settings = GetSettings<HotkeySettings>();
        }

        // The UI part
        public override void DoSettingsWindowContents(Rect canvas)
        {
            var listing = new Listing_Standard();
            listing.ColumnWidth = canvas.width;
            listing.Begin(canvas);

            listing.CheckboxLabeled("Architect Hotkeys", ref settings.useArchitectHotkeys, "Check to enable the use of hotkeys to select subtabs in the Architect Tab.");
            listing.CheckboxLabeled("Direct Hotkeys", ref settings.useDirectHotkeys, "Check to enable direct hotkeys to any designator");
            listing.GapLine();

            if (settings.useDirectHotkeys)
            {
                listing.ButtonText("Add", "Add additional direct hotkeys");
            }

            listing.End();

            settings.Write();
        }
    }

    public class HotkeySettings : ModSettings
    {
        public bool useArchitectHotkeys;
        public bool useDirectHotkeys;

        public List<string> desCategories;
        public List<string> designators;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref useArchitectHotkeys, "Enable_Architect_Hotkeys");
            Scribe_Values.Look(ref useDirectHotkeys, "Enable_Direct_Hotkeys");

            Scribe_Collections.Look(ref desCategories, "Designation_Categories");
            Scribe_Collections.Look(ref designators, "Designators");

            // If lists don't exist create them
            if (desCategories == null) { desCategories = new List<string>();}
            if (designators == null) { designators = new List<string>(); }
        }
    }
}


