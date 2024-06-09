using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLineManager : MonoBehaviour
{
    public static AimLineManager Instance { get; private set; }

    [SerializeField]
    private GameObject aimLinePrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one AimLineManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public AimLine CreateAimLine(Transform originTransform, Vector3 lineDirection)
    {
        AimLine aimLine = Instantiate(aimLinePrefab, originTransform).GetComponent<AimLine>();
        aimLine.SetupLine(originTransform, lineDirection);
        return aimLine;
    }
}
