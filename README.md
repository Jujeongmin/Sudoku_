# 스도쿠 (Sudoku)

Unity로 제작한 캐주얼 스도쿠 퍼즐 게임입니다.
난이도를 고르면 매 판 새로운 퍼즐이 생성되고, 메모 기능으로 후보 숫자를 적어가며 풀 수 있습니다.

- **엔진** : Unity
- **언어** : C#
- **인원** : 1인 개발
- **플랫폼** : 모바일 · 웹

---

## 게임 흐름

```
StartScene  →  난이도 선택 (Easy / Medium / Hard)
                      ↓  PlayerPrefs 로 빈칸 수 전달
MainScene   →  퍼즐 생성 → 플레이 → 클리어 판정 → 클리어 패널
```

---

## 주요 구현

### 스도쿠 퍼즐 자동 생성

`PuzzleGenerator` 가 **완전한 해답판을 먼저 만든 뒤, 칸을 지워서** 문제를 만듭니다.

```csharp
public int[,] GeneratePuzzle(int emptyCells, out int[,] solution)
{
    Array.Clear(board, 0, board.Length);
    FillBoard();                       // 1. 완성된 정답판 생성
    solution = (int[,])board.Clone();  // 2. 정답 보관
    RemoveCells(emptyCells);           // 3. 지정된 수만큼 칸 제거
    return board;
}
```

**1. 정답판 생성 — 백트래킹**

`FillBoard()` 는 왼쪽 위부터 빈 칸을 찾아 1~9를 섞은 순서로 하나씩 시도합니다.
스도쿠 규칙에 맞으면 채우고 재귀 호출하며, 막히면 되돌린 뒤 다음 숫자를 시도합니다.

```csharp
if (IsValidMove(r, c, num))
{
    board[r, c] = num;
    if (FillBoard()) return true;
    board[r, c] = 0;        // 실패 시 되돌리기
}
```

숫자를 매번 셔플하기 때문에 실행할 때마다 다른 정답판이 나옵니다.

**2. 규칙 검사**

`IsValidMove()` 는 같은 행 · 같은 열 · 같은 3×3 블록에 해당 숫자가 있는지 확인합니다.
블록 시작 좌표는 `(row / 3) * 3` 으로 구합니다.

**3. 칸 제거**

전체 81칸 좌표를 리스트에 담아 셔플한 뒤, 앞에서부터 `emptyCells` 개를 0으로 만듭니다.
제거 위치가 매번 달라집니다.

### 난이도 = 빈칸 수

`ModeManager` 가 난이도별로 빈칸 수를 `PlayerPrefs` 에 저장하고 씬을 전환합니다.

| 난이도 | 빈칸 수 |
|---|---|
| Easy | 30 |
| Medium | 40 |
| Hard | 50 |

난이도를 별도 자료구조로 두지 않고 **빈칸 개수 하나로 표현**해, 생성기는 숫자만 받아 동작합니다.

### 메모 기능과 자동 정리

한 칸에 후보 숫자 9개를 따로 표시할 수 있습니다. `Cell` 이 `numberText` 와
`memoText[9]` 를 나눠 들고, 메모 모드에서는 이미 적힌 숫자를 다시 누르면 지워집니다.

정답을 확정 입력하면 **같은 행 · 열 · 3×3 블록의 중복 메모가 자동으로 지워집니다.**

```csharp
bool sameRow   = r == row;
bool sameCol   = c == col;
bool sameBlock = r >= startRow && r < startRow + 3 &&
                 c >= startCol && c < startCol + 3;

if (sameRow || sameCol || sameBlock)
    cells[r, c].RemoveMemo(number);
```

손으로 메모를 지우는 반복 작업을 없애기 위한 처리입니다.

### 선택 강조 표시

칸을 누르면 세 종류의 강조가 동시에 적용됩니다.

- 선택한 칸
- 같은 행 · 같은 열 (십자)
- 화면 전체에서 같은 숫자를 가진 칸

다음 칸을 누를 때 `ResetAllHighlights()` 로 초기화한 뒤 다시 칠합니다.

### 숫자 버튼 자동 비활성화

`UpdateNumberPanel()` 이 보드 전체를 훑어 숫자별 개수를 세고,
**9개를 모두 채운 숫자는 버튼을 비활성화**하고 회색으로 바꿉니다.
더 쓸 수 없는 숫자를 누르는 실수를 막습니다.

### 클리어 판정

숫자를 입력할 때마다 `CheckGameClear()` 가 호출되어
보드가 가득 찼는지와 정답과 일치하는지를 확인합니다.
조건을 만족하면 `OnGameCleared` 이벤트가 발생하고,
`GManager` 가 이를 받아 클리어 패널을 띄웁니다.

```csharp
public event Action OnGameCleared;
...
OnGameCleared += GManager.Instance.ShowClearPanel;
```

보드 로직과 UI 표시를 이벤트로 분리했습니다.

### 전역 매니저

`GManager` 는 싱글톤으로 `DontDestroyOnLoad` 상태를 유지하며,
씬이 바뀌어도 남아 클리어 패널 제어를 담당합니다.

---

## 프로젝트 구조

```text
Assets/
├── 01.Scenes/
│   ├── StartScene.unity        난이도 선택 화면
│   └── MainScene.unity         퍼즐 플레이 화면
├── 02.Scripts/
│   ├── Board/
│   │   ├── Cell.cs             칸 하나 (숫자 · 메모 9개 · 강조 · 고정 여부)
│   │   └── PuzzleGenerator.cs  백트래킹 퍼즐 생성
│   └── Manager/
│       ├── BoardManager.cs     보드 구성 · 입력 처리 · 메모 · 클리어 판정
│       ├── ModeManager.cs      난이도 선택 · 씬 전환
│       └── GManager.cs         전역 싱글톤 · 클리어 패널 제어
├── 03.Prefabs/
└── 99.Resources/
```

---

## 사용 기술

- Unity · C#
- TextMesh Pro
- PlayerPrefs (난이도 값 전달)
- C# `event` 기반 클리어 알림
