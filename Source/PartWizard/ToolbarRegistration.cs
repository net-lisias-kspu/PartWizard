using UnityEngine;
using ToolbarControl_NS;

namespace PartWizard
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(PartWizardPlugin.MODID, PartWizardPlugin.MODNAME);
        }
    }
}