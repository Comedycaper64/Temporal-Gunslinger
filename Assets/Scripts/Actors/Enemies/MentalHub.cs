using System;
using System.Collections.Generic;
using UnityEngine;

public class MentalHub : MonoBehaviour, IReactable
{
    private struct LinkPosition
    {
        public LinkPosition(MentalLinkTether tether, Vector3 tetherEnd)
        {
            this.tether = tether;
            tetherEndPoint = tetherEnd;
        }

        public MentalLinkTether tether;
        public Vector3 tetherEndPoint;
    }

    [SerializeField]
    private GameObject linkTetherPrefab;

    [SerializeField]
    private EnemyHuskStateMachine[] linkedEnemies;

    [SerializeField]
    private List<LinkPosition> linkTethers = new List<LinkPosition>();

    private void Awake()
    {
        foreach (EnemyHuskStateMachine enemy in linkedEnemies)
        {
            MentalLinkTether linkTether = Instantiate(linkTetherPrefab, transform)
                .GetComponent<MentalLinkTether>();

            LinkPosition linkPosition = new LinkPosition(
                linkTether,
                enemy.GetMentalLink().transform.position
            );
            linkTethers.Add(linkPosition);

            //linkTether.SetTetherPoint(transform.position, enemy.GetMentalLink().transform.position);
        }

        SetupLinks(true);
    }

    private void SetupLinks(bool toggle)
    {
        if (toggle)
        {
            for (int i = 0; i < linkedEnemies.Length; i++)
            {
                linkedEnemies[i].GetMentalLink().OnLinkSevered += TransmitFeedback;
                linkTethers[i].tether.SetTetherPoint(
                    transform.position,
                    linkTethers[i].tetherEndPoint
                );
            }
        }
        else
        {
            for (int i = 0; i < linkedEnemies.Length; i++)
            {
                linkedEnemies[i].GetMentalLink().OnLinkSevered -= TransmitFeedback;
                linkTethers[i].tether.SeverTetherPoint();
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

        StartReaction.ReactionStarted(this);
    }

    public void UndoReaction()
    {
        SetupLinks(true);
    }
}
