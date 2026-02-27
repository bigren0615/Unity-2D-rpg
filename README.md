<div align="center">

# ⚔️ TopDown Action RPG

### *A Zenless Zone Zero-inspired 2D Real-Time Combat Game*

![Unity](https://img.shields.io/badge/Unity-2022+-black?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-orange)
![Progress](https://img.shields.io/badge/Overall%20Progress-30%25-blue)

> **Goal:** Real-time physics-driven top-down action RPG with fluid combat, dodge mechanics, and responsive input — inspired by Zenless Zone Zero.

</div>

---

## 🎯 Design Pillars

| ❌ What We Are NOT Building | ✅ What We ARE Building |
|---|---|
| Pokémon-style turn-based RPG | Real-time input combat |
| Grid / coroutine movement | Physics-driven movement |
| Static isMoving locks | Combat that interrupts movement |
| Flat basic attacks | Dash / knockback / hitstop system |

---

## 📊 Overall Progress

```
Total Completion  ████████░░░░░░░░░░░░  30%
```

---

## 🗺️ Development Phases

### 🧱 Phase 0 — Mindset & Architecture
```
██████████████████████  100%  ✅ COMPLETE
```
- [x] Real-time input model locked in
- [x] Physics-driven movement chosen over grid-based
- [x] Combat-interruptible movement design confirmed
- [x] Dash / knockback / hitstop planned for later phases

---

### 🛠️ Phase 1 — Project & Scene Setup
```
██████████████████████  100%  ✅ COMPLETE
```
- [x] Project created (2D Built-in Render Pipeline)
- [x] `MainScene.unity` created and saved
- [x] Main Camera set to Orthographic, position `(0, 0, -10)`
- [x] Camera size configured for top-down clarity

---

### 🗂️ Phase 2 — Scene Hierarchy
```
███████████████░░░░░░░   65%  🔄 IN PROGRESS
```
- [x] `MainScene` exists
- [x] `Environment` node with Tilemap assets
- [x] `Player` GameObject present
- [ ] `Enemies` node created
- [ ] `NPCs` node created
- [ ] `BattleZones` node created
- [ ] `UI` node created

<details>
<summary>📌 Target Hierarchy</summary>

```
MainScene
├── Environment
│   ├── Tilemap_Floor     ✅
│   ├── Tilemap_Walls     ✅
│   └── Props             ⏳
├── Player                ✅
├── Enemies               ❌
├── NPCs                  ❌
├── BattleZones           ❌
└── UI                    ❌
```
</details>

---

### 🎨 Phase 3 — Sorting Layers
```
████████░░░░░░░░░░░░░░   35%  🔄 IN PROGRESS
```
- [x] Floor layer
- [ ] Characters layer
- [ ] Props layer
- [ ] Effects layer
- [ ] UI layer

---

### 🧱 Phase 4 — Physics Layers & Collision Matrix
```
████████░░░░░░░░░░░░░░   35%  🔄 IN PROGRESS
```
- [x] Player layer defined
- [ ] Enemy layer defined
- [ ] Solid layer defined
- [ ] Interactable layer defined
- [ ] BattleZone layer defined
- [ ] Collision matrix configured

<details>
<summary>📌 Target Collision Matrix</summary>

| Layer | Player | Enemy | Solid | Interactable | BattleZone |
|---|:---:|:---:|:---:|:---:|:---:|
| **Player** | ❌ | ✅ | ✅ | ✅ | ✅ |
| **Enemy** | ✅ | ❌ | ✅ | ❌ | ❌ |
| **Solid** | ✅ | ✅ | ❌ | ❌ | ❌ |
</details>

---

### 🌍 Phase 5 — Tilemap & Environment
```
██████████████████░░░░   80%  🔄 IN PROGRESS
```
- [x] Floor Tilemap with 458 tiles (TilesetFloor)
- [x] Nature Tilemap with 382 tiles (TilesetNature)
- [x] Floor Tilemap (no collider, Sorting Layer: Floor)
- [ ] Walls Tilemap with `TilemapCollider2D` + `CompositeCollider2D`
- [ ] Rigidbody2D (Static) on Walls
- [ ] Props with individual `BoxCollider2D` / `CircleCollider2D`

---

### 🧍 Phase 6 — Player Setup
```
█████████████████████░   92%  🔄 IN PROGRESS
```
- [x] `Player` GameObject named and in scene
- [x] `SpriteRenderer` attached (skull knight sprites)
- [x] `Animator` attached with `PlayerAnimator.controller`
- [x] `Rigidbody2D` — Dynamic, Gravity Scale: 0, Freeze Rotation Z ✅, Interpolate ✅
- [x] `Collider2D` attached
- [ ] Collider adjusted to feet (smaller than sprite to avoid wall snagging)

---

### 🏃 Phase 7 — Real-Time Movement
```
█████████████████████░   90%  🔄 IN PROGRESS
```
- [x] `PlayerController.cs` implemented
- [x] `Input.GetAxisRaw` for instant response
- [x] `rb.MovePosition` physics-based movement (no coroutines!)
- [x] Diagonal normalization (no speed boost)
- [x] Animation sync (`isMoving`, `moveX`, `moveY` params)
- [ ] Dash / dodge system (Phase 9)
- [ ] Movement cancellation during hit-stun

<details>
<summary>📌 PlayerController.cs</summary>

```csharp
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()   { ReadInput(); UpdateAnimation(); }
    private void FixedUpdate() { Move(); }

    private void ReadInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized; // no diagonal speed boost
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimation()
    {
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            animator.SetFloat("moveX", movementInput.x);
            animator.SetFloat("moveY", movementInput.y);
        }
    }
}
```
</details>

---

## ⚔️ Core Feature Status

### 🕹️ Movement
```
█████████████████████░   90%  🔄 IN PROGRESS
```
| Feature | Status |
|---|:---:|
| 8-directional real-time movement | ✅ |
| Physics-based (`Rigidbody2D`) | ✅ |
| Diagonal normalization | ✅ |
| Wall collision | ✅ |
| Dash / Dodge roll | ❌ |
| Knockback on hit | ❌ |

---

### 🎬 Animation
```
████████████████░░░░░░   70%  🔄 IN PROGRESS
```
| Animation | Status |
|---|:---:|
| Idle Down / Left / Up | ✅ |
| Walk Down / Left / Up | ✅ |
| Attack Down / Left / Up | ✅ |
| Dodge / Roll animation | ❌ |
| Hit / Hurt animation | ❌ |
| Death animation | ❌ |

---

### ⚔️ Combat
```
████░░░░░░░░░░░░░░░░░░   15%  🔄 IN PROGRESS
```
| Feature | Status |
|---|:---:|
| Attack animations (Down/Left/Up) | ✅ |
| Attack input detection | ❌ |
| Hitbox system | ❌ |
| Damage calculation | ❌ |
| Enemy HP system | ❌ |
| Hitstop (freeze frames) | ❌ |
| Knockback on enemy | ❌ |
| Dodge / i-frames | ❌ |
| Combo system | ❌ |

---

### 🖥️ UI / Menu
```
░░░░░░░░░░░░░░░░░░░░░░    0%  ❌ NOT STARTED
```
| Feature | Status |
|---|:---:|
| Main Menu screen | ❌ |
| HUD (HP bar, stamina/energy bar) | ❌ |
| Pause Menu | ❌ |
| Game Over screen | ❌ |

---

### 🔊 Sound Effects
```
░░░░░░░░░░░░░░░░░░░░░░    0%  ❌ NOT STARTED
```
| Feature | Status |
|---|:---:|
| Player footstep SFX | ❌ |
| Attack SFX | ❌ |
| Dodge SFX | ❌ |
| Hit / Impact SFX | ❌ |
| Background Music | ❌ |
| UI interaction SFX | ❌ |

---

## 🗓️ Upcoming Phases

| Phase | Feature | Priority |
|---|---|:---:|
| **Phase 8** | Combat System (hitboxes, damage, HP) | 🔴 High |
| **Phase 9** | Dodge / Dash with i-frames | 🔴 High |
| **Phase 10** | Enemy AI (patrol, chase, attack) | 🟠 Medium |
| **Phase 11** | HUD & UI (HP bar, menus) | 🟠 Medium |
| **Phase 12** | Sound Effects & Background Music | 🟡 Low |
| **Phase 13** | Polish (hitstop, screenshake, VFX) | 🟡 Low |

---

## 📁 Project Structure

```
Assets/
├── Animations/
│   └── Player/
│       ├── PlayerAnimator.controller        ✅
│       ├── Player_IdleDown.anim             ✅
│       ├── Player_IdleLeft.anim             ✅
│       ├── Player_IdleUp.anim               ✅
│       ├── Player_WalkDown.anim             ✅
│       ├── Player_WalkLeft.anim             ✅
│       ├── Player_walkUp.anim               ✅
│       ├── Player_AttackDown.anim           ✅
│       ├── Player_AttackLeft.anim           ✅
│       └── Player_AttackUp.anim             ✅
├── Scripts/
│   └── PlayerController.cs                 ✅
├── Sprites/
│   ├── SpriteSheet.png
│   ├── TilesetFloor.png
│   ├── TilesetNature.png
│   └── skull knight.png                    ✅
├── Tiles/
│   ├── TilesetFloor_0..457.asset           ✅
│   └── TilesetNature_0..381.asset          ✅
└── MainScene.unity                         ✅
```

---

<div align="center">

*Built with ❤️ in Unity — inspired by Zenless Zone Zero*

</div>
