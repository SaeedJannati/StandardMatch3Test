# StandardMatch3Test

## Summary
This is simple match 3 project developed as a test project. User can play the game by swiping on the elments so they create a match of at least three tiles of same colour in vertical or horizontal order.

## Settings Window
You can use Match3|Setting window to access game settings window.
There are two tabs in the setting window: Grid Configuration and Move test config.

### Grid configuration tab

In the Grid configuration tab grid features such as width, height and amount of different colours possible for the tile can be set.
There is a Create grid button as well to see the changes immediately. Colours of the tiles based on the selected colour count should be set here too. Automaticlly enough number of colours will be generated upon chaning colour count but colours themselevs could be changed on desire here.

### Move Test tab

Move test tab is where details of the test could be set. There are two types of test: graphical and non graphical.
In graphical test all of the steps will be visualised like real player playing the game and indeed it wil be the slower of the two.
Non graphical test on the other hand will only show final state of the grid after the test.
Number of moves in the tes could be set in this menu and there is a run test button there too.
For both test there will be logs in the console for begining and end of the test and for the end log, the duration of the test will be loged in seconds too.
