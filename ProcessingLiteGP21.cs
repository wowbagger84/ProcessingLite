using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ProcessingLite
{
	/// <summary>
	///     Base ProcessingLite class.
	///     For Unity 2020 and newer.
	/// </summary>
	public class GP21 : MonoBehaviour
	{
		public const int MAXNumberOfObjects = 1000;
		private const float PointSize = 0.02f;

		public static float PStrokeWeight = 1;           //Processing
		public static Color PStroke = Color.white; //Processing
		public static Color PFill = Color.black; //Processing
		public static int PFontSize = 14;

		internal static bool DrawStroke = true;
		internal static bool DrawFill = true;

		//Private variables
		private PLine _pLine;
		private PRect _pRect;
		private PShape _pShape;
		private PEllipse _pEllipse;
		private PText _pText;

		internal static Camera _cameraRef;

		public GP21() => ProcessingLiteGP21.Resets += ResetRenderers;
		private void OnDestroy() => ProcessingLiteGP21.Resets -= ResetRenderers;

		private void ResetRenderers()
		{
			_pLine?.LateUpdate();
			_pRect?.LateUpdate();
			_pShape?.LateUpdate();
			_pEllipse?.LateUpdate();
			_pText?.LateUpdate();
		}

		public float Width
		{
			get
			{
				_cameraRef ??= Camera.main;
				return _cameraRef.orthographicSize * _cameraRef.aspect * 2;
			}
		}

		public float Height
		{
			get
			{
				_cameraRef ??= Camera.main;
				return _cameraRef.orthographicSize * 2;
			}
		}

		public float MouseX
		{
			get
			{
				_cameraRef ??= Camera.main;
				return _cameraRef.ScreenToWorldPoint(Input.mousePosition).x;
			}
		}

		public float MouseY
		{
			get
			{
				_cameraRef ??= Camera.main;
				return _cameraRef.ScreenToWorldPoint(Input.mousePosition).y;
			}
		}



		#region draw functions

		/// <summary>
		/// The Background() function sets the color used for the background.
		/// </summary>
		/// <param name="rgb">specifies a value between white and black</param>
		public void Background(int rgb) => Background(rgb, rgb, rgb);

		/// <summary>
		/// The Background() function sets the color used for the background.
		/// </summary>
		/// <param name="r">red</param>
		/// <param name="g">green</param>
		/// <param name="b">blue</param>
		public void Background(int r, int g, int b) => Background(new Color32((byte)r, (byte)g, (byte)b, 255));

		/// <summary>
		/// The Background() function sets the color used for the background.
		/// </summary>
		public void Background(Color color)
		{
			_cameraRef ??= Camera.main;
			_cameraRef.backgroundColor = color;
			_cameraRef.clearFlags = CameraClearFlags.Color;
			ProcessingLiteGP21.Background = Math.Max(1, ProcessingLiteGP21.Background);
			ProcessingLiteGP21.EarlyReset();
		}

		/// <summary>
		/// Draws a line (a direct path between two points) to the screen.
		/// </summary>
		/// <param name="x1">x-coordinate of the first point</param>
		/// <param name="y1">y-coordinate of the first point</param>
		/// <param name="x2">x-coordinate of the second point</param>
		/// <param name="y2">y-coordinate of the second point</param>
		public void Line(float x1, float y1, float x2, float y2)
		{
			_pLine ??= new PLine();
			_pLine.Line(x1, y1, x2, y2);
		}

		/// <summary>
		/// A quad is a quadrilateral, a four sided polygon.
		/// </summary>
		/// <param name="x1">x-coordinate of the first corner</param>
		/// <param name="y1">y-coordinate of the first corner</param>
		/// <param name="x2">x-coordinate of the second corner</param>
		/// <param name="y2">y-coordinate of the second corner</param>
		/// <param name="x3">x-coordinate of the third corner</param>
		/// <param name="y3">y-coordinate of the third corner</param>
		/// <param name="x4">x-coordinate of the fourth corner</param>
		/// <param name="y4">y-coordinate of the fourth corner</param>
		public void Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>(
				new[] {
					new Vector2(x1, y1),
					new Vector2(x2, y2),
					new Vector2(x3, y3),
					new Vector2(x4, y4)
				});
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, DrawFill);
		}

		/// <summary>
		/// A quad is a quadrilateral, a four sided polygon.
		/// </summary>
		/// <param name="pos1">first position</param>
		/// <param name="pos2">second position</param>
		/// <param name="pos3">third position</param>
		/// <param name="pos4">fourth position</param>
		public void Quad(Vector2 pos1, Vector2 pos2, Vector2 pos3, Vector2 pos4)
		{
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>(
				new[] { pos1, pos2, pos3, pos4 }
				);
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, DrawFill);
		}

		/// <summary>
		/// A triangle is a plane created by connecting three points.
		/// </summary>
		/// <param name="x1">x-coordinate of the first corner</param>
		/// <param name="y1">y-coordinate of the first corner</param>
		/// <param name="x2">x-coordinate of the second corner</param>
		/// <param name="y2">y-coordinate of the second corner</param>
		/// <param name="x3">x-coordinate of the third corner</param>
		/// <param name="y3">y-coordinate of the third corner</param>
		public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
		{
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>(
				new[] {
					new Vector2(x1, y1),
					new Vector2(x2, y2),
					new Vector2(x3, y3)
				});
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, DrawFill);
		}

		/// <summary>
		/// A quad is a quadrilateral, a four sided polygon.
		/// </summary>
		/// <param name="pos1">first position</param>
		/// <param name="pos2">second position</param>
		/// <param name="pos3">third position</param>
		public void Triangle(Vector2 pos1, Vector2 pos2, Vector2 pos3)
		{
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>(
				new[] { pos1, pos2, pos3 }
			);
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, DrawFill);
		}

		/// <summary>
		/// Draws a rectangle to the screen.
		/// </summary>
		/// <param name="x1">x-coordinate of the rectangle</param>
		/// <param name="y1">y-coordinate of the rectangle</param>
		/// <param name="x2">width of the rectangle</param>
		/// <param name="y2">height of the rectangle</param>
		public void Rect(float x1, float y1, float x2, float y2)
		{
			_pRect ??= new PRect();
			_pRect.Rect(x1, y1, x2, y2);
			if (!DrawStroke) return;
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>(
				new[] {
					new Vector2(x1, y1),
					new Vector2(x1, y2),
					new Vector2(x2, y2),
					new Vector2(x2, y1)
				});
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, false);
		}

		/// <summary>
		/// Draws a square to the screen.
		/// </summary>
		/// <param name="x">x-coordinate of the rectangle</param>
		/// <param name="y">y-coordinate of the rectangle</param>
		/// <param name="extent">width and height of the rectangle</param>
		public void Square(float x, float y, float extent)
		{
			_pRect ??= new PRect();
			_pRect.Square(x, y, extent);

			if (!DrawStroke) return;
			_pShape ??= new PShape();
			float x1 = x + extent / 2, y1 = y - extent / 2, x2 = x - extent / 2, y2 = y + extent / 2;
			_pShape.ShapeKeys = new List<Vector2>(
				new[] {
					new Vector2(x1, y1),
					new Vector2(x1, y2),
					new Vector2(x2, y2),
					new Vector2(x2, y1)
				});
			_pShape.ShapeMode = PShapeMode.Default;
			_pShape.Shape(true, false);
		}

		/// <summary>
		/// Draws a point, a coordinate in space.
		/// </summary>
		/// <param name="x">x-coordinate of the point</param>
		/// <param name="y">y-coordinate of the point</param>
		public void Point(float x, float y)
		{
			_pRect ??= new PRect();
			_pRect.Point(x, y, PointSize);
		}

		/// <summary>
		/// Draws an ellipse (oval) to the screen.
		/// </summary>
		/// <param name="x">x-coordinate of the ellipse</param>
		/// <param name="y">y-coordinate of the ellipse</param>
		/// <param name="height">width of the ellipse</param>
		/// <param name="width">height of the ellipse</param>
		public void Ellipse(float x, float y, float height, float width)
		{
			_pEllipse ??= new PEllipse();
			if (DrawStroke)
			{
				_pEllipse.Ellipse(x, y, height + PStrokeWeight / 8f, width + PStrokeWeight / 8f, true);
				_pEllipse.Ellipse(x, y, height - PStrokeWeight / 8f, width - PStrokeWeight / 8f);
			}
			else _pEllipse.Ellipse(x, y, height, width);
		}

		/// <summary>
		/// Draws a circle to the screen.
		/// </summary>
		/// <param name="x">x-coordinate of the circle</param>
		/// <param name="y">y-coordinate of the circle</param>
		/// <param name="diameter">width and height of the circle</param>
		public void Circle(float x, float y, float diameter)
		{
			_pEllipse ??= new PEllipse();
			if (DrawStroke)
			{
				_pEllipse.Circle(x, y, diameter + PStrokeWeight / 8f, true);
				_pEllipse.Circle(x, y, diameter - PStrokeWeight / 8f);
			}
			else _pEllipse.Circle(x, y, diameter);
		}

		/// <summary>
		/// Draws Text to the screen.
		/// </summary>
		/// <param name="string">String to display</param>
		/// <param name="x">x-coordinate of the text</param>
		/// <param name="y">y-coordinate of the text</param>
		public void Text(string text, float x, float y)
		{
			_pText ??= new PText();
			var pos = _cameraRef.WorldToScreenPoint(new Vector3(x, y, 0));
			_pText.Text(text, pos.x, pos.y);
		}


		/// <summary>
		/// Using the BeginShape() and EndShape() functions allow creating more complex forms.
		/// </summary>
		/// <param name="mode">Either PShapeMode. Default or PShapeMode.Lines</param>
		public void BeginShape(PShapeMode mode = PShapeMode.Default)
		{
			_pShape ??= new PShape();
			_pShape.ShapeKeys = new List<Vector2>();
			_pShape.ShapeMode = mode;
		}

		/// <summary>
		/// All shapes are constructed by connecting a series of vertices.
		/// It is used exclusively within the BeginShape() and EndShape() functions.
		/// </summary>
		/// <param name="x">x-coordinate of the vertex</param>
		/// <param name="y">y-coordinate of the vertex</param>
		/// <exception cref="Exception">Vertex() is used exclusively within the beginShape() and endShape() functions.</exception>
		public void Vertex(float x, float y)
		{
#if UNITY_EDITOR
			if (_pShape.ShapeKeys is null)
				throw new Exception("Can't creates Vertex before a new shape has been started.");
#endif
			_pShape.ShapeKeys.Add(new Vector2(x, y));
		}

		/// <summary>
		/// The endShape() function is the companion to beginShape() and may only be called after beginShape().
		/// </summary>
		/// <param name="close">when true, the shape will have the first and last point connected</param>
		/// <exception cref="Exception"></exception>
		public void EndShape(bool close = false)
		{
#if UNITY_EDITOR
			if (_pShape?.ShapeKeys is null) throw new Exception("Can't end shape before a new shape has been started.");
			if (_pShape.ShapeKeys.Count == 0)
				throw new Exception("Can't end shape before any vertexes have been defined.");
#endif
			if (_pShape.ShapeMode == PShapeMode.Lines)
			{
				_pLine ??= new PLine();
				for (int i = 0; i + 1 < _pShape.ShapeKeys.Count; i += 2)
					_pLine.Line(
						_pShape.ShapeKeys[i].x, _pShape.ShapeKeys[i].y, _pShape.ShapeKeys[i + 1].x,
						_pShape.ShapeKeys[i + 1].y);
				_pShape.ShapeKeys = null;
			}
			else _pShape.Shape(close);
		}

		#endregion

		#region Change properties

		/// <summary>
		/// Sets the width of the stroke used for lines, points, and the border around shapes.
		/// </summary>
		/// <param name="weight">the weight of the stroke</param>
		public void StrokeWeight(float weight)
		{
			PStrokeWeight = Mathf.Max(weight, 0f);
			DrawStroke = PStrokeWeight != 0 && PStroke.a != 0;
		}

		/// <summary>
		/// Sets the font size of the following text elements.
		/// </summary>
		/// <param name="size">font size</param>
		public void TextSize(int size)
		{
			PFontSize = Mathf.Max(size, 0);
		}

		/// <summary>
		/// Disables drawing the stroke (outline).
		/// </summary>
		public void NoStroke()
		{
			PStroke.a = 0;
			DrawStroke = false;
		}

		/// <summary>
		/// Sets the color used to draw lines and borders around shapes.
		/// </summary>
		/// <param name="rgb">specifies a value between white and black</param>
		/// <param name="a">opacity of the stroke</param>
		public void Stroke(int rgb, int a = 255) => Stroke(rgb, rgb, rgb, a);

		/// <summary>
		/// Sets the color used to draw lines and borders around shapes.
		/// </summary>
		/// <param name="r">red</param>
		/// <param name="g">green</param>
		/// <param name="b">blue</param>
		/// <param name="a">opacity of the stroke</param>
		public void Stroke(int r, int g, int b, int a = 255)
		{
			PStroke = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
			DrawStroke = PStrokeWeight != 0 && a != 0;
		}

		/// <summary>
		/// Disables filling geometry.
		/// </summary>
		public void NoFill()
		{
			PFill.a = 0;
			DrawFill = false;
		}

		/// <summary>
		/// Sets the color used to fill shapes.
		/// </summary>
		/// <param name="rgb">specifies a value between white and black</param>
		/// <param name="a">opacity of the stroke</param>
		public void Fill(int rgb, int a = 255) => Fill(rgb, rgb, rgb, a);

		/// <summary>
		/// Sets the color used to fill shapes.
		/// </summary>
		/// <param name="r">red</param>
		/// <param name="g">green</param>
		/// <param name="b">blue</param>
		/// <param name="a">opacity of the stroke</param>
		public void Fill(int r, int g, int b, int a = 255)
		{
			PFill = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
			DrawFill = a != 0;
		}

		#endregion
	}

	//Class added to holde object in scene.
	public class ProcessingLiteGP21 : MonoBehaviour
	{
		public delegate void LateReset();

		public const float ZOffset = -0.001f; //offset between objects in depth.
		internal static int Background = 2;
		internal static float DrawZOffset; //current offset

		private static Transform _holder;
		private static Transform _canvas;

		private Camera cameraRef;

#if !UNITY_2020_2_OR_NEWER && UNITY_EDITOR
		private ProcessingLiteGP21()
		{
			Debug.LogError("Unity version not supported\nUnity 2020.2 or newer required");
		}
#endif
#if UNITY_EDITOR
		private void Awake()
		{
			if (transform.name == "Holder" && (_holder is null || _holder == transform)) return;
			Debug.LogError("Improper use of ProcessingLiteGP21.");
		}
#endif
		private void Start()
		{
			cameraRef = Camera.main;
			float width = cameraRef.orthographicSize * cameraRef.aspect;
			float height = cameraRef.orthographicSize;
			cameraRef.transform.Translate(width, height, 0);
		}

		public static Transform Holder
		{
			get
			{
				if (_holder is { }) return _holder;
				var tmp = new GameObject("Holder");
				tmp.AddComponent<ProcessingLiteGP21>();
				return _holder = tmp.transform;
			}
		}

		public static Transform Canvas
		{
			get
			{
				if (_canvas is { }) return _canvas;
				var tmp = new GameObject("Canvas");
				tmp.AddComponent<Canvas>();
				tmp.AddComponent<CanvasScaler>();
				tmp.AddComponent<GraphicRaycaster>();
				tmp.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				return _canvas = tmp.transform;
			}
		}

		private void LateUpdate()
		{
			if (Background > -1)
			{
				if (Background == 0) cameraRef.clearFlags = CameraClearFlags.Nothing;
				Background--;
				if (Background == 1) return;
			}

			EarlyReset();
		}

		private void OnDestroy() => _holder = null;

		public static event LateReset Resets;


		public static void EarlyReset()
		{
			DrawZOffset = 0;
			Resets?.Invoke();
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ProcessingLiteGP21))]
	public class ProcessingLiteEditor : Editor
	{
		private void OnEnable()
		{
			if (Application.isPlaying) return;
			Debug.LogError("Improper use of ProcessingLiteGP21.\nProcessingLiteGP21 is not allowed to be assigned as a component.\nRemoving from Scene.");
			Destroy(this);
		}
	}
#endif

	public class ObjectPoolingBase : MonoBehaviour
	{
		protected Transform _holder;
		protected Material _material;
		public int CurrentID { get; set; }

		internal void UpdateID<T>(List<T> _lines) where T : Component
		{
			for (int i = CurrentID; i < _lines.Count; i++)
				if (_lines[i].gameObject.activeSelf)
					_lines[i].gameObject.SetActive(false);
				else break;

			CurrentID = 0;
		}

		internal void IncrementID() //Increment to next line in list
		{
			if (CurrentID + 1 == GP21.MAXNumberOfObjects)
				Debug.LogWarning("Maximum number of obects reached, object culling is occuring!");

			CurrentID = (CurrentID + 1) % GP21.MAXNumberOfObjects;
		}
	}

	public interface IObjectPooling
	{
		int CurrentID { get; set; }
		void LateUpdate();
	}

	public class PLine : ObjectPoolingBase, IObjectPooling
	{
		private readonly List<LineRenderer> _lines = new List<LineRenderer>();

		public void LateUpdate()
		{
			UpdateID(_lines);
		}

		private Material GetMaterial() => _material = new Material(Shader.Find("Sprites/Default"));

		public void Line(float x1, float y1, float x2, float y2) => Line(new Vector2(x1, y1), new Vector2(x2, y2));

		private void Line(Vector2 startPos, Vector2 endPos)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			LineRenderer newLineRenderer;

			//Check for line from list, re-use or create new.
			if (CurrentID + 1 > _lines.Count || _lines[CurrentID] is null)
			{
				var newObject = new GameObject("Line" + (_lines.Count + 1).ToString("000"));
				newObject.transform.parent = _holder ? _holder : _holder = ProcessingLiteGP21.Holder;
				newLineRenderer = newObject.AddComponent<LineRenderer>();
				newLineRenderer.material = _material ?? GetMaterial();
				newLineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				newLineRenderer.receiveShadows = false;
				newLineRenderer.useWorldSpace = false;
				newLineRenderer.sortingOrder = CurrentID;
				_lines.Add(newLineRenderer);
			}
			else
			{
				newLineRenderer = _lines[CurrentID];
				newLineRenderer.gameObject.SetActive(true);
			}

			newLineRenderer.transform.position = new Vector3(0, 0, ProcessingLiteGP21.DrawZOffset);

			//Apply settings
			newLineRenderer.SetPosition(0, startPos);
			newLineRenderer.SetPosition(1, endPos);

			newLineRenderer.startWidth = GP21.PStrokeWeight * 0.1f;
			newLineRenderer.endWidth = GP21.PStrokeWeight * 0.1f;

			newLineRenderer.startColor = newLineRenderer.endColor = GP21.PStroke;

			IncrementID();
		}
	}

	public enum PShapeMode { Default, Lines }

	public class PShape : ObjectPoolingBase, IObjectPooling
	{
		private readonly List<LineRenderer> _lines = new List<LineRenderer>();
		private readonly List<MeshFilter> _mesh = new List<MeshFilter>();
		private readonly List<MeshRenderer> _meshRenderer = new List<MeshRenderer>();

		private Material _meshMaterial;

		public List<Vector2> ShapeKeys;
		public PShapeMode ShapeMode;

		public void LateUpdate()
		{
			UpdateID(_lines);
		}

		private Material GetMaterial() => _material = new Material(Shader.Find("Sprites/Default"));
		private Material GetMeshMaterial() => _meshMaterial = new Material(Shader.Find("Unlit/Color"));

		public void Shape(bool loop = false, bool fill = true)
		{
			switch (ShapeMode)
			{
				case PShapeMode.Default:
					DrawShape(ShapeKeys.ToArray(), loop, fill);
					break;
			}
		}

		//creates a LineRenderer for the outline and a MeshRenderer for the fill.
		private void DrawShape(Vector2[] shapeKeys, bool loop = false, bool fill = true)
		{
			LineRenderer newLineRenderer;
			MeshFilter newMeshFilter;
			MeshRenderer newMeshRenderer;

			//Check for line from list, re-use or create new.
			if (CurrentID + 1 > _lines.Count || _lines[CurrentID] is null)
			{
				//create a new object with the following components:
				//LineRenderer, MeshFilter, and MeshRenderer
				var newObject = new GameObject("Shape" + (_lines.Count + 1).ToString("000"));
				newObject.transform.parent = _holder ? _holder : _holder = ProcessingLiteGP21.Holder;
				newLineRenderer = newObject.AddComponent<LineRenderer>();
				newLineRenderer.material = _material ?? GetMaterial();
				newLineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				newLineRenderer.receiveShadows = false;
				newLineRenderer.useWorldSpace = false;
				_lines.Add(newLineRenderer);
				newMeshFilter = newObject.AddComponent<MeshFilter>();
				newMeshRenderer = newObject.AddComponent<MeshRenderer>();
				newMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				newMeshRenderer.receiveShadows = false;
				newMeshRenderer.material = _meshMaterial ?? GetMeshMaterial();
				_meshRenderer.Add(newMeshRenderer);
				_mesh.Add(newMeshFilter);
			}
			else
			{
				newLineRenderer = _lines[CurrentID];
				newMeshFilter = _mesh[CurrentID];
				newMeshRenderer = _meshRenderer[CurrentID];
				newLineRenderer.gameObject.SetActive(true); //enable the hole gameObject
			}

			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;
			newLineRenderer.transform.position = new Vector3(0, 0, ProcessingLiteGP21.DrawZOffset);

			if (GP21.DrawFill && loop && fill) ShapeFill(shapeKeys, newMeshFilter.mesh, newMeshRenderer);

			if (GP21.DrawStroke)
			{
				newLineRenderer.positionCount = shapeKeys.Length;
				newLineRenderer.loop = loop;
				ShapeStroke(shapeKeys, newLineRenderer);
			}
			else newLineRenderer.positionCount = 0;

			IncrementID();
		}

		private void ShapeFill(Vector2[] shapeKeys, Mesh mesh, MeshRenderer meshRenderer)
		{
			//reverse if facing the wrong direction so that the mesh will rendered correctly
			if (Vector2.Angle(shapeKeys[0], shapeKeys[1]) -
				Vector2.Angle(shapeKeys[0], shapeKeys[2]) > 0)
				Array.Reverse(shapeKeys);

			//Apply shape
			var verts = new Vector3[shapeKeys.Length];
			for (int i = 0; i < verts.Length; i++) verts[i] = shapeKeys[i];
			mesh.vertices = verts;

			//Map vertices ids to group of 3 making it into a face / triangle
			int triLenght = shapeKeys.Length - 1;
			var tri = new List<int>();
			for (int i = 0; i < triLenght; i += 2)
			{
				tri.Add(i);
				tri.Add(i + 1);
				tri.Add(i + 2);
			}

			//unless the shape is a triangle, close the fill. The last index would otherwise be out of bounds.
			if (shapeKeys.Length > 3)
				tri[tri.Count - 1] = 0;
			//Apply faces
			mesh.triangles = tri.ToArray();

			//Apply settings
			meshRenderer.material.color = GP21.PFill;
		}

		private void ShapeStroke(Vector2[] shapeKeys, LineRenderer newLineRenderer)
		{
			//Apply shape
			for (int i = 0; i < shapeKeys.Length; i++)
				newLineRenderer.SetPosition(i, shapeKeys[i]);

			//Apply settings
			newLineRenderer.startWidth = GP21.PStrokeWeight * 0.1f;
			newLineRenderer.endWidth = GP21.PStrokeWeight * 0.1f;

			newLineRenderer.startColor = newLineRenderer.endColor = GP21.PStroke;
		}
	}


	public class PRect : ObjectPoolingBase, IObjectPooling
	{
		private readonly List<SpriteRenderer> _sprite = new List<SpriteRenderer>();

		private Sprite _squareTexture;

		public void LateUpdate()
		{
			UpdateID(_sprite);
		}

		private Sprite GetSquareTexture() => _squareTexture = AssetDatabase.LoadAssetAtPath<Sprite>(
			"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png");

		public void Rect(float x1, float y1, float x2, float y2)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = GP21.PFill;

			//apply size and position
			Transform transform = newSpriteRenderer.transform;
			transform.position = new Vector3((x1 + x2) / 2f, (y1 + y2) / 2f, ProcessingLiteGP21.DrawZOffset);
			transform.localScale = new Vector3(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2), 1f);

			IncrementID();
		}

		public void Square(float x, float y, float extent)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = GP21.PFill;

			//apply size and position
			Transform transform = newSpriteRenderer.transform;
			transform.position = new Vector3(x, y, ProcessingLiteGP21.DrawZOffset);
			transform.localScale = new Vector3(extent, extent, 1f);

			IncrementID();
		}

		public void Point(float x, float y, float PointSize)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = GP21.PStroke;

			//apply size and position
			Transform transform = newSpriteRenderer.transform;
			transform.position = new Vector3(x, y, ProcessingLiteGP21.DrawZOffset);
			transform.localScale = new Vector3(PointSize, PointSize, 1f);

			IncrementID();
		}

		private SpriteRenderer GetSpriteRenderer()
		{
			if (CurrentID < _sprite.Count && _sprite[CurrentID] is { })
			{
				_sprite[CurrentID].gameObject.SetActive(true);
				return _sprite[CurrentID];
			}

			var newObject = new GameObject("Rect" + (_sprite.Count + 1).ToString("000"));
			newObject.transform.parent = _holder ? _holder : _holder = ProcessingLiteGP21.Holder;
			var newSpriteRenderer = newObject.AddComponent<SpriteRenderer>();
			newSpriteRenderer.sprite = _squareTexture ?? GetSquareTexture();
			_sprite.Add(newSpriteRenderer);
			return newSpriteRenderer;
		}
	}

	public class PEllipse : ObjectPoolingBase, IObjectPooling
	{
		private readonly List<SpriteRenderer> _sprite = new List<SpriteRenderer>();

		private Sprite _squareTexture;

		public void LateUpdate()
		{
			UpdateID(_sprite);
		}

		private Sprite GetSquareTexture() => _squareTexture = AssetDatabase.LoadAssetAtPath<Sprite>(
			"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Circle.png");

		public void Ellipse(float x, float y, float height, float width, bool swapColor = false)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = swapColor ? GP21.PStroke : GP21.PFill;

			//apply size and position
			Transform transform = newSpriteRenderer.transform;
			transform.position = new Vector3(x, y, ProcessingLiteGP21.DrawZOffset);
			transform.localScale = new Vector3(height, width, 1f);

			IncrementID();
		}

		public void Circle(float x, float y, float diameter, bool swapColor = false)
		{
			ProcessingLiteGP21.DrawZOffset += ProcessingLiteGP21.ZOffset;

			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = swapColor ? GP21.PStroke : GP21.PFill;
			newSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			newSpriteRenderer.sortingOrder = CurrentID;

			//creates sprite mask objects for circles without fill
			if (!swapColor && !GP21.DrawFill)
			{
				newSpriteRenderer.enabled = false;
				SpriteMask newSpriteMask = newSpriteRenderer.gameObject.GetComponent<SpriteMask>();
				if (newSpriteMask == null) //Short hand version ??= doesn't work here?
					newSpriteMask = newSpriteRenderer.gameObject.AddComponent<SpriteMask>();
				newSpriteMask.sprite = newSpriteRenderer.sprite;
				newSpriteMask.isCustomRangeActive = true;
				newSpriteMask.frontSortingOrder = CurrentID;
				newSpriteMask.backSortingOrder = CurrentID - 2;
			}

			//apply size and position
			Transform transform = newSpriteRenderer.transform;
			transform.position = new Vector3(x, y, ProcessingLiteGP21.DrawZOffset);
			transform.localScale = new Vector3(diameter, diameter, 1f);

			IncrementID();
		}

		private SpriteRenderer GetSpriteRenderer()
		{
			if (CurrentID < _sprite.Count && _sprite[CurrentID] is { })
			{
				_sprite[CurrentID].gameObject.SetActive(true);
				return _sprite[CurrentID];
			}

			var newObject = new GameObject("Circle" + (_sprite.Count + 1).ToString("000"));
			newObject.transform.parent = _holder ? _holder : _holder = ProcessingLiteGP21.Holder;
			var newSpriteRenderer = newObject.AddComponent<SpriteRenderer>();
			newSpriteRenderer.sprite = _squareTexture ?? GetSquareTexture();
			_sprite.Add(newSpriteRenderer);
			return newSpriteRenderer;
		}
	}

	public class PText : ObjectPoolingBase, IObjectPooling
	{
		private readonly List<Text> _text = new List<Text>();

		private Transform _canvas;
		private Font _font;

		public void LateUpdate()
		{
			UpdateID(_text);
		}

		private Font GetFont() => _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		//AssetDatabase.LoadAssetAtPath<Font>(
		//	"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Fonts/Arial.ttf");

		public void Text(string text, float x, float y, bool swapColor = false)
		{
			Text newTextComponent = GetTextComponent();
			newTextComponent.text = text;
			newTextComponent.color = GP21.PFill;
			newTextComponent.font = _font ?? GetFont();
			newTextComponent.alignment = TextAnchor.MiddleCenter;
			newTextComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
			newTextComponent.verticalOverflow = VerticalWrapMode.Overflow;
			newTextComponent.fontSize = GP21.PFontSize;

			//apply size and position
			RectTransform transform = newTextComponent.GetComponent<RectTransform>();
			transform.anchorMin = Vector2.zero;
			transform.anchorMax = Vector2.zero;
			transform.position = new Vector3(x, y, ProcessingLiteGP21.DrawZOffset);

			IncrementID();
		}

		private Text GetTextComponent()
		{
			if (CurrentID < _text.Count && _text[CurrentID] is { })
			{
				_text[CurrentID].gameObject.SetActive(true);
				return _text[CurrentID];
			}

			var newObject = new GameObject("Text" + (_text.Count + 1).ToString("000"));
			newObject.transform.parent = _canvas ? _canvas : _canvas = ProcessingLiteGP21.Canvas;
			var newTextComponent = newObject.AddComponent<Text>();

			_text.Add(newTextComponent);
			return newTextComponent;
		}
	}
}
