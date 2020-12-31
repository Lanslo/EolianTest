using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    /* 
     * Different devices control head-hand position and rotation differently.
     * They also don't always access buttons and joysticks in the same manner.
     * 
     * Creating a controller for different devices allows you to map each
     * manufacturers methods to the standard buttons defined and used in your framework.
     */
    public abstract class Controller : MonoBehaviour
    {
        protected Selector selector = null;
        protected virtual void Awake()
        {
            selector = gameObject.GetComponent<Selector>();
        }

        protected virtual void Update()
        {
            // Get your devices 
            //eg. this.transform.localPosition = pos;
            //eg. this.transform.localRotation = rot;

            // Send button down (true) and up (false) events to your client selector when they occur.
            //eg. selector.SendButtonEvent(Selector.ButtonType.Trigger, true);
            //eg. selector.SendButtonEvent(Selector.ButtonType.Grip, false);
        }

    }
}
