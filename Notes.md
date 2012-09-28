
Another sudoku application sample;
http://www.iui-js.org/powered-by/index.html?id=esudoku

---
Wishlist:
. Database connection?
. Unit Testing?
. Sudoku generator
. Drag & drop the numbers
. Multi-language
. Offline mode
. Mobile application

---
- clear the cases
101, 102, hard etc.?

- fatih's sample? and new samples!

- change the repository name

---
. CASE 1: ID 5, square 1 cannot have any number except 1.
ID 8, 3. grid cannot have any number except 1,2,3.

. CASE 2: ID 1, square 36 cannot have number 9.

. squaresLeft (numbersModel.ZeroValue count) can be improved, maybe moved to sudoku
details? - STILL NECESSARY?

. retrieve only updated values from server - usedSquares + numbers count + availabilities!

. validation; in general OK, but numpad values are ignored as well!

. improve general (content) styling! general list template? + use jQuery animations
?! - IE doesnt support "inherit" - try to have more generic css items ?!

. add samples for different sizes - 4 + 9 (OK) + 16 + 25? + wrong cases; invalid
sudoku, invalid number, invalid square, invalid assignment!

. firefox shows an error in case of 400 ?! is it normal

. step counter ?!

. test with all browsers + mobile!

. clean up; css etc. + check TODO in general!

---
try to have a new (initialized) sudoku on the page - try to decrease the first page load time

try to remove calculateId for square!
would it help?
sample sudoku ids need to be updated!!! and probably square.Groups calculation ?!

try to load availabilities as a sepearate thingy?

use ff net panel to improve the performance

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
try to use a profiler (eaqutec?) to see that how many times functions being called (especially set availability etc.)

---
sudokuId as querystring - and the application should listen when its loading - localhost/5 - load sudoku 5
history.js?!

---
size 4 sudoku is okay
but UI calculations are terrible - it takes too much time to apply them
try to create some shortcut css block for different size - 2,3,4,5 = 4,9,16,25?
also in size 4, counts are dissappeared ?! - how to display them?

---
horizontal + vertical groups for square? and then use them as well with ToggleSelected()
these calculations can come from the server?

---
diger SIKISMA caseleri neler olabilir? onlari arastir

eger bir numaranin konulabilecegi 2 kare varsa ve karedeki diger numara da sadece o 2 kareye konulabiliyorsa, o zaman SIKISMA var ihtimali uzerinde duruyoruz

ancak bu durumda sadece identical karelerde oluyormuþ gibi de gelmiyor.
bu olayý anlamak için daha fazla case gerekiyor gibi ?!?!?!

---
first grade availability - only from its own group?
and is there any second grade availability?

---
how about;
public class Square + internal (or private) class Availability() + internal UpdateAvailability()

---
pass squares to group constructor - to make square property readonly and remove setsquare method

---
CONTINUE WITH new potential + proper potential removal!
merge potential class + square!

---
html template for sudoku killa!
is it possible to download a template from vix?

---
with APM51, it works very slow?!

---
webapi.tester doesnt work at the moment (after the package updates) - check webapiclient!
