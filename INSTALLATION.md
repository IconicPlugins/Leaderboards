# ICN.Leaderboards - Installation Guide

## 📦 Required Files

When installing the Leaderboards plugin, you need to copy specific DLL files to your server. Here's what you need:

### ✅ **Required - Copy to `Rocket/Plugins/` folder:**
- `ICN.Leaderboards.dll` - The main plugin file

### ⚠️ **Dependencies - Copy to `Rocket/Libraries/` folder:**

Most servers already have these, but if you get errors, copy these DLLs to `Rocket/Libraries/`:

**MySQL Dependencies:**
- `MySql.Data.dll` (1.2 MB)
- `BouncyCastle.Crypto.dll` (3.3 MB)
- `Google.Protobuf.dll` (401 KB)
- `K4os.Compression.LZ4.dll` (67 KB)
- `K4os.Compression.LZ4.Streams.dll` (74 KB)
- `K4os.Hash.xxHash.dll` (13 KB)

**System Dependencies (usually already present):**
- `System.Buffers.dll`
- `System.IO.Pipelines.dll`
- `System.Memory.dll`
- `System.Numerics.Vectors.dll`
- `System.Runtime.CompilerServices.Unsafe.dll`
- `System.Threading.Tasks.Extensions.dll`

**JSON (RocketMod usually has this):**
- `Newtonsoft.Json.dll` (702 KB)

---

## 🚀 Quick Installation

### Option 1: Minimal Installation (Recommended)
1. Copy `ICN.Leaderboards.dll` to `Rocket/Plugins/`
2. Start your server
3. If you get errors about missing DLLs, proceed to Option 2

### Option 2: Full Installation (If you get errors)
1. Copy `ICN.Leaderboards.dll` to `Rocket/Plugins/`
2. Copy ALL the dependency DLLs listed above to `Rocket/Libraries/`
3. Restart your server

---

## 📁 Server Folder Structure

```
YourServer/
├── Rocket/
│   ├── Plugins/
│   │   └── ICN.Leaderboards.dll          ← Main plugin
│   └── Libraries/
│       ├── MySql.Data.dll                 ← If needed
│       ├── BouncyCastle.Crypto.dll        ← If needed
│       ├── Google.Protobuf.dll            ← If needed
│       ├── K4os.Compression.LZ4.dll       ← If needed
│       ├── K4os.Compression.LZ4.Streams.dll ← If needed
│       ├── K4os.Hash.xxHash.dll           ← If needed
│       └── ... (other dependencies)
```

---

## ❓ FAQ

**Q: Do I need all these DLLs?**
A: Not necessarily. RocketMod and other plugins may already provide some of these. Start with just `ICN.Leaderboards.dll` and add dependencies only if you get errors.

**Q: Where do I put the DLLs?**
A: 
- Main plugin: `Rocket/Plugins/ICN.Leaderboards.dll`
- Dependencies: `Rocket/Libraries/` (if needed)

**Q: I'm getting "Could not load file or assembly" errors**
A: Copy the missing DLL from the list above to `Rocket/Libraries/`

**Q: What about Newtonsoft.Json.dll?**
A: RocketMod usually includes this. Only copy it if you get an error about it missing.

**Q: Can I delete the other files in bin/Release/net48/?**
A: Keep them for backup, but you only need to copy the files listed above to your server.

---

## 🔍 Common Errors & Solutions

### Error: "Could not load MySql.Data"
**Solution:** Copy `MySql.Data.dll` and all MySQL dependencies to `Rocket/Libraries/`

### Error: "Could not load System.Memory"
**Solution:** Copy `System.Memory.dll` and related System.* DLLs to `Rocket/Libraries/`

### Error: "Method not found" or "Type not found"
**Solution:** You may have an old version of a dependency. Replace it with the version from the plugin's bin folder.

---

## ✅ Verification

After installation, check your server console. You should see:
```
╔═══════════════════════════════════════╗
║   Leaderboards by Iconic Plugins      ║
╚═══════════════════════════════════════╝
Successfully connected to MySQL database.
ICN.Leaderboards loaded successfully!
```

If you see errors, check which DLL is missing and copy it to `Rocket/Libraries/`.
