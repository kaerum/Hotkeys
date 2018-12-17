﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Hotkeys
{
    [StaticConstructorOnStartup]
    static class GizmoKeys
    {
        public static Dictionary<string, GizmoKeyData> gizmoKeys;

        static GizmoKeys()
        {
            gizmoKeys = Hotkeys.settings.gizmoKeys;
            AddSetterKey();
            BuildGizmoKeys();
        }

        private static void AddSetterKey()
        {
            var keyDef = new KeyBindingDef();
            keyDef.category = DefDatabase<KeyBindingCategoryDef>.GetNamed("GizmoKeys");
            keyDef.defName = "Hotkeys_GizmoAssigner";
            keyDef.label = "Assign Gizmos";
            keyDef.defaultKeyCodeA = UnityEngine.KeyCode.Ampersand;
            keyDef.modContentPack = DefDatabase<KeyBindingCategoryDef>.GetNamed("GizmoKeys").modContentPack;
            DefGenerator.AddImpliedDef<KeyBindingDef>(keyDef);
        }

        public static void BuildGizmoKeys()
        {
            if (gizmoKeys == null)
            {
                gizmoKeys = new Dictionary<string, GizmoKeyData>();
            }

            foreach (var keyData in gizmoKeys.Values)
            {
                keyData.BuildKeyDef();
            }
            
            KeyPrefs.Init();
        }

        public static void AddKey(Command command, bool name = true, bool type = false, bool desc = false)
        {
            string keyName = command.Key(name, type, desc);
            var data = new GizmoKeyData
            {
                defName = keyName
            };
            data.BuildKeyDef();

            KeyPrefs.Init();
            KeyMods.BuildKeyModData();

            gizmoKeys[keyName] = data;
            Hotkeys.settings.Write();
        }

        public static void RemoveKey(Command command)
        {
            GizmoKeyData data = TryKey(command);
            gizmoKeys.Remove(data.defName);

            List<KeyBindingDef> keyDefs = new List<KeyBindingDef>
                {
                    data.keyDef
                };

            InitializeMod.RemoveKeyDefs(keyDefs);
            KeyPrefs.Init();
            Hotkeys.settings.Write();
        }

        public static bool KeyPresent(Command command)
        {
            if (TryKey(command) != null)
            {
                return true;
            }
            return false;
        }

        public static GizmoKeyData GetKey(Command command)
        {
            return TryKey(command);
        }

        public static GizmoKeyData GetKey(string key)
        {
            gizmoKeys.TryGetValue(key, out GizmoKeyData data);
            return data;
        }

        private static GizmoKeyData TryKey(Command command)
        {
            GizmoKeyData data;
            List<string> keys = command.KeyList();

            foreach (string key in keys)
            {
                if (gizmoKeys.TryGetValue(key, out data)) { return data; }
            }

            return null;
        }
    }
}
