# Sudoku Solver

A sudoku solving application that developed by the latest .NET technologies.
It aims to solve any sudoku with the least possible steps.

---

## Environment ##
- .NET Framework 4.0
- Visual Studio 11 Express Beta
- C#
- IIS Express 7.5
- Libraries;
.. ASP .NET MVC 4 + WCF Rest
.. json
.. knockout
.. EF?

## Possible Improvements ##
- All squares can hold the groups which can affect them (SecondGradeGroups) ?
Maybe this can be done in SquareGroup level?
In general, we have to be able to pin-point the squares
At the moment we are still doing extra checks? It should be less and less
Try to use GetPotentialSquare() method to see the number of action?

- GetPotentialSquare() should not be necessary in ideal situation?
There should be one case to solve all the time?
It should not be necessary to validate it later on
Because AvailabilityChanged is not working good enough in reserve mode, validation is necessary
But if removing the potentials will work as it should be, probably there will be no INVALID POTENTIAL left!

- clear the cases
101, 102, hard etc.?

- fatih's sample? and new samples!

- test application?!

- others;
UPDATE & AUTOSOLVE is now using POST! - [more info] (http://jcalcote.wordpress.com/2008/10/16/put-or-post-the-rest-of-the-story/)

- do we need containers? is it possible to use the original classes?
there is a problem about serialization but is it possible to make them compatible?

- try to send more data in one request. ie. availability sends a request for each square?

- try to clean!

- change the repository name

---
CONTINUE WITH WEB APPLICATION;
. get list is okay
. continue with get item!
or learn knockout first;
http://learn.knockoutjs.com/#/?tutorial=collections

what is model
what to bind?
what is observable
try to use two viewModel (list + item) on the same page

sample projects; Comments + SPA_Sample
