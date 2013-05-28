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
. Store the user's sudokus? sql || sql localdb || xml || json?
. Better html template - vix.com?
. Multi-language
. Offline mode
. Mobile application
. Add "number of actions" counter
. Measure the performance (eqatec profiler + firefox net panel etc.)
. Drag & drop the numbers?
. Info about;
.. Colors (Statuses) of the squares
.. Keys (ESC, Delete)
.. Links of the external libraries
. Try to use web requester (needs to be included as nuget package);
powershell start-process -WindowStyle Hidden D:\WebRequester.exe http://localhost:56107/default.aspx

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

. Solving Methods?;
. Square's last no
. Number's last square
. Group's last square
. Group's last number
. Sudoku's last square
. Sudoku's last number

---
// Welcome content?
//- blank
//- generate new
//- load cases

//sudoku/blank
//sudoku/generated

---
check these knockout bindings;

    validate – invokes jQuery validation on an element
    selected – selects (or unselects) a DOM element based on the bound property’s value
    blurOnEnter – loses focus when the user clicks the ENTER key
    placeHolder – a shim for the HTML5 placeholder for older browsers

	also binding for escape?
	I do this in my Code Camper SPA to create simple handlers like escape (which performs an action when the ESCAPE key is pressed)

---
. change the repository name
. merge the branches!
. Social media + google analytics + google adsense?
. SEO;
http://support.google.com/webmasters/bin/answer.py?hl=en&answer=182072
https://www.google.com/webmasters/tools/home
. add it to the showcase of libraries (knockout + history.js etc.)
https://github.com/balupton/history.js/wiki/Showcase
. use CDN?
. Check + clean Helpers folder
. clear web.config
. do we need 404?
. knockout on default.js + content as well?
but then how to call ko.applybindings after content load ?!

. Work on cases - make them more clear + have proper samples for invalid cases as well

. try to have a new (initialized) sudoku on the page - try to decrease the first page load time

. how about; public class Square + internal (or private) class Availability() + internal UpdateAvailability()

. relatives instead of related?

. pass squares to group constructor - to make square property readonly and remove setsquare method
not that easy, first it generates the groups and then squares.. ?!

. can sudokuDto + squareDto object be the same on both sides?
try to use ko.toJSON instead of JSON.stringify

. all javascript functions should be under an object ?!

. do we need ko.throttle? for css calculation?

. proper new/remove hint - is it possible to merge hint + square
dynamic hints?

. webapi.tester doesnt work at the moment (after the package updates) - check webapiclient!

. probably console tester doesnt work - after horizontal to square id change

. using content class + navigate function looks a good idea
but then, if the content will be generated from cms - how the user's links will be using Navigate function.. ?!
types: internal links + internal commands + external links

. try to get rid of sudokunumbergroup?

. check the performance by comparing with a plain html

. test history js with old browsers - how about !history.enabled + return false block?
try it with it and without it?

. Check Dynamic Css for Square + SudokuNumber + Availability;
If its on html, it generates to much text on the page
If its generated dynamically, it can be slow?
In general css changes are not that fast? especially related square selection?

// We have to put action; update square and toggleautosolve
// In this case, a GET request for “api/products/details/1” would map to the Details method.
// This style of routing is similar to ASP.NET MVC, and may be appropriate for an RPC-style API.
// For a RESTful API, you should avoid using verbs in the URIs, because a URI should identify a resource, not an action.
check this (last item); http://stackoverflow.com/questions/9569270/custom-method-names-in-asp-net-web-api?lq=1

- webgrease (bundle.config) + jslint;
http://www.jshint.com

. external templates;
http://www.knockmeout.net/2011/03/using-external-jquery-template-files.html

. CONTINUE WITH;
cleaning up this file
try to remove stringify
merge square + hint
update consoleapp + webapp tester ?!

try to get rid of last separator of console.app "|"

github diff. problem + git ignore + git attr file examples?

Note: cache manager - contents cant get updates ?!
