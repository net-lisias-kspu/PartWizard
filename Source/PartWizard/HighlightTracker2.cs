﻿// Copyright (c) 2015, Eric Harris (ozraven)
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

using Highlighting;

namespace PartWizard
{
    /// <summary>
    /// A class for modifying and restoring the state of globally shared resource, such as the highlighting state of Part objects, in this case.
    /// </summary>
    internal sealed class HighlightTracker2 : IHighlightTracker
    {
        #region Global Part Highlight Tracking

        /// <summary>
        /// Simple generic class for managing two of the same type of object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Pair<T> where T : new()
        {
            public T Left;
            public T Right;

            public Pair()
            {
                this.Left = new T();
                this.Right = new T();
            }

            public Pair(T left, T right)
            {
                this.Left = left;
                this.Right = right;
            }

            public void Swap()
            {
                T temp = this.Left;
                this.Left = this.Right;
                this.Right = temp;
            }
        }

        private static int nextInstance = 0;    // Automatically incremented each time an instance of this class is created.

        // A global dictionary of parts sorted by HighlightTracker instance.
        private static Dictionary<int, Pair<Dictionary<Part, bool>>> instanceParts = new Dictionary<int, Pair<Dictionary<Part, bool>>>();

        #endregion

        private int instance;
        private volatile bool tracking;

        #region Private Logic Simplifiers

        private Dictionary<Part, bool> Parts
        {
            get
            {
                return HighlightTracker2.instanceParts[this.instance].Left;
            }
        }

        private Dictionary<Part, bool> PreviousParts
        {
            get
            {
                return HighlightTracker2.instanceParts[this.instance].Right;
            }
        }

        private void Swap()
        {
            HighlightTracker2.instanceParts[this.instance].Swap();
        }

        #endregion

        public HighlightTracker2()
        {
            this.instance = HighlightTracker2.nextInstance++;
            this.tracking = false;

            HighlightTracker2.instanceParts.Add(this.instance, new Pair<Dictionary<Part, bool>>());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CancelTracking"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EndTracking")]
        public void BeginTracking()
        {
            if(tracking)
                throw new HighlightTrackerException("Highlight tracking may not be started more than once; call EndTracking or CancelTracking.");

            this.tracking = true;
        }

        public void Add(Part part, Color color, Color symmetryColor)
        {
            this.Add(part, color, symmetryColor, false);
        }

        public void Add(Part part, Color color, Color symmetryColor, bool recursive)
        {
            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding parts to track.");

            if(part == null)
                throw new ArgumentNullException("part");

            this.Add(part, color, recursive);

            foreach(Part counterpart in part.symmetryCounterparts)
            {
                this.Add(counterpart, symmetryColor, recursive);
            }
        }

        public void Add(Part part, Color color, bool recursive)
        {
            bool tracked= false;

            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding parts to track.");

            if(part == null)
                throw new ArgumentNullException("part");

            if(recursive)
            {
                foreach(Part childPart in part.children)
                {
                    this.Add(childPart, color, recursive);
                }
            }

            if(!this.Parts.ContainsKey(part) && this.PreviousParts.ContainsKey(part))
            {
                tracked = true;
                // This part was previously tracked, so move it to the current set.
                HighlightTracker2.Transfer(part, this.PreviousParts, this.Parts);
            }

            if(!this.Parts.ContainsKey(part) && !this.PreviousParts.ContainsKey(part))
            {

                // This part wasn't previously tracked by this instance. Look for it in the master set and move it to the current set if found.

                bool found = false;

                foreach(var globalParts in HighlightTracker2.instanceParts)
                {
                    if(globalParts.Key != this.instance)
                    {
                        if(globalParts.Value.Left.ContainsKey(part))
                        {
                            HighlightTracker2.Transfer(part, globalParts.Value.Left, this.Parts);

                            found = true;

                            break;
                        }
                        else if(globalParts.Value.Right.ContainsKey(part))
                        {
                            HighlightTracker2.Transfer(part, globalParts.Value.Right, this.Parts);

                            found = true;

                            break;
                        }
                    }
                }

                // The part wasn't found, add it to the current set.
                if(!found)
                {
                    this.Parts.Add(part, recursive);
                }
            }

            // The part should be in the current set by this point.
            Log.Assert(this.Parts.ContainsKey(part));
            if (!tracked)
                HighlightTracker2.Modify(part, color, recursive);
        }

        public void Add(Part part, Color color)
        {
            this.Add(part, color, false);
        }

        private static void Transfer(Part part, Dictionary<Part, bool> source, Dictionary<Part, bool> destination)
        {
            bool recursive = source[part];

            source.Remove(part);

            destination.Add(part, recursive);
        }

        public void Add(PartGroup group, Color color, bool recursive)
        {
            if(!tracking)
                throw new HighlightTrackerException("Highlight tracking must be started before adding part groups to track.");

            if(group == null)
                throw new ArgumentNullException("group");

            foreach(Part part in group.Parts)
            {
                this.Add(part, color, recursive);
            }
        }

        public void Add(PartGroup group, Color color)
        {
            this.Add(group, color, false);
        }

        /// <summary>
        /// Finalizes the tracking actions. All parts in the current set are activated for highlighting. All parts in the previous set are restored to their original
        /// state. The current set becomes the previous set and the previous set is emptied and recycled to become the next current set.
        /// </summary>
        public void EndTracking()
        {
            if(!tracking)
                throw new GUIControlsException("Highlight tracking must be started before tracking can be completed.");

            if(Event.current.type == EventType.Repaint)
            {
                HighlightTracker2.Restore(this.PreviousParts);

                this.PreviousParts.Clear();

                this.Swap();
            }

            this.tracking = false;
        }

        /// <summary>
        /// Cancels the current tracking operation. The current set and previous set are cleared and all parts are restored to their original states.
        /// </summary>
        public void CancelTracking()
        {
            this.tracking = false;

            // Eliminate duplicates held in previousParts.
            foreach(Part part in this.Parts.Keys)
            {
                this.PreviousParts.Remove(part);
            }

            // Now previousParts and parts have all the parts we need to restore.
            HighlightTracker2.Restore(this.Parts);
            HighlightTracker2.Restore(this.PreviousParts);

            this.Parts.Clear();
            this.PreviousParts.Clear();
        }
        
        private static void Modify(Part part, Color color, bool recursive)
        {
            //
            // Special code for fuel lines and struts
            //
            if (part is CompoundPart)
            {
                var p = part as CompoundPart;

                p.SetHighlightDefault();
                p.SetHighlightType(Part.HighlightType.AlwaysOn);
                p.SetHighlight(true, recursive);
                
                Color highlightC = XKCDColors.White, 
                    edgeHighlightColor = XKCDColors.White;


                switch (p.name)
                {
                    case "fuelLine":
                        if (p.attachState == CompoundPart.AttachState.Detached || p.attachState == CompoundPart.AttachState.Attaching || p.target == p.parent)
                        {
                            highlightC = Configuration.BadFuelLineColor;
                            edgeHighlightColor = Configuration.EdgeBadFuelLineColor;
                        }
                        else
                        {
                            highlightC = Configuration.GoodFuelLineColor;
                            edgeHighlightColor = Configuration.EdgeGoodFuelLineColor;
                        }
                        break;

                    case "strutConnector":                    
                        if (p.attachState == CompoundPart.AttachState.Detached || p.attachState == CompoundPart.AttachState.Attaching || p.target == p.parent)
                        {
                            highlightC = Configuration.BadStrutColor;
                            edgeHighlightColor = Configuration.EdgeBadStrutColor;
                        }
                        else
                        {
                            highlightC = Configuration.GoodStrutColor;
                            edgeHighlightColor = Configuration.EdgeGoodStrutColor;
                        }

                        break;
                }
                p.SetHighlightColor(highlightC);
                p.highlighter.ConstantOn(edgeHighlightColor);
                p.highlighter.SeeThroughOn();
            }
            else
            {
                part.SetHighlightDefault();
                part.SetHighlightType(Part.HighlightType.AlwaysOn);

                // Update the part with the requested settings.
                part.SetHighlightColor(color);
                part.SetHighlight(true, recursive);

                Highlighter highlighter = part.highlighter;
                part.highlighter.SeeThroughOn();
                part.highlighter.ConstantOn(color);
            }
        }

        private static void Restore(Dictionary<Part, bool> parts)
        {
            foreach(KeyValuePair<Part, bool> entry in parts)
            {
                Part part = entry.Key;
                bool recursive = entry.Value;

                HighlightTracker2.Restore(part, recursive);
            }
        }

        private static void Restore(Part part, bool recursive)
        {
            try
            {
                // Restore the part to default.
                part.SetHighlightDefault();
                part.SetHighlight(false, recursive);

                Highlighter highlighter = part.highlighter;
                part.highlighter.ConstantOff();
                part.highlighter.SeeThroughOff();
            }
            catch(NullReferenceException)
            {
                // Nothing to do here except swallow it, the part was likely deleted and now attempting to restore it is going to fail because
                // the object is being torn down.

                // Maybe only do actual part tracking during repaint?
            }
        }
    }
}
