
Another sudoku application sample;
http://www.iui-js.org/powered-by/index.html?id=esudoku

---
Wishlist:
. Only retrieve "changes" from the server; squares + numbers + availabilites etc. ("used" version don't work in case of removed objects)
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
powershell start-process -WindowStyle Hidden D:\Development\Libraries\APMI\WebRequester\20111004\APMI.WebRequester.exe http://localhost:56105/default.html

---
- clear the cases
101, 102, hard etc.?

- fatih's sample? and new samples!

- change the repository name

---
. CASE 1: ID 5, square 1 cannot have any number except 1.
ID 8, 3. grid cannot have any number except 1,2,3.

. CASE 2: ID 1, square 36 cannot have number 9.

. add samples for different sizes - 4 + 9 (OK) + 16 + 25? + wrong cases; invalid
sudoku, invalid number, invalid square, invalid assignment!

size 4 sudoku is okay
but UI calculations are terrible - it takes too much time to apply them
try to create some shortcut css block for different size - 2,3,4,5 = 4,9,16,25?
also in size 4, counts are dissappeared ?! - how to display them?

. clean up; css etc. + check TODO in general!

---
about hints, add these cases;
new hint
removal of an existing square
removal of an existing number

number changing and changed should have similar operation but reverse?
and also double check the zero case?

also hints should be a dynamic list?

---
try to have a new (initialized) sudoku on the page - try to decrease the first page load time

---
bir grupta; 
atanabýlecek 1 numara kaldýysa, kalan numara dýþýndakilerin hiçbiri atanamaz
atanabýlecek 2 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 3 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 4 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 5 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 6 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 7 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz
atanabýlecek 8 numara kaldýysa, kalan numaralar dýþýndakilerin hiçbiri atanamaz

---
bir availability assign edildiginde (false) oldugunda, gruptaki diger squarelerin yeni availability listelerine o rakam ekleniyor olabilir..? ama sonra nasýl çýkaracaðýz?

---
diger SIKISMA caseleri neler olabilir? onlari arastir

eger bir numaranin konulabilecegi 2 kare varsa ve karedeki diger numara da sadece o 2 kareye konulabiliyorsa, o zaman SIKISMA var ihtimali uzerinde duruyoruz

ancak bu durumda sadece identical karelerde oluyormuþ gibi de gelmiyor.
bu olayý anlamak için daha fazla case gerekiyor gibi ?!?!?!

---
first grade availability - only from its own group?
and is there any second grade availability?

---
how about; public class Square + internal (or private) class Availability() + internal UpdateAvailability()

---
. relatives instead of related?

---
pass squares to group constructor - to make square property readonly and remove setsquare method
not that easy, first it generates the groups and then squares.. ?!

---
webapi.tester doesnt work at the moment (after the package updates) - check webapiclient!

--- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- 
HOT STUFF;

. proper new/remove hint - is it possible to merge hint + square
dynamic hints?

. sudokuId as querystring - and the application should listen when its loading - localhost/5 - load sudoku 5
history.js?!

. horizontal + vertical groups for square? and then use them as well with ToggleSelected()
these calculations can come from the server?

. new sudoku window with title + desc + size

. make the cases more clear - have a sample for the ones that have a problem

. is it possible to load availabilities async. way?

. what is webgrease? and learn more about modernizer! and how about nav.js?

. try to remove calculateId for square! would it help?
sample sudoku ids need to be updated!!! and probably square.Groups calculation ?!


work on loading message
http://stackoverflow.com/questions/1023072/jquery-ui-dialog-how-to-initialize-without-a-title-bar

http://api.jqueryui.com/dialog/#option-title

---
Solving Methods?;
. Square's last no
. Number's last square
. Group's last square
. Group's last number
. Sudoku's last square
. Sudoku's last number
