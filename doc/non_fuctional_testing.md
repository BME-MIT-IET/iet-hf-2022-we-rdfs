# __Nem funkcionális tesztek - teljesítmény teszt__
Azt vizsgáltam, hogy a könyvtár milyen futási idővel hajt végre különböző feladatokat.

Minden teszt futtatása a saját gépemen történt, az nyilván befolyásolta ez eredményeket.

## <ins>Gráf beolvasása fájlból és kiírása fájlba</ins>

Felhasznált adatbázis: [szepmuveszeti.rdf](https://datahub.io/dataset/data-szepmuveszeti-hu)

### 1. _Különböző fájlformátumok generálása_
A program 4-féle formátumból tud beolvasni gráf modelleket:
- Rdf/xml
- N-triples
- TriX
- Turtle

Első lépésként előállítottam az .rdf fájlból a vele ekvivalens .n3 és .trix fájlokat.

Sajnos a .ttl fájlt nem tudta deszerializálni a program, ezért azt nem teszteltem. Érdekes módon kisebb gráf esetén működött a beolvasás, ennél viszont nem. Ezt [ebben](https://github.com/BME-MIT-IET/iet-hf-2022-we-rdfs/issues/22) az issue-ban jeleztem.

### 2. _Fájlok beolvasása_
Minden fájlt 5-ször olvastam be és kiszámítottam a teljes, valamint az átlagos futási időt. 

| Formátum    | Teljes futási idő | Átlagos futási idő |
| :---        |    :----:         |          :---:     |
| rdf/xml     | 57706 ms          | 11541 ms           |
| n-triples   | 72802 ms          | 14560 ms           |
| triX        | 53736 ms          | 10747 ms           |

A leglassab az n-triple formátum olvasása volt.

### 3. _Fájlok kiírása_
Minden fájl formátumba 5-ször írtam ki a gráfot és kiszámítottam a teljes, valamint az átlagos futási időt. 

| Formátum    | Teljes futási idő | Átlagos futási idő |
| :---        |    :----:         |          :---:     |
| rdf/xml     | 234658 ms         | 46931 ms           |
| n-triples   | 7856 ms           | 1571 ms           |
| triX        | 6545 ms           | 1309 ms           |

Az rdf formátumba történő kiírása drasztikusan lassabb, mint a másik két formátum használata.

A részletes eredményeket az RDFSharp.NonFunctionalTests modul Results mappájában található "_load_and_write_results.txt_" tartalmazza.

## <ins>SPARQL lekérdezések futtatása</ins>

Felhasznált adatbázis: [szepmuveszeti.rdf](https://datahub.io/dataset/data-szepmuveszeti-hu)

Minden lekérdezést 10-szer futtattam és kiszámoltam a teljes és az átlagos futási időt.

### 1. _Összes alkotó kikeresése_

Ez egy egyszerű lekérdezés nincs szükség se összefűzésre se szűrésre.

- <ins>Teljes futási idő:</ins> 278 ms

- <ins>Átlagos futási idő:</ins> 28 ms

### 2. _Giovanni nevü alkotók kikeresése_

Ez már egy kicsit össztettebb lekérdezés, szükség van szűrés használatára.

- <ins>Teljes futási idő:</ins> 2161 ms

- <ins>Átlagos futási idő:</ins> 216 ms

A filter használata egy nagyságrendi ugrást hozott a futási időben.

### 3. _Alkotók és műalkotásaik összekötése_

Itt össze kellett kötni a lekérdezésnek az alkotókat az alkotásokkal, és azokat a fizikai tárgyakkal, ez már várhatóan egy időigényesebb folyamat.

- <ins>Teljes futási idő:</ins> 907 ms

- <ins>Átlagos futási idő:</ins> 91 ms

Az összefűzés kevesebb időt vesz igénybe, mint a szűrés.

### 4. _Rembrandt rézkarc technikával készült alkotásainak kikeresése_

Itt szükség van erőforrások összefűzésére és szűrésre is.

- <ins>Teljes futási idő:</ins> 10927 ms

- <ins>Átlagos futási idő:</ins> 1093 ms

Mint, ahogy várható volt ennek a lekérdezésnek kellett a legtöbb idő, de ez se jelentős.

A részletes eredményeket az RDFSharp.NonFunctionalTests modul Results mappájában található "_query_results.txt_" tartalmazza.

A nagy fájlok olvasása és írása nagyon időigényes, ez már felhasználóként is feltünik, azonban a lekérdezések futtatása nem igényel ember által érzékelhető időt.