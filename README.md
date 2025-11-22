# ICN.Leaderboards

A modern Unturned RocketMod plugin that fetches player statistics from PlayerStatsUI's MySQL database and posts beautiful leaderboards to Discord via webhooks.

## Features

- 🏆 **Customizable Leaderboards** - Display top players sorted by Kills, K/D Ratio, Headshots, Accuracy, or Playtime
- 📊 **Rich Discord Embeds** - Beautiful formatted leaderboards with medals, stats, and custom colors
- ⚡ **Async Operations** - Non-blocking database queries and webhook posting
- 🔄 **Auto-Posting** - Automatically post leaderboards at configurable intervals
- 🎨 **Configurable Display** - Toggle K/D ratio, accuracy, and playtime display
- 🛡️ **Robust Error Handling** - Comprehensive error handling and logging
- 🔒 **SQL Injection Protection** - Parameterized queries for security
- ⏱️ **Command Cooldown** - Prevents spam with built-in cooldown system

## Requirements

- Unturned server with RocketMod
- PlayerStatsUI plugin with MySQL database
- MySQL database access
- Discord webhook URL

## Installation

1. Download `ICN.Leaderboards.dll` from releases
2. Place the DLL in your `Rocket/Plugins` folder
3. Start your server to generate the configuration file
4. Configure the plugin (see Configuration section)
5. Restart your server or reload the plugin

## Configuration

The configuration file is located at `Rocket/Plugins/ICN.Leaderboards/ICN.Leaderboards.configuration.xml`

### Example Configuration

```xml
<?xml version="1.0" encoding="utf-8"?>
<LeaderboardsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <WebhookUrl>https://discord.com/api/webhooks/YOUR_WEBHOOK_ID/YOUR_WEBHOOK_TOKEN</WebhookUrl>
  <MySQLConnectionString>Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=yourpassword;</MySQLConnectionString>
  <LeaderboardCount>10</LeaderboardCount>
  <IgnoreAdmins>false</IgnoreAdmins>
  <LeaderboardSortBy>Kills</LeaderboardSortBy>
  <ShowKDRatio>true</ShowKDRatio>
  <ShowAccuracy>true</ShowAccuracy>
  <ShowPlaytime>false</ShowPlaytime>
  <AutoPostIntervalMinutes>0</AutoPostIntervalMinutes>
  <EmbedColor>#FFD700</EmbedColor>
</LeaderboardsConfiguration>
```

### Configuration Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `WebhookUrl` | string | - | Discord webhook URL for posting leaderboards |
| `MySQLConnectionString` | string | - | MySQL connection string (same as PlayerStatsUI) |
| `LeaderboardCount` | int | 10 | Number of top players to display |
| `IgnoreAdmins` | bool | false | Whether to exclude admins from leaderboard |
| `LeaderboardSortBy` | string | "Kills" | Sort criteria: "Kills", "KDRatio", "Headshots", "Accuracy", "Playtime" |
| `ShowKDRatio` | bool | true | Display K/D ratio in leaderboard |
| `ShowAccuracy` | bool | true | Display headshot accuracy percentage |
| `ShowPlaytime` | bool | false | Display total playtime |
| `AutoPostIntervalMinutes` | int | 0 | Auto-post interval in minutes (0 = disabled) |
| `EmbedColor` | string | "#FFD700" | Hex color code for Discord embed |

## Commands

| Command | Aliases | Permission | Description |
|---------|---------|------------|-------------|
| `/postleaderboard` | `/leaderboard`, `/lb` | `leaderboards.post` | Manually post leaderboard to Discord |

## Permissions

Add to your permissions file:

```xml
<Permission Cooldown="0">leaderboards.post</Permission>
```

## Discord Webhook Setup

1. Go to your Discord server settings
2. Navigate to Integrations → Webhooks
3. Click "New Webhook"
4. Choose the channel for leaderboard posts
5. Copy the webhook URL
6. Paste it into the `WebhookUrl` configuration option

## Auto-Posting

To enable automatic leaderboard posting:

1. Set `AutoPostIntervalMinutes` to desired interval (e.g., 60 for hourly)
2. Reload the plugin or restart the server
3. Leaderboards will automatically post at the specified interval

## Troubleshooting

### Database Connection Failed
- Verify your MySQL connection string is correct
- Ensure the MySQL server is running and accessible
- Check that the database name matches PlayerStatsUI's database
- Verify MySQL user has SELECT permissions on the PlayerStats table

### Webhook Not Working
- Verify the webhook URL is correct and not expired
- Check that the webhook channel still exists
- Ensure the bot has permission to post in the channel
- Check server logs for detailed error messages

### No Players Showing
- Ensure PlayerStatsUI is properly tracking player stats
- Verify the PlayerStats table has data
- Check that the table structure matches expectations
- Review server logs for database query errors

## Version

**Current Version:** 1.0.0

## Credits

Developed by Iconic Plugins

## License

This plugin is provided as-is for use with Unturned servers.
