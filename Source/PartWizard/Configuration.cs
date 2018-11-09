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
using UnityEngine;
using KSPe;

namespace PartWizard
{
    internal static class Configuration
    {
        // Example partwizard.cfg:
        // PART_WIZARD_SETTINGS
        // {
        //     SYMMETRY_EDITOR_WINDOW
        //     {
        //         x = 275
        //         y = 25
        //         width = 250
        //         height = 400
        //     }
        //     PART_WIZARD_WINDOW
        //     {
        //         x = 25
        //         y = 25
        //         width = 250
        //         height = 400
        //     }
        //
        //     toolbarIconActive = PartWizard/Icons/partwizard_active_toolbar_24_icon
        //     toolbarIconInactive = PartWizard/Icons/partwizard_inactive_toolbar_24_icon
        // }

        private const string SettingsNodeName = "PART_WIZARD_SETTINGS";
		private static readonly PluginConfig CONFIG = PluginConfig.ForType<PartWizardPlugin>(SettingsNodeName, "PartWizard.cfg");
        private const string KeyRectX = "x";
        private const string KeyRectY = "y";
        private const string KeyRectWidth = "width";
        private const string KeyRectHeight = "height";

        public static readonly GUILayoutOption PartActionButtonWidth = GUILayout.Width(22);

        public static readonly Color HighlightColorDeletablePart = Color.red;
        public static readonly Color HighlightColorDeletableCounterparts = Color.red;
        public static readonly Color HighlightColorSinglePart = Color.green;
        public static readonly Color HighlightColorCounterparts = Color.yellow;
        public static readonly Color HighlightColorEditableSymmetryRoot = Color.yellow;
        public static readonly Color HighlightColorEditableSymmetryCounterparts = Color.yellow;
        public static readonly Color HighlightColorEditableSymmetryChildParts = Color.white;
        public static readonly Color HighlightColorRootPart = Color.blue;
        public static readonly Color HighlightColorSymmetryEditor = Color.cyan;
        public static readonly Color HighlightColorBuyablePart = Color.white;
        public static readonly Color HighlightColorActionEditorTarget = XKCDColors.Blue;


        public static readonly Color GoodFuelLineColor = XKCDColors.Amethyst;
        public static readonly Color BadFuelLineColor = XKCDColors.Amethyst;
        public static readonly Color GoodStrutColor = XKCDColors.OffWhite;
        public static readonly Color BadStrutColor = XKCDColors.OffWhite;

        public static readonly Color EdgeGoodFuelLineColor = new Color(1, 1, 0.0784313f);
        public static readonly Color EdgeBadFuelLineColor = new Color(1, 0.5058824f, 0.7529412f);
        public static readonly Color EdgeGoodStrutColor = new Color(1, 1, 0.8941177f);
        public static readonly Color EdgeBadStrutColor = new Color(0, 0, 1);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Configuration()
        {
        }

        public static void Save()
        {
            CONFIG.Save();
        }

        private static void ValidateKeyName(string key)
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if(string.IsNullOrEmpty(key))
                throw new ArgumentException("Invalid key name.", "key");
        }

        private static void SetValue(ConfigNode node, string key, float value)
        {
            if(node == null)
                throw new ArgumentNullException("node");

            // key is validated by public facing methods.

            if(node.HasValue(key))
            {
                node.RemoveValue(key);
            }

            node.AddValue(key, value);
        }

        public static void SetValue(string key, Rect value)
        {
            Configuration.ValidateKeyName(key);

            if(CONFIG.Node.HasNode(key))
            {
                CONFIG.Node.RemoveNode(key);
            }

            ConfigNode rectNode = CONFIG.Node.AddNode(key);

            Configuration.SetValue(rectNode, KeyRectX, value.x);
            Configuration.SetValue(rectNode, KeyRectY, value.y);
            Configuration.SetValue(rectNode, KeyRectWidth, value.width);
            Configuration.SetValue(rectNode, KeyRectHeight, value.height);
        }

        public static void SetValue(string key, bool value)
        {
            Configuration.SetValue(key, value.ToString().ToLowerInvariant());
        }

        public static void SetValue(string key, string value)
        {
            Configuration.ValidateKeyName(key);

            if(CONFIG.Node.HasValue(key))
            {
                CONFIG.Node.RemoveValue(key);
            }

            CONFIG.Node.AddValue(key, value);
        }

        private static float GetValue(ConfigNode node, string key, float defaultValue)
        {
            if(node == null)
                throw new ArgumentNullException("node");

            // key is validated by public facing methods.

            float result = default(float);

            if(!float.TryParse(node.GetValue(key), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        private static bool GetValue(ConfigNode node, string key, bool defaultValue)
        {
            if(node == null)
                throw new ArgumentNullException("node");

            // key is validated by public facing methods.

            bool result = default(bool);

            if(!bool.TryParse(node.GetValue(key), out result))
            {
                Log.Write("Configuration.GetValue(ConfigNode, string, bool) parse failed, result: {0}", result);

                result = defaultValue;
            }
            else
            {
                Log.Write("Configuration.GetValue(ConfigNode, string, bool) parse OK, result: {0}", result);
            }

            return result;
        }

        private static string GetValue(ConfigNode node, string key, string defaultValue)
        {
            if(node == null)
                throw new ArgumentNullException("node");

            // key is validated by public facing methods.

            string result = node.GetValue(key);

            if(string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }

            return result;
        }

        public static Rect GetValue(string key, Rect defaultValue)
        {
            Configuration.ValidateKeyName(key);

            Rect result = defaultValue;

            if(CONFIG.Node.HasNode(key))
            {
                ConfigNode rectNode = CONFIG.Node.GetNode(key);

                result.x = Configuration.GetValue(rectNode, KeyRectX, defaultValue.x);
                result.y = Configuration.GetValue(rectNode, KeyRectY, defaultValue.y);
                result.width = Configuration.GetValue(rectNode, KeyRectWidth, defaultValue.width);
                result.height = Configuration.GetValue(rectNode, KeyRectHeight, defaultValue.height);
            }

            return result;
        }

        public static bool GetValue(string key, bool defaultValue)
        {
            Configuration.ValidateKeyName(key);

            bool result = defaultValue;

            if(CONFIG.Node.HasValue(key))
            {
                result = Configuration.GetValue(CONFIG.Node, key, defaultValue);
            }

            return result;
        }

        public static string GetValue(string key, string defaultValue)
        {
            Configuration.ValidateKeyName(key);

            string result = defaultValue;

            if(CONFIG.Node.HasValue(key))
            {
                result = Configuration.GetValue(CONFIG.Node, key, defaultValue);
            }

            return result;
        }
    }
}
