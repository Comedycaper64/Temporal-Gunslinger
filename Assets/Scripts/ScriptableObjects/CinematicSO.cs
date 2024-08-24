using UnityEngine;

[CreateAssetMenu(fileName = "CinematicSO", menuName = "Cinematic Node/CinematicSO", order = 0)]
public class CinematicSO : ScriptableObject
{
    [SerializeField]
    private CinematicNode[] cinematicNodes;

    public CinematicNode[] GetCinematicNodes()
    {
        return cinematicNodes;
    }
}
