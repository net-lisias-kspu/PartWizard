// Copyright (c) 2014, Eric Harris (ozraven)
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
using System.Diagnostics;
using System.Reflection;
using KSP.UI.Screens;
using UnityEngine;

using KSPe.GameDB;
using ToolbarControl_NS;


namespace PartWizard
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    [CLSCompliant(false)]
    public sealed class PartWizardPlugin : MonoBehaviour
    {
        static internal PartWizardPlugin Instance;


        ToolbarControl toolbarControl;

        private PartWizardWindow partWizardWindow;

        public static readonly string Name = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;
        public static readonly string Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        internal static bool ToolbarIsStock;
        internal static bool ToolbarTypeToggleActive = false;
        private readonly string BlizzyToolbarIconActive = Asset<PartWizardPlugin>.Solve("Icons/partwizard_active_toolbar_24_icon");
        private readonly string BlizzyToolbarIconInactive = Asset<PartWizardPlugin>.Solve("Icons/partwizard_inactive_toolbar_24_icon");
        private readonly string StockToolbarIconActive = Asset<PartWizardPlugin>.Solve("Icons/partwizard_active_toolbar_38_icon");
        private readonly string StockToolbarIconInactive = Asset<PartWizardPlugin>.Solve("Icons/partwizard_inactive_toolbar_38_icon");

        internal const string MODID = "PartWizard";
        internal const string MODNAME = "Part Wizard";

        public void Awake()
        {
            Instance = this;
            if(HighLogic.LoadedSceneIsEditor)
            {
                this.partWizardWindow = new PartWizardWindow(PartWizardPlugin.Name, PartWizardPlugin.Version);
                this.partWizardWindow.OnVisibleChanged += partWizardWindow_OnVisibleChanged;

                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(ToggleVisibility, ToggleVisibility,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                    MODID, MODID+"Button",
                    StockToolbarIconActive, StockToolbarIconInactive,
                    BlizzyToolbarIconActive,BlizzyToolbarIconInactive,
                    MODNAME
                );
            }
        }

        private void partWizardWindow_OnVisibleChanged(GUIWindow window, bool visible)
        {
            this.UpdateToolbarIcon();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "GUI")]
        public void OnGUI()
        {

            if(this.partWizardWindow != null)
            {
                this.partWizardWindow.Render();
            }
        }

        public void OnDestroy()
        {
            this.partWizardWindow.OnVisibleChanged -= partWizardWindow_OnVisibleChanged;

            this.partWizardWindow.Hide();
            this.partWizardWindow = null;

            toolbarControl.OnDestroy();
            Destroy(toolbarControl);

        }

        private void ToggleVisibility()
        {
            if(this.partWizardWindow.Visible)
            {
                this.partWizardWindow.Hide();
            }
            else
            {
                this.partWizardWindow.Show();
            }
        }

        private void partWizardButton_Click(ClickEvent e)
        {
            this.ToggleVisibility();
        }

        private void UpdateToolbarIcon()
        {
            if (partWizardWindow.Visible)
            {
                toolbarControl.SetTexture(StockToolbarIconActive, BlizzyToolbarIconActive);
            }
            else
            {
                toolbarControl.SetTexture(StockToolbarIconInactive, BlizzyToolbarIconInactive);

            }
        }



        internal void DummyHandler()
        {
        }


        internal void SaveToolbarConfiguration()
        {
            Configuration.Save();
        }
    }
}
