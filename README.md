# Part Wizard /L Unofficial

Part Wizard is a vehicle design utility plugin that adds a few conveniences when building your next strut/booster carrier. Unofficial fork by Lisias.


## In a Hurry

* [Latest Release](https://github.com/net-lisias-kspu/PartWizard/releases)
	+ [Binaries](https://github.com/net-lisias-kspu/PartWizard/tree/Archive)
* [Source](https://github.com/net-lisias-kspu/PartWizard)
* [Project's README](https://github.com/net-lisias-kspu/PartWizard/blob/master/README.md)
* [Change Log](./CHANGE_LOG.md)
* [TODO](./TODO.md) list


## Description

Part Wizard is a vehicle design utility plugin that adds a few conveniences when building your next strut/booster carrier.﻿

### Features

* Provides a list of parts with part highlighting for easier identification.
* Allows deleting of parts from the list on eligible[1] parts.
* Allows editing of symmetry[2] on eligible[3] parts.
* Shows either all parts or only those that are hidden from the editor's Parts List.
	* In career or science games, shows all parts that are currently unavailable.
		* Eligible[4] parts can be purchased directly from Part Wizard.
* Allows selecting parts from the list for action group assignments.
* Part List Filtering
	* Unlinked struts and fuel lines are highlighted in red.
	* Filter by part category.
	* Filter by specific resources.

[1] Parts eligible for deleting via Part Wizard are those that are not the "root" part and have no attached parts.
[2] Symmetry editing modifies the designated part, its counter parts and all symmetrical child parts.
[3] Parts eligible for "breaking" of symmetry must not include the "root" part as a counterpart or as a child part. 
[4] Parts eligible for purchase must have their technology requirement(s) researched and there must be sufficient funds to make the purchase.

### Usage

The Part Wizard toolbar icon is only available in the Vehicle Assembly Building (VAB) or Space Plane Hangar (SPH) editors. The first time you start Kerbal Space Program with the Part Wizard plugin installed, you will need to configure your toolbar to display the Part Wizard icon. Once configured, clicking the Part Wizard toolbar icon will show Part Wizard, and clicking the toolbar icon again will hide it.

#### Part List Overview

The Part Wizard interface presents a list of parts that currently make up your vehicle. Mousing over each part on the list will highlight the part green. Parts with symmetry will also be highlighted green, all counterparts will be highlighted in yellow, and all child parts will be highlighted in white. At the bottom of the window is a status area that always shows the current total part count and, when mousing over a part, will show that part's internal name

##### View Mode

###### Sandbox Games

There are four buttons at the top of the interface, "All", "Hidden", "Categories", and "Resources". These buttons control the current viewing mode, which is indicated by a depressed button with green text. 

 "All" mode shows all of the current vessel's parts in the list. 

"Hidden" mode shows only those parts that have been hidden from the Parts List, as designated by either the part's configuration (.cfg) file or as modified by the Module Manager [1] plugin (or similar). Hidden mode is intended to allow the easy locating of parts that may have been made obsolete by the author.  

"Categories" mode is intended to show a list of part categories with toggles for each; if toggled, then parts in the toggled categories will be shown in the "All" & "Hidden" viewing mode. 

"Resources" mode will show a list of all the different resources on the current vessel; if toggled, then parts in the toggled resources will be shown in the "All" & "Hidden" viewing mode.  There is a special line listed, called "Parts without resources", which covers those parts which don't have any resources.

###### Career and Science Games

There are five buttons at the top of the interface: "All", "Hidden",  "Categories", and "Resources" and "Unavailable". These buttons control the current viewing mode, which is indicated by a depressed button with green text.
	      
"All" mode shows all of the current vessel's parts in the list. 

"Hidden" mode shows only those parts that have been hidden from the Parts List, as designated by either the part's configuration (.cfg) file or as modifid by the Module Manager [1] plugin (or similar). Hidden mode is intended to allow the easy locating of parts that may have been made obsolete by the author. 

"Categories" mode is intended to show a list of part categories with toggles for each; if toggled, then parts in the toggled categories will be shown in the "All" & "Hidden" viewing mode. 

"Resources" mode will show a list of all the different resources on the current vessel; if toggled, then parts in the toggled resources will be shown in the "All" & "Hidden" viewing mode.  There is a special line listed, called "Parts without resources", which covers those parts which don't have any resources.

"Unavailable" mode shows only the parts that aren't available due to not having researched the relevant technology and/or the part has not yet been purchased. The Unavailable mode will allow direct purchasing of parts that your space program is currently qualified to use, so long as the necessary funds are available. Additionally, in Unavailable mode, when possible there is a button at the bottom of the list that allows you to purchase all parts with a single command. NOTE: When parts are purchased your game will be saved immediately.

##### Deleting Parts

Each active part in the Part Wizard list has a small button (labelled with a "X"). When this button is enabled, a part may be deleted by clicking the associated "X" button. The part will be removed immediately. If the part intended for deletion is symmetrical with other parts, ALL of its counterparts will also be deleted. (If this is not intended, see Breaking Symmetry, below.)

##### Editing Simmetry

Each active part in the Part Wizard list has a small button (labelled with a "B"). When this button is enabled, a part's symmetry may be edited by clicking the associated "B" button. The Symmetry Editor window will appear. Mousing over this window will highlight the relevant parts in cyan, and by default each counterpart will be automatically placed in a group. Clicking OK at this point will "break" symmetry for each part, all of its counterparts, and all symmetrical child parts will be converted to non-symmetrical parts and can be manipulated indvidually. However, parts can be moved between groups, groups can be removed and created. Clicking OK will apply the relevant changes, where groups with one part will be symmetrically "broken" and groups with more than one part will have a unique symmetry created for those parts. Note that non-symmetrical child parts are ignored by the Break Symmetry feature and will not be modified.

##### Selecting Parts for Action Group Assignment
	
When in the Action Group editor, each part on the parts list will become a button. Clicking this button will select that part (and it's symmetric counterparts) for action group assignment.
	
##### Undoing an Operation

Part Wizard utilizes the editor to enable its features and as such, its operations are undoable as normal. However, it may require more than one "undo" action to completely revert the actions due to the operations Part Wizard performs, especially when breaking symmetry.

[1] Module Manager is a plugin for patching parts at load time, maintained by sarbian: http://forum.kerbalspaceprogram.com/threads/55219/

### Acknowledgment

The original author expressed his appreciation and thanks to the contributors of Part Wizard:

* **sarbian** for reviewing Part Wizard before the first release, making suggestions and continual help and advice.
* **m4v** for allowing me to base the editor locking code on his mod, RCS Build Aid.
* **antgeth** for the feature idea that became Symmetry Editor.
* **Vlos** for the Action Group part selection feature.
* **Papa_Joe** for adding support for the stock toolbar support contribution.
* **linuxgurugamer** for general updates and finally adding the long-planned filtering features.
* Everyone involved in with KSP that adds their ideas, time and thoughts to make a fun community for all of us to play with.


## Installation

To install, place the GameData folder inside your Kerbal Space Program folder.

**REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**.

### Dependencies

<!-- * [KSP API Extensions/L](https://github.com/net-lisias-ksp/KSPAPIExtensions) 2.0 or later -->
* Hard Dependencies
	+ [Click Through Blocker](https://forum.kerbalspaceprogram.com/index.php?/topic/170747-141-click-through-blocker/)
	+ [Toolbar Control](https://github.com/net-lisias-kspu/ToolbarControl) 

### Licensing
This work is licensed under [BSD 3 Clause](https://opensource.org/licenses/BSD-3-Clause). See [here](./LICENSE).

Please note the copyrights and trademarks in [NOTICE](./NOTICE).


## UPSTREAM

* [LinuxGuruGamer](https://forum.kerbalspaceprogram.com/index.php?/profile/129964-linuxgurugamer/)
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/154466-151-part-wizard-continued/)
	+ [SpaceDock](https://spacedock.info/mod/1148)
	+ [GitHub](https://github.com/linuxgurugamer/PartWizard)
* [ozraven](https://forum.kerbalspaceprogram.com/index.php?/profile/106313-ozraven/): ROOT
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/72468-113-part-wizard-125-22-jun-2016/)
	+ [CurseForge](https://kerbal.curseforge.com/projects/part-wizard)
	+ [GitHub](https://github.com/ozraven/PartWizard)
