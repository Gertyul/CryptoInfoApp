# CryptoInfoApp

CryptoInfoApp is a WPF application that provides comprehensive information about cryptocurrencies. It integrates multiple APIs to display current market data, detailed information for selected currencies, interactive charts, and a currency converter—all with support for theming and localization.

## Features

- **Multi-Page Navigation**  
  Easily navigate between different pages:
  - **Main Page:** Displays the top cryptocurrencies by market capitalization.
  - **Currency Details Page:** Shows detailed information for a selected cryptocurrency, including its current price, trading volume, 24-hour price change, and an interactive price chart. You can switch between a line chart and Japanese candlestick charts.
  - **Search Page:** Allows you to search for cryptocurrencies by name or symbol.
  - **Currency Converter:** Convert amounts between different cryptocurrencies, with a display of the USD equivalent.

- **Interactive Charting**  
  The app uses [LiveCharts](https://github.com/Live-Charts/Live-Charts) to render charts:
  - A standard line chart for price trends over the last 24 hours.
  - Japanese candlestick (OHLC) charts that can be toggled on or off.

- **API Integration**  
  Data is fetched from open APIs (e.g., CoinGecko) using the standard `HttpClient`. The application employs retry policies (with Polly) and caching techniques to handle rate limits and reduce API calls.

- **Currency Converter**  
  Provides conversion between cryptocurrencies. Prices are formatted to always display in USD using a custom converter, regardless of the interface language.

- **Theming**  
  Supports both light and dark themes that can be toggled at runtime, giving users the flexibility to choose their preferred UI style.

- **Localization**  
  Fully localized for English and Ukrainian. The app uses resource files (.resx) and a custom `LocExtension` to load texts dynamically based on the selected language. Users can change the interface language without restarting the application.

## Technologies Used

- **WPF (.NET Framework)** for building the user interface.
- **HttpClient** for API calls.
- **Newtonsoft.Json** for JSON deserialization.
- **LiveCharts.Wpf** for interactive charting.
- **Polly** (optional) for implementing retry policies.
- **Resource Files (.resx)** for localization.
- **MVVM Pattern** with custom implementations (e.g., RelayCommand) for separation of concerns.

## Project Structure

- **Pages:** Contains all the XAML pages (MainPage, CurrencyDetailsPage, SearchPage, CurrencyConverterPage).
- **ViewModels:** Contains the MVVM view models that drive the application logic.
- **Models:** Defines the data models (e.g., Currency, CurrencyDetails, OhlcData).
- **Services:** Handles API integration and data retrieval.
- **Localization:** Contains classes for dynamic localization (e.g., LocExtension, LocalizationProvider) and the resource files are located in the Properties folder.
- **Converters:** Contains value converters like FixedCurrencyConverter for consistent currency formatting.
