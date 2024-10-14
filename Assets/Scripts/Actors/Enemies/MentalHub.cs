using System;
using UnityEngine;

public class MentalHub : MonoBehaviour
{
    [SerializeField]
    private GameObject linkTetherPrefab;

    [SerializeField]
    private EnemyHuskStateMachine[] linkedEnemies;

    private void Awake()
    {
        SetupLinks(true);

        foreach (EnemyHuskStateMachine enemy in linkedEnemies)
        {
            MentalLinkTether linkTether = Instantiate(linkTetherPrefab, transform)
                .GetComponent<MentalLinkTether>();
            linkTether.SetTetherPoint(transform.position, enemy.GetMentalLink().transform.position);
        }
    }

    private void SetupLinks(bool toggle)
    {
        if (toggle)
        {
            foreach (EnemyHuskStateMachine enemy in linkedEnemies)
            {
                enemy.GetMentalLink().OnLinkSevered += TransmitFeedback;
            }
        }
        else
        {
            foreach (EnemyHuskStateMachine enemy in linkedEnemies)
            {
                enemy.GetMentalLink().OnLinkSevered -= TransmitFeedback;
            }
        }
    }

    private void TransmitFeedback(object sender, EventArgs e)
    {
        MentalLink destroyedLink = sender as MentalLink;

        foreach (EnemyHuskStateMachine enemy in linkedEnemies)
        {
            MentalLink link = enemy.GetMentalLink();
            if (link == destroyedLink)
            {
                continue;
            }

            link.LinkFeedback();
        }

        SetupLinks(false);

        // Rewind Event for undoing
    }
}
