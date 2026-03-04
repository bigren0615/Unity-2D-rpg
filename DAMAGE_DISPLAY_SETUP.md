# Damage Display System Setup Guide
## Zenless Zone Zero Style - Yellow Damage Numbers

This guide will help you set up the damage display system in your Unity project.

---

## 📋 What's Been Created

Three new scripts have been added to your project:

1. **DamageText.cs** - Handles individual damage number animation
2. **DamageTextManager.cs** - Manages damage text pooling and spawning
3. **EnemyHealth.cs** - Updated to trigger damage display when enemies take damage

---

## 🎨 Setting Up the Damage Text Prefab

### Step 1: Create the Damage Text Prefab

1. **Create a new GameObject:**
   - In the Hierarchy, right-click → Create Empty
   - Name it `DamageText`

2. **Add TextMeshPro component:**
   - Select the `DamageText` GameObject
   - Click "Add Component" → Search for "TextMeshPro - Text"
   - If prompted to import TMP Essentials, click "Import TMP Essentials"

3. **Configure TextMeshPro Settings:**
   - **Text:** Set to "99" (placeholder)
   - **Font Size:** 3-4 (for world space)
   - **Color:** Yellow/Gold (#FFE644) - or use the custom color in DamageTextManager
   - **Alignment:** Center (both horizontal and vertical)
   - **Sorting Layer:** Make sure it's above your game objects (e.g., "UI" or "Effects")
   - **Order in Layer:** Set to a high value like 100

4. **Configure TextMeshPro Rect Transform:**
   - **Width:** 5
   - **Height:** 2

5. **Add the DamageText component:**
   - Click "Add Component" → Search for "DamageText"
   - The script will automatically attach

6. **Configure DamageText Component Settings (Optional):**
   - **Float Speed:** 1.5 (how fast text moves up)
   - **Lifetime:** 1.2 seconds (how long the text stays visible)
   - **Scale In Time:** 0.15 seconds (punch-in animation)
   - **Fade Out Time:** 0.4 seconds (fade duration)
   - **Start Scale:** 0.5 (initial size)
   - **Peak Scale:** 1.3 (punch effect peak)
   - **End Scale:** 1.0 (final size)
   - **Horizontal Drift Range:** 0.3 (random side-to-side movement)

7. **Make it a Prefab:**
   - Drag the `DamageText` GameObject from Hierarchy into your `Assets/Prefab` folder
   - Delete the GameObject from the Hierarchy (the prefab is saved)

---

## 🎮 Setting Up the Damage Text Manager

### Step 2: Add Manager to Scene

1. **Create a GameObject for the Manager:**
   - In the Hierarchy, right-click → Create Empty
   - Name it `DamageTextManager`

2. **Add the DamageTextManager component:**
   - Select the `DamageTextManager` GameObject
   - Click "Add Component" → Search for "DamageTextManager"

3. **Configure DamageTextManager Settings:**
   - **Damage Text Prefab:** Drag your `DamageText` prefab here (REQUIRED!)
   - **Damage Color:** RGB(255, 230, 68) or #FFE644 (bright yellow/gold - ZZZ style)
   - **Critical Color:** RGB(255, 128, 0) or #FF8000 (orange - for future critical hits)
   - **Initial Pool Size:** 10 (start with 10 damage texts ready)
   - **Max Pool Size:** 30 (maximum before recycling)
   - **Vertical Offset:** 0.5 (spawn above the enemy)
   - **Horizontal Spread:** 0.2 (random spread for visual variety)

---

## ✅ Testing

### Step 3: Test the Damage Display

1. **Make sure you have:**
   - ✓ DamageText prefab created and saved
   - ✓ DamageTextManager GameObject in your scene
   - ✓ DamageText prefab assigned to DamageTextManager

2. **Play the game:**
   - Start your game
   - Attack an enemy
   - You should see yellow damage numbers appear and float upward!

3. **If nothing appears, check:**
   - Console for error messages
   - DamageTextManager has the prefab assigned
   - TextMeshPro sorting layer is visible
   - Camera can see the damage text (check Z position and layers)

---

## 🎨 Customization

### Change Colors

Edit the colors in **DamageTextManager**:
- **Yellow (default):** RGB(255, 230, 68) - Bright gold
- **Orange (critical):** RGB(255, 128, 0) - For critical hits
- **Red:** RGB(255, 50, 50) - For a more aggressive look
- **White:** RGB(255, 255, 255) - Classic style

### Adjust Animation

Edit these values in the **DamageText** component on your prefab:
- **Float Speed:** How fast numbers rise (1.5 = moderate, 2.5 = fast)
- **Lifetime:** How long they stay (1.2 = quick, 2.0 = longer)
- **Peak Scale:** Size punch effect (1.3 = subtle, 1.8 = dramatic)
- **Horizontal Drift:** Side movement (0.3 = subtle, 0.8 = more random)

### Font Customization

1. Select your DamageText prefab
2. In the TextMeshPro component:
   - Change **Font Asset** to any TMP font
   - Add **Outline** for better visibility (Material tab → Outline)
   - Adjust **Font Size** (3-5 recommended for world space)

---

## 🔧 Advanced: Critical Hits (Future Implementation)

The system supports critical hits! To use them:

```csharp
// In your damage dealing code:
bool isCritical = Random.value < 0.2f; // 20% crit chance
float damage = isCritical ? baseDamage * 2f : baseDamage;

// Show damage with critical flag
DamageTextManager.Instance.ShowDamage(damage, enemyPosition, isCritical);
```

---

## 📝 Notes

- The system uses **object pooling** for performance - damage texts are reused
- **Sorting layers** are important - make sure damage text renders above everything
- For **3D games**, you may need to adjust the TextMeshPro settings or use TextMeshPro - Text (UI) with a Canvas
- The **Zenless Zone Zero style** features smooth animations, bright yellow colors, and subtle screen shake

---

## 🐛 Troubleshooting

### I don't see any damage numbers!

1. Check Console for errors
2. Verify DamageTextManager prefab is assigned
3. Check TextMeshPro sorting layer (should be visible)
4. Make sure DamageTextManager.Instance is not null

### Numbers appear but in the wrong place!

1. Adjust **Vertical Offset** in DamageTextManager (try 0.5 to 1.0)
2. Check your enemy's **pivot point** - damage spawns at transform.position

### Numbers are too small/large!

1. Adjust **Font Size** in TextMeshPro (try 3-5 for world space)
2. Modify **Scale** values in DamageText component

### Performance issues with many damage numbers?

1. Increase **Initial Pool Size** to reduce runtime instantiation
2. Decrease **Lifetime** so they disappear faster
3. Reduce **Max Pool Size** if you have hundreds of enemies

---

## 🎉 Enjoy!

Your damage display is now set up in Zenless Zone Zero style! The bright yellow numbers will pop up whenever enemies take damage, adding visual feedback to your combat system.

For questions or issues, check the Unity Console for detailed error messages.
