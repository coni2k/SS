Wishlist:
. Minified versions?
. Only retrieve "changes" from the server; squares + numbers + availabilites etc. ("used" version don't work in case of removed objects)
. Square Id Order Type - currently it's square type by default - horizontal and vertical as well?
. Horizontal + Vertical groups for square on client-side? And use them with ToggleActiveSelected() (Is it possible to retrieve this info from the server?)
. Database connection?
. Unit testing?
. Sudoku generator
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
. nav.js: browser's history/navigation? needs sample - similar to history.js?
http://blog.stevensanderson.com/category/knockout/

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

--- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- 
HOT STUFF;

. sudokuId as querystring - and the application should listen when its loading - localhost/5 - load sudoku 5
history.js?!

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
new wrong
12345678(9hint)
.
.
.
.
.
.
.
........9
it doesnt count hints when it comes to availabilities

---
continue with availabilities css
probably we dont need availability.size4+9etc. for width
it already takes that?
focus on inner items?

---
attr: class + templates went well in general.. but;
when we use square templates, passiveSelected doesn't work..
also not sure whether it's performance ?!

---
<span data-bind="attr: { 'class': CssTest() }">test</span><br />
handle dynamic css with class attribute..
dynamic template
http://www.knockmeout.net/2011/03/quick-tip-dynamically-changing.html

---
all css work is okay but the performance is terrible.. ?!
try to use less computed and observable in css calc.. ?1
