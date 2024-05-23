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
[FIXED][BUG] sleep doesnt work
[FIXED][hotfix]player can only go outside once a day




<============== TODO LIST ===========>

[BUG] When you re-clcik shop again -> shouldn't be able to
[FEATURE] Kanna needs to add build slot border thingy
Possible [BUG]: Shop might be randomly cycling through items, RNG might not be working correctly. Should look into it after spawning multiple items.
[DEFECT] There should be an X button to exit out of interacting
[BUG] When player moves over interactable and mouse moves over interactable, can have 2 highlighted interactable objects

[QUESTION] Should re-clicking a subgroup menu reload or close the menu?
[QUESTION] Should menus have an exit button?




<============== FEATURES ============>

[Shopping category]
have each shopping iten have a category (shown in tooltip). instead of showing random object, please display a random object from each caterory per day. there will be 5 categories. [car][floral][artistic][gamer][retro]

[COOKING INTERACTION]
when you interact with the oven and select a food you would like to cook, a new pop up will show on screen displaying the 'secret ingredient'
the player will start interacting after the secret ingredient is selected. the ingreadients will have metadata that the player cannot see. these metadata will categorise the ingredients as either [normal] or [weird] 

[RECORDING PLAYER]
please make a way to record the players action on a txt file that can be accessed via file explorere.
please record the following:
the player will have a base score of 6.
1. everytime a player goes to the disco, -1 to score, if library then +1
2. everytime a player adds a [weird] ingredient then -1, everytime a player adds a normal ingredient then +1 
3. evertime the player decorates the room with the same category +1, else -1

the game will end after 3 in game days. after this the player will answer 3 questions:

I would rather go to a library than to a party 
I enjoy doing things spontaneously
I like to collect information about categories of things

player can answer these questions from definitely aggree to definitely disagree. please beable to also record these data as follows
1.I would rather go to a library than to a party 
	definitely aggre +2
	aggree +1
	disagree -1
	definitely disagree -2
2. I enjoy doing things spontaneously
	definitely aggre -2
	aggree -1
	disagree +1
	definitely disagree +2
3.I like to collect information about categories of things
	definitely aggre +2
	aggree +1
	disagree -1
	definitely disagree -2

[Player emotions][low priority]
-when ever the player is interacting, a emotion bubble will pop up on top of the player head. make the pop up emotion changeable through editor (based on interaction)

tyty ily baby




	