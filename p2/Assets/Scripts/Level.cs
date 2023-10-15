using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour, IPointerClickHandler
{
    public int LevelIdx;
    public bool disabled = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!disabled)
            EventBus.Publish<ClickLevelEvent>(new ClickLevelEvent(LevelIdx));
    }
}

public class ClickLevelEvent
{
    public int levelIdx;
    public ClickLevelEvent(int _levelIdx) { levelIdx = _levelIdx; }
}
