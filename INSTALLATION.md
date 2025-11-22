# ICN.Leaderboards - Installation Guide

## 📦 Required Files

When installing the Leaderboards plugin, you need to copy specific DLL files to your server. Here's what you need:

### ✅ **Required - Copy to `Rocket/Plugins/` folder:**
- `ICN.Leaderboards.dll` - The main plugin file

### ⚠️ **Dependencies - Copy to `Rocket/Libraries/` folder:**

**IMPORTANT:** Copy dependencies to `Rocket/Libraries/`, NOT to `Rocket/Plugins/`!

#### **MySQL Dependencies (Required):**
These are needed for database connection:
- `MySql.Data.dll` (1.2 MB)
- `BouncyCastle.Crypto.dll` (3.3 MB)
- `Google.Protobuf.dll` (401 KB)
- `K4os.Compression.LZ4.dll` (67 KB)
- `K4os.Compression.LZ4.Streams.dll` (74 KB)
- `K4os.Hash.xxHash.dll` (13 KB)

#### **Newtonsoft.Json (Usually Already Present):**
- `Newtonsoft.Json.dll` (702 KB)
  - ⚠️ **Check first:** RocketMod usually includes this already
  - ✅ **Only copy if:** You get an error about missing Newtonsoft.Json
  - 📍 **Location:** `Rocket/Libraries/` (NOT Plugins folder)

#### **System Dependencies (Usually Not Needed):**
RocketMod and .NET Framework usually have these. Only copy if you get specific errors:
- `System.Buffers.dll`
- `System.IO.Pipelines.dll`
- `System.Memory.dll`
- `System.Numerics.Vectors.dll`
- `System.Runtime.CompilerServices.Unsafe.dll`
- `System.Threading.Tasks.Extensions.dll`

---

## 🚀 Quick Installation (Recommended)

### Step 1: Install Main Plugin
1. Copy `ICN.Leaderboards.dll` to `Rocket/Plugins/`
2. Start your server

### Step 2: Check for Errors
If you see errors like:
```
Could not load file or assembly 'MySql.Data'
```

### Step 3: Install MySQL Dependencies
Copy these to `Rocket/Libraries/`:
- `MySql.Data.dll`
- `BouncyCastle.Crypto.dll`
- `Google.Protobuf.dll`
- `K4os.Compression.LZ4.dll`
- `K4os.Compression.LZ4.Streams.dll`
- `K4os.Hash.xxHash.dll`

### Step 4: Restart Server
Restart your server and check the console.

---

## 📁 Server Folder Structure

```
YourServer/
├── Rocket/
│   ├── Plugins/
│   │   └── ICN.Leaderboards.dll          ← Main plugin ONLY
│   │
│   └── Libraries/
│       ├── MySql.Data.dll                 ← MySQL dependencies
│       ├── BouncyCastle.Crypto.dll
│       ├── Google.Protobuf.dll
│       ├── K4os.Compression.LZ4.dll
│       ├── K4os.Compression.LZ4.Streams.dll
│       ├── K4os.Hash.xxHash.dll
│       └── Newtonsoft.Json.dll            ← Only if needed
```

---

## ❓ FAQ

**Q: Do I need all these DLLs?**
A: Not necessarily. Start with just the main plugin and MySQL dependencies. Add others only if you get errors.

**Q: Where do I put Newtonsoft.Json.dll?**
A: `Rocket/Libraries/` folder. But check first - RocketMod usually has it already!

**Q: Should I put dependencies in the Plugins folder?**
A: NO! Dependencies go in `Rocket/Libraries/`, NOT `Rocket/Plugins/`. Only `ICN.Leaderboards.dll` goes in Plugins.

**Q: I'm getting "Could not load file or assembly 'Newtonsoft.Json'"**
A: Copy `Newtonsoft.Json.dll` from the bin folder to `Rocket/Libraries/`

**Q: I'm getting "Could not load MySql.Data"**
A: Copy all MySQL dependencies listed above to `Rocket/Libraries/`

**Q: What about System.* DLLs?**
A: Usually not needed. Only copy if you get specific errors about them.

**Q: Can I delete the other files in bin/Release/net48/?**
A: Keep them for backup, but you only need to copy the files listed above to your server.

**Q: Do I need to restart my server after copying DLLs?**
A: Yes, always restart after adding new DLLs to Libraries folder.

---

## 🔍 Common Errors & Solutions

### Error: "Could not load MySql.Data"
**Solution:** Copy all MySQL dependencies to `Rocket/Libraries/`:
- MySql.Data.dll
- BouncyCastle.Crypto.dll
- Google.Protobuf.dll
- K4os.Compression.LZ4.dll
- K4os.Compression.LZ4.Streams.dll
- K4os.Hash.xxHash.dll

### Error: "Could not load Newtonsoft.Json"
**Solution:** Copy `Newtonsoft.Json.dll` to `Rocket/Libraries/`

### Error: "Could not load System.Memory"
**Solution:** Copy `System.Memory.dll` and related System.* DLLs to `Rocket/Libraries/`

### Error: "Method not found" or "Type not found"
**Solution:** You may have an old version of a dependency. Replace it with the version from the plugin's bin folder.

### Error: Plugin loads but doesn't work
**Solution:** Check that all DLLs are in `Rocket/Libraries/`, not in `Rocket/Plugins/`

---

## ✅ Verification

After installation, check your server console. You should see:
```
╔═══════════════════════════════════════╗
║   Leaderboards by Iconic Plugins      ║
╚═══════════════════════════════════════╝
Licensed to: Your Server Name
Copyright © 2025 Iconic Plugins. All Rights Reserved.
Successfully connected to MySQL database.
ICN.Leaderboards loaded successfully!
```

If you see errors, check which DLL is missing and copy it to `Rocket/Libraries/`.

---

## 📝 Summary

**Main Plugin:** `Rocket/Plugins/ICN.Leaderboards.dll`
**Dependencies:** `Rocket/Libraries/` (MySQL DLLs + Newtonsoft.Json if needed)
**Never put dependencies in Plugins folder!**

