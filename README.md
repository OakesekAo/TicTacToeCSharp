# Tic-Tac-Toe Console (C#)

Single–player Tic-Tac-Toe with an optimal computer opponent, implemented as a
cross-platform C# console application.

The goal of this implementation is **clarity** and **good software practices**
rather than golfing for the fewest lines of code.

- Clean separation between game orchestration, board state, and AI.
- Unbeatable computer using a minimax search.
- Input validation and friendly console UX.
- Runs on macOS, Linux, and Windows via the .NET SDK.

---

## Quick Start

Pick one of these:

- **Option A (recommended):** GitHub Codespaces – no local install, just `dotnet run`.
- **Option B:** Run locally with the .NET SDK.

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)

Verify installation:

```
dotnet --version
```

---

## Getting Started

## Option A: Run in GitHub Codespaces (no install)

1. Fork this repository to your own GitHub account.
2. On your fork, click the green **Code** button → **Create codespace on main**.
3. Wait for the dev container to build (30–90 seconds the first time).
4. In the integrated terminal at the bottom, run:

   ```
   dotnet run```

or, if present:
```
./run.sh
```


## Option B: Run locally

1. Clone or download this repository:

```
git clone https://github.com/OakesekAo/tic-tac-toe-console.git
cd tic-tac-toe-console
```

2. Build:

```
dotnet build
```

3. Run:

```
dotnet run
```

---

## How to Play

- You are **X**
- Computer is **O** (minimax AI)
- Enter moves as: `row column` (e.g., `1 3`)
- Type `q` to quit

---

## Project Structure

```
TicTacToe.Console/
  TicTacToe.Console.csproj
  Program.cs
  Game.cs
  Board.cs
  Mark.cs
  Position.cs
  ComputerPlayer.cs
  README.md
  run.sh
```
