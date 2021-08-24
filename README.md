# ProcessingLite

This mini-project aims to recreate some basic processing functions in Unity3D, using a single drag and drop file.

This is made for Unity3D version 2020.x or newer but is largely untested.


## Useage
1. Download the ProcessingLite.cs file.
2. Drag and drop the ProcessingLite.cs file into your unity project.
3. Change inherence from the class MonoBehaviour to ProcessingLite.  
   (You will still have all MonoBehaviour functionality).
4. Use the supported commands in Start or Update.


## Supported Commands
All these commands might not have full support (Example: rounded corners on rectangles). The base version of these commands should work.

[Processing documentation](https://processing.org/reference/)

#### Environment
- Height
- Width

#### Shape
- Attributes
  - StrokeWeight()
- Vertex
  - BeginShape()
  - EndShape()
  - Vertex()
- 2D primitives
  - Line()
  - Rect()
  - Square()
  - Circle()
  - Ellipse()

#### Color
- Background()
- Fill()
- NoFill()  
- NoStroke()
- Stroke()


## Features covered by Unity
Some features from Processing will not be ported over since the already exist in Unity.

- Data
- Input
- Transform
- Math and Constants
- PVector


## Contributions
- [Wowbagger84](https://github.com/wowbagger84)
- [millennIumAMbiguity](https://github.com/millennIumAMbiguity)


## References
- [Processing](https://processing.org/)
