[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/eNaAwWLc)
# Mesterséges Intelligenciák I. beadandó feladat

|                     |                                 |
| ------------------- | ------------------------------- |
| Név                 | Kukk Kristóf                    |
| NEPTUN              | P2MZHY                          |
| Programozási nyelv  | C# 12 (dotnet 8.0.403)          |
| Eltöltött idő (óra) | ~12 (Programozás és utánajárás) |
| Mi volt unalmas?    | Nem mondanék semmit kifejezetten unalmasnak. |
| Mi volt érdekes?    | Fejlesztés során meg lehetett figyelni, hogy apróbb változtatások mennyivel befolyásolják egy algoritmus futási idejét és kimenetét, akár csak a szúrás helye is nagy változást jelenthet. |
| Mi volt nehéz?      | Mivel sok óra kimaradt családi okokból, így az algoritmusok megértését mondanám. |

## Feladat

A feladat egyszerű és optimális keresők programozása
**választott programozási nyelvben**.
A teszteléshez a gráfok a [`graphs.txt`](./graphs.txt) fájlban találhatóak.
A feladatot **egyénileg kell elkészíteni**, a félév végén **mindenkinek be kell mutatnia a megoldását**!

Implementálja és tesztelje az alábbi algoritmusokat:

- DFS
- BFS
- Hegymászó keresés (Hill-climbing)
- Nyalábkeresés (Beam search)
  - $w = 2$
- Elágazás és korlátozás (B&B)
  - Mindhárom verzió
- A\*

A programnak képesnek kell lennie a graphs.txt beolvasására és a gráf automatikus felépítésére!


### Dokumentáció
!Ahhoz, hogy a kiiratás szép legyen a konzolnak legalább 80 karakter szélesnek kell lennie és monospace font legyen beállítva!

Az algoritmusok működését és a kapott eredményeket dokumentálja!

- Algoritmusok ismertetése
- Futási eredmények (útvonal, hossz)
- Futási idő
- Kiterjesztett csomópontok száma

A fájl tetején található `yaml` táblázat kitöltése egyéni bevallás alapján kötelező!

[Angol nyelvű segédlet](./tutorial.md).

## Algoritmusok bemutatása

### DFS
Depth-First Search esetén, mindig az első elemét vesszük a listánknak, azt kibővítjük és csak akkor dobjuk el, ha már nem tudunk hova lépni, csak visszafelé és nem értünk a célba. Ha célba értünk megkapunk egy megoldást.

### BFS
Breadth-First Search megegyezik a Depth First Search-el, annyi változtatás van, hogy a lista végére szúrjuk a kibővített utvonalakat, nem az elejére.

### HC
Olyan mint a Depth-First Search, viszont a kibővített útvonalakat Heurisztika alapján sorba rendezzük és úgy szúrjuk be a megfelelő helyre.

### Beam
Beam azaz nyalábkezelésnél a sorunkban lévő összes útvonalat szétbontjuk, majd azokat rendezzük heurisztika alapján. Amint a rendezés megtörtént, egy előre meghatározott w (width = szélesség) paraméternyi legjobb megoldásokat tartjuk meg, azaz ha w = 2, akkor csak a legjobb 2 értékkel rendelkező útvonalat tartjuk meg, a többit eldobjuk.

### B&B
A sorunkat minden szétbontás után rendezzük az eddig az útvonalon megtett távolság alapján.

#### Lista
A soronkat ugyanúgy rendezzük mint korábban, viszont egy külön, másodlagos listában nyilvántartjuk az eddig szétbontott pontokat, és ha valahol a szétbontásnál egy- a listában szereplő ponthoz jutunk, akkor azt az útvonalat eldobjuk.

#### Heurisztika
Ugyanaz, mint az eredeti Branch and Bound (Elágazás és korlátozás), viszont a megtett távolsághoz még hozzá adjuk az útvonal utolsó elemének a heurisztikus távolságát a célponthoz.

### A\*
Branch and Bound listával és a heurisztikai számítással, azaz a korábban meglátogatott pontokat eldobjuk és távolság + heurisztika alapján rendezünk

## Futási eredmények

### Tesztelési hardver
- Processzor  | Ryzen 5 2600 @ 3.85 GHz
- Memória     | 2x 8GB @ 3200 MT/s on DIMM2

### GRAPH_0

| Algoritmus   | Útvonal      | Futási idő (ms) | Kiterjesztések száma  |
| :----------- | :----------- | :-------------: | :-------------------: |
| DFS          | n1 n2 n3 n4  |     0.0199      |           3           |
| BFS          |    n1 n4     |     0.0035      |           2           |
| HC           |    n1 n4     |     0.0064      |           1           |
| Beam         |    n1 n4     |     0.0355      |           1           |
| Best-first   |    n1 n4     |     0.0043      |           1           |
| B&B          |    n1 n4     |     0.0054      |           2           |
| B&B w. List  |    n1 n4     |     0.0069      |           2           |
| B&B w. Heur. |    n1 n4     |     0.0048      |           1           |
| A\*          |    n1 n4     |     0.0164      |           1           |

### GRAPH_1

| Algoritmus   | Útvonal | Futási idő (ms) | Kiterjesztések száma  |
| :----------- | :------ | :-------------: | :-------------------: |
| DFS          | a b c d |     0.0165      |           3           |
| BFS          | a b d   |     0.0035      |           4           |
| HC           | a b c d |     0.0661      |           3           |
| Beam         | a c d   |     0.0230      |           4           |
| Best-first   | a b c d |     0.0082      |           3           |
| B&B          | a c d   |     0.0115      |           5           |
| B&B w. List  | a c d   |     0.0227      |           4           |
| B&B w. Heur. | a b d   |     0.0119      |           5           |
| A\*          | a b d   |     0.0093      |           4           |

### GRAPH_2

| Algoritmus   | Útvonal       | Futási idő (ms) | Kiterjesztések száma |
| :----------- | :------------ | :-------------: | :------------------: |
| DFS          | S A C D E F G |     0.0237      |           9          |
| BFS          |   S A C E G   |     0.0447      |          26          |
| HC           |  S A D H F G  |     0.0084      |           5          |
| Beam         |  S B Y C E G  |     0.0283      |           8          |
| Best-first   |   S A D E G   |     0.0114      |           5          |
| B&B          |   S B C E G   |     0.1518      |          14          |
| B&B w. List  |   S B C E G   |     0.0635      |          13          |
| B&B w. Heur. |   S B C E G   |     0.0307      |           6          |
| A\*          |   S B C E G   |     0.0352      |           6          |

### GRAPH_3

| Algoritmus   | Útvonal | Futási idő (ms) | Kiterjesztések száma  |
| :----------- | :------ | :-------------: | :-------------------: |
| DFS          | s x w g |     0.0063      |           5           |
| BFS          | s x w g |     0.0092      |           9           |
| HC           | s y w g |     0.0190      |           3           |
| Beam         | s y w g |     0.0126      |           5           |
| Best-first   | s y w g |     0.0092      |           3           |
| B&B          | s x w g |     0.0378      |           9           |
| B&B w. List  | s x w g |     0.0169      |           7           |
| B&B w. Heur. | s z w g |     0.0253      |           5           |
| A\*          | s y w g |     0.0164      |           3           |

### GRAPH_FOR_HEURISTICS

| Algoritmus   | Útvonal | Futási idő (ms) | Kiterjesztések száma  |
| :----------- | :------ | :-------------: | :-------------------: |
| DFS          | S A C G |     0.0206      |           4           |
| BFS          | S A C G |     0.0056      |           6           |
| HC           | S A C G |     0.0056      |           3           |
| Beam         | S B C G |     0.0228      |           5           |
| Best-first   | S A C G |     0.0074      |           3           |
| B&B          | S A C G |     0.0130      |           7           |
| B&B w. List  | S A C G |     0.0203      |           5           |
| B&B w. Heur. | S A C G |     0.0147      |           3           |
| A\*          | S A C G |     0.0096      |           3           |

### graph GRAPH_FOR_HEURISTICS_TRICKY

| Algoritmus   | Útvonal   | Futási idő (ms) | Kiterjesztések száma  |
| :----------- | :-------- | :-------------: | :-------------------: |
| DFS          | S A B D G |     0.0247      |           5           |
| BFS          | S A B D G |     0.0067      |           7           |
| HC           | S A B D G |     0.0175      |           4           |
| Beam         | S A B D G |     0.0134      |           6           |
| Best-first   | S A B D G |     0.0087      |           4           |
| B&B          | S A B D G |     0.0322      |           8           |
| B&B w. List  | S A B D G |     0.0129      |           6           |
| B&B w. Heur. | S A B D G |     0.0264      |           4           |
| A\*          | S A B D G |     0.0120      |           4           |