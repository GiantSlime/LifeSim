TODO LIST

BUG items list isn't globally static, so it gets recreated every time we instantiate a shop
	we want the items in the shop to once purchased not be purchaseable again from shop
	perhaps a static class that holds the list as a singleton object that gets fetched by any shop controllers on spawn
	would be better.

BUG Shop doesn't spawn multiple items together
BUG When you re-clcik shop again -> shouldn't be able to
QUESTION Should you be able to start working once you click on inventory?
QUESTION Should re-clicking a subgroup menu reload or close the menu?
QUESTION Should menus have an exit button?
BUG when shop is opened, it doesn't do the alignment logic.
FEATURE Kanna needs to add build slot border thingy
BUG ClearBuild does not have X at the start and needs to hover to get ridd of circle -> i.e. default sprite is wrong
BUG ClearBuild doesn't not go away when you're outside of inventory mode
BUG You are able to open inventory whilst interacting
Possible BUG: Shop might be randomly cycling through items, RNG might not be working correctly. Should look into it after spawning multiple items.
BUG Placing an item in a buildslot that has an item, deletes the item instead of swapping them.
BUG There is no X button to exit out of interacting
DEFECT Time should stop while in inventory mode
FEATURE Starting Money = Customizable
FEATURE Player should lose energy levels over time
BUG When player moves over interactable and mouse moves over interactable, can have 2 highlighted interactable objects
BUG When re-clicking shop, attempting to purchase reloaded items, can lead to negative money.

INNOVATION Create a GameState object, that holds almost all, state based game information. (i.e. shop,etc)

