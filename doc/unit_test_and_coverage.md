# Kód tesztlefedettségének javítása

### Okok
<p>Úgy láttuk, hogy a program unit teszt lefedettségén még lehetne javítani, ezért úgy döntöttünk, hogy bővítjük az egységteszt készletet.
A programkód alaposabb átnézése után úgy határoztunk, hogy a Semantics mappában kevés tesztelhető logika van 
(valószínűleg ezért nem lett írva hozzá eredetileg se unit test), ezért úgy döntöttünk a másik kevésbé lefedett mappát a Query-t fogjuk tesztelni, azon belül is a Mirella mappát.</p>

![image](https://user-images.githubusercontent.com/79516856/169266190-7145f934-5523-4c26-adaa-20c7c9c607a4.png)

![image](https://user-images.githubusercontent.com/79516856/169266832-9b4e100f-f6fd-4f0f-81a5-d5465e3b4d3c.png)

![image](https://user-images.githubusercontent.com/79516856/169266870-20a7ca26-1b52-45ec-92c2-60e078df6826.png)

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

![image](https://user-images.githubusercontent.com/79516856/169266994-829a1bfb-58b4-40dd-aba2-fa2a5a71ae47.png)
![image](https://user-images.githubusercontent.com/79516856/169267047-c5f45db8-aef9-4cf6-98d1-31f036d2062b.png)

### Egységtesztek javítása

<p>A tesztek felülvizsgálata további 5 egységtesztet eredményezett és a meglévők javítása is megtörtént. </p>
<ol>
    <li>RDFBoleanFilterTest.cs (szum új 3 unit teszt)
    <ol>
    <li>RDFBooleanAndFilter, RDFBooleanNotFilter és RDFBooleanOrFilter osztályokhoz</li>
    </ol></li>
    <li>RDFExsistFilterTest.cs		(2 új unit teszt)</li>
</ol>

<p>A végleges lefedettsége a Mirella mappának még 1,5%-kal nőtt, az Algebra mappa lefedettsége még 2%-ot emelkedett, ezzel a tesztekre választott 
"filter" osztályok tesztlefedettsége elérte a 100%-ot.</p>

![image](https://user-images.githubusercontent.com/79516856/169267178-1bdbb9d1-91b1-4f0f-98ca-4b4d733af42f.png)

![image](https://user-images.githubusercontent.com/79516856/169267218-66c58956-08ef-405e-963c-aaddf8546290.png)

### Összegezve
<p> Ezzekkel összesen  250 sor lett letesztelve a 45 darab tesztben, és az Algebra mappa tesztlefedettsége összesen 9%-kal lett nagyobb, a Mirella mappáé 6.5%-kal, még a Filter mappa tesztlefedettsége majdnem elérte a 100%-ot.
(Az általunk választott osztáylok lefedettsége ebben a mappában 100%.)</p>

![image](https://user-images.githubusercontent.com/79516856/169267298-fdabf4ad-586a-4c4a-8402-bab2114081ea.png) 
![image](https://user-images.githubusercontent.com/79516856/169267360-b2a64d9c-712e-46aa-a05e-33bf47a6040a.png)

