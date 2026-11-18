# 🐱 Happy Todo 💜💗

A dark-mode ASP.NET Core MVC task manager with pink and purple accents, recurring schedules, JSON storage, and a smiling cat.

![Happy Todo logo](wwwroot/images/happy-todo-logo.svg)

## ✨ Features

- 📝 Full CRUD: add, view, edit, delete, complete, and reopen todos.
- 🔁 Recurring tasks: daily, weekly, monthly, every N days, or chosen days of the month (such as 1, 5, 8, 22).
- 💸 **Pay Bills** is preloaded as an important task for the **26th of each month** with a red 4px border.
- 🎨 20 selectable 4px border colors; new tasks default to dark gray (`#444444`).
- 🪥 51 hygiene items are preloaded as `TodoModel` objects in a `List<TodoModel>`.
- 💡 260 selectable task suggestions on the create/edit form.
- 🔐 `/?walteradmin=walter` grants a persistent admin cookie and redirects to remove the parameter from the URL.
- 💾 JSON file storage, protected by a semaphore and atomic file replacement.
- 🌙 Responsive dark-mode Razor UI.

## 🚀 Getting started

Install .NET 8 SDK, then run:

```bash
dotnet restore
dotnet run
```

Open the displayed localhost URL. Append `/?walteradmin=walter` once to enter admin mode. You remain signed in using a persistent cookie until logout or cookie expiration.

## 📂 Database

On first run, `App_Data/todos.seed.json` is copied to `App_Data/todos.json`. The latter is the live JSON database (gitignored). It contains a JSON array deserialized to `List<TodoModel>`. Changes are saved automatically.

## 📅 Recurrence semantics

Marking a recurring task finished schedules its next occurrence. At the next due date, it automatically returns to unfinished status when the app is visited. Multiple monthly dates are supported; invalid dates for shorter months are skipped. Scheduling currently uses UTC and checks on page access, not a background scheduler. Recurrence can be changed in the edit form.

## 🔐 Admin security

**Important:** The requested fixed query-string access key is not strong authentication. It can leak through browser history, logs, and shared links. For any publicly accessible deployment, replace this mechanism with real authentication. You can override `Admin:AccessKey` using `Admin__AccessKey` environment configuration. Always deploy with HTTPS.

## 📸 Screenshots

Capture verified screenshots from the running site and add them here after browser testing. No unverified screenshot is included.

## 🛠 Tech

C# · ASP.NET Core MVC / Razor · CSS · JavaScript · JSON

## 📖 Documentation policy

Update this README whenever features are added or changed, including relevant icons and verified screenshots.
