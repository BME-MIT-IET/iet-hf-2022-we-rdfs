# Kód tesztlefedettségének javítása

### Okok
<p>Úgy láttuk, hogy a program unit teszt lefedettségén még lehetne javítani, ezért úgy döntöttünk, hogy bővítjük az egységteszt készletet.
A programkód alaposabb átnézése után úgy határoztunk, hogy a Semantics mappában kevés tesztelhető logika van 
(valószínűleg ezért nem lett írva hozzá eredetileg se unit test), ezért úgy döntöttünk a másik kevésbé lefedett mappát a Query-t fogjuk tesztelni, azon belül is a Mirella mappát.</p>

### Egységtesztek bővítése

<p>Elsőkörben 37 darab egységteszttel egészítettük ki a meglévő teszteket a Query>Mirella mappában lévő osztályokhoz. </p>

<ol>
    <li>RDFBooleanFilterTest.cs 	  (16 db Unit teszt)
    <ol>
    <li>RDFBooleanAndFilter, RDFBooleanNotFilter és RDFBooleanOrFilter osztályokhoz</li>
    </ol></li>
    <li>RDFExistsFilterTest.cs 	    (10 db Unit teszt)
    <ol>
    <li>RDFExistFilter és RDFNotExistsFilter osztályokhoz</li>
    </ol></li>
    <li>RDFOperationPrinterTest.cs 	(7 Unit teszt)</li>
    <li>RDFAskQueryTest.cs          (4 új Unit teszt)</li>
</ol>

<p>Ezekkel a tesztekkel sikerült a Mirella mappa lefedettségét növelni összesen 5%-kal, az Algebra mappáét 7%-kal. Ez összesen  209 új letesztelt sort jelent, 
ám az  Algebra/Filter mappára vonatkozó teszteink hagytak lefedetlen részeket a kódban, ezért ezeket felülvizsgáltuk.</p>

### Egységtesztek javítása

<p>A tesztek felülvizsgálata további 8 egységtesztet eredményezett és a meglévők javítása is megtörtént. </p>
<ol>
    <li>RDFBoleanFilterTest.cs (szum új 4 unit teszt)
    <ol>
    <li>RDFBooleanAndFilter, RDFBooleanNotFilter és RDFBooleanOrFilter osztályokhoz</li>
    </ol></li>
    <li>RDFExsistFilterTest.cs		(4 új unit teszt)</li>
</ol>

<p>A végleges lefedettsége a Mirella mappának még 1,5%-kal nőtt, az Algebra mappa lefedettsége még 2%-ot emelkedett, ezzel a tesztekre választott 
"filter" osztályok tesztlefedettsége elérte a 100%-ot.</p>

### Összegezve
<p> Ezzekkel összesen  250 sor lett letesztelve a 45 darab tesztben, és az Algebra mappa tesztlefedettsége összesen 9%-kal lett nagyobb, a Mirella mappáé 6.5%-kal, még a Filter mappa tesztlefedettsége majdnem elérte a 100%-ot.
(Az általunk választott osztáylok lefedettsége ebben a mappában 100%.)</p>
