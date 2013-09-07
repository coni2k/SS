Wishlist:
. Minified versions?
. Only retrieve "changes" from the server; squares + numbers + availabilites etc. ("used" version don't work in case of removed objects)
. Square Id Order Type - currently it's square type by default - horizontal and vertical as well?
. Horizontal + Vertical groups for square on client-side? And use them with ToggleActiveSelected() (Is it possible to retrieve this info from the server?)
. Handle arrow keys?
. Improve unit testing with proper cases
. Sudoku generator
. knockout
.. unobtrusive?
http://bsatrom.github.com/Knockout.Unobtrusive/
http://userinexperience.com/?p=633
http://userinexperience.com/?p=689
.. throttle for css calculation - necessary?
.. Knockout-ES5 ?
. Store the user's sudokus? sql || sql localdb || xml || json?
. Better html template - bootstrap || vix.com
. Multi-language
. "Offline mode" messages
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

. first grade availability - only from its own group?
and is there any second grade availability?

---
[obsolete?]
number changing and changed should have similar operation but reverse?
and also double check the zero case?

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

. move DTO classes to web class ?!

. all javascript functions should be under an object ?!

. using content class + navigate function looks a good idea
but then, if the content will be generated from cms - how the user's links will be using Navigate function.. ?!
types: internal links + internal commands + external links

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

. webgrease (bundle.config) + jslint

. content manager could be improved - with all external - internal - read file - checkupdates etc. stuff
but do it at the end. probably will not have external content ?!

. check todoList.dataAccess.js - leuk programming

---
Stats:
Group_Square_AvailabilityChangedCounter: 81
Square_Group_SquareAvailabilityChangedCounter: 729

23
81

checkmethod2 runs on affectedgroups
checkmethod1 runs on all squares? - make it on sudoku level?

affectedsquares from this operation ?!

clearsquare() just to put zeronumber
and use this, if there is an existing number on the square

---
groupavailability guzel oldu gibi..
simdi kontrollerin tam olarak nerde olmasi gerektigine bak, misal direk bunun icinde olabilir mi - yoksa illa bu islem bittikten sonra mi olmasi lazim ? - burda olabiliyorsa zaten direk setavailability icine yerlestirsek te olur;

            // New way
            // Set availabilities of the related squares
            foreach (var group in SquareGroups)
            {
                foreach (var square in group.Squares)
                {
                    square.SetAvailability(SudokuNumber, group.GroupType, this);

                    foreach (var squareGroup in square.SquareGroups)
                    {
                        squareGroup.SetAvailability(SudokuNumber, square);
                    }
                }
            }
			
clear number() - clear availability() vs.

---
new set availabilityler calisiyor - sadece kontrollerin tam olarak nerede olmasi gerektigine bakiyoruz
check1 iyi gibi ama check2 yi setavailabilitysi icine koyunca cok yamuk sonuclar verdi - nedir ne degildir bakmak lazim ama once guzel bir test ortami gerekiyor..

bir de, o yamuk sonuclar icerisinde hint olmasi mumkun olmayan durumlar vardi. hemen yan tarafi dolu olan bir kareyi hint olarak gosteriyor?
hint found demeden once diger availabilityleri de mi kontrol etmek gerekir?!

---
group availabilitylerle ilgili;
aslinda ayni availability caselerini 3 farkli grup turunda tutuyor gibiyiz.
square type, hor. vert. typelarinin hepsi aslinda ayni olasiliklari tutuyor gibi gorunuyor.
haliyle eger birinde hint bulunacak olursa, diger 2 tipte de ayni hinti buluyor olacagiz.
1. bunu teke dusurmenin bir faydasi var midir?
2. ya da bu similarity aslinda baska bir duruma mi isaret ediyor? (bu availabilityleri aslinda sudoku seviyesinde tutuluyor olmasi gibi?)

---
square clear doesnt work

group - check availability'yi simdilik set availability icinden cikaracagiz.
orada kaldigi zaman soyle bir case olusuyor. bir rakami set ettigimizde, tek tek grup icinden availabilityleri set ederken,
8. set sonrasinda, 9. kare hintmis gibi gorunuyor (tek bir square ile deneme yapildiginda acikca goruluyor).
bu aslinda yanlis bir hint, sadece set etme isi bitmemis oldugu icin ortaya cikiyor.
eger check isini set icinde yapacaksak, bu durumda, 9. kare icin availability set edildiginde, bu "yanlis hint"i de kaldirabiliyor olmamiz lazim.
simdilik bununla ugrasmayi istemedigimizden dolayi simdilik check isini, setten sonra yapacagiz.
her ihtimalde hint removal isleminin temiz olmasi lazim ve sonraki asamalarda bununla ilgileniyor olacagiz.
hallolursa tekrar check + set group availability birlestirilebilir (bu arada square set + checkte benzer bir sorun yok - umarim ilerde de neden buna gerek olmadigini kolaylikla gorebiliyor olurum).

---
clear availability ile devam ediyor olmamiz lazim..
eskide numberchanging kullaniyorduk ama bunu da yine update square altindan digerlerine eriserek yapabiliriz..
setavailability methodlarini mi kullanmali, yoksa clearavailability gibi farkli bir method mu yaratmali?

---
bunlar da henuz kullanilmiyor;
        public bool UseGroupSolvingMethod { get; set; }
        public bool UseSquareSolvingMethod { get; set; }

check test case 14 + 15;
. these cases were wrong till we start holding hint on the square - after that change these cases were fixed.
on the other hand, we dont hold the hint on the square at the moment

usings;
http://stackoverflow.com/questions/125319/should-usings-be-inside-or-outside-the-namespace

---
. build a js grid - static probably - with data source - no square groups, just items..

. groupsnumbersgrid;
it only uses square type groups
since it doesnt show the numbers (but only available squareids) it looks bit weird!
set selected square doesnt work (handle it with the new grid)

. check updated group number availabilities

---
headache'in karmasikligini ortaya koy;
su anda eldeki tek casete; 3 rakamin konulabilecegi 3 tane kalmis durumda.. bunun uzerinden bir formul cikabilir..
ancak soyle caseler olabilir mi bakmak lazim;
2 tane numaranin konabilecegi 2 tane kare kaldi
1 numaranin konabilecegi 1 kare kaldi
4 numaranin konuabilecegi 4 kare kaldi? vs. vs.

ayrica
1 numaranin konabilecegi 1 kare kaldi
+ 2 numaranin konabilecegi 2 kare kaldi
+ 3 numaranin konabilecegi 3 kare kaldi (ayni anda)?

---
hint remove!

about hints, add these cases;
new hint
removal of an existing square
removal of an existing number

need 3 hint lists ?!
real hints
hints to be assigned
hints to be removed

in updatesquare, if the square has an existing number;
assign 0 first?
collect the hints to be removed

square hintlerini tutma ile ilgili - her square hint type 1 ve hint type 2 sekilde iki olasi hinti de tutuyor olmali gibi?
eger hintlerden biri gitse bile, digeri duruyorsa (ki sadece iki mi olabilir yoksa daha fazla da olabilir mi, bakmak lazim) hala hint olarak sayilmasi lazim.

before assigning the number, call remove hints method?

square hintlerini tutma ile ilgili - her square hint type 1 ve hint type 2 sekilde iki olasi hinti de tutuyor olmali gibi?
eger hintlerden biri gitse bile, digeri duruyorsa (ki sadece iki mi olabilir yoksa daha fazla da olabilir mi, bakmak lazim) hala hint olarak sayilmasi lazim.

---
hinti direk square ustunde tutmayi istiyoruz
ancak ne zaman set etme islemini gerceklestirecegiz?
hintleri toplayip, update islemi sonunda mi bakmak lazim? bu cok uzun bir yol gibi gorunuyor..
yoksa direk checkavailability icinden mi set etmek lazim? ancak bu da su haldeyken mumkun degil, henuz setavailability islemleri bitmemis durumda. iki setavail. de bittikten sonra set etsek yeterli olur mu?

bir de sunu kontrol et;
hint foundlar squarelerin availabilityleri kontrol edilmeden raise ediliyor - hint olarak bulunan squarein uzerinde rakam bile olabilir gibi?
nitekim group numberlar icin case 20!
square levelda da ayni sey oluyor mu, bak bakalim!

---
update square method optimization! - update availability + check availability cases!

---
square hint & group number hint
there is hintnew abstract class and ihint interface
to have public list of the hint, we can use ihint
but internally to be able to search them, it can be better to have 2 seperate lists?

---
maybe we can have a method for adding hints ?!

check lastavailability.square.isavailable before adding it - compare it with square hint add - OK

remove hint found event handler! - OK

2 important parts will be left;
. hint removal!
. use Square itself for holding the hint (it can have both of the hint types (square + group number))

. and then check headache!

---
currently case 6 clearly show that we can have multiple hint for one square
but of course the number must be the same - it looks like that we can have 4 different hint for one square;
. square type
. square group type
. horizontal group type
. vertical group type

---
THESE SHOULD BE OKAY?
but it feels like keep more than we need. cant we have this result directly from the squares or somethin'?
+ Isavailable still need to use Square.IsAvailable. cant we have it without it?

---
group - updateavailability;
// TODO Can this be better, elegant?
// Only updates the Updated property of the availabilities that uses the main square
var relatedAvailabilities = GroupNumbers
    .Select(groupNumber => groupNumber.Availabilities.Single(availability => availability.Square.Equals(square)));

---
after solve operation - if there ara multiple square that were solved, only the first one has Updated flag or something' ?!

---
after square value removal, some availabilities still stay on the page ?!?!?
and the color ?!

---
square method hint looks good
tests need to be updated
if its totally okay, remove the old version and go on with group number method

new method
square.assigntype will be hint !

---
sudoku exception + check invalid sudoku cases - throw sudoku exp. and catch that in try catch block
keep assert.fail() at the end - if you get assertfailedexception then its wrong?

---
try to do domino in reverse - remove the main inital value and then watch it removes all the other hints ?!

---
check these 2 exceptions ?!

square - remove hints (!sqr.equals(this)))
void RemoveHints()
{
    // Square level
    if (Sudoku.UseSquareLevelMethod)
        foreach (var square in RelatedSquares.Where(sqr => !sqr.Equals(this)))
            square.RemoveSquareMethodHint()

---
test it properly;
hint update + hint availability bug seems good but TEST IT !!!!


then continue with groupnumber square hint ?!?!?!?!?

---
web has some error probably because of hint updates!!

---
continue with invalid sudoku - next line fails ?!?!?;
sudoku.UpdateSquare(43, 1);

this is again about headache case - there are 4 squares left in the group but one of the squares cant have any of the numbers?!

+ real case timout!

---
check this block;
a. sudoku.updatesquare()
if (!(!selectedSquare.IsAvailable
    && !selectedSquare.IsHint
    && square.IsHint))

---
have a better "hints" section in web
squares have the hints - we dont have to retrieve them seperately anymore

---
domino case - remove a value from a square doesnt work
it cant remove the hints in reverse way
think about the order of the actions;
it could be;
a. set the values (set the old number to a temp value)
b. update availabilities (old number, new number)
c. update hint (remove + search)
?

and it feels like hints need to be checked whether they are still have the conditions ?!
probably this is not going to work;
var relatedSquares = RelatedGroups.SelectMany(group => group.Squares.Where(square => !square.Equals(this) && square.IsHint));

---
currently there is no "fail case"
but still removehints method doesnt look good
try to have better "reverse domino" cases
can a square only remove one hint? cant it be responsible for more than one?

---
YES, one square can be responsible for 4 hints!
check domino advanced case
when id 1 was removed it should remove 4 hints
currently it looks good but actually hints remove each other and thats why end result looks okay
but actually when the first hint removed, it should not find any other hint to remove - the other ones were unrelated to the first one
it just make a wide search and then find a hint to remove - check try to have the same route as in search hints ?!
make this thing BETTER!

add domino advanced to tests!
also update GroupNumberRemoveHintBug test method!

---
also check the case that the hints holding each other
should be "domino" case
it can find the hints by chain reaction, but we cant remove them with the same way?!

---
b2 bomber - there is a clear problem with this one.
update 1,1 + update 1,0 - it set squareId 45 to 0 and then finds it again. this is because it removes too much. make sure that this is not happening!

bomber b2 - v2 = have hint with 4 diff. ways!
add this to test cases!

---
start implementing Hints list in square - 4 different hints
if there is 1 hint, square will be hint type
remove square must follow the same router as search hint!
square type, it should be easy
think about group number type!

---
square + group number . isavailable prop are changed - check related places

---
group number hint removal doesnt work - simply continue with Case 5
remove one of the 1s and the hint stays
the problem is that the hint itself sets the other squares availability for number1 and when we check whether there is any availability, it cant find any.
in other words, it blocks itself ?! how to continue;
. earlier we were removing it anyway - without checking any condition - should we try that again?
. or we should ignore availability false situation in removehints case? but then can it lead to other problems?

