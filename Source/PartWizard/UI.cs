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

*/
using System;
using UnityEngine;

using Asset = KSPe.IO.Asset<PartWizard.Startup>;

namespace PartWizard
{
	internal static class UI
	{
		internal static class Icon
		{
			private const string DIR = "Icons";

			private static Texture2D _BlizzyToolbarIconActive;
			internal static Texture2D BlizzyToolbarIconActive => _BlizzyToolbarIconActive??(_BlizzyToolbarIconActive = Asset.Texture2D.LoadFromFile(DIR, "partwizard_active_toolbar_24_icon"));

			private static Texture2D _BlizzyToolbarIconInactive;
			internal static Texture2D BlizzyToolbarIconInactive => _BlizzyToolbarIconInactive??(_BlizzyToolbarIconInactive = Asset.Texture2D.LoadFromFile(DIR, "partwizard_inactive_toolbar_24_icon"));

			private static Texture2D _StockToolbarIconActive;
			internal static Texture2D StockToolbarIconActive => _StockToolbarIconActive??(_StockToolbarIconActive = Asset.Texture2D.LoadFromFile(DIR, "partwizard_active_toolbar_38_icon"));

			private static Texture2D _StockToolbarIconInactive;
			internal static Texture2D StockToolbarIconInactive => _StockToolbarIconInactive??(_StockToolbarIconInactive = Asset.Texture2D.LoadFromFile(DIR, "partwizard_inactive_toolbar_38_icon"));
		}
	}
}
