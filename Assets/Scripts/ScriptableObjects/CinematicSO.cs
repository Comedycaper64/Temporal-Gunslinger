using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CinematicSO", menuName = "Temporal Gunslinger/CinematicSO", order = 0)]
public class CinematicSO : ScriptableObject
{
    [SerializeField]
    private CinematicNode[] cinematicNodes;

    public CinematicNode[] GetCinematicNodes()
    {
        return cinematicNodes;
    }
}
