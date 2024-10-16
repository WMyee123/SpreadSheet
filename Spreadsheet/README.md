~~~
Author: William Myers
Partner: none
Start Date: September 23, 2024
Course: CS 3500, University of Utah, School of Computing
GitHub ID: WMyee123
Repo: https://github.com/WMyee123/SpreadSheet/tree/master
Commit date: October 15, 2024
Solution: Spreadsheet
Copyright: CS 3500 and William Myers - This work may not be copied for use in Academic Coursework

# Comments To Evaluators

I decided to store the values within dictionaries to allow for easy access to the data, 
while also maintaining the relations between the cell names, the dependencies between them, 
and the contents held within each cell. This formatting, while requiring access for ech method to the 
internal data multiple times, allows for more itnensive manipulation of the data.

As well, within each cell, rather than changing the data held within them, whenever a cell is reset 
to a different value, a new instance is added to the spreadsheet. This was due to issues with storing this value 
and recovering it later as the larger reference in the dictionary would not refer to the updated cell, but the 
old cell without the changed contents.
~~~