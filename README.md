# BitCoinPriceTrackerAppReact
BitcoinPriceTrackerAppReact je aplikace pro sledování aktuálních cen Bitcoinu v EUR a jejich převod na CZK pomocí API od Coindesk a ČNB. Aplikace poskytuje možnost sledovat ceny v reálném čase a ukládat data do databáze pro pozdější analýzu.

## Funkce

- Zobrazení aktuální ceny Bitcoinu v EUR a její konverze na CZK.
- Real-time data přístupná prostřednictvím Coindesk API pro Bitcoin a ČNB pro směnný kurz CZK.
- Ukládání historických dat do databáze pro pozdější analýzu.
- Interaktivní grafy pro vizualizaci změn cen pomocí **Chart.js**.
- Příjemné uživatelské rozhraní implementované ve **React**.

## Technologie

- **Frontend**: **React** pro interaktivní a responzivní uživatelské rozhraní, **Chart.js** pro vizualizaci dat (grafy).
- **Backend**: **ASP.NET Core Web API** pro serverovou logiku.
- **API**: Coindesk pro zajištění cen Bitcoinu a ČNB pro směnné kurzy mezi EUR a CZK.
- **Databáze**: **MSSQL** pro ukládání historických dat.
- **Entity Framework (EF)** pro připojení a manipulaci s databází MSSQL

## Instalace a spuštění

### Požadavky

- **.NET 8**
- **Node.js**
- **MSSQL server**

### Klonování repozitáře
```bash
git clone https://github.com/bzivica/BitCoinPraceTrackerAppReact.git

V backendové části je potřeba nakonfigurovat správné připojení k databázi MSSQL a nastavit API pro Coindesk a ČNB. Ujistěte se, že máte správně nakonfigurovaný soubor appsettings.json.

Po spuštění obou částí aplikace bude frontendová aplikace přístupná na http://localhost:3000 a backend na http://localhost:7081.

