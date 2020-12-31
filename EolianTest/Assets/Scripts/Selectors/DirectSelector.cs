using UnityEngine;

namespace Vodgets
{
    // The DirectSelector selects Vodgets through direct collision with a moving hand or hand held stylus.
    public class DirectSelector : Selector
    {

        [SerializeField]
        Transform JackCursor;

        Vodget HitVodget = null;

        // CommonUsage(primary2DAxisClick)

        private void OnTriggerEnter(Collider collision)
        {

            if (focus_grabbed)
                return;

            if (collision.gameObject.GetComponent<Vodget>() == null)
            {
                return;
            }
            else
            {
                HitVodget = collision.gameObject.GetComponent<Vodget>();
            }



            // A collider was hit. Look for a vodget component on the collision.gameObject!


            // If no vodget component is found on the collision.gameObject... return.
            // If focus_grabbed (see Selector base class) is true... return.

            // Otherwise, focusVodget needs to change.
            // Let the previous focusVodget know that it has lost focus here (if not null)
            // Change focusVodget and let the new focusVodget know that it now has selector focus.

            // Note: When focus is grabbed the collider can still enter and exit other vodgets.
            // While not a requirement, an advanced implementation would keep track of the vodgets
            // that are currently entered and exited to potentially choose a new client when 
            // focus grab is released.

        }

        private void OnTriggerExit(Collider collision)
        {

            if (collision.gameObject.GetComponent<Vodget>() == null)
            {
                return;
            }
            else
            {
                HitVodget = null;
            }

            if (focus_grabbed)
            {
                this.SendButtonEvent(Selector.ButtonType.Trigger, false);

                if (focusVodget != null)
                {
                    focusVodget.Focus(this, false);
                    focusVodget = null;
                    HitVodget = null;
                }

                return;

            }

            // If exiting the focusVodget (if not null), notify the focusVodget that it has lost focus
            // Clear the focusVodget.

        }

        protected override void SetCursor()
        {

            cursor.localPosition = JackCursor.position;
            cursor.localRotation = JackCursor.rotation;
            cursor.localScale = Vector3.one;

            // With the fixed selector the world cursor is always the hand position and rotation.
            // Set the cursor ( see Vodget base class ) to the controller position and rotation.
            // Note: The center of the controller can sometimes be awkward and you might want to add 
            // a child 3D cursor gameObject that moves with the hand at an offset. 
            // In this case you would set cursor to the world location of the offset gameObject.

        }

        private void Update()
        {
            // A selector is responsible for giving any vodgets with "focus" a heartbeat by calling FocusUpdate.
            if (HitVodget != focusVodget)
            {
                SetCursor();

                if (focusVodget != null)
                {
                    focusVodget.Focus(this, false);
                }

                focusVodget = HitVodget;

                if (focusVodget != null)
                {
                    focusVodget.Focus(this, true);
                }

            }

            if (focusVodget != null)
            {
                SetCursor();
                focusVodget.FocusUpdate(this);

            }
        }
    }
}