## Leaderboard Popup - Architecture Overview

### What this is
An Addressables-driven popup that presents a leaderboard, built with a clean MVP separation and wired via dependency injection (Extenject). The UI stays responsive while the popup loads/initializes asynchronously.

### Key components
- **MenuMockPanel**: View/controller for the menu button. On click, shows "Loading" on the button, disables interaction, and opens the popup via `IPopupManagerService`. Restores the button state via `OnOpened`/`OnClosed` callbacks.
- **LeaderboardPopupView**: The popup View. Implements `IPopupInitialization` so the popup manager can initialize it. Forwards UI events (like Close) to the presenter. Only handles rendering, layout, and Unity-specific wiring.
- **LeaderboardPresenter**: Orchestrates data loading and view updates. After building the list it signals `OnOpened`. Handles close requests (asks popup manager to close and triggers `OnClosed`). Presenter stays free of Unity lifecycle APIs.
- **IPopupManagerService**: Abstracts popup open/close. Implementation uses Addressables for async load/instantiate and calls `IPopupInitialization` on the popup.
- **LeaderboardPopupParams**: Parameter object passed into popup initialization containing: `PopupManager`, `OnOpened`, `OnClosed`.

### Why MVP here
- **Separation of concerns**: View renders; Presenter coordinates logic and talks to the View over the `ILeaderboardView` interface.
- **Testability**: Presenter is pure C# (no MonoBehaviour), making it straightforward to unit test.
- **Maintainability**: View changes (prefabs/layout) do not leak into business logic, and data parsing/mapping stays out of Unity UI code.

### Async behavior
- Addressables instantiate the popup asynchronously.
- The popup's `IPopupInitialization.Init` awaits the presenter’s initialization.
- `MenuMockPanel` sets button text to "Loading" and disables the button while opening; it clears the loading text on `OnOpened` and keeps the button disabled until `OnClosed`.

### Suggestions and improvements
- **Use UniTask instead of Task**: UniTask integrates better with Unity’s player loop, avoids extra allocations, and provides ergonomics for main-thread continuations without `ConfigureAwait` boilerplate. Replace method signatures (`Task` -> `UniTask`) in presenter and initialization to reduce GC and improve performance on mobile.
- **Use an enum for popup types instead of string keys**: Define an enum (e.g., `PopupName.LeaderboardPopup`) and map it to the Addressables key in a single place. This improves refactor safety and discoverability.
  - Pros: compile-time safety, IDE autocomplete, safer renames.
  - Cons of current string approach: typo-prone, harder refactors, implicit coupling to Addressables key names scattered across code.

### Files of interest
- `Assets/Scripts/Leaderboard/UI/MenuMockPanel.cs`
- `Assets/Scripts/Leaderboard/UI/Leaderboard/LeaderboardPopupView.cs`
- `Assets/Scripts/Leaderboard/UI/Leaderboard/LeaderboardPresenter.cs`
- `Assets/Scripts/SimplePopupManager/Services/PopupManagerServiceService.cs`