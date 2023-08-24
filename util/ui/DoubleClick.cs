using UnityEngine;
using UnityEngine.EventSystems;

namespace animals.scripts.contentDB
{
    public class DoubleClick:MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            
            if (eventData.clickCount == 2) {
               TapLoginManager.instance.Share();
            }

        }
    }
}