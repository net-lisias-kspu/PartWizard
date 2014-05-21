﻿// Copyright (c) 2014, Eric Harris (ozraven)
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the copyright holder nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL ERIC HARRIS BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;

using UnityEngine;

namespace PartWizard
{
#if TEST
    using Part = global::PartWizard.Test.MockPart;
#endif
    
    internal sealed class HighlightTracker
    {
        private Dictionary<Part, Color> parts;
        private Dictionary<Part, Color> previousParts;
        private volatile bool tracking;

        public HighlightTracker()
        {
            this.parts = new Dictionary<Part, Color>();
            this.previousParts = new Dictionary<Part, Color>();
            this.tracking = false;
        }

        public void BeginTracking()
        {
            if(tracking)
                throw new HighlightTrackerException("Highlight tracking may not be started more than once; call EndTracking or CancelTracking.");

            this.tracking = true;
        }

        public void Add(Part part, Color color, Color symmetryColor)
        {
            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding parts to track.");

            if(part == null)
                throw new ArgumentNullException("part");

            this.Add(part, color);

            foreach(Part counterpart in part.symmetryCounterparts)
            {
                this.Add(counterpart, symmetryColor);
            }
        }

        public void Add(Part part, Color color)
        {
            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding parts to track.");

            if(part == null)
                throw new ArgumentNullException("part");

            if(!this.parts.ContainsKey(part) && this.previousParts.ContainsKey(part))
            {
                Color originalHighlightColor = this.previousParts[part];
                this.previousParts.Remove(part);

                this.parts.Add(part, originalHighlightColor);
            }
            if(!this.parts.ContainsKey(part) && !this.previousParts.ContainsKey(part))
            {
                this.parts.Add(part, part.highlightColor);
            }

            part.SetHighlightColor(color);
        }

        public void Add(PartGroup group, Color color)
        {
            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding part groups to track.");

            if(group == null)
                throw new ArgumentNullException("group");

            foreach(Part part in group.Parts)
            {
                this.Add(part, color);
            }
        }

        public void EndTracking()
        {
            if(!tracking)
                throw new GUIControlsException("iwiweiewie");

            foreach(Part part in this.parts.Keys)
            {
                part.SetHighlight(true);
            }

            this.Restore(this.previousParts);

            previousParts.Clear();

            var swap = this.previousParts;
            this.previousParts = this.parts;
            this.parts = swap;

            this.tracking = false;
        }

        public void CancelTracking()
        {
            this.tracking = false;

            // Eliminate duplicates held in previousParts.
            foreach(Part part in this.parts.Keys)
            {
                this.previousParts.Remove(part);
            }
            
            // Now previousParts and parts have all the parts we need to restore.
            this.Restore(this.parts);
            this.Restore(this.previousParts);
            
            this.parts.Clear();
            this.previousParts.Clear();
        }

        private void Restore(Dictionary<Part, Color> parts)
        {
            foreach(KeyValuePair<Part, Color> partColor in parts)
            {
                partColor.Key.SetHighlightColor(partColor.Value);
                partColor.Key.SetHighlight(false);
            }
        }
    }
}
