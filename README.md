# Pab.Projekt

**REST + GraphQL + WebAdmin + Testy**  
Aplikacja do zarz�dzania produktami i zam�wieniami w ASP .NET Core 8, EF Core, HotChocolate (GraphQL) oraz Razor Pages.

---

##  Spis tre�ci

1. [Opis projektu](#opis-projektu)  
2. [Architektura](#architektura)  
3. [Wymagania](#wymagania)  
4. [Konfiguracja](#konfiguracja)  
5. [Uruchamianie](#uruchamianie)  
6. [GraphQL Playground](#graphql-playground)  
7. [Testy](#testy)  
8. [Migracje EF Core](#migracje-ef-core)  
9. [WebAdmin](#webadmin)  
10. [Git / Wdro�enie](#git--wdro�enie)  

---

##  Opis projektu

Projekt sk�ada si� z kilku warstw:

- **Pab.API** � REST API + GraphQL + JWT + Identity  
- **Pab.WebAdmin** � frontend (Razor Pages)  
- **Pab.Tests** � testy jednostkowe i integracyjne  
- **Pab.Domain** � encje i interfejsy  
- **Pab.Application** � logika aplikacji (CQRS/Handlery)  
- **Pab.Infrastructure** � EF Core, repozytoria  

---

##  Architektura

```
/PabProjekt
  /Pab.API
  /Pab.WebAdmin
  /Pab.Tests
  /Pab.Application
  /Pab.Domain
  /Pab.Infrastructure
```

---

##  Wymagania

- .NET 8 SDK  
- SQL Server (LocalDB)  
- Visual Studio 2022 lub VS Code  

---

##  Konfiguracja

1. Skopiuj plik `appsettings.json` lub `appsettings.Development.json` i wype�nij:

   ```jsonc
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=PabDb;Trusted_Connection=true;"
     },
     "Jwt": {
       "Key": "TU_WSTAW_SW�J_SECURE_KLUCZ",
       "Issuer": "pab.local",
       "Audience": "pab.users"
     }
   }
   ```

2. Sprawd� w `Properties/launchSettings.json`, na jakich portach startuje API i WebAdmin.

---

##  Uruchamianie

```bash
# w folderze g��wnym
dotnet restore
dotnet build

# uruchom API (np. https://localhost:7251)
cd Pab.API
dotnet run

# w nowej konsoli uruchom WebAdmin (np. https://localhost:5130)
cd ../Pab.WebAdmin
dotnet run
```

---

##  GraphQL Playground

Po starcie API wejd� w przegl�darce na:

```
https://localhost:7251/graphql
```

Mo�esz wykonywa� Query i Mutation.

---

##  Testy

```bash
# w katalogu g��wnym
dotnet test
```

Powiniene� zobaczy� �All tests passed�.

---

## Migracje EF Core

```bash
# w folderze Pab.API lub Pab.Infrastructure
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## WebAdmin

- **Produkty:** `https://localhost:5130/Products`  
- **Zam�wienia:** `https://localhost:5130/Orders`  

---

## Git / Wdro�enie

1. **Zainicjuj repozytorium:**

   ```bash
   git init
   git add .
   git commit -m "Initial commit � PabProjekt"
   ```

2. **Dodaj zdalne repozytorium (GitHub):**

   ```bash
   git remote add origin https://github.com/TwojUser/PabProjekt.git
   git branch -M main
   git push -u origin main
   ```

3. **Kolejne zmiany:**

   ```bash
   git add .
   git commit -m "Opis zmian"
   git push
   ```


#   p a b - p r o j e k t 
 
 