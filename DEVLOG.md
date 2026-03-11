# 📓 Development Log

- Devlog started on 11-02-2026

## Working on Ball Shooting Scene 

## 🗓️ 09-02-2026 - 11-02-2026 — Creation of game objects

**Time Spent:** ~2 hours 

**What I Did:**
- Created floor of court, ball and racket prefabs. 
- Added rigidBody and necessary colliders to the gameObjects.
- added scriptable object script to handle all the game value data (ItemData.cs).

## 🗓️ 12-02-2026 

**Time Spent:**  

**What I Did:**
- Scaling script attached to court prefab
- gameObject called *prefabScaler*

## 🗓️ 15-02-2026 

**Time Spent:** ~ 2 hrs 

**What I Did:**
- prefabs all got ready
- racket follow camera script added 
- ball launcher gameobject added


## 🗓️ 17-02-2026 

**Time Spent:** ~ 2 hrs 

**What I Did:**
- fixing the scale of the court (ScaleFactor = 6)
- random shooting of the ball in the court 
- random in range (3.6 to 4)

 - ISSUE : player not getting enough reaction time

## 🗓️ 18-02-2026 

**Time Spent:** ~ 3 hrs 

**What I Did:**
- Scorecard added on screen
- displays score, balls missed and high score
- fixed and updated several settings and values
- elastic collision made to semi elastic (bounciness 0.8 (80%))

## 🗓️ 25-02-2026 

**Time** 6 pm :  

**What I Did:**
- Fixed start shooting problem (fixed)
- Reset High Score button added in adjustment panel


## 🗓️ 04-03-2026 
 
**What I Did:**
- fixed screen timeout issue
- add new scene to place Lambo car in real env. (for revision purpose)

## 🗓️ 05-03-2026 
 
**What I Did:**
- video player (video texture added to Ball shooting scene)
- changed video player setting to play using URL (not video stored in app, cutting app size)
- added script to control video Player
- replaced cube with quad to play video on one side

## 🗓️ 06-03-2026 
 
**What I Did:**
- learned and applied scriptable object to Ball Shooting Scene

## 🗓️ 07-03-2026 
 
**What I Did:**
- Fixing of oriention issue of the court prefab in BallShootingScene (still not fixed)
- Fixed the orientation issue by placing the court prefab on detected ground plan
- fixing shooting angle default close to player's start position
- Fix: Update shooting logic to allow targeting near initial position of camera and in its direction 
- Fix: final fix to the issue of shooting in vicinity of initial position direction of the camera


## 🗓️ 08-03-2026 
 
**What I Did:**
- Added feature for player to choose whether they are standing or sitting while playing


## 🗓️ 09-03-2026 
 
**What I Did:**
- Made Duplicate of BallShootingGameScene as BallShootingGameSceneExtension for experimenting more and keep original Scene safe
- (Build Version 3.8.0)


## Working on Ball Shooting Scene Extension

## 🗓️ 09-03-2026 
 
**What I Did:**
- Diegetic UI : Score Board display inside the world BallShootingGameSceneExtension
- (Build Version 4.0.0)


## 🗓️ 10-03-2026 
 
**What I Did:**
- Detected planes visible and diable them (no longer visible) after court is set
- Quad of court (ground) replaced with solid court
- (Build Version 4.1.0)
- Tutorial for beginner players

## 🗓️ 12-03-2026 
 
**What I Did:**
- Calculated the shooting of balls
- (Build Version 4.2.0)



---

<!-- Keep adding entries as work -->