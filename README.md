# CryptoInfoApp

CryptoInfoApp is a feature-rich WPF application for tracking cryptocurrency market data. It implements **Data Aggregation**, combining real-time prices from multiple sources (CoinGecko & CoinMarketCap) to provide the most accurate market overview. The app features advanced charting, a smart currency converter, and a modern, adaptive UI with support for theming and localization.

## Key Features

### 🚀 Advanced Dashboard (Main Page)
- **Data Aggregation:** Displays prices from two independent sources simultaneously:
  - **CoinGecko:** Primary data source (White text).
  - **CoinMarketCap:** Secondary validation source (Orange text).
- **Comprehensive Metrics:** Shows Rank, current Price, and percentage changes for **1h, 24h, and 7d**.
- **Sparklines:** Visualizes 7-day price trends using mini-charts directly in the list.
- **Auto-Refresh:** Data updates automatically every 60 seconds without blocking the UI.
- **Smart Loading:** Optimized for performance using `MemoryCache` and lightweight API queries.

### 📈 Interactive & Detailed Charting
- **Candlestick & Line Charts:** Switch between detailed Japanese Candlestick charts (Green/Red) and standard Line charts.
- **Timeframe Selection:** Analyze price movements over different periods: **24 Hours, 7 Days, and 30 Days**.
- **Real-time Updates:** The details page includes a dedicated timer to fetch the latest price updates every minute.
- **External Links:** Quick access to the cryptocurrency's official website.

### 💱 Smart Currency Converter
- **Optimized Performance:** Uses a separate "lightweight" API strategy to load 50-100 currencies instantly without hitting rate limits.
- **Offline/Fallback Mode:** Includes a fail-safe mechanism to generate basic market data if the API is unreachable.
- **Instant Calculation:** Converts amounts automatically as you type or change currencies.
- **USD Equivalent:** Always displays the total value in USD for reference.

### 🎨 Modern UI/UX
- **Adaptive Design:** Fully responsive layout using Grids and DockPanels (no empty spaces).
- **Custom Controls:**
  - Styled Scrollbars (Thin, rounded, and adaptive to themes).
  - Modern "Card" layout with drop shadows.
  - Vector Icon buttons (Path-based geometry).
- **Theming:** Runtime toggling between **Light** and **Dark** themes with dynamic resource updates.
- **Localization:** Full support for **English** and **Ukrainian** languages, switchable on the fly.

## Technologies Used

- **WPF (.NET Framework)** – UI Architecture.
- **MVVM Pattern** – Clean separation of logic (Models, ViewModels, Views).
- **HttpClient & Polly** – Resilient API communication with Retry policies for handling Rate Limits (429).
- **Newtonsoft.Json** – JSON parsing.
- **LiveCharts.Wpf** – Advanced charting (Candlesticks, Lines).
- **System.Runtime.Caching** – In-memory caching to optimize API usage.
- **Multi-API Integration** – CoinGecko (Public API) & CoinMarketCap (Pro API).

## Project Structure

- **Pages:** XAML views (MainPage, CurrencyDetailsPage, CurrencyConverterPage).
- **ViewModels:** Logic for Aggregation, Timer management, and Data binding.
- **Models:** Data structures including `SparklineData` and extended `Currency` properties (1h/7d changes).
- **Services:** - `APIService`: Handles CoinGecko requests (Heavy & Light methods).
  - `CoinMarketCapService`: Fetches alternative prices.
- **Converters:** Value converters for UI logic (`SparklineToPath`, `PriceColor`, `FixedCurrency`).
- **Themes:** Resource dictionaries for Light/Dark modes and control styles.

## Getting Started

1. Clone the repository.
2. Ensure you have an active Internet connection (for API calls).
3. Build and Run the solution in Visual Studio.
4. *Note:* The CoinMarketCap API key is pre-configured in `CoinMarketCapService.cs`.
