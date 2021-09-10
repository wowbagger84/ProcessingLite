# ProcessingLite

This mini-project aims to recreate some basic processing functions in Unity3D, using a single drag and drop file.

This is made for Unity3D version 2020.2 or newer but is largely untested (It uses C# v8 stuff).


## Usage / Installation
1. Download the `ProcessingLite.cs` file.
2. Drag and drop the `ProcessingLite.cs` file into your unity project.  
  (You don't need to add it to your scene)
3. Change inherence from the class `MonoBehaviour` to `ProcessingLite.GP21`.  
   (You will still have all MonoBehaviour functionality).
4. Use the supported commands in `Start` and/or `Update` functions.


## Differences From Processing

Please note that the **origin** of the program will be different from processing. The camera is automatically moved so that origin is placed in the lower left corner of the screen.

The positions is measured in **Unity Units** and **not pixels**.

> _These changes have been made to make the future transition into Unity much easier._


## Supported Commands
All these commands will not have full processing support (Example: rounded corners on rectangles). The base version of these commands should work.

[Processing documentation](https://processing.org/reference/)


#### Environment
- `Height`, this returns the height of the screen in Unity-units.
- `Width`, this returns the width of the screen in Unity-units.

#### Shape
- Attributes
  - `StrokeWeight(line width)`
- Vertex
  - `BeginShape()`
  - `EndShape()`
  - `Vertex(x, y)`
- 2D primitives
  - `Circle(x, y, diameter)`
  - `Ellipse(x, y, height, width)`
  - `Line(x1, y1, x2, y2)`
  - `Point(x, y)`
  - `Quad(x1, y1, x2, y2, x3, y3, x4, y4)`
  - `Rect(x1, y1, x2, y2)`
  - `Square(x1, y1, size)`
  - `Triangle(x1, y1, x2, y2, x3, y3)`

#### Color
- `Background(color)`
- `Fill(color)`
- `NoFill()`
- `NoStroke()`
- `Stroke(color)`

#### Input
- `MouseX`
- `MouseY`

## Features covered by Unity
Some features from Processing will not be ported over since the already exist in Unity.

- Data
- Input (most of them)
- Transform
- Math and Constants
- PVector


## Test Code
In Unity, create a class called `Test`.
Change the inheritance from `MonoBehaviour` to `ProcessingLite.GP21`
Run the program. If you see the word _Hi_, then it's working as intended.

```CS
using UnityEngine;

public class Test : ProcessingLite.GP21
{
  void Start()
  {
    Line(4, 7, 4, 3);
    Line(4, 5, 6, 5);
    Line(6, 7, 6, 3);
    Line(8, 5.5f, 8, 3);
    Line(8, 7, 8, 6.8f);
  }
}
```

## Contributors
- [Wowbagger84](https://github.com/wowbagger84)
- [millennIumAMbiguity](https://github.com/millennIumAMbiguity)


## References
- [Processing](https://processing.org/)
