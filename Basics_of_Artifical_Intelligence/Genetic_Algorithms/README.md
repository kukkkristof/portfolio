[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/Gu1mgFJm)

|                     |                                 |
| ------------------- | ------------------------------- |
| Name                | Kukk Kristóf                    |
| NEPTUN              | P2MZHY                          |
| Programming Language| C# 13 (dotnet 9.0.403)          |
| Time Spend (Hours)  | ~15                             |
| What was boring     | I would not say, that any part of it was boring. |
| What was interesting| The Rastrigin function          |
| What was Difficult  | Trying to make one algorithm for TSP and the functions (I gave up) |
| Opinion             | More task, and less documentation heavy assignment |

[**A feladat leírása**](feladat.pdf).

# Algoritmus(ok) ismertetése

## Függvény minimalizáció:
A program alapja a 'Runner.cs', amiben definiálva van egy 'DebugRun' függvény, ha kisérletezgetni akarunk, akkor ebben kell állítani a paramétereket és ezt kell meghívni a 'program.cs'-ből

### GeneticEngine
A genetic engine tárol az algoritmusról minden információt
A mutáció, rekombináció, szelekció és túlélési valószínégeknek külön osztályokkal rendelkeznek, ezekben az osztályokban vannak a megfelelő beállítások részletesen itt nem mennék bele minden részletbe, majd a védésen, mert hajnali 3 van és 2 ZH-t is írok ma :(

### Egyéb
A rekombináció, túlélési valószínűség és kiválasztási algoritmusok statikus függvényenként vannak implementálva majd egy Function pointerrel van rájuk refferálva a megfelelő beállítási osztályokban

## TSP:
Egy egyszerűbb kivitelezése az előzőnek, nem annyira összetett, a kromoszómák indexeket tárolnak

### Problem.cs
A 'problem.cs'-ben találhatóak a városok a koordinátáikkal és egy távolság lekérő függvény euklédeszi távolságra nézve

### Szelekció és túlélési valószínűségek
Teljes mértékben át van emelve a Függvény minimalizációs problémából, hiszem a kromoszómáknak itt is van fitness és value értékei, amiket ezek a függvények használnak, különbség annyi, hogy más a kromoszóma osztály neve.

# Futási eredmények

Megtalálhatóak a Research.xlsx fájlban

# Rastrigin My Beloved
Check this out(2D rastrigin): https://www.desmos.com/3d/vshrfyqlmu
