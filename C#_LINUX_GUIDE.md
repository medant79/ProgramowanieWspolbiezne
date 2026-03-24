# 📚 Poradnik: Testy, Program i Struktura Katalogów C#

## 🗂️ GDZIE SĄ TESTY?

```
ProgramowanieWspolbiezne/
└── ReactiveInteractiveUserInterface/
    ├── DataTest/                    👈 TESTY dla warstwy Data
    │   ├── VectorUnitTest.cs        ✅ Testy Vector
    │   ├── BallUnitTest.cs          ✅ Testy Ball
    │   └── DataTest.csproj
    │
    ├── BusinessLogicTest/            👈 TESTY dla warstwy BusinessLogic
    │   ├── BusinessLogicUnitTest.cs  ✅ Testy logiki biznesowej
    │   └── BusinessLogicTest.csproj
    │
    ├── PresentationModelTest/        👈 TESTY dla warstwy Model
    │   ├── ModelBallUnitTest.cs      ✅ Testy modelu piłki
    │   └── PresentationModelTest.csproj
    │
    └── PresentationViewModelTest/    👈 TESTY dla ViewModel
        ├── MainWindowViewModelUnitTest.cs
        └── PresentationViewModelTest.csproj
```

---

## ▶️ JAK URUCHOMIĆ TESTY NA LINUX?

### Uruchomić WSZYSTKIE testy:
```bash
cd /home/files/szkolne/sem4/TPW/ProgramowanieWspolbiezne
dotnet test
```

### Uruchomić KONKRETNY test file:
```bash
dotnet test --filter DataTest
```

### Uruchomić test z SZCZEGÓŁAMI (co się dzieje):
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Wynik:
```
✅ Passed!  - Failed: 0, Passed: 22, Skipped: 0, Total: 22
```

---

## ▶️ JAK URUCHOMIĆ PROGRAM NA LINUX?

### ❌ Zły sposób (GUI nie działa na Linux):
```bash
dotnet run --project ReactiveInteractiveUserInterface/GraphicalUserInterface/PresentationView.csproj
```
❌ Błąd: WPF nie jest dostępny na Linux

### ✅ Dobry sposób na Linux - uruchom TESTY zamiast GUI:
```bash
dotnet test
```

### ✅ Jak uruchomić na WINDOWS:
Na Windows możesz:
```cmd
dotnet run --project ReactiveInteractiveUserInterface/GraphicalUserInterface/PresentationView.csproj
```

Albo otworzyć w Visual Studio i kliknąć **F5** (Start)

---

## 📂 STRUKTURA KATALOGÓW - CO GDZIE JEST?

### Główne foldery:

```
ProgramowanieWspolbiezne/
│
├── ConcurrentProgramming.sln          👈 Plik projektu (otwórz w Visual Studio)
├── README.md                          👈 Dokumentacja
│
└── ReactiveInteractiveUserInterface/  👈 GŁÓWNY FOLDER Z KODEM
    │
    ├── Data/                          👈 WARSTWA 1: DANE
    │   ├── Ball.cs                    (klasa Ball - piłka)
    │   ├── Vector.cs                  (klasa Vector - wektor)
    │   ├── DataAbstractAPI.cs         (interfejs)
    │   ├── DataImplementation.cs      (implementacja)
    │   └── Data.csproj                (konfiguracja projektu)
    │
    ├── BusinessLogic/                 👈 WARSTWA 2: LOGIKA BIZNESOWA
    │   ├── BusinessBall.cs            (logika piłki)
    │   ├── Position.cs                (pozycja)
    │   ├── BusinessLogicAbstractAPI.cs (interfejs)
    │   ├── BusinessLogicImplementation.cs (implementacja)
    │   └── BusinessLogic.csproj
    │
    ├── PresentationModel/             👈 WARSTWA 3: MODEL (MVVM)
    │   ├── ModelBall.cs               (piłka dla UI)
    │   ├── PresentationModel.cs       (model główny)
    │   ├── ModelAbstractApi.cs        (interfejs)
    │   └── PresentationModel.csproj
    │
    ├── PresentationViewModel/         👈 WARSTWA 3: VIEWMODEL (MVVM)
    │   ├── MainWindowViewModel.cs     (logika okna)
    │   └── PresentationViewModel.csproj
    │
    └── GraphicalUserInterface/        👈 WARSTWA 3: INTERFEJS (GUI)
        ├── MainWindow.xaml            (wygląd okna - XML)
        ├── MainWindow.xaml.cs         (logika okna)
        ├── App.xaml                   (ustawienia aplikacji)
        ├── App.xaml.cs
        └── PresentationView.csproj
```

---

## 🧠 ZROZUMIEĆ C# - ARCHITEKTURA

### Co to jest "warstwa"?

```
┌─────────────────────────────────────┐
│  Warstwa 3: PREZENTACJA (GUI)      │  ← Okno aplikacji, przyciski
├─────────────────────────────────────┤
│  Warstwa 2: LOGIKA BIZNESOWA       │  ← Reguły, obliczenia
├─────────────────────────────────────┤
│  Warstwa 1: DANE                   │  ← Piłki, wektory, dane
└─────────────────────────────────────┘
```

### Jak dane płyną przez warstwy:

```
Warstwa 3: "Użytkownik klika przycisk" 
        ↓
Warstwa 2: "Oblicz nową pozycję piłki"
        ↓
Warstwa 1: "Zapamiętaj nową pozycję"
        ↓
Warstwa 2: "Przekaż obliczenia z powrotem"
        ↓
Warstwa 3: "Pokaż piłkę w nowym miejscu"
```

---

## 📝 CO TO JEST PLIK .csproj?

Plik `.csproj` to **konfiguracja projektu C#**. Zawiera:
- Co ma być zbudowane
- Jakie biblioteki są potrzebne
- Wersje .NET Framework

**Ważne**: Każdy folder ma swój `.csproj` (jeśli jest osobnym projektem)

```
Data/
├── Ball.cs
├── Vector.cs
└── Data.csproj          👈 Konfiguracja projektu "Data"

BusinessLogic/
├── BusinessBall.cs
└── BusinessLogic.csproj 👈 Konfiguracja projektu "BusinessLogic"
```

---

## 📝 CO TO JEST PLIK .sln?

`.sln` to **Solution** - plik, który łączy wszystkie projekty w jeden:

```
ConcurrentProgramming.sln zawiera:
├── Data.csproj
├── BusinessLogic.csproj
├── PresentationModel.csproj
├── PresentationViewModel.csproj
├── GraphicalUserInterface.csproj
├── DataTest.csproj
└── [inne projekty...]
```

**W Visual Studio**: Otwierasz `.sln` i widać wszystkie projekty

---

## 📝 CO TO JEST XAML?

XAML to **XML do tworzenia interfejsów graficznych** (jak HTML, ale dla Windows):

```xml
<!-- MainWindow.xaml -->
<Window>
    <Button Content="Uruchom" Click="Button_Click"/>
    <TextBlock Text="Liczba piłek: 5"/>
</Window>
```

Równoważne w C#:
```csharp
Button btn = new Button();
btn.Content = "Uruchom";
btn.Click += Button_Click;
```

XAML jest **krócej i bardziej czytelnie** ✅

---

## 🏗️ JAK ZBUDOWAĆ PROJEKT?

```bash
# Opcja 1: Zbuduj debug (do testowania)
dotnet build

# Opcja 2: Zbuduj release (szybszą)
dotnet build --configuration Release

# Opcja 3: Wyczyść i przebuduj
dotnet clean
dotnet build
```

**Wynik budowy**:
```
Build succeeded.
0 Error(s)
3 Warning(s)
```

✅ 0 błędów = OK! Warningi to nie problem.

---

## 📊 CZYTANIE STRUKTURY KATALOGÓW

### Symbol "/"
```
ReactiveInteractiveUserInterface/Data/
                                  ↑
                          To jest FOLDER
```

### Znaczenie structury:

```
ReactiveInteractiveUserInterface/
    Data/
        Ball.cs           ← Plik Ball.cs w folderze Data
    BusinessLogic/
        BusinessBall.cs   ← Plik BusinessBall.cs w folderze BusinessLogic
```

### W C# namespace musi zgadzać się ze ścieżką:

```csharp
// Plik: ReactiveInteractiveUserInterface/Data/Ball.cs
namespace TP.ConcurrentProgramming.Data  // 👈 Zgadza się z folderem
{
    public class Ball
    {
        // kod
    }
}
```

---

## 🔍 PRZYKŁAD CZYTANIA TESTU

Weź plik testowy i czytaj tak:

```csharp
// 1. Namespace - gdzie jest test
namespace TP.ConcurrentProgramming.Data.Test
{
  // 2. Klasa testowa
  [TestClass]
  public class VectorUnitTest
  {
    // 3. Metoda testowa
    [TestMethod]
    public void VectorZeroValuesTestMethod()  // Nazwa testu
    {
      // 4. ARRANGE - przygotuj dane
      Vector zeroVector = new(0.0, 0.0);
      
      // 5. ACT - wykonaj operację
      // (już wykonana powyżej)
      
      // 6. ASSERT - sprawdź wynik
      Assert.AreEqual<double>(0.0, zeroVector.x);
      Assert.AreEqual<double>(0.0, zeroVector.y);
    }
  }
}
```

---

## 📚 SŁOWNIK C#

| Termin | Znaczenie | Przykład |
|--------|-----------|----------|
| `class` | Klasa (szablon) | `public class Ball` |
| `public` | Publiczny (widoczny) | `public string Name` |
| `private` | Prywatny (ukryty) | `private int position` |
| `interface` | Umowa/Kontrakt | `public interface IBall` |
| `namespace` | Przestrzeń nazw | `namespace TP.ConcurrentProgramming.Data` |
| `new` | Tworzenie obiektu | `new Vector(1.0, 2.0)` |
| `readonly` | Nie można zmienić | `public readonly int id` |
| `async` | Asynchroniczny | `async Task DoWork()` |

---

## 🚀 SZYBKI START NA LINUX

```bash
# 1. Wejdź do folderu projektu
cd /home/files/szkolne/sem4/TPW/ProgramowanieWspolbiezne

# 2. Zbuduj projekt
dotnet build

# 3. Uruchom testy
dotnet test

# 4. Uruchom testy ze szczegółami
dotnet test --logger "console;verbosity=detailed"

# 5. Uruchom konkretny test
dotnet test --filter VectorZeroValuesTestMethod
```

---

## ❓ PYTANIA I ODPOWIEDZI

**P: Co to jest MSTest?**
O: Framework do testów. Atrybuty `[TestClass]` i `[TestMethod]` mówią do niego co testować.

**P: Gdzie jest Main()?**
O: W `App.xaml.cs` - tam aplikacja się startuje.

**P: Jak debugować na Linux?**
O: Uruchamiaj z `dotnet test` i czytaj błędy w konsoli.

**P: Czy mogę edytować testy?**
O: TAK! Testy są w plikach `.cs` jak każdy kod.

**P: Czemu WPF nie działa na Linux?**
O: WPF to technologia tylko Windows. Linux ma inne frameworki (np. Avalonia).

**P: Ile testów powinno być?**
O: Teraz masz 22 testy - im więcej, tym lepiej!

