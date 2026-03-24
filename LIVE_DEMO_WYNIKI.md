# 🎯 SZYBKI PORADNIK - LIVE DEMO

## ✅ WŁAŚNIE ZROBILIŚMY:

### 1️⃣ BUDOWANIE PROJEKTU
```
Build succeeded.
0 Errors
```
✅ Projekt się kompiluje bez błędów!

### 2️⃣ URUCHOMIANIE WSZYSTKICH TESTÓW
```
DataTest:              8 testów ✅  (w tym nowy: VectorZeroValuesTestMethod)
BusinessLogicTest:     7 testów ✅
PresentationModelTest: 5 testów ✅
PresentationViewModelTest: 2 testów ✅
─────────────────────────────────
RAZEM: 22 testów ✅
```

### 3️⃣ URUCHOMIENIE KONKRETNEGO TESTU
```
Passed VectorZeroValuesTestMethod [12 ms]

Test Run Successful.
Total tests: 1
Passed: 1
```
✅ Nowy test przechodzi!

---

## 📂 GDZIE SĄ TESTY - POKAZANE NA EKRANIE

```
ReactiveInteractiveUserInterface/
├── BusinessLogicTest/
│   ├── BusinessBallUnitTest.cs      👈 4 testy
│   ├── BusinessLogicUnitTest.cs     👈 2 testy
│   └── PositionUnitTest.cs          👈 1 test
│
├── DataTest/
│   ├── BallUnitTest.cs              👈 2 testy
│   ├── VectorUnitTest.cs            👈 2 testy (+ 1 nowy!)
│   └── DataImplementationUnitTest.cs 👈 4 testy
│
├── PresentationModelTest/
│   ├── ModelBallUnitTest.cs         👈 2 testy
│   └── PresentationModelUnitTest.cs 👈 3 testy
│
└── PresentationViewModelTest/
    └── MainWindowViewModelUnitTest.cs 👈 2 testy
```

---

## 🚀 KOMENDY NA LINUX (Skopiuj i uruchom!)

### Wejdź do folderu:
```bash
cd /home/files/szkolne/sem4/TPW/ProgramowanieWspolbiezne
```

### Uruchom wszystkie testy:
```bash
dotnet test
```

### Uruchom testy ze szczegółami (zobaczysz każdy test):
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Uruchom konkretny test:
```bash
dotnet test --filter VectorZeroValuesTestMethod
```

### Uruchom testy z konkretnej warstwy:
```bash
dotnet test DataTest
dotnet test BusinessLogicTest
```

### Zbuduj projekt:
```bash
dotnet build
```

### Zbuduj Release (szybsza wersja):
```bash
dotnet build --configuration Release
```

---

## 🪟 JAK URUCHOMIĆ PROGRAM NA WINDOWS

Jeśli masz Windows:

### Metoda 1: Z wiersza poleceń
```cmd
cd C:\ścieżka\do\ProgramowanieWspolbiezne
dotnet run --project ReactiveInteractiveUserInterface/GraphicalUserInterface/PresentationView.csproj
```

### Metoda 2: Visual Studio (najlepiej!)
1. Otwórz `ConcurrentProgramming.sln` w Visual Studio
2. Kliknij na projekt `PresentationView` w prawym panelu
3. Wciśnij **F5** (Run)

### Wynik:
Zobaczysz okno WPF z interfejsem graficznym!

---

## 🧩 CZYTANIE STRUKTURY - PRZYKŁAD

Weź plik testowy:
```
ReactiveInteractiveUserInterface/DataTest/VectorUnitTest.cs
     ↑                           ↑       ↑
     |                           |       Plik testu
     |                           Folder z testami
     Główny folder projektu
```

**Ścieżka bezwzględna na Linux:**
```
/home/files/szkolne/sem4/TPW/ProgramowanieWspolbiezne/ReactiveInteractiveUserInterface/DataTest/VectorUnitTest.cs
```

**Namespace w kodzie C#:**
```csharp
namespace TP.ConcurrentProgramming.Data.Test
{
    [TestClass]
    public class VectorUnitTest
    {
        // zgadza się z folderem: DataTest
    }
}
```

---

## 🔍 OTWARCIE PLIKU TESTU (CZYTAJ TO!)

```bash
# Otwórz w edytorze tekstowym na Linux
cat /home/files/szkolne/sem4/TPW/ProgramowanieWspolbiezne/ReactiveInteractiveUserInterface/DataTest/VectorUnitTest.cs
```

**Będziesz widzieć:**

```csharp
namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]           👈 To mówi: "To klasa testów"
  public class VectorUnitTest
  {
    [TestMethod]        👈 To mówi: "To jeden test"
    public void ConstructorTestMethod()
    {
      // Przygotuj dane
      Vector newInstance = new(1.0, 2.0);
      
      // Sprawdź wynik
      Assert.AreEqual<double>(1.0, newInstance.x);  👈 Ten test sprawdza czy x = 1.0
      Assert.AreEqual<double>(2.0, newInstance.y);  👈 Ten test sprawdza czy y = 2.0
    }

    [TestMethod]        👈 NOWY TEST - dodałem ja!
    public void VectorZeroValuesTestMethod()
    {
      Vector zeroVector = new(0.0, 0.0);
      Assert.AreEqual<double>(0.0, zeroVector.x);
      Assert.AreEqual<double>(0.0, zeroVector.y);
    }
  }
}
```

---

## 📊 ARCHITEKTURA - WIZUALNIE

```
                    WARSTWA 3: PREZENTACJA
                    ┌──────────────────┐
                    │ MainWindow.xaml  │  ← To co widzi użytkownik
                    │ (Interfejs GUI)  │
                    └────────┬─────────┘
                             │
                    WARSTWA 2: LOGIKA
                    ┌────────┴─────────┐
                    │ ViewModel        │
                    │ BusinessLogic    │  ← Tu się dzieje magia!
                    └────────┬─────────┘
                             │
                    WARSTWA 1: DANE
                    ┌────────┴─────────┐
                    │ Data Layer       │
                    │ (Ball, Vector)   │  ← Przechowywanie danych
                    └──────────────────┘
```

**Każda warstwa ma swoje testy:**
- Warstwa 1: `DataTest/` - testuje Ball i Vector
- Warstwa 2: `BusinessLogicTest/` + `PresentationModelTest/` - testuje logikę
- Warstwa 3: `PresentationViewModelTest/` - testuje ViewModel

---

## 💡 PRZYPOMNIJ SOBIE POJĘCIA

| Pojęcie | Co to jest | Gdzie jest |
|---------|-----------|-----------|
| `Test` | Sprawdza czy kod robi co powinien | `*Test.cs` pliki |
| `Assert` | Sprawdzenie w teście | W środku metody `[TestMethod]` |
| `Namespace` | Organizacja kodu | Na górze pliku `namespace ...` |
| `[TestClass]` | Mówi: "To zawiera testy" | Nad klasą |
| `[TestMethod]` | Mówi: "To jeden test" | Nad metodą |
| `XAML` | Wygląd interfejsu | Pliki `.xaml` |
| `.csproj` | Konfiguracja projektu | W każdym folderze projektu |

---

## ❓ CZEMU TO JEST WAŻNE?

✅ **Testy** = Pewność że kod działa  
✅ **Architektura warstwowa** = Łatwa zmiana kodu  
✅ **Linux + .NET** = Praca wszędzie  
✅ **C#** = Nowoczesny, łatwy do nauki

---

## 🎓 NASTĘPNE KROKI

1. **Uruchom testy na swoim komputerze:**
   ```bash
   dotnet test
   ```

2. **Otwórz plik testu i czytaj go:**
   ```bash
   cat ReactiveInteractiveUserInterface/DataTest/VectorUnitTest.cs
   ```

3. **Spróbuj dodać swój test** (będę ci pomagać!)

4. **Wyślij do prowadzącego** konto GitHub z tagiem (0.1.0)

Masz pytania? Napisz! 🚀

