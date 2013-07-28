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

. pass squares to group constructor - to make square property readonly and remove setsquare method
not that easy, first it generates the groups and then squares.. ?!

. all javascript functions should be under an object ?!

. do we need ko.throttle? for css calculation?

. proper new/remove hint - is it possible to merge hint + square
dynamic hints?

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

. content manager can be better - with all external internal read file checkupdates etc. stuff
but do it at the end. probably will not have external content ?!

. square + availabilities?

. check todoList.dataAccess.js - leuk programming

---
square + hint merge;
. init;
.. first constructors (square + groups)
.. second cross bind
.. third event registrations!

. number changing + number changed + set availiability parts look good
. find hint + remove hint parts need to be updated ?!

try to create good test cases!!!

---
REMOVAL OF THE HINTS ?

IF THERE IS AN EXISTING NUMBER - FIRST REMOVE THAT ONA AND RELATED HINTS..
THEN ASSIGN THE NEW NUMBER!!!

write this part from scracth!

assign and removal of the squares and the hints..

need 3 lists ?!
real hints
hints to be assigned
hints to be removed

in updatesquare, if the square has an existing number;
assign 0 first?
collect the hints to be removed

before assigning the number, call remove hints method?

---
first of all, we are calling some methods too much ?!?!?!

source square + changed -> group-square + changed -> related squares + availability changed -> related squares groups 


Stats:
Group_Square_AvailabilityChangedCounter: 81
Square_Group_SquareAvailabilityChangedCounter: 729

23
81

checkmethod2 runs on affectedgroups
checkmethod1 runs on all squares? - make it on sudoku level?

hint removal is gone!! - be careful

do we need groupavailability? and squareavailability?

affectedsquares from this operation ?!

clearsquare() just to put zeronumber
and use this, if there is an existing number on the square
and then we dont need square_numberchanging probably..

after these, continue with hints..

---
CHECK METHOD 1I KISALLTIK, BIR KONTROL ET BAKALIM, 81 YERINE SADECE AVAILABILITYLERI DEGISMIS OLANLAR DURTULUYOR SIMDI ?!?!?!?!?

AYNI SEYI GROUP AVAILABILITY OLARAK TERKAR ET VE UYGULAMAYA BAK.. AMA KONTROLLERI NASIL YAPACAN, ONA DIKKAT ET!

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
			
uygulamayi temizle

sonra da hintlerle devam et

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
hint removal

square clear doesnt work

group - check availability'yi simdilik set availability icinden cikaracagiz.
orada kaldigi zaman soyle bir case olusuyor. bir rakami set ettigimizde, tek tek grup icinden availabilityleri set ederken,
8. set sonrasinda, 9. kare hintmis gibi gorunuyor (tek bir square ile deneme yapildiginda acikca goruluyor).
bu aslinda yanlis bir hint, sadece set etme isi bitmemis oldugu icin ortaya cikiyor.
eger check isini set icinde yapacaksak, bu durumda, 9. kare icin availability set edildiginde, bu "yanlis hint"i de kaldirabiliyor olmamiz lazim.
simdilik bununla ugrasmayi istemedigimizden dolayi simdilik check isini, setten sonra yapacagiz.
her ihtimalde hint removal isleminin temiz olmasi lazim ve sonraki asamalarda bununla ilgileniyor olacagiz.
hallolursa tekrar check + set group availability birlestirilebilir (bu arada square set + checkte benzer bir sorun yok - umarim ilerde de neden buna gerek olmadigini kolaylikla gorebiliyor olurum).

square hintlerini tutma ile ilgil - her square hint type 1 ve hint type 2 sekilde iki olasi hinti de tutuyor olmali gibi?
eger hintlerden biri gitse bile, digeri duruyorsa (ki sadece iki mi olabilir yoksa daha fazla da olabilir mi, bakmak lazim) hala hint olarak sayilmasi lazim.

---
clear availability ile devam ediyor olmamiz lazim..
eskide numberchanging kullaniyorduk ama bunu da yine update square altindan digerlerine eriserek yapabiliriz..
setavailability methodlarini mi kullanmali, yoksa clearavailability gibi farkli bir method mu yaratmali?

---
clear availability kismi da tamam gibi - numberchanging eventi yerine yeni yolu (direk) kullaniyoruz.
tek sorun, square set availability icinde check availability var ve hint ariyor.. oysa bu sefer hint remove deniyor olmasi gerekiyordu..
ozetle hint removal ile devam et! su anda yuksek ihtimalle duzgun calismiyor

ayrica sudoku classi uzerinde biraz daha temizlik yapiyor olabiliriz

bunlar da henuz kullanilmiyor;
        public bool UseGroupSolvingMethod { get; set; }
        public bool UseSquareSolvingMethod { get; set; }

bir de, groupnumberavailabilityleri web tarafindan gostermek icin;
[squareId'ler] karelere sirali - kare sirasi hep 1-9 - is avail degilse ustu cizili gibi dene bakalim ?!?!?

check test case 14 + 15;
. these cases were wrong till we start holding hint on the square - after that change these cases were fixed.
on the other hand, we dont hold the hint on the square at the moment

CONTINUE WITH;
hint remove!
update square method optimization!
draw groupnumberavailabilities on the client!

usings;
http://stackoverflow.com/questions/125319/should-usings-be-inside-or-outside-the-namespace

---
group number availabilities i draw etmeye calisiyoruz ama cok yamuk gidiyor!!!
groups yok hata mesaji var su anda..

genel olarak guzel bir grid templateimiz olsa, statik, guzel olabilirdi.. biz ona data source versek te o da ne geliyorsa ona doldursa vs. ?!

groupsnumbergrid ile ilgili, sadece square type group number availabilityleri alacagiz.. ve onlari, numara sirasina gore, square id leri yazarak cizecegiz..

diger taraftan.. set selected square calismayacak misal, cunku o square classi ile birlikte calisiyor, oysa burda number kullaniyoruz ana obje olarak - hatta bu number bile uygun degil, cunku uzerinde availabilities gibi bir array yok - sonradan ekleyecegiz gibi duruyor..

---
group number availabilityleri generate ediyoruz ama find kisminda sorun var - oradan devam et!
