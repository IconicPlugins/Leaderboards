# Changelog

All notable changes to ICN.Leaderboards will be documented in this file.

## [1.0.0] - 2025-11-22

### Added
- Initial release of ICN.Leaderboards plugin
- MySQL database integration with PlayerStatsUI
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
- Color-coded command feedback messages
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
