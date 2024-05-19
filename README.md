CHANGELOG

[FIXED][BUG] items list isn't globally static, so it gets recreated every time we instantiate a shop
[FIXED][BUG] Shop doesn't spawn multiple items together
[DONE][FEATURE] Starting Money = Customizable
	Now customizable under player.statuscontroller.
[FIXED][BUG] when shop is opened, it doesn't do the alignment logic.
[FIXED][BUG] ClearBuild does not have X at the start and needs to hover to get ridd of circle -> i.e. default sprite is wrong
[FIXED][BUG] ClearBuild doesn't not go away when you're outside of inventory mode
[NOT REALLY FIXED BUT SHOULDNT HAPPEN ANYMORE][BUG] When re-clicking shop, attempting to purchase reloaded items, can lead to negative money.
[FIXED][BUG] You are able to open inventory whilst interacting
[FIXED][DEFECT] Time should stop while in inventory mode
[FIXED][BUG] Placing an item in a buildslot that has an item, deletes the item instead of swapping them.
[DONE][FEATURE] Player should lose energy levels over time


TODO LIST

[BUG] When you re-clcik shop again -> shouldn't be able to
[FEATURE] Kanna needs to add build slot border thingy
Possible [BUG]: Shop might be randomly cycling through items, RNG might not be working correctly. Should look into it after spawning multiple items.
[DEFECT] There should be an X button to exit out of interacting
[BUG] When player moves over interactable and mouse moves over interactable, can have 2 highlighted interactable objects

[QUESTION] Should re-clicking a subgroup menu reload or close the menu?
[QUESTION] Should menus have an exit button?