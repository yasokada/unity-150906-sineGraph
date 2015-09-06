using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List<>

/*
 * v0.2 2015/09/06
 *   - add sine and cosine graph
 * v0.1 2015/09/06
 *   - draw line over Panel
 */ 

public class graphDrawControl : MonoBehaviour {

	private GameObject lineGroup; // for grouping
//	List<Vector2> my2DPoint;
	public GameObject sinPanel;
	public GameObject cosPanel;
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

	void drawGraph(List<Vector2> my2DVec, GameObject panel) {
		// TODO: prevent mutiple draw
//		if (lineGroup != null) {
//			Destroy (lineGroup.gameObject);
//		}
		lineGroup = new GameObject ("LineGroup");

		for (int idx=0; idx < my2DVec.Count - 1; idx++) {
			DrawLine (my2DVec, /* startPos=*/idx);
		}

		lineGroup.transform.parent = panel.transform; // to belong to panel
	}

	void addPointNormalized(List<Vector2> my2DVec, GameObject panel, Vector2 point)
	{
		// point: normalized point data [-1.0, 1.0] for each of x, y

		RectTransform panelRect = panel.GetComponent<RectTransform> ();
		float width = panelRect.rect.width;
		float height = panelRect.rect.height;
		
		RectTransform canvasRect = myCanvas.GetComponent<RectTransform> ();

		Vector2 pointPos;
		
		// Bottom Left
		pointPos = panel.transform.position;
		pointPos.x += point.x * width * 0.5f * canvasRect.localScale.x;
		pointPos.y += point.y * height * 0.5f * canvasRect.localScale.y;
		my2DVec.Add (pointPos);
	}
	
	void Test_drawBox(List<Vector2> my2DVec, GameObject panel)
	{
		addPointNormalized (my2DVec, panel, new Vector2 (-1.0f, -1.0f));
		addPointNormalized (my2DVec, panel, new Vector2 (-1.0f, 1.0f));
		addPointNormalized (my2DVec, panel, new Vector2 (1.0f, 1.0f));
		addPointNormalized (my2DVec, panel, new Vector2 (1.0f, -1.0f));
		addPointNormalized (my2DVec, panel, new Vector2 (-1.0f, -1.0f));

		drawGraph (my2DVec, panel);
	}

	void Test_sineGraph(List<Vector2> my2DVec, GameObject panel)
	{
		float arg = 0.0f; // deg
		float step = 0.5f; // deg
		float rad, xnorm;

		while (arg < 360.0f) {
			rad = arg * Mathf.Deg2Rad;
			xnorm = arg / 180.0f - 1.0f;
			addPointNormalized(my2DVec, panel, new Vector2(xnorm, Mathf.Sin(rad)));
			arg += step;
		}
		drawGraph (my2DVec, panel);
	}
	void Test_cosineGraph(List<Vector2> my2DVec, GameObject panel)
	{
		float arg = 0.0f; // deg
		float step = 0.5f; // deg
		float rad, xnorm;
		
		while (arg < 360.0f) {
			rad = arg * Mathf.Deg2Rad;
			xnorm = arg / 180.0f - 1.0f;
			addPointNormalized(my2DVec, panel, new Vector2(xnorm, Mathf.Cos(rad)));
			arg += step;
		}
		drawGraph (my2DVec, panel);
	}

	void Start () {
		List<Vector2> my2DPointSin = new List<Vector2> ();
		Test_drawBox (my2DPointSin, sinPanel);
		Test_sineGraph (my2DPointSin, sinPanel);

		List<Vector2> my2DPointCos = new List<Vector2> ();
		Test_drawBox (my2DPointCos, cosPanel);
		Test_cosineGraph (my2DPointCos, cosPanel);

		// 2nd draw
		my2DPointCos.Clear ();
		Test_drawBox (my2DPointCos, cosPanel);
		Test_cosineGraph (my2DPointCos, cosPanel);

		// TODO: remove lineGroup under Panel

	}
}
