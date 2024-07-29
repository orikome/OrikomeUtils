# OrikomeUtils

A collection of utility scripts for Unity to help streamline common development tasks such as UI transitions and layer mask management.

## Installation
1. Open your Unity project.
2. Navigate to `Window > Package Manager`.
3. Click the `+` button and select `Add package from git URL...`.
4. Enter the GitHub URL: `https://github.com/orikome/OrikomeUtils.git`.

## Usage Example
```csharp
using OrikomeUtils;

// Fades out all Image and TextMeshProUGUI components under 'myTransform' over 2 seconds.
StartCoroutine(TransitionUtils.FadeTransition(myTransform, 2.0f, 1.0f, 0.0f));
```

## Code Formatting
[CSharpier](https://github.com/belav/csharpier) is used to maintain consistent code formatting.
