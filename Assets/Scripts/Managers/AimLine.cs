using UnityEngine;

public class AimLine : MonoBehaviour
{
    private int randomInterval = 0;
    private int interval = 2;
    private float lineRange = 100f;
    private float sphereCastRadius = 0.1f;
    private float hitVisualHoverDistance = 0.025f;
    private bool bShowLine = false;
    private Vector3 linePosSave;

    //private bool bShowFocusLine = false;
    private Transform lineOrigin;
    private Vector3 lineDirection;
    private Transform currentTarget;

    [SerializeField]
    private Sprite hitSprite;

    [SerializeField]
    private Color hitColour;

    [SerializeField]
    private Sprite killSprite;

    [SerializeField]
    private Color killColour;

    [SerializeField]
    private SpriteRenderer hitVisual;

    [SerializeField]
    private LayerMask hitLayerMask;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private LineRenderer highlightLineRenderer;

    private void Awake()
    {
        ToggleHitVisualVisibility(false);
        randomInterval = Random.Range(0, interval + 1);
    }

    // private void OnEnable()
    // {
    //     FocusManager.OnFocusToggle += ToggleFocusLine;
    // }

    // private void OnDisable()
    // {
    //     FocusManager.OnFocusToggle -= ToggleFocusLine;
    // }

    public void SetupLine(Transform lineOrigin, Vector3 lineDirection, float sphereCastRadius)
    {
        this.lineOrigin = lineOrigin;
        this.lineDirection = lineDirection;
        this.sphereCastRadius = sphereCastRadius;
    }

    private void LateUpdate()
    {
        if (!lineOrigin || !bShowLine)
        {
            return;
        }

        // if (bShowFocusLine)
        // {
        //     DrawFocusLine();
        // }
        // else
        // {
        if ((Time.frameCount + randomInterval) % interval == 0)
        {
            DrawLine();
        }
        else
        {
            Vector3 originPosition = lineOrigin.position;
            Vector3[] positionArray = new Vector3[2] { originPosition, linePosSave };

            lineRenderer.SetPositions(positionArray);
            highlightLineRenderer.SetPositions(positionArray);
        }
        //}
    }

    private void DrawLine()
    {
        // Vector3 originPosition = lineOrigin.position;
        // Vector3[] positionArray = new Vector3[2] { originPosition, lineDirection * lineRange };

        // ToggleHitVisualVisibility(false);
        // lineRenderer.SetPositions(positionArray);

        Vector3 originPosition = lineOrigin.position;
        RaycastHit hit;
        Vector3[] positionArray = new Vector3[2] { originPosition, Vector3.zero };

        //Physics.Raycast(originPosition, lineDirection, out hit, lineRange, hitLayerMask)
        if (
            Physics.SphereCast(
                originPosition,
                sphereCastRadius,
                lineDirection,
                out hit,
                lineRange,
                hitLayerMask
            )
        )
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
        linePosSave = positionArray[1];
        lineRenderer.SetPositions(positionArray);
        highlightLineRenderer.SetPositions(positionArray);
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
            hitVisual.color = killColour;
        }
        else
        {
            hitVisual.sprite = hitSprite;
            hitVisual.color = hitColour;
        }
    }

    public void ToggleLine(bool toggle)
    {
        bShowLine = toggle;
        lineRenderer.enabled = toggle;
        highlightLineRenderer.enabled = toggle;
        ToggleHitVisualVisibility(false);
    }

    // private void ToggleFocusLine(object sender, bool toggle)
    // {
    //     if (bShowLine)
    //     {
    //         bShowFocusLine = toggle;
    //     }
    // }

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
