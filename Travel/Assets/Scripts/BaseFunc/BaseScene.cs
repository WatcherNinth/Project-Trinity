using UnityEngine;
using System.Collections;

namespace Lucky
{ 
    public class BaseScene : MonoBehaviour {

	    public void SetVisable(bool visiable)
        {
            gameObject.SetActive(visiable);
        }

        public bool GetIsVisible()
        {
            return gameObject.activeSelf;
        }

        public void Dispose()
        {

        }
    }
}
