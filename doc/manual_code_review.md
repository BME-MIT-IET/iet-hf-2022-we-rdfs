# Manuális kód átvizsgálás dokumentációja 


## Kezdeti lépések:
 <ul>
    <li>Első lépésként tüzetesen átnéztük a könytárat. Ezzel arra jöttünk rá, hogy a Semantics csomag nincs tesztelve így ezt lenne érdemes átnéznünk.</li>
     <li>A Semantics csomag elég nagy így csak egy részét néztük meg.</li>
     <li>A következőket állapítottuk meg:</li>
     <ul>
     <li> Az RDFSemanticsEnums enumokat gyűjti össze, RDFSemanticsEvents az eventeket ,RDFSemanticsException az exceptionokat amiket a Semantics tud dobni.
     <li>Az RDFSemanticsUtilities a convertet végzi, RDF gráfból ontológiát.</li>
     </ul>
 </ul>
 
## Checklist:
<p>Az interneten talált checklist alapján haladtunk tovább:</p>

![Code review checklist](images\manual_code_review/code_review_steps.png)

<p>Lépések:</p>

<ol>
    <li>Verify feature requirements:</li>
    <ul>
        <li>Ezen a ponton biztosan átmegy, kódban megvan és dokumentálva is vannak a megvalósított funkciók. </li>
    </ul>
    <li>Code readability:</li>
    <ul>
        <li>Kód olvashatóság szempontjából is megfelelő a kapott kód. Jól vannak tagolva a függvények, néhol egy-egy eléggé hosszú viszont ez a téma komplexitása miatt indokolt. </li>
    </ul>
    <li>Coding Style:</li>
    <ul>
        <li>A kódolási stílus (Coding Style) megfelelel a c#-os konvencióknak: nagybetűs osztálynevek, interfészeknél I-kezdet, publikus tagokat nagybetűvel használja a privát és belső tagokhoz "_" használ és camelCaset.
<a>https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions</a> </li>
    </ul>
    <li>Clear naming:</li>
    <ul>
        <li>A kódban levő osztálynevek, interfész nevek és függvény nevek érthetőek és eléggé lefedik azt, hogy pontosan milyen funkciót töltenek be. Így a kód a Clear naming ponton is átment. </li>
    </ul>
    <li>Code duplication:</li>
    <ul>
        <li>Kód duplikációt (Code duplication) manuálisan nem találtunk ebben a csomagban.
Próbáltunk keresni olyan extension-t ami megkeresi a kód duplikációkat viszont nem találtunk ilyet ezért csak manuálisan néztük át. </li>
    </ul>
    <li>Tests:</li>
    <ul>
        <li>Ez a pont a tesztek átnézése viszont erre a csomagra nincs teszt, így ezeket nem tudtuk megnézni, hogy megfelelőek-e. </li>
    </ul>
    <li>Documentation:</li>
    <ul>
        <li>A Dokumentáció teljesen rendben van a kód maga is végig van kommentezve a konvencióknak megfelelően
(<a>https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags</a>), valamint a GitHub-on van PDF alapú dokumentáció is csomagokra bontottan. </li>
    </ul>
</ol>

## Osztálydiagram:
<ul>
    <li>Generáltunk a Visual Studio segítségével egy osztálydiagramot.</li>
    <li>Semantics projekt struktúrájának vizsgálata: generált osztálydiagrammból azt szűrtük le, hogy nincsenek körkörös függőségek.</li>
</ul>

![Class diagram #1](images\manual_code_review/class_diagram_2.png)

<ul>
    <li>Vannak olyan osztályok amik csak bizonyos függvényeken belül vannak használva viszont nem mint attribútum így ezek nincsenek összekötve egyik másik osztállyal sem.</li>
</ul>

![Class diagram #2](images\manual_code_review/class_diagram.png)