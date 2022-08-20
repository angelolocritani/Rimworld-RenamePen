using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Multiplayer.API;
using System.Reflection;

namespace RenamePen
{
    [StaticConstructorOnStartup]
    public class RenamePenMod : Mod
    {
        public RenamePenMod(ModContentPack content) : base(content)
        {
        }
    }

    [StaticConstructorOnStartup]
    public static class Multiplayer
    {
        static Multiplayer()
        {
            if (!MP.enabled) return;
            MP.RegisterAll();
        }
    }

    public class Dialog_RenameAnimalPen_sync : Dialog_RenameAnimalPen
    {
        public Dialog_RenameAnimalPen_sync(Map map, CompAnimalPenMarker marker) : base(map, marker) { }

        protected override void SetName(string name)
        {
            FieldInfo markerField = typeof(Dialog_RenameAnimalPen).GetField("marker", BindingFlags.NonPublic | BindingFlags.Instance);

            var marker = markerField.GetValue(this) as CompAnimalPenMarker;
            SyncedSetName(name, marker);
        }

        // public/private/protected doesn't matter
        [SyncMethod]
        protected static void SyncedSetName(string name, CompAnimalPenMarker marker)
        {
            marker.label = name;
        }
    }
    public class RenamablePenComp : ThingComp
    {
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {

            Command_Action command_Action = new Command_Action();
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Buttons/Rename");
            command_Action.defaultLabel = "RenameAnimalPen".Translate();
            command_Action.defaultDesc = "RenamePen.RenameAnimalPenDesc".Translate();
            command_Action.action = delegate
            {
                Dialog_RenameAnimalPen_sync dialog_RenamePen = new Dialog_RenameAnimalPen_sync(parent.Map, parent.GetComp<CompAnimalPenMarker>());
                if (KeyBindingDefOf.Misc1.IsDown)
                {
                    dialog_RenamePen.WasOpenedByHotkey();
                }
                Find.WindowStack.Add(dialog_RenamePen);
            };
            command_Action.hotKey = KeyBindingDefOf.Misc1;
            yield return command_Action;

        }
    }

}
