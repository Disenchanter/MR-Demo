# MR Demo

## Overview

MR Demo is a Unity project showcasing spatial audio behaviors for mixed reality applications. The scene demonstrates how different wall materials affect audio occlusion and low-pass filtering as the listener moves or toggles presets.

## Features

- **Acoustic material presets** – Scriptable objects that store per-material filtering and volume settings.
- **Global material switching** – Apply one preset to every wall at once for quick comparisons.
- **Per-wall overrides** – Switch individual wall materials at runtime via keyboard shortcuts.
- **Distance-based low-pass filter** – Smoothly attenuates high frequencies as the listener moves away from an audio source.
- **Occlusion detection** – Casts rays toward the listener and stacks the effects of any intersecting materials.

## Getting Started

1. Open the project folder (`MR Demo`) in Unity Hub and launch it with the matching Unity version used by your team.
2. Let Unity import all packages. When the editor finishes compiling, open the main demo scene in `Assets/Scenes`.
3. Enter Play Mode to interact with the sample.

## Runtime Controls

- **Space** – Switch to the next acoustic material preset.
- **Shift + Space** – Switch to the previous preset.
- **Number keys 1–6** – Jump directly to the corresponding preset index:
  - **1** – Brick
  - **2** – Concrete  
  - **3** – Curtain
  - **4** – Drywall
  - **5** – Glass
  - **6** – Wood

The HUD labels in the scene show the active preset for either the global controller or each wall.

## Project Structure

- `Assets/Scripts/Audio` – Runtime scripts that drive material presets, occlusion, and distance-based filtering.
- `Assets/ScriptableObjects` (or project-specific folder) – Stores acoustic material presets.
- `Assets/Scenes` – Contains the demo scene used for testing.

## Notes

- Adjust the `AcousticMaterialPreset` assets to match the acoustic profiles you need.
- When adding new geometry, attach an `AcousticMaterial` component so occlusion rays can read its preset.
- Tweak the `OcclusionByMaterial` smoothing and clamp values to fit your environment.
