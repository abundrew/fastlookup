# fastlookup

I was thinking a little bit on my way back about the problem you shared.

As I see it:

There is a huge table of complex entity names (let it have a unique ID column as well).
We want to associate all possible character combinations (of some length - for each length the problem can be resolved separately) with several (limited to some number) entity names.
So the user can get those entity names to lookup when they enter the associated character combination.

I think such possible combinations and their associated entity names (limited to some number) can be found and put into a separate "lookup key" table, and sorted and used in fast lookup queries.

All data needed for this table can be gathered in a memory array during one scan of the original table of entities and then exported into the "lookup key" table.

You can take a look at a prototype of such lookup array : https://github.com/abundrew/fastlookup.
There is also some input data and output for "key width" = 5.

If "keywidth" = 5 then the array has ~ 12000000 elements, and if "lookup height" = 10 then it takes about 1G of memory.
