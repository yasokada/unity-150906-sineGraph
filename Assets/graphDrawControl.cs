using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List<>

public class graphDrawControl : MonoBehaviour {

	private GameObject lineGroup; // for grouping
	List<Vector2> my2DPoint;
	public GameObject panel;
	float accTime = 0.0f;
	
	void DrawLine(List<Vector2> my2DVec, int startPos) {
		List<Vector3> myPoint = new List<Vector3>();
		for(int idx=0; idx<2; idx++) {
			myPoint.Add(new Vector3(my2DVec[startPos+idx].x, my2DVec[startPos+idx].y, 0.0f));
		}
		
		GameObject newLine = new GameObject ("Line" + startPos.ToString() );
		LineRenderer lRend = newLine.AddComponent<LineRenderer> ();
//		lRend.useWorldSpace = true; // test
		lRend.SetVertexCount(2);
		lRend.SetWidth (0.05f, 0.05f);
		Vector3 startVec = myPoint[0];
		Vector3 endVec   = myPoint[1];
		lRend.SetPosition (0, startVec);
		lRend.SetPosition (1, endVec);
		
		newLine.transform.parent = lineGroup.transform; // for grouping
	}

	void drawGraph() {
		if (lineGroup != null) {
			Destroy (lineGroup.gameObject);
		}
		lineGroup = new GameObject ("LineGroup");

		for (int idx=0; idx < my2DPoint.Count - 1; idx++) {
			DrawLine (my2DPoint, /* startPos=*/idx);
		}

		lineGroup.transform.parent = panel.transform; // to belong to panel
	}

	public Canvas myCanvas;

	Vector2 getPanelPosition(Transform panel) {
		Vector2 offset;
		RectTransform rect = panel.GetComponent(typeof(RectTransform)) as RectTransform;

		offset.x = -rect.pivot.x * rect.rect.width;
		offset.y = -rect.pivot.y * rect.rect.height;

		return myCanvas.worldCamera.ScreenToWorldPoint (offset);
	}

	void Start () {
		Vector2 pointPos;
		my2DPoint = new List<Vector2> ();

		RectTransform canvasRect = myCanvas.GetComponent<RectTransform> ();
		RectTransform panelRect = panel.GetComponent<RectTransform> ();

//		Debug.Log ("panel transform:" + panel.transform.position.ToString ());
//		Debug.Log ("panel rect:" + panelRect.rect.width.ToString () + " x " 
//			+ panelRect.rect.height.ToString ());
//		Debug.Log ("canvas sizeDelta:" + canvasRect.sizeDelta.ToString ());
//		Debug.Log("panel transform localPosition:" + panel.transform.localPosition.ToString());

		Vector2 canSca = canvasRect.localScale;

		Debug.Log ("canvas scale:" + canvasRect.localScale.ToString ());

		float width = panelRect.rect.width;
		float height = panelRect.rect.height;

		Debug.Log ("pivot: " + panelRect.pivot.ToString ());

		pointPos = panel.transform.position;
		pointPos.x -= width * 0.5f * canvasRect.localScale.x;
		pointPos.y -= height * 0.5f * canvasRect.localScale.y;
//
//		Vector2 worldPos = myCanvas.worldCamera.ScreenToWorldPoint (pointPos);
//		Vector2 viewPos = myCanvas.worldCamera.ScreenToViewportPoint (pointPos);

		if (false) {
			pointPos = new Vector2 (-8.45f, -2.61f);
			pointPos.x += panel.transform.position.x;
			pointPos.y += panel.transform.position.y;
		}
		my2DPoint.Add (pointPos);

		pointPos = new Vector2 (8.45f, 2.61f);
		pointPos.x += panel.transform.position.x;
		pointPos.y += panel.transform.position.y;
		my2DPoint.Add (pointPos);

		drawGraph ();
	}
	
	void Update () {
		
	}

	void OnGUI() {
		accTime += Time.deltaTime;
		if (accTime < 1.0f) {
			return;
		}

		accTime = 0.0f;
//		drawGraph ();
	}
}
