using System;
using UnityEngine;

public class FallingShelf : RewindableMovement, IReactable, IFireStarter
{
    [SerializeField]
    private bool naturuallyAflame;
    private bool isAflame;
    private bool bFalling = false;
    private bool bFallen = false;
    private int RestingHash;
    private int FallingHash;

    [SerializeField]
    private float onFallTime;
    private float fallTimer = 0f;

    private Animator shelfAnimator;

    [SerializeField]
    private WeakPoint shelfSupport;

    [SerializeField]
    private GameObject[] shelfDamagers;

    public event EventHandler OnFireStarted;

    private void Awake()
    {
        shelfAnimator = GetComponent<Animator>();
        RestingHash = Animator.StringToHash("Rest");
        FallingHash = Animator.StringToHash("Fall");

        if (naturuallyAflame)
        {
            isAflame = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        shelfSupport.OnHit += StartFalling;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        shelfSupport.OnHit -= StartFalling;
    }

    private void Update()
    {
        if (bFalling)
        {
            fallTimer += GetSpeed() * Time.deltaTime;
            //Debug.Log("Addition = " + GetSpeed() * Time.deltaTime);
            //Debug.Log(fallTimer);

            if (!bFallen && (fallTimer >= onFallTime))
            {
                //Debug.Log("Onfallen");
                if (isAflame)
                {
                    OnFireStarted?.Invoke(this, null);
                }

                bFallen = true;
            }
            else if (bFallen && (fallTimer < onFallTime))
            {
                bFallen = false;
            }
        }
    }

    private void StartFalling(object sender, EventArgs e)
    {
        GameObject weakPoint = (sender as WeakPoint).gameObject;

        Collider weakPointCollider = weakPoint.GetComponent<Collider>();
        weakPointCollider.enabled = false;
        DissolveController weakPointDissolve = weakPoint.GetComponent<DissolveController>();
        weakPointDissolve.StartDissolve();
        FocusHighlight weakPointHighlight = weakPoint.GetComponent<FocusHighlight>();
        weakPointHighlight.ToggleHighlight(false);
        weakPointHighlight.enabled = false;

        StartReaction.ReactionStarted(this);

        bFalling = true;
        bFallen = false;
        fallTimer = 0f;

        ToggleMovement(true);

        shelfAnimator.CrossFade(FallingHash, 0f, 0);
        foreach (GameObject damager in shelfDamagers)
        {
            damager.SetActive(true);
        }
    }

    public void UndoReaction()
    {
        //stop falling

        shelfSupport.GetComponent<Collider>().enabled = true;
        shelfSupport.GetComponent<FocusHighlight>().enabled = true;
        shelfSupport.GetComponent<DissolveController>().StopDissolve();

        bFalling = false;
        fallTimer = 0f;

        ToggleMovement(false);

        shelfAnimator.CrossFade(RestingHash, 0f, 0);
        foreach (GameObject damager in shelfDamagers)
        {
            damager.SetActive(false);
        }
    }

    public bool GetIsAflame()
    {
        return isAflame;
    }

    public void SetIsAflame(bool aflame)
    {
        if (naturuallyAflame)
        {
            return;
        }

        isAflame = aflame;
    }
}
