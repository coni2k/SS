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
atanab�lecek 1 numara kald�ysa, kalan numara d���ndakilerin hi�biri atanamaz
atanab�lecek 2 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 3 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 4 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 5 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 6 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 7 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz
atanab�lecek 8 numara kald�ysa, kalan numaralar d���ndakilerin hi�biri atanamaz

. bir availability assign edildiginde (false) oldugunda, gruptaki diger squarelerin yeni availability listelerine o rakam ekleniyor olabilir..? ama sonra nas�l ��karaca��z?

. diger SIKISMA caseleri neler olabilir? onlari arastir

. eger bir numaranin konulabilecegi 2 kare varsa ve karedeki diger numara da sadece o 2 kareye konulabiliyorsa, o zaman SIKISMA var ihtimali uzerinde duruyoruz
ancak bu durumda sadece identical karelerde oluyormu� gibi de gelmiyor.
bu olay� anlamak i�in daha fazla case gerekiyor gibi ?!?!?!

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

. linq for javas.

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
ko arrayforeach
ko arrayfirst
$ each
$ grep
$ inarray
linq..

---

resettable
groups
squares
assign type check..

var jsonArray = [
    { "user": { "id": 100, "screen_name": "d_linq" }, "text": "to objects" },
    { "user": { "id": 130, "screen_name": "c_bill" }, "text": "g" },
    { "user": { "id": 155, "screen_name": "b_mskk" }, "text": "kabushiki kaisha" },
    { "user": { "id": 301, "screen_name": "a_xbox" }, "text": "halo reach" }
]

{ "group": { "id": 1,
	"square": { "id:" 1, "value" 1 }
	} }

Enumerable.From([    { "user": { "id": 155, "screen_name": "b_mskk" }, "text": "kabushiki kaisha" },
    { "user": { "id": 301, "screen_name": "a_xbox" }, "text": "halo reach" }])
.toArray()
.WriteLine("$.index + ':' + $.value")

Enumerable.Range(0, 20)
.Where("$ % 3 == 0")
.Select("value, index => {index:index, value:value * 10}")
.WriteLine("$.index + ':' + $.value")

---
Enumerable.From([
{ "Group": { "GroupId": 1,
"Squares": [
{ "Square": { "SquareId": 1, "Value": 1 } },
{ "Square": { "SquareId": 2, "Value": 2 } }
]}},
{ "Group": { "GroupId": 2,
"Squares": [
{ "Square": { "SquareId": 3, "Value": 1 } },
{ "Square": { "SquareId": 4, "Value": 2 } }
]}},
])
.Where(function(x) { return x.Group.GroupId == 1 })
.WriteLine(function(x) x.Group.GroupId)

---
Enumerable.From([
{ "Group": { "GroupId": 1, "Squares": [
{ "Square": { "SquareId": 1, "Value": 1 } },
{ "Square": { "SquareId": 2, "Value": 2 } }
]}},
{ "Group": { "GroupId": 2, "Squares": [
{ "Square": { "SquareId": 3, "Value": 1 } },
{ "Square": { "SquareId": 4, "Value": 2 } }
]}},
])
.Where(function(group) { return group.Squares. })
.WriteLine(function(x) x.Group.Squares)
