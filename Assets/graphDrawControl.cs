using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List<>

/*
 * v0.1 2015/09/06
 *   - draw line over Panel
 */ 

public class graphDrawControl : MonoBehaviour {

	private GameObject lineGroup; // for grouping
	List<Vector2> my2DPoint;
	public GameObject panel;
	public Canvas myCanvas; // to obtain canvas.scale
	
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

	void drawBottomLeftToTopRight(){
		Vector2 pointPos;

		RectTransform panelRect = panel.GetComponent<RectTransform> ();
		float width = panelRect.rect.width;
		float height = panelRect.rect.height;
		
		RectTransform canvasRect = myCanvas.GetComponent<RectTransform> ();

		// Bottom Left
		pointPos = panel.transform.position;
		pointPos.x -= width * 0.5f * canvasRect.localScale.x;
		pointPos.y -= height * 0.5f * canvasRect.localScale.y;
		my2DPoint.Add (pointPos);

		// Top Right
		pointPos = panel.transform.position;
		pointPos.x += width * 0.5f * canvasRect.localScale.x;
		pointPos.y += height * 0.5f * canvasRect.localScale.y;
		my2DPoint.Add (pointPos);
		
		drawGraph ();
	}

	void Start () {
		my2DPoint = new List<Vector2> ();

		drawBottomLeftToTopRight ();
	}
}
