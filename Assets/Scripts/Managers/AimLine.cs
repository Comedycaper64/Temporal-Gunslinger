using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    private float lineRange = 100f;
    private float hitVisualHoverDistance = 0.01f;
    private bool bShowLine = false;
    private Transform lineOrigin;
    private Vector3 lineDirection;
    private Transform currentTarget;

    [SerializeField]
    private Sprite hitSprite;

    [SerializeField]
    private Sprite killSprite;

    [SerializeField]
    private SpriteRenderer hitVisual;

    [SerializeField]
    private LayerMask hitLayerMask;

    [SerializeField]
    private LineRenderer lineRenderer;

    private void Awake()
    {
        ToggleHitVisualVisibility(false);
    }

    public void SetupLine(Transform lineOrigin, Vector3 lineDirection)
    {
        this.lineOrigin = lineOrigin;
        this.lineDirection = lineDirection;
    }

    private void LateUpdate()
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
            hitVisual.transform.position = hit.point + (hit.normal * hitVisualHoverDistance);
            hitVisual.transform.rotation = Quaternion.LookRotation(-hit.normal);
            positionArray[1] = hit.point;
            if (currentTarget != hit.transform)
            {
                currentTarget = hit.transform;
                ShowDeathHit(currentTarget.GetComponent<WeakPoint>());
            }
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

    private void ShowDeathHit(bool toggle)
    {
        if (toggle)
        {
            hitVisual.sprite = killSprite;
        }
        else
        {
            hitVisual.sprite = hitSprite;
        }
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
