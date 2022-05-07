**Projekt céljának összefoglalása**

Az RDFSharp egy *pehelysúlyú C# könyvtár*, melynek célja, hogy különböző alkalmazások és szolgáltatások számára lehetővé tegye RDF adatok modellezését, tárolását valamint lekérdezését. Ezenfelül az RDF adatok validálására is lehetőséget nyújt XHACL modellek segítségével.
A könyvtár célja, hogy a .NET közösség számára egy olyan keretet adjon, melyen keresztül egyszerűen, gyorsan és barátságos módon
dolgozhatnak az RDF modellekkel és egyéb szemantikus technikákkal.

A projekt 4 fő package-et tartalmaz, a különböző Szemantikus Web rétegeknek megfeleltethetően: 

**Model**:      RDF modellek létrehozása és managelése, SHACL shapes létrehozása és validálása

**Store**:      RDF gyűjteméhnyek létrehozása és menedzselése.

**Query**:      Létrehoz és végrehajt SPARSQL lekérdezéseket és módosításokat gráfokon vagy gyűjteményeken

**Semantics**:  Létrehoz és menedzsel OWL ontológiákat és azok elemeit. Különféle szamantikai szabályok alapján lehet őket validálni, valamint SKOS sémák és SWRL reasoners-eket is itt lehet létrehozni.
