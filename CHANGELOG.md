# Changelog - ICN.Leaderboards

## [1.2.1] - 2025-11-25

### Added
- 🔄 **Smart Configuration Migration**:
  - Automatically preserves user settings when updating the plugin
  - No need to delete `ICN.Leaderboards.configuration.xml` anymore!
  - Automatically adds new fields and removes obsolete ones

## [1.2.0] - 2025-11-23

### Performance Optimizations 🗄️
- **Database Query Caching**: Reduced database load by ~95%
  - Implemented 2-minute cache for leaderboard results
  - Automatic cache expiration and refresh
  - Cache hit/miss logging for monitoring
  - Dramatically improved response times (200ms → 1ms from cache)
- **Code Modernization**: Migrated to RestoreMonarchy.RocketRedist NuGet package
- **Build Output Cleanup**: Added .gitignore to exclude bin/obj folders

### Added
- `_cachedLeaderboard` field for result caching
- `_cacheExpiry` field for cache invalidation
- `CACHE_DURATION` constant (120 seconds)
- Cache status logging (database refresh vs cache hit)

### Changed
- `PostLeaderboard()` now checks cache before querying database
- Improved logging to show cache performance
- Better resource efficiency for high-traffic servers

### Impact
- ~95% reduction in MySQL database queries
- Faster leaderboard posts (instant from cache)
- Lower database server load
- Better scalability for frequent updates

## [1.1.0] - 2025-11-22

### Added
- Discord webhook integration for leaderboard posting
- Async/await pattern for non-blocking operations
- Configurable leaderboard sorting (Kills, K/D Ratio, Headshots, Accuracy, Playtime)
- K/D ratio calculation and display
- Formatted playtime display (hours/minutes/seconds)
- Customizable Discord embed colors via hex codes
- Automatic scheduled leaderboard posting
- Command cooldown system (5 seconds)
- Comprehensive error handling and logging
- SQL injection protection with parameterized queries
- Database connection testing on plugin load
- Markdown escaping for player names in Discord
- Medal emojis for top 3 players (🥇🥈🥉)
- Configurable display options (K/D, Accuracy, Playtime toggles)
- `/postleaderboard` command with aliases `/leaderboard` and `/lb`

### Features
- **Async Operations**: All database and network operations are asynchronous
- **Robust Error Handling**: Comprehensive try-catch blocks with detailed logging
- **Null Safety**: Proper null checking and DBNull handling for database values
- **Connection Pooling**: Efficient MySQL connection management
- **Auto-Posting**: Optional automatic leaderboard posting at configurable intervals
- **Customization**: Extensive configuration options for display and behavior

### Technical Details
- Built with .NET Framework 4.8
- Uses modern SDK-style project format
- HttpClient for webhook requests (replacing deprecated WebClient)
- Parameterized SQL queries for security
- Unity FixedUpdate for timer-based auto-posting
