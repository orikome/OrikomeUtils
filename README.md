# 🛠️ OrikomeUtils

A collection of utility scripts for Unity to help streamline common development tasks such as UI transitions, layer management and common spatial operations.

## ✨ Features

- 🔄 **TransitionUtils** - Smooth UI, transform, and light transitions
- 🎭 **LayerMaskUtils** - Simple layer management helpers
- 📐 **GeneralUtils** - Common spatial calculations and positioning utilities

## 📦 Installation
1. Open your Unity project.
2. Navigate to `Window > Package Manager`.
3. Click the `+` button and select `Add package from git URL...`.
4. Enter the GitHub URL: `https://github.com/orikome/OrikomeUtils.git`.

## 🚀 Usage Example

```csharp
using OrikomeUtils;

// Fade out UI elements
StartCoroutine(TransitionUtils.FadeTransition(
    myTransform,  // Target transform with UI elements
    2.0f,         // Duration in seconds
    1.0f,         // Start alpha (fully visible)
    0.0f          // End alpha (fully transparent)
));
```
