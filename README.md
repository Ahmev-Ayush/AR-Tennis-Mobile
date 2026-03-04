# AR Learning (Unity AR foundation : ARCore)

Prototype-ready AR Foundation project featuring fast plane detection, multi-mode placement (tap, drag, feature points), and a set of demo scenes: **ScenePrime** for the core flow, **ImageTrackingScene** for tracked prefabs, **PointCloudScene** for feature-point placement, **carScene** for vehicle anchoring, and mini-game playgrounds like **BallShootingGameScene** and **Scene_Dragon**. Great for showcasing interactions or bootstrapping an Android AR prototype.

An AR Foundation sample focused on reliable plane detection, responsive placement controls, and lightweight diagnostics. The project targets Unity **6000.2.6f2** (Unity 6) and ships with XR Interaction Toolkit, ARCore XR Plug-in, and AR Foundation 6.2, making it a solid starting point for Android AR prototypes.

Repository URL: https://github.com/Ahmev-Ayush/AR-Learning.git

## Working with Scenes
- **ScenePrime** – default showcase scene combining plane toggles, menu controls, and placement scripts.
- **ImageTrackingScene** – configured with a reference image library to test `ImageTrackingManager` and prefab spawning.

<img src="Demo_Videos/imageTracking_screenshot1.jpeg" width="300" alt="App Screenshot"> <img src="Demo_Videos/imageTracking_screenshot2.jpeg" width="300" alt="App Screenshot">

🎬 [Watch video 1 →](./Demo_Videos/imageTracking_video1.mp4) | [Watch video 2 →](./Demo_Videos/imageTracking_video2.mp4)

- **PointCloudScene** – use to place a flying saucer prefab to interact with point cloud feature points.

<img src="Demo_Videos/PointCloud_screenshot1.jpeg" width="300" alt="App Screenshot"> <img src="Demo_Videos/PointCloud_screenshot2.jpeg" width="300" alt="App Screenshot">

🎬 [Watch video →](./Demo_Videos/PointCloudScene_video.mp4)

- **carScene** & **Scene_Dragon** – uses the car and dragon placement scripts to anchor a vehicle and dragon prefab on detected planes.

<img src="Demo_Videos/PlaneDetection_screenshot1.jpeg" width="300" alt="App Screenshot"> <img src="Demo_Videos/PlaneDetection_screenshot2.jpeg" width="300" alt="App Screenshot">
<img src="Demo_Videos/PlaneDetection_screenshot3.jpeg" width="300" alt="App Screenshot">

- **BallShootingGameScene** ⭐ — **Best scene in the repo.** A fully playable AR tennis mini-game: the court anchors to a real-world plane, balls fire automatically, and you hit them with a racket that follows your camera. Includes a live scoreboard, high-score persistence, and a reset button.

<img src="Demo_Videos/BallShooting_screenshot1.png" width="300" alt="Ball Shooting Screenshot 1"> <img src="Demos/BallShooting_screenshot1.jpeg" width="300" alt="Ball Shooting Screenshot 2">

🎬 **[Watch Ball Shooting gameplay →](./Demo_Videos/BallShooting_video.mp4)**

## Note: View demo videos: [Demo_Videos](./Demo_Videos)

## Project Layout
| Folder | Purpose |
| --- | --- |
| `Assets/MobileARTemplateAssets` | Core scripts, prefabs, materials, and UI assets used across the demo scenes. |
| `Assets/Scenes` | Ready-to-open Unity scenes covering plane placement, image tracking, vehicle placement, and mini-game prototypes. |
| `Assets/Pipelines`, `Settings`, `TextMesh Pro`, `XR`, `XRI` | Supporting render pipeline assets, volume profiles, fonts, and XR Interaction Toolkit configurations. |
| `Build` | Auto-generated Burst debug data from previous builds (safe to delete if storage is a concern). |

## Project Structure
```text
📦 YourProject/
├── 📂 Assets/
│   ├── 📂 Butterfly (Animated)/
│   ├── 📂 MobileARTemplateAssets/
│   │   ├── 📂 Materials/
│   │   ├── 📂 Prefabs/
│   │   ├── 📂 Scripts/     ← All scripts used in the project are here
│   │   ├── 📂 Shaders/
│   │   ├── 📂 Tutorial/
│   │   └── 📂 UI/
│   ├── 📂 Prefabs/         ← All prefabs are here that are used in the project
│   ├── 📂 Resources/
│   │   ├── 📂 Images/      
│   │   └── 📂 Materials/   ← All Materials are here that are used in the project
│   ├── 📂 Samples/
│   ├── 📂 Settings/
│   ├── 📂 Scenes/
│   ├── 📂 TextMesh Pro/
│   ├── 📂 XR/
│   └── 📂 models/
├── 📂 Packages/
├── 📂 ProjectSettings/
├── 📂 Demos/                  ← demos videos and screenshots
│   ├── 📂 /
│   ├── 📂 /
├── 📄 .gitignore
├── 📄 README.md
├── 📄 project-structure.txt   
└── 📄 DEVLOG.md               
```

## Requirements
- Unity Hub with **Unity 6000.2.6f2** plus Android  Build Support, OpenJDK, and SDK/NDK tools.
- ARCore-device running Android 10+ .
- USB debugging enabled (Android).
- Optional: TextMeshPro Essentials imported (already configured in this project) for the diagnostics overlay.

## Quick Start
1. Clone the repository (`git clone https://github.com/Ahmev-Ayush/AR-Learning.git`) or download the ZIP into a local folder.
2. Open Unity Hub → **Open** → select `Plane Detection/Plane Detection.sln` or the folder root.
3. When prompted, install Unity 6000.2.6f2; allow the Editor to update the project.
4. In **Build Settings**, switch the platform to **Android**  and click **Apply**.
5. Open `Assets/Scenes/ScenePrime.unity` to explore the standard plane-detection flow. Use Play Mode with a webcam/AR simulation or deploy to device for accurate tracking.
6. Connect a device, press **Build & Run**, and test the placement modes, menu toggles, and debug overlays directly on hardware.

## Building for Device
- **Android**: 
	- Enable **ARM64** architecture with IL2CPP and strip unused managed code for smaller builds.
	- Under **Project Settings → XR Plug-in Management**, enable **ARCore** for Android and ensure required permissions (camera) are checked.
	- If you use feature-point placement, keep depth and point-cloud subsystems enabled in **XR Origin**:

## Ball Shooting Mini-Game Flow
- Launch flow: `StartUIInBallShootingScene` gates the welcome and adjust overlays before play begins.
- Court setup: `prefabScalerForBallShooting` sizes the court/racket/ball prefabs and positions the court in front of the camera.
- Racket tracking: `racketFollow` parents the racket offset to the AR camera and adjusts distance as the scale changes.
- Firing loop: `BallLauncher` spawns physics balls on an interval; `tennisBall` tracks player hits for scoring or effects.
- Scene entry: Use the buttons wired to `changeScene` to jump into `BallShootingGameScene` from the main menu.

## Performance Overlay
1. Drop the **PerformanceMonitor** prefab (or add the script to an empty GameObject) inside your scene Canvas.
2. Assign a TextMeshProUGUI element to `statsText`.
3. Press Play or deploy; memory usage (MB) and GPU frame time (ms) will stream in real time.
4. Uncomment the draw-call line inside `Update()` if you want render-thread statistics too.

## Troubleshooting
- **No planes appear in Play Mode** – Verify your XR Origin includes an `ARPlaneManager` with the plane prefab assigned and that plane detection is toggled on in `ARModeController`.
- **Layer-based deletion fails** – Confirm you created `ARPlanes` and `PlacedObjects` layers and assigned them to plane prefabs and spawned objects respectively.
- **Image targets never track** – Check that the active XR Reference Image Library is linked to `ARTrackedImageManager` and that the physical print size matches the library metadata.
- **Build errors about missing TMP assets** – Reimport TextMeshPro Essentials via `Window → TextMeshPro → Import TMP Essential Resources`.

## What I Learned

| Area | Key Takeaways |
|---|---|
| **AR Foundation** | How `ARPlaneManager`, `ARRaycastManager`, and `ARTrackedImageManager` work together to provide stable world anchors |
| **Plane Detection** | Tuning plane mesh faders, handling plane life-cycle events, and merging overlapping planes cleanly |
| **Image Tracking** | Setting up reference image libraries, handling `trackingStateChanged` events, and spawning/despawning prefabs reliably |
| **Point Cloud** | Using feature points as alternative placement targets when flat planes aren't available |
| **Physics in AR** | Attaching Rigidbodies and colliders to AR-anchored objects; tuning bounciness (0.8) for realistic but playable ball bounce |
| **Prefab Scaling** | Dynamically scaling anchored prefabs to match real-world size using `prefabScalerForBallShooting` |
| **Camera-Follow UI** | Offsetting world-space objects from the AR camera transform for a reliable racket-tracking feel |
| **Scriptable Objects** | Using `ItemData.cs` (ScriptableObject) to centralise game values and avoid magic numbers |
| **New Input System** | Migrating from the legacy Input system to Unity's New Input System (`PlaceDragInScene_NewInputSystem.cs`) |
| **Performance Monitoring** | Building a live FPS/memory overlay (`PerformanceMonitor.cs`) with TextMeshPro for on-device diagnostics |
| **Scene Management** | Wiring multi-scene navigation with `changeScene.cs` and `GoalManager.cs` |



## License
Distributed under the [MIT License](LICENSE). Review the license text before shipping commercial builds or redistributing modified assets.
