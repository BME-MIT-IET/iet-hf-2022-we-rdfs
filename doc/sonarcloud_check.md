# A SonarCloud statikus analízis eszköz futtatása és hibák átnézése

### Beüzemelés
Első lépésként a _SonarCloud_-ot a _Git repositorynk_-kal összekötve beüzemeltük, beállítottuk hogy minden _pull request_ és _merge_ esetén automatikusan lefusson a _SonarCloud_ által végzett statikus ellenőrzés.
_Kezdeti hibajelzések:_


### Ellenőrzés & Javítás
Elsőnek a 8 db _bug_ jelzést ellenőriztük, ezekből 2 volt ténylegesen lehetéges hibaforrás, ezeket javítottuk, a többi jelzés pedig _false pozitiv_ volt, ezekeket jelöltük.
Ellenőriztük a jelzett _sérülékenységeket_, ezek mind az _XML parsing_-hoz kapcsolódtak, biztonsági rést jelentett, az ismeretlen forrásból származó fájlok olvasása, ezt minden előforduló helyen kijavítottuk.
A _Security hotspot_ jelzések arra vonatkoztak, hogy az _RDF resource_-ok megadásához http protokollt használnak nem https-t, de ebben az esetben ez  nem jár kockázattal, valamint az RDF-ek kezeléséhez szükséges. Tehát ezekezt a jelzéseket biztonságosnak jelöltük.

Legvégül következtek a _Code Smellek_ javításai. 
Ezekből minél többet igyekeztünk átnézni és azokat amelyek valóban valamilyen hibát jelenthettek, javítottuk. (Elágazásokat olvasztottunk össze, type castolást egyszerűsítettünk c# nyelvi elemmel, felesleges metódus paramétereket és felesleges lokális változó értékadásokat töröltünk, teljesítmény javítása (pl .Count==0 helyett .Any() hívás) és szálbiztosság (statikus változó csak olvashatóvá tétele) érdekében, kódrészleteket cseréltünk.

## A javítások után a SonarCloud átviszgálásának eredménye:

Látható, hogy sikeresen ellenőrizünk minden jelzést, egyedül _Code Smell_-ek maradtak a kódban, de azok számát is sikerült jelentősen csökkentenünk.

### Összefoglalás
Összességében elmondható, hogy a vizsgált projekt nagyon alaposan és jól el lett készítve. Ahhoz képest, hogy milyen óriási mennyiségű kódot tartalmaz, csak néhány hibás részletet találtunk benne.
Ezeket most kijavítottuk, így a kód minőségét sikeresen javítottuk, valamint biztonságosabb lett a használata is.




