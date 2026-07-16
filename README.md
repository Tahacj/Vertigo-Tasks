# Vertigo Task Project

Welcome to the Vertigo Task project! 

> [!NOTE]
> There is nothing of importance in the main `Assets` directory. All the relevant content and work for this project is located entirely inside the `Assets/Tasks` folder.

Inside the `Tasks` folder, you will find three subfolders. The most important of these is the **Scenes** folder, which contains two Unity scenes (one for each task).

---

## Task 1: Battlepass UI

This task revolves around a fully interactive, dynamic Battlepass UI system.

### Scene Location
Open the Task 1 scene from the `Assets/Tasks/Scenes` folder.

### Interactions & Features
When you run the scene, all the Battlepass levels will instantly pop up and populate. Here is what you can do:
- **Scroll the levels:** You can horizontally scroll through the entire Battlepass.
- **Scroll-to-Current Level:** Click the left or right floating scroll buttons to instantly snap the view back to your current active level.
- **Advance Level:** Click the green advance button (the one with gems on it) to simulate leveling up.
- **Collect Prizes:** You can collect rewards from open levels (the ones with a gold/glowing background). As you advance your level, more rewards will unlock and become collectable.
- **Dynamic Updates:** Whenever you advance a level, the Level XP at the top of the screen updates automatically, and the number inside the scroll-to-level button updates to reflect your new active level.

### Deep Testing & Customization
If you want to test deeper functionalities:
1. **Modify Level Data:** Go to `Assets/Tasks/Task1/Battlepass Levels Objects` to tweak the individual level Scriptable Objects.
2. **Global Settings:** Select the **Battlepass Levels** GameObject inside the Canvas in the hierarchy. Here you can tweak settings such as:
   - Shine and Halo colors
   - The starting level index
   - How many items to skip/give before the numbering starts (e.g., skip 5 items before Level 1 starts).
3. **Responsive UI:** The layout is built to scale properly. Feel free to test it across different screen sizes and aspect ratios in the Game view!

---

## Task 2: Aura Effects

This task was specifically focused on visual effects and shaders.

### Scene Location
Open the Task 2 scene from the `Assets/Tasks/Scenes` folder.

### Interactions & Features
Because this task is heavily visual, there aren't many UI interactions. 
- **Run the scene** to view the Aura effects in action.
- **Fiddle with the settings!** You can select the Aura effect objects in the hierarchy and play around with their parameters. It's quite fun to customize and experiment with!

---

### A Note on Aesthetics
*If the exact colors or fonts don't perfectly match the original references, apologies! Unity refused to be of much help on this specific styling aspect, but the core functionality and layout have been meticulously implemented.*
