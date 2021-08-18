using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProcessingLiteGP21 : MonoBehaviour
{
	public int maxNumberOfObjects = 100;
	public Color color = Color.white;
	public float StrokeWeight { get; set; }     //Processing
	public Color Stroke { get; set; }           //Processing

	//Private variables
	bool init;
	Transform holder;
	int currentLine = 0;
	Material material;
	Sprite squareTexture;

	private void Init()
	{
		var tmp = new GameObject("Holder");
		holder = tmp.transform;
		Debug.Log(holder);
		material = new Material(Shader.Find("Unlit/Texture"));
		squareTexture = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png");

		init = true;
	}

	public void Line(float x1, float y1, float x2, float y2)
	{
		Line(new Vector2(x1, y1), new Vector2(x2, y2));
	}

	public void Line(Vector2 startPos, Vector2 endPos, float width = 1)
	{
		if (!init)
			Init();

		//Check for line from list, re-use or create new.
		var newObject = new GameObject("Line");
		newObject.transform.parent = holder;
		var newLineRenderer = newObject.AddComponent<LineRenderer>();

		//Apply settings
		newLineRenderer.SetPosition(0, startPos);
		newLineRenderer.SetPosition(1, endPos);

		newLineRenderer.startWidth = width * 0.1f;
		newLineRenderer.endWidth = width * 0.1f;

		newLineRenderer.material = material;
		newLineRenderer.startColor = color;
		newLineRenderer.endColor = color;

		//Increment to next line in list
		currentLine++;
	}

	private void LateUpdate()
	{
		currentLine = 0;
	}

	public void Rect(float x1, float y1, float x2, float y2)
	{
		if (!init)
			Init();

		var newObject = new GameObject("Rect");
		newObject.transform.parent = holder;
		newObject.transform.position = new Vector3((x1 + x2) / 2, (y1 + y2) / 2, 0);
		var newSpriteRenderer = newObject.AddComponent<SpriteRenderer>();
		newSpriteRenderer.sprite = squareTexture;

		float width = Mathf.Abs(x1 - x2);
		float height = Mathf.Abs(y1 - y2);

		newObject.transform.localScale = new Vector3(width, height, 1);
	}
}
