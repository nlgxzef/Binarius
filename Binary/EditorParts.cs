﻿using Binary.Prompt;
using Binary.UI;

using Nikki.Reflection.Abstract;
using Nikki.Support.Shared.Class;

using System;
using System.Windows.Forms;

namespace Binary
{
    /// <summary>
    /// Car parts (DBModelParts) in the editor: the find and replace offered after copying a collection. Script Node
    /// writes the parts of a collection too.
    /// </summary>
    public partial class Editor
    {
        /// <summary>
        /// After a DBModelParts collection is copied, offers to replace the old name with the new one in its parts,
        /// the same as the Find and Replace of the parts editor.
        /// </summary>
        private void AskReplaceInCopiedParts(string filename, Collectable copy, string from)
        {
            if (!(copy is DBModelPart model) || model.CarPartsCount == 0) return;

            var text = $"Replace {from} with {model.CollectionName} in the parts of {model.CollectionName}?";

            using var prompt = new Choice("Copy Car Parts", text, new[] { "Find and replace", "Skip" }, 0, 1);
            prompt.ShowDialog(this);
            if (prompt.Value != 0) return;

            if (CarPartsEditor.FindAndReplace(model, from, model.CollectionName, out var line, filename))
            {

                this.WriteLineToEndCommandPrompt(line);
                this.EditorPropertyGrid.Refresh();

            }
        }
    }
}
