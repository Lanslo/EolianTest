using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    public abstract class Selector : MonoBehaviour
    {
        public enum ButtonType { Trigger, Grip, Touchpad }

        protected Vodget focusVodget = null;

        protected abstract void SetCursor();

        // Optional event that can be used by non vodget components like WorldGrabber.
        public event ButtonEventHandler ButtonEvt;
        public delegate void ButtonEventHandler(ButtonType button, bool state);

        // This method is provided to allow buttons to be sent from outside components (See: Controller)
        public void SendButtonEvent(ButtonType button, bool state)
        {
            // Automatically forward all button events to the current focus vodget if not null.
            // Note: This was originally a virtual abstract method but when all selectors 
            // implemented it exactly the same way it was migrated up to a non-virtual parent class method.
            if (focusVodget != null)
            {
                SetCursor();
                focusVodget.Button(this, button, state);
            }

            // Send all button events to any other remote clients that are not Vodgets (See: WorldGrabber).
            if ( ButtonEvt != null)
                ButtonEvt.Invoke(button, state);
        }

        // When a selectors focus is grabbed(true) by a Vodget client (focusVodget), the Selector cannot focus
        // on another vodget client until GrabFocus(false) is called by focusVodget releasing the grab.
        protected bool focus_grabbed = false;
        public virtual void GrabFocus(bool val)
        {
            focus_grabbed = val;
        }

        // Inheriting selectors must all maintain a model of a 3D cursor in world coordinates. 
        protected Srt cursor = new Srt();
        public Srt Cursor
        {
            get { return cursor; }
        }
    }
}