/*
	This file is part of Part Wizard /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2017-2018 LinugGuruGamer
		© 2014-2016 Eric Harris (ozraven)

	Part Wizard /L is double licensed, as follows:

		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	Part Wizard /L Unleashed is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Part Wizard /L Unleashed.
	If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with Part Wizard /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

------ To satisfy the preivous BSD-3 License ------

Copyright (c) 2014, Eric Harris (ozraven)
All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the copyright holder nor the
		  names of its contributors may be used to endorse or promote products
		  derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL ERIC HARRIS BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
using System;
using System.Diagnostics;
using System.Reflection;
using KSP.UI.Screens;
using UnityEngine;

using Toolbar = KSPe.UI.Toolbar;


namespace PartWizard
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    [CLSCompliant(false)]
    public sealed class PartWizardPlugin : MonoBehaviour
    {
        static internal PartWizardPlugin Instance;


        Toolbar.Button toolbarControl;

        private PartWizardWindow partWizardWindow;

        internal static bool ToolbarIsStock;
        internal static bool ToolbarTypeToggleActive = false;

        public void Awake()
        {
            Instance = this;
            if(HighLogic.LoadedSceneIsEditor)
            {
                this.partWizardWindow = new PartWizardWindow(Version.FriendlyName, Version.Text);

                this.toolbarControl = Toolbar.Button.Create(this
                        , ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH
                        , UI.Icon.StockToolbarIconActive, UI.Icon.StockToolbarIconInactive
                        , UI.Icon.BlizzyToolbarIconActive, UI.Icon.BlizzyToolbarIconInactive
                        , Version.FriendlyName
                    );

                this.toolbarControl.Toolbar.Add(Toolbar.Button.ToolbarEvents.Kind.Active, new Toolbar.Button.Event(this.ToggleVisibility, this.ToggleVisibility));

                ToolbarController.Instance.Add(this.toolbarControl);
            }
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
            this.partWizardWindow.Hide();
            this.partWizardWindow = null;

            ToolbarController.Instance.Destroy();
            this.toolbarControl = null;
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

        internal void SaveToolbarConfiguration()
        {
            Configuration.Save();
        }
    }
}
