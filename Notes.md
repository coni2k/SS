Wishlist:
. Minified versions?
. Only retrieve "changes" from the server; squares + numbers + availabilites etc. ("used" version don't work in case of removed objects)
. Square Id Order Type - currently it's square type by default - horizontal and vertical as well?
. Horizontal + Vertical groups for square on client-side? And use them with ToggleActiveSelected() (Is it possible to retrieve this info from the server?)
. Handle arrow keys?
. Unit testing?
. Sudoku generator
. knockout unobtrusive?
http://bsatrom.github.com/Knockout.Unobtrusive/
http://userinexperience.com/?p=633
http://userinexperience.com/?p=689
. Store in the database?
. Better html template - vix.com?
. Multi-language
. Offline mode
. Mobile application
. Add "number of actions" counter
. Measure the performance (eqatec profiler + firefox net panel etc.)
. Drag & drop the numbers?
. Try to use web requester (needs to be included as nuget package);
powershell start-process -WindowStyle Hidden D:\WebRequester.exe http://localhost:56107/default.html

Research:
. modernizr: currently we dont use modernizer, can there be a case that we should use it? what might not be supported?
http://modernizr.com/docs/#installing
https://github.com/Modernizr/Modernizr/wiki/HTML5-Cross-browser-Polyfills
. webgrease: javascript + css compile+check+bundle? needs sample
http://www.asp.net/mvc/tutorials/mvc-4/bundling-and-minification
http://kenhaines.net/post/2012/06/25/How-to-use-Webgrease-Configuration-Files.aspx
http://codebetter.com/howarddierking/2012/06/04/web-optimization-in-visual-studio-2012-rc/
http://kenhaines.net/post/2012/06/09/WebGrease-As-seen-in-Visual-Studio-2012.aspx
http://bartwullems.blogspot.nl/2012/07/minification-and-bundling-in-aspnet.html

---
BIG HINT ISSUE!

. fatih's sample? and new samples!

. CASE 1: ID 5, square 1 cannot have any number except 1.
ID 8, 3. grid cannot have any number except 1,2,3.

. CASE 2: ID 1, square 36 cannot have number 9.

. bir grupta;
atanabýlecek 1 numara kaldýysa, kalan numara dýþýndakilerin hiçbiri atanamaz
atanabýlecek 2 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 3 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 4 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 5 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 6 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 7 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 8 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz

. bir availability assign edildiginde (false) oldugunda, gruptaki diger squarelerin yeni availability listelerine o rakam ekleniyor olabilir..? ama sonra nasýl çýkaracaðýz?

. diger SIKISMA caseleri neler olabilir? onlari arastir

. eger bir numaranin konulabilecegi 2 kare varsa ve karedeki diger numara da sadece o 2 kareye konulabiliyorsa, o zaman SIKISMA var ihtimali uzerinde duruyoruz
ancak bu durumda sadece identical karelerde oluyormuþ gibi de gelmiyor.
bu olayý anlamak için daha fazla case gerekiyor gibi ?!?!?!

. first grade availability - only from its own group?
and is there any second grade availability?

---
about hints, add these cases;
new hint
removal of an existing square
removal of an existing number

number changing and changed should have similar operation but reverse?
and also double check the zero case?

also hints should be a dynamic list?

---
. change the repository name

. merge the branches!

. check helpers + clean + add to the solution?

. work on cases - make them more clear + have proper samples for invalid cases as well

. clean up; css etc. + check TODO in general!

. try to have a new (initialized) sudoku on the page - try to decrease the first page load time

. how about; public class Square + internal (or private) class Availability() + internal UpdateAvailability()

. relatives instead of related?

. pass squares to group constructor - to make square property readonly and remove setsquare method
not that easy, first it generates the groups and then squares.. ?!

. work on ui side javas. objects?
is it possible to map to server objects? how to extend?
and sudoku class instead of sudokuContainer? + square class instead of squareContainer (during post operations)

. server side validation for post operations (new sudoku, updatesquare)

. webapi.tester doesnt work at the moment (after the package updates) - check webapiclient!

. probably console tester doesnt work - after horizontal to square id change

. try to get rid of sudokunumbergroup

. mention the keys (esc, delete)

. mention the links of the libraries

. check the performance by comparing with a plain html

. test history js with old browsers - how about !history.enabled + return false block?
try it with it and without it?

. default.aspx or default.html?

. external html templates;
the templates should be on different htm files and should be loaded dynamically (on request)
but since the design of the application is not clear, work on it at the end

. Check Dynamic Css for Square + SudokuNumber + Availability;
If its on html, it generates to much text on the page
If its generated dynamically, it can be slow?
In general css changes are not that fast? especially related square selection?

. at the end;

- webgrease + jslint;
http://www.jshint.com

- add it to the showcase of libraries (knockout + history.js etc.)
https://github.com/balupton/history.js/wiki/Showcase
- refer to the global js files, instead of local ones?

. use strict;
http://ejohn.org/blog/ecmascript-5-strict-mode-json-and-more/

http://richarddingwall.name/2008/08/09/three-common-aspnet-mvc-url-routing-issues/
http://www.4guysfromrolla.com/articles/012710-1.aspx - ROUTING IN ASP.NET 4
URL AS UI - http://www.useit.com/alertbox/990321.html

. event delegation (for square to update sudoku - selected square);
http://davidwalsh.name/event-delegate

. external templates;
http://max.jsrhost.com/ - Ajaxify
http://www.knockmeout.net/2011/03/using-external-jquery-template-files.html
http://ifandelse.com/?p=100

.. knockout sessions;
http://www.knockmeout.net/2012/08/thatconference-2012-session.html
http://www.knockmeout.net/2012/10/twincitiescodecamp-2012-session.html

. Try to learn more about history.js;
https://github.com/balupton/history.js/wiki/Intelligent-State-Handling
https://gist.github.com/854622

loading a new content (changing the state) doesnt look good? changes the content suddenly etc.?

also check this in IE; when select a sudoku in 'help' page (is it a bug);
/help#sudoku/?id=19

--- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- 
HOT STUFF;

CONTINUE WITH;
1. KO.TOJSON + mapping ?!
Now, if you do a ko.toJSON(viewModel) or ko.toJSON(selectedItem), you will just get your item and not the style object. The toJSON() method will see that selectedItem is an observable and then unwrap it. It does not look for any properties/observables attached to the observable itself (which is a function). So, this is a nice way to hide values that are not important to send back to the server.

2. should we try to put all functions under appviewmodel ?!?!?

.. need throttle ?

. proper new/remove hint - is it possible to merge hint + square
dynamic hints?

. Solving Methods?;
. Square's last no
. Number's last square
. Group's last square
. Group's last number
. Sudoku's last square
. Sudoku's last number

---
CONTINUE WITH TRACING + LOGGING;
LOG4NET OR ELMAH OR ..?
http://www.asp.net/web-api/overview/testing-and-debugging/tracing-in-aspnet-web-api
http://blogs.msdn.com/b/jmstall/archive/2012/04/16/how-webapi-does-parameter-binding.aspx
http://blogs.msdn.com/b/roncain/archive/2012/04/12/tracing-in-asp-net-web-api.aspx
http://blogs.msdn.com/b/roncain/archive/2012/08/16/asp-net-web-api-tracing-preview.aspx

AND THEN (load data from server);
http://knockoutjs.com/documentation/json-data.html

ALSO DONT FORGET TO CHECK;
. TOJSON PROTOTYPE
. square().SudokuNumber(newValue); - square() is new, not just square
. on server side;
public bool IsAvailable
{
    get
    {
        if (SudokuNumber != null)
            return SudokuNumber.IsZero;
        return 
    }
}
