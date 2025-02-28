using UnityEngine;

public class DeathDeadzone : RewindableMovement
{
    private float timer = 0f;
    private float deadzoneTargetScale = 2.5f;

    [SerializeField]
    private Transform deadzoneVisual;

    [SerializeField]
    private Transform deadzoneCollider;

    private void Start()
    {
        deadzoneVisual.localScale = Vector3.zero;
        deadzoneCollider.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (IsActive())
        {
            timer += Time.deltaTime * GetSpeed();

            //Debug.Log(timer);

            float scale = Mathf.Lerp(0f, deadzoneTargetScale, timer);

            Vector3 scaleVector = new Vector3(scale, scale, scale);

            deadzoneVisual.localScale = scaleVector;
            deadzoneCollider.localScale = scaleVector;
        }
    }

    public override void ToggleMovement(bool toggle)
    {
        base.ToggleMovement(toggle);
        timer = 0f;
    }

    public void ResetZone()
    {
        timer = 0f;
        Vector3 scaleVector = new Vector3(0f, 0f, 0f);

        deadzoneVisual.localScale = scaleVector;
        deadzoneCollider.localScale = scaleVector;
        ToggleMovement(false);
    }
}
