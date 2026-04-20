# Concurrent Programming - Billiard Table Simulator

## Project Information

**Group**: 2026-wt1030_mcmj (Tuesday 10:30 - MCJM)

### Team Members

| Name Surname (initials)        | GUID                                     | Role |
| ------------------------------ | ---------------------------------------- | ---- |
| Anton Prykhodzka (AP) medant79 | `{0A0AA8A2-9DFC-48BD-B7B0-61B205664181}` | Developer |
| Piotr Małkiewicz (PM)          | `{E33EEA6F-84FB-42DB-BFA4-A0F8A396FF95}` | Developer |

## Project Description

Multi-stage project implementing a **billiard table simulator** with balls moving on a bounded rectangular plane. 

### Current Stage: **Stage 1** ✅

- **Objective**: Create multi-layered architecture with Data, Logic, and Presentation layers
- **GUI**: Interactive and reactive user interface with MVVM pattern
- **Language**: C# (.NET 8.0)
- **Architecture**: Layered with Dependency Injection
- **Testing**: Unit tests for independent layer testing

## Architecture Overview

```
┌────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER (WPF + XAML)                        │
│ - GraphicalUserInterface (View)                         │
│ - PresentationViewModel (ViewModel)                     │
│ - PresentationModel (Model)                            │
└────────────────────────────────────────────────────────┘
                         ↓
┌────────────────────────────────────────────────────────┐
│ BUSINESS LOGIC LAYER                                   │
│ - BusinessLogic (Services)                             │
│ - BusinessBall (Movement Logic)                        │
│ - BusinessLogicAbstractAPI (Public API)                │
└────────────────────────────────────────────────────────┘
                         ↓
┌────────────────────────────────────────────────────────┐
│ DATA LAYER                                             │
│ - Ball, Vector, Position (Data Models)                 │
│ - DataAbstractAPI (Public API)                         │
│ - DataImplementation (Implementation)                  │
└────────────────────────────────────────────────────────┘
```

## Project Structure

```
ProgramowanieWspolbiezne/
├── ConcurrentProgramming.sln                    # Main solution file
├── ReactiveInteractiveUserInterface/
│   ├── Data/                                    # Data Layer
│   │   ├── Ball.cs, Vector.cs, Position.cs
│   │   ├── DataAbstractAPI.cs                  # Abstract API
│   │   └── DataImplementation.cs               # Implementation
│   ├── DataTest/                                # Data Layer Tests (8 tests)
│   ├── BusinessLogic/                           # Logic Layer
│   │   ├── BusinessBall.cs
│   │   ├── BusinessLogicAbstractAPI.cs         # Abstract API
│   │   └── BusinessLogicImplementation.cs      # Implementation
│   ├── BusinessLogicTest/                       # Logic Layer Tests (7 tests)
│   ├── PresentationModel/                       # Presentation Model (MVVM)
│   │   ├── PresentationModel.cs
│   │   └── ModelBall.cs
│   ├── PresentationModelTest/                   # Model Tests (5 tests)
│   ├── PresentationViewModel/                   # Presentation ViewModel (MVVM)
│   │   └── MainWindowViewModel.cs
│   ├── PresentationViewModelTest/               # ViewModel Tests (2 tests)
│   └── GraphicalUserInterface/                  # Presentation View (MVVM + XAML)
│       ├── MainWindow.xaml
│       ├── App.xaml
│       └── ...
├── TASK_CHECKLIST.md                            # Developer checklist
├── GRADING_CHECKLIST.md                         # Grader checklist
├── C_SHARP_LINUX_GUIDE.md                       # Linux guide
├── LIVE_DEMO_WYNIKI.md                          # Test results
└── ARCHITECTURE_AUDIT_REPORT.md                 # Architecture verification
```

## Getting Started

### Prerequisites

- .NET SDK 8.0 or higher
- Linux/Windows/macOS
- X11 or Wayland (for GUI on Linux)

### Installation

```bash
# Clone repository
git clone https://github.com/yourusername/ProgramowanieWspolbiezne.git
cd ProgramowanieWspolbiezne

# Restore packages and build
dotnet build ConcurrentProgramming.sln
```

### Running Tests

```bash
# Run all tests
dotnet test ConcurrentProgramming.sln

# Run specific layer tests
dotnet test ReactiveInteractiveUserInterface/DataTest/DataTest.csproj
dotnet test ReactiveInteractiveUserInterface/BusinessLogicTest/BusinessLogicTest.csproj
dotnet test ReactiveInteractiveUserInterface/PresentationModelTest/PresentationModelTest.csproj
dotnet test ReactiveInteractiveUserInterface/PresentationViewModelTest/PresentationViewModelTest.csproj
```

### Running the Application

```bash
# Run GUI application
dotnet run --project ReactiveInteractiveUserInterface/GraphicalUserInterface/PresentationView.csproj
```

## Test Results

| Component | Tests | Status |
|-----------|-------|--------|
| Data Layer | 8 | ✅ All Green |
| Business Logic Layer | 7 | ✅ All Green |
| Presentation Model | 5 | ✅ All Green |
| Presentation ViewModel | 2 | ✅ All Green |
| **TOTAL** | **22** | **✅ All Green** |

## Key Features

- ✅ **Layered Architecture**: Data, Logic, Presentation layers with clear separation
- ✅ **Abstract APIs**: Data and Logic layers expose only abstract interfaces
- ✅ **MVVM Pattern**: Proper implementation with View, ViewModel, Model
- ✅ **Dependency Injection**: Factory pattern and Lazy<T> for proper DI
- ✅ **Unit Testing**: Each layer tested independently, 22 green tests
- ✅ **Data Binding**: XAML data binding with INotifyPropertyChanged
- ✅ **Commands**: ICommand implementation for user interactions
- ✅ **No External Mocks**: Using DI instead of external Mock packages

## Documentation

- **[C# Linux Guide](C_SHARP_LINUX_GUIDE.md)** - How to run C# on Linux
- **[Live Demo Results](LIVE_DEMO_WYNIKI.md)** - Compilation and test results
- **[Architecture Audit Report](ARCHITECTURE_AUDIT_REPORT.md)** - Detailed architecture verification
- **[Task Checklist](TASK_CHECKLIST.md)** - Developer's task checklist
- **[Grading Checklist](GRADING_CHECKLIST.md)** - Instructor's grading checklist

## Build Status

- ✅ **Compilation**: Success (0 errors, 3 minor warnings)
- ✅ **Tests**: 22/22 passing
- ✅ **Architecture**: Verified and compliant

## License

Educational project - Copyright (C) 2024-2026

## References

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [C# Language Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [MVVM Pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)
- [Semantic Versioning](https://semver.org/)

## Submission

To submit the project for grading:

1. Create a tag: `git tag 1.a.n` (where a is approach number, n is any number)
2. Push tag: `git push origin 1.a.n`
3. Create a GitHub Release
4. Submit to WIKAMP with:
   - Repository URL
   - Tag number
   - Team GUIDs (from table above)
