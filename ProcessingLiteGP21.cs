using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProcessingLite
{
	/// <summary>
	/// Base ProcessingLite class.
	/// For Unity 2020 and newer.
	/// </summary>
	public class GP21 : MonoBehaviour
	{
		public const  int   MAXNumberOfObjects = 500;
		public static float PStrokeWeight      = 1;           //Processing
		public static Color PStroke            = Color.white; //Processing
		public static Color PFill              = Color.black; //Processing

		//Private variables
		private        PLine     _pLine;
		private        PRect     _pRect;
		private static Transform _holder;
		private        int       _background = 2;

		public static Transform Holder {
			get {
				if (_holder is { }) return _holder;
				var tmp = new GameObject("Holder");
				return _holder = tmp.transform;
			}
		}

		public GP21() => _holder = null;

		private void LateUpdate()
		{
			if (_background > -1) {
				if (_background == 0) Camera.main.clearFlags = CameraClearFlags.Nothing;
				_background--;
				if (_background == 1) return;
			}

			ResetRenderers();
		}

		private void ResetRenderers()
		{
			_pLine?.LateUpdate();
			_pRect?.LateUpdate();
		}

#region draw functions

		public void Background(int rgb)             => Background(rgb, rgb, rgb);
		public void Background(int r, int g, int b) => Background(new Color32((byte) r, (byte) g, (byte) b, 255));

		public void Background(Color color)
		{
			Camera.main.backgroundColor = color;
			Camera.main.clearFlags      = CameraClearFlags.Color;
			_background                 = Math.Max(1, _background);
			ResetRenderers();
		}

		public void Line(float x1, float y1, float x2, float y2)
		{
			_pLine ??= new PLine();
			_pLine.Line(x1, y1, x2, y2);
		}

		public void Rect(float x1, float y1, float x2, float y2)
		{
			_pRect ??= new PRect();
			_pRect.Rect(x1, y1, x2, y2);
		}

		public void Square(float x, float y, float extent)
		{
			_pRect ??= new PRect();
			_pRect.Square(x, y, extent);
		}

#endregion

#region Change properties

		public void StrokeWeight(float weight) => PStrokeWeight = Mathf.Max(weight, 0f);

		public void Stroke(int rgb, int a = 255) => Stroke(rgb, rgb, rgb, a);

		public void Stroke(int r, int g, int b, int a = 255) =>
			PStroke = new Color32((byte) r, (byte) g, (byte) b, (byte) a);

		public void Fill(int rgb, int a = 255) => Fill(rgb, rgb, rgb, a);

		public void Fill(int r, int g, int b, int a = 255) =>
			PFill = new Color32((byte) r, (byte) g, (byte) b, (byte) a);

#endregion
	}

	public interface IObjectPooling
	{
		int  CurrentID { get; set; }
		void LateUpdate();
	}

	public class PLine : IObjectPooling
	{
		public int CurrentID { get; set; }

		public void LateUpdate()
		{
			for (int i = CurrentID; i < _lines.Count; i++) {
				if (_lines[CurrentID].gameObject.activeSelf)
					_lines[CurrentID].gameObject.SetActive(false);
				else break;
			}

			CurrentID = 0;
		}

		private          Transform          _holder;
		private          Material           _material;
		private readonly List<LineRenderer> _lines = new List<LineRenderer>();
		private          Material           GetMaterial() => _material = new Material(Shader.Find("Sprites/Default"));

		public void Line(float x1, float y1, float x2, float y2) => Line(new Vector2(x1, y1), new Vector2(x2, y2));

		private void Line(Vector2 startPos, Vector2 endPos)
		{
			LineRenderer newLineRenderer;

			//Check for line from list, re-use or create new.
			if (CurrentID + 1 > _lines.Count || _lines[CurrentID] is null) {
				var newObject = new GameObject("Line" + (CurrentID + 1).ToString("000"));
				newObject.transform.parent        = _holder ? _holder : _holder = GP21.Holder;
				newLineRenderer                   = newObject.AddComponent<LineRenderer>();
				newLineRenderer.material          = _material ?? GetMaterial();
				newLineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				_lines.Add(newLineRenderer);
			} else {
				newLineRenderer = _lines[CurrentID];
				newLineRenderer.gameObject.SetActive(true);
			}

			//Apply settings
			newLineRenderer.SetPosition(0, startPos);
			newLineRenderer.SetPosition(1, endPos);

			newLineRenderer.startWidth = GP21.PStrokeWeight * 0.1f;
			newLineRenderer.endWidth   = GP21.PStrokeWeight * 0.1f;

			newLineRenderer.startColor = newLineRenderer.endColor = GP21.PStroke;

			//Increment to next line in list
			CurrentID = (CurrentID + 1) % GP21.MAXNumberOfObjects;
		}
	}
	
	public class PRect : IObjectPooling
	{
		public int CurrentID { get; set; }

		public void LateUpdate()
		{
			for (int i = CurrentID; i < _sprite.Count; i++) {
				if (_sprite[CurrentID].gameObject.activeSelf)
					_sprite[CurrentID].gameObject.SetActive(false);
				else break;
			}

			CurrentID = 0;
		}

		private          Transform            _holder;
		private          Sprite               _squareTexture;
		private readonly List<SpriteRenderer> _sprite = new List<SpriteRenderer>();

		private Sprite GetSquareTexture() => _squareTexture = AssetDatabase.LoadAssetAtPath<Sprite>(
			"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png");

		public void Rect(float x1, float y1, float x2, float y2)
		{
			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();

			//apply size and position
			var transform = newSpriteRenderer.transform;
			transform.position   = new Vector3((x1 + x2) / 2f,     (y1 + y2) / 2f,     0f);
			transform.localScale = new Vector3(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2), 1f);

			//Increment to next line in list
			CurrentID = (CurrentID + 1) % GP21.MAXNumberOfObjects;
		}

		public void Square(float x, float y, float extent)
		{
			SpriteRenderer newSpriteRenderer = GetSpriteRenderer();
			newSpriteRenderer.color = GP21.PFill;

			//apply size and position
			var transform = newSpriteRenderer.transform;
			transform.position   = new Vector3(x,      y,      0f);
			transform.localScale = new Vector3(extent, extent, 1f);

			//Increment to next line in list
			CurrentID = (CurrentID + 1) % GP21.MAXNumberOfObjects;
		}

		private SpriteRenderer GetSpriteRenderer()
		{
			if (CurrentID < _sprite.Count && _sprite[CurrentID] is { }) {
				_sprite[CurrentID].gameObject.SetActive(true);
				return _sprite[CurrentID];
			}

			var newObject = new GameObject("Rect" + (CurrentID + 1).ToString("000"));
			newObject.transform.parent = _holder ? _holder : _holder = GP21.Holder;
			var newSpriteRenderer = newObject.AddComponent<SpriteRenderer>();
			newSpriteRenderer.sprite = _squareTexture ?? GetSquareTexture();
			_sprite.Add(newSpriteRenderer);
			return newSpriteRenderer;
		}
	}
}