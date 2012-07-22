---
WRONG SUDOKU! - try this after fixing CASE 1: ID 5

1,1
2,2
3,3
4,4
5,5
6,6
7,7
8,8
9,9
10,4
11,5
12,6
19,7
20,8
21,9
13,1
14,2

---
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

- change the repository name

---
. CASE 1: ID 5, square 1 cannot have any number except 1.
ID 8, 3. grid cannot have any number except 1,2,3.

. CASE 2: ID 1, square 36 cannot have number 9.

. how about drag&drop the numbers?

. squaresLeft (numbersModel.ZeroValue count) can be improved, maybe moved to sudoku
details? - STILL NECESSARY?

. checkpotential & getpotentials methods in the engine should be better!

. retrieve only updated values from server - usedSquares + numbers count + availabilities!

. try to remove Container classes!
there is a problem about serialization but is it possible to make them compatible?
How to serialize Read-Only properties with Json.Net?

. validation; in general OK, but numpad values are ignored as well!

. improve general (content) styling! general list template? + use jQuery animations
?! - IE doesnt support "inherit" - try to have more generic css items ?!

. IEnumerable or list or ..? and any() or exists() or contains()?

. add samples for different sizes - 4 + 9 (OK) + 16 + 25? + wrong cases; invalid
sudoku, invalid number, invalid square, invalid assignment!

. unit test projects!

. firefox shows an error in case of 400 ?! is it normal

. step counter ?!

. multiple language support

. save to db?

. sudoku generator?

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
en son bunu gruba ekledýk, kontrol et sonuclarý;
GetAvailableSquaresForNumber

--
now its possible to use readonly property by default - check objects!

---
bir availability assign edildiginde (false) oldugunda, gruptaki diger squarelerin yeni availability listelerine o rakam ekleniyor olabilir..? ama sonra nasýl çýkaracaðýz?

---
try to use a profiler (eaqutec?) to see that how many times functions being called (especially set availability etc.)

---
sudokuId as querystring - and the application should listen when its loading - localhost/5 - load sudoku 5

---
size 4 sudoku is okay
but UI calculations are terrible - it takes too much time to apply them
try to create some shortcut css block for different size - 2,3,4,5 = 4,9,16,25?
also in size 4, counts are dissappeared ?! - how to display them?

---
horizontal + vertical groups for square? and then use them as well with ToggleSelected()
these calculations can come from the server?

---
offline mode? if the server is not there etc.?

---
first grade availability - only from its own group?
and is there any second grade availability?

---
diger SIKISMA caseleri neler olabilir? onlari arastir

eger bir numaranin konulabilecegi 2 kare varsa ve karedeki diger numara da sadece o 2 kareye konulabiliyorsa, o zaman SIKISMA var ihtimali uzerinde duruyoruz

ancak bu durumda sadece identical karelerde oluyormuþ gibi de gelmiyor.
bu olayý anlamak için daha fazla case gerekiyor gibi ?!?!?!

---
potentialfound eventine karþýlýk potentiallost eventi de olmasý gerekmiyor mu?

---
how about;
public class Square
{
	internal (or private) class Availability()
	{
		internal UpdateAvailability()
	}
	
	AND
	
	internal (or private) class Potential()
	{
	
	}
}

---
AND HOW ABOUT PUTTING POTENTIAL FLAG ON A SQUARE!
. PROPERTY ISPOTENTIAL? - POTENTIAL NUMBER - HOW IT BECAME POTENTIAL (REFERENCE GROUP OR SQUARE)
. AND THEN ADD/REMOVE POTENTIAL EVENTS!

---
TOGGLE AVAILABILITY ALTINDAKI EVENT RAISE DURUMUNU TEST ET!
AVAILABILITY CHANGED!

---
SQUARE LERE RELATED (NEIGHBOURS) EKLE VE ONLARIN EVENTLERINI TAKIP ET!
BOYLECE GROUP ALTINDA CHECKPOTENTIALSQUARE VE GETPOTENTIALSQUARE METHODLARINI SQUARELERE TASIYABILIRSIN..

ANCAK SU TUHAF SEYE DIKKAT ET!;
var b = CheckPotentialSquare(number);

if (b)
	System.Console.WriteLine("this strage thing found something");    
	
---
AYRICA POTENTIAL DEGIL, SOLVER FOUND THE NEXT ONE, ITS NOT A POTENTIAL ?!
VE SQUARE UZERINDE TUTMAYA CALIS BUNU, POTENTIALS LIST GIBI AYRI BIRSEYE GEREK YOK!

---
SQUAREGROUPTAKI SETSQUARE YERINE DAHA GUZEL BIR APPROACH OLABILIR MI?

---
square bulmamýza yardým eden kac event, method var, onlarý bir gözden gecir, fazla birþey mý vardýr?

---
Remove osp from namespace for now ?!

---
Doesnt work with vs2010 now?

 

Server Error in '/' Application.


--------------------------------------------------------------------------------

Could not load type 'System.Web.Http.RouteParameter' from assembly 'System.Web.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. 

---
CONTINUE WITH;

            foreach (var number in RelatedNumbers)
            {
                var b = CheckPotentialSquare(number);

                if (b)
                    System.Console.WriteLine("this strage thing found something");                
            }
			
NOW WE HAVE;
        void Group_SquareNumberChanged(Group sourceGroup, Square sourceSquare)
        {
            //if (this != sourceSquare)
            ToggleAvailability(sourceSquare.Number, sourceGroup.GroupType, sourceSquare);
        }
		
AND TRY TO MOVE THESE FUNC. INTO SQUARE CLASS!

IS IT POSSIBLE TO HAVE "FALSE" POTENTIAL, TRY TO TEST AND GET RID OF ADDITIONAL CHECKS!
CheckPotentialSquare()!

AND CONTINUE WITH THIS;
            var list = Availabilities.Where(a => a.IsAvailable());

            if (list.Count().Equals(1))
            {
                // TODO This should be a potential square!
            }
			
---
IT WAS NOT GOING BAD BUT, WE DID THIS PART AT THE END AND CASE 5 DOESNT WORK NOW;
            foreach (var number in AvailableNumbers)
            {
                var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable()));

                if (list.Count().Equals(1))
                {
                    // Get the item from the list
                    var item = list.Single();

                    if (PotentialFound != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Square.Group_SquareNumberChanged found a potential");
                        PotentialFound(new Potential(item, this, number, PotentialTypes.Group));
                    }                
                }
            }
			
			BECAUSE SQUARE TYPE GROUP (ID 1) IS NOT EVEN CHECKED. IT' ONLY CHECKING ITS OWN GROUPS (HOR 3, VER 7, SQU 3)
			BE CAREFUL ABOUT THESE TRIGGERS
			BUT ITS GOING GOOD AND HOPEFULLY IT WILL BE BETTER.. ?!

---
GITHUB TEST; TRYING TO UNDERSTAND ROLLBACK + REVERT OPTIONS
thýs ýs the second note before trying revert option

---
Git Tests

1. TortoiseGit - Working copy test - (looks) OK
2. TortoiseGit - Checkout test;
Note 1
