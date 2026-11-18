# 🐱 Happy Todo 💜💗

**Happy Todo** is a C# **Blazor Web App (Interactive Server)** with a dark purple-and-pink theme, a happy cat logo, recurring tasks, and a JSON-file database.

![Happy Todo cat logo](wwwroot/images/logo1.png)

## ✨ Features

- 📝 Full CRUD: create, read, update, delete, finish, and reopen tasks using interactive Blazor components.
- 🔁 Recurrence: daily, weekly, monthly, every N days, or selected month days (e.g. 1, 5, 8, 22).
- 💸 **Pay Bills** is seeded for the **26th** of each month, with an important flag and red border.
- 🎨 20 colors for the 4px left border; the default is dark gray (`#444444`).
- 🪥 51 hygiene tasks loaded from JSON into `List<TodoModel>`.
- 💡 260 suggestions loaded from `wwwroot/data/suggestions.json` for the Blazor task editor.
- 🔐 Persistent cookie-based admin access after visiting `/?walteradmin=walter`.
- 💾 Atomic JSON writes guarded by a semaphore.
- 🌙 Responsive dark-mode UI.

## 🚀 Run

Install the .NET 8 SDK, then:

```bash
dotnet restore
dotnet run
```

Open the local address printed by `dotnet run`. For admin access, visit `/?walteradmin=walter` once. You will be redirected to `/` and signed in with a persistent cookie. The URL parameter is only used to establish the cookie; later navigation doesn't require it.

## 🧱 Architecture

| Path | Purpose |
| --- | --- |
| `Program.cs` | Blazor Interactive Server hosting, cookie authentication, admin entry |
| `Components/App.razor` | HTML shell and Blazor scripts |
| `Components/Routes.razor` | Component router |
| `Components/Layout/MainLayout.razor` | Shared dark layout |
| `Components/Pages/Home.razor` | Todo list and finish/reopen/delete interactions |
| `Components/Pages/TodoEditor.razor` | Create/edit form, schedule, color palette |
| `Models/TodoModel.cs` | Todo model and 20-color palette |
| `Services/TodoStore.cs` | JSON persistence and recurrence |
| `Services/SuggestionCatalog.cs` | Suggested todo titles |
| `App_Data/todos.seed.json` | Initial hygiene and bill-payment tasks |
| `wwwroot/data/suggestions.json` | Suggested tasks |

The existing C# model, JSON store, seed data, and CSS were reused. MVC controllers and views were removed; UI interactions are now Blazor components.

## 📅 Repeat behavior

After completion, a recurring task is scheduled for its next due occurrence. On the next visit after that due date, it returns to unfinished. For monthly day selection, months lacking a selected day are skipped. The current implementation uses **UTC**, not the browser's local timezone, and does not run a background notification service.

## 🔐 Security warning

The fixed `walteradmin=walter` URL parameter is a shared secret and is **not safe as production authentication**. Query-string secrets may appear in browser history, server logs, or shared links. Override `Admin:AccessKey` with `Admin__AccessKey`, use HTTPS, and replace the shortcut with proper authentication before public hosting.

## 📸 Screenshots

Verified application screenshots have not yet been captured. After running and browser-testing the Blazor UI, add screenshots under `docs/` and link them here. The cat logo above is an illustration, not a screenshot.

## 📖 Maintenance

Whenever new functionality is added, update this README, relevant icons, and verified screenshots.
