using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    private float lineRange = 100f;
    private bool bShowLine = false;
    private Transform lineOrigin;
    private Vector3 lineDirection;

    [SerializeField]
    private Transform hitVisual;

    [SerializeField]
    private LayerMask hitLayerMask;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ToggleHitVisualVisibility(false);
    }

    public void SetupLine(Transform lineOrigin, Vector3 lineDirection)
    {
        this.lineOrigin = lineOrigin;
        this.lineDirection = lineDirection;
    }

    private void Update()
    {
        if (!lineOrigin || !bShowLine)
        {
            return;
        }

        DrawLine();
    }

    private void DrawLine()
    {
        Vector3 originPosition = lineOrigin.position;
        RaycastHit hit;
        Vector3[] positionArray = new Vector3[2] { originPosition, Vector3.zero };
        if (Physics.Raycast(originPosition, lineDirection, out hit, lineRange, hitLayerMask))
        {
            ToggleHitVisualVisibility(true);
            hitVisual.position = hit.point;
            positionArray[1] = hit.point;
        }
        else
        {
            ToggleHitVisualVisibility(false);
            positionArray[1] = lineDirection * lineRange;
        }
        lineRenderer.SetPositions(positionArray);
    }

    private void ToggleHitVisualVisibility(bool toggle)
    {
        hitVisual.gameObject.SetActive(toggle);
    }

    public void ToggleLine(bool toggle)
    {
        bShowLine = toggle;
        lineRenderer.enabled = toggle;
        ToggleHitVisualVisibility(false);
    }

    public void UpdateLineDirection(Vector3 direction)
    {
        lineDirection = direction;
    }

    public Vector3 GetLineDirection()
    {
        if (lineRenderer.positionCount <= 0)
        {
            return Vector3.forward;
        }

        Vector3[] rendererPositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(rendererPositions);
        return (rendererPositions[1] - rendererPositions[0]).normalized;
    }
}
