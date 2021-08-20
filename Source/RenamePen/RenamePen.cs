using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RenamePen
{
    public class RenamePenMod : Mod
    {
        public RenamePenMod(ModContentPack content) : base(content)
        {

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
                Dialog_RenameAnimalPen dialog_RenamePen = new Dialog_RenameAnimalPen(parent.Map, parent.GetComp<CompAnimalPenMarker>());
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
