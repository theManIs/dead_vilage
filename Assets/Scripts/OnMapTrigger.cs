using System.Collections;
using System.Collections.Generic;
using Assets.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter;
using UnityEngine;

public class OnMapTrigger : MonoBehaviour
{
//    public SceneField SceneToLoad = new SceneField();
    public SceneIndexerEnum BuildIndex = SceneIndexerEnum.None;

    private void OnTriggerEnter(Collider otherCollider)
    {
        StaticProxyEvent.EMatchHasComeToAnEnd.Invoke(new EventMatchHasComeToAnEndParams()
        {
            BuildIndex = BuildIndex
        });
    }
}
