# Book Fair

![C#](https://img.shields.io/badge/C%23-10-512BD4?logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/UI-WPF-0078D4?logo=windows&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4?logo=windows&logoColor=white)

**Book Fair** (*Sajam Knjiga*) is a Windows desktop application for managing the people, books, publishers, and purchases connected with a book fair. It was developed as a team project for the **Basics of Information Systems and Software Engineering (OISISI)** course at the Faculty of Technical Sciences, University of Novi Sad.

The application provides a graphical interface in Serbian and English. Users can maintain the book fair's records, connect related entities, search and sort data, manage wishlists and purchases, and compare books by visitor interest.

## Features

- Create, view, edit, and logically delete visitors, authors, books, and publishers.
- Maintain relationships between authors, books, publishers, and visitors.
- Add books to a visitor's wishlist and record purchases, ratings, and comments.
- Search records using entity-specific fields.
- Sort table columns and browse large result sets with pagination.
- Compare two books and find:
  - visitors who have both books on their wishlists;
  - visitors who bought the first book but not the second.
- Assign a publisher's manager and review its associated authors and books.
- Protect referential integrity when records are edited or deleted.
- Switch the interface between Serbian and English at runtime.
- Use keyboard shortcuts for common actions and tab navigation.
- Save changes automatically to local text files.

## Technologies

| Technology | Use in the project |
| --- | --- |
| C# 10 | Domain models, application logic, controllers, and data access |
| .NET 6 | Runtime and project platform |
| WPF | Windows desktop user interface |
| XAML | Windows, dialogs, controls, data binding, and layouts |
| RESX resources | Serbian and English localization |
| Custom text-file persistence | Pipe-delimited serialization without an external database |
| Git and GitHub | Version control and team collaboration |

The project uses no third-party runtime packages. Persistence, querying, entity relationships, and UI synchronization are implemented in the repository.

## Project structure

```text
.
├── Core/           Domain models, DAOs, storage, serialization, and helpers
├── WPFClient/      Main desktop application, controllers, DTOs, and views
├── ConsoleClient/  Earlier console interface built on the same Core library
└── OISISI_Projekat.sln
```

The solution is split into three projects:

- **Core** contains the reusable domain and data layer. Its generic DAO and storage classes handle entities and flat-file persistence. Relation helpers restore one-to-many and many-to-many links after loading the data.
- **WPFClient** is the main application. Controllers sit between the views and DAOs, while DTOs prepare models for editing, validation, and display.
- **ConsoleClient** is a console-based client created during the earlier stages of development. It demonstrates that the Core layer can support more than one interface.

The observer pattern keeps open views synchronized when a DAO changes. Shared search, sorting, and pagination helpers keep the same behavior across entity screens.

## My contribution

I, **Andrej Vujić**, worked mainly on the application's architecture, shared infrastructure, and integration. My contribution included:

- setting up the solution structure and the shared Core project;
- implementing generic text-file storage, serialization, DAOs, entity identifiers, and logical deletion;
- building relation management for authors, books, publishers, visitors, wishlists, and purchases;
- developing most of the visitor workflow, including editing, wishlists, purchases, ratings, and automatic average-rating updates;
- implementing reusable search, sorting, and pagination logic used by the WPF screens;
- building the book-comparison feature and its visitor categories;
- developing publisher management, including manager selection and connected author/book views;
- creating the main WPF shell, menus, status bar, shortcuts, dialogs, and shared controller/DTO patterns;
- adding Serbian and English localization support;
- integrating the separate features and fixing synchronization, validation, deletion, and referential-integrity issues across the application.

## Running the application

### Requirements

- Windows 10 or Windows 11
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 with the **.NET desktop development** workload, recommended

WPF is Windows-only, so the graphical client cannot run natively on macOS or Linux.

### Visual Studio

1. Clone the repository:

   ```bash
   git clone https://github.com/kzi-nastava/e2-oisisi-2025-grupa-4-tim-1.git
   ```

2. Open `OISISI_Projekat.sln` in Visual Studio.
3. Set `WPFClient` as the startup project.
4. Build and run the solution with `F5`.

### Command line

From the repository root on Windows, run:

```powershell
dotnet restore OISISI_Projekat.sln
dotnet run --project WPFClient/WPFClient.csproj
```

To run the console client instead:

```powershell
dotnet run --project ConsoleClient/ConsoleClient.csproj
```

## Data storage

Application data is stored in the `Data` directories as pipe-delimited text files. The repository includes sample records, so the main screens contain data on first launch. Changes are written locally as users work with the application; no database server or additional setup is required.

The stored entities include:

- visitors and addresses;
- authors, books, and publishers;
- purchases and ratings;
- author-book and visitor-wishlist relationships.

## Team

| Student | Index |
| --- | --- |
| Andrej Vujić | RA 130/2023 |
| Radovan Vitomirov | RA 132/2023 |
