using UnityEngine;

namespace Vodgets
{
    public class RaySelector : Selector
    {
        // The cursor_ball_obj, local to each hand, that marks the world point where the ray hits.
        public Transform cursor_ball_obj = null;

        // Declare any class variables here. 
        // Note: You will want to save the raylength at the moment focus is grabbed.

        float length = 0;
        RaycastHit targetHit;

        private void Start()
        {
            // Depending on which VR/AR device you are using, find a means to get trigger button events.
            // See Unity OpenVR and XR documentation for examples.
            // You should consider abstracting a controller class that could provide buttons and tracking for any Selector.
            // Hint: The public Selector.SendButtonEvent(...) and Selector.ButtonEvt were added to allow this.

        }

        private Vodget DoRaycast()
        {
           Vodget hitVodget = null;

            // Do a Physics.Raycast starting at the hand_pos in the direction of hand.forward (this.transform.position and this.transform.forward)
            if (Physics.Raycast(this.transform.position, this.transform.forward, out targetHit, 10.0f))
            {
                hitVodget = targetHit.collider.gameObject.GetComponent<Vodget>();

                if (hitVodget != null)
                {
                    // cursor_ball_obj = targetHit.collider.gameObject.transform;
                    cursor_ball_obj.position = targetHit.point;
                }
                else
                {
                    cursor_ball_obj.localPosition = Vector3.zero;
                }
            }
            else
            {
                cursor_ball_obj.localPosition = Vector3.zero;
            }

            return hitVodget;
        }

        protected override void SetCursor()
        {
            // Set the Cursor to the cursor_ball_obj world position.

            cursor.localPosition = cursor_ball_obj.position;
            cursor.localRotation = transform.rotation;
            cursor.localScale = Vector3.one;

            // Note: This will either be the hand position or the world point where focusVodget was last hit by the ray.
            // It is also interesting to note that, because the cursor_ball_obj is a child of the hand, the world cursor position 
            // will move at a fixed distance from the hand along the local Z axis whenever focus is grabbed and ray casting is temporarily inactive.
        }

        //public override void GrabFocus(bool val)
        //{
        //    base.GrabFocus(val);
        //    if (val == true)
        //    {
        //        // Note: Most vodgets will grab the selector focus when buttons go down (true)
        //        // Vodgets typical grab focus to guarantee they get the button up (false) event.
        //        // Focus has just been grabbed!

        //        // The initial hit point was found by casting a vector from the hand along the hands forward direction. 
        //        // When focus is grabbed save the length of the ray in raylength.
           
        //        // You can do this by transforming the cursor into the local frame and taking the z (forward) value.
        //        // You could also just get the distance between the hit and current hand points. 

        //    }
        //}

        private void Update()
        {
            // Stop looking for vodgets in the scene when your selectors focus is grabbed.
            if (!focus_grabbed)
            {

                // Use a physics raycast to find vodgets in the scene.
                Vodget hitVodget = DoRaycast();

                // If a vodgets is found, give it focus and begin forwarding any events while it still has focus.
                if (hitVodget != focusVodget)
                {

                    // Set your Cursor to the hit point 

                    SetCursor();  // Should be good.

                    // Let the previous focusVodgetCurr know that it has lost focus here.

                    if (focusVodget != null)
                    {
                        focusVodget.Focus(this, false);
                    }

                    // Let the focusVodgetCurr know that it now has selector focus here.

                    focusVodget = hitVodget;

                    if (focusVodget != null)
                    {
                        focusVodget.Focus(this, true);
                    }
                }
            }

            // A selector is responsible for giving any vodgets with "focus" a heartbeat by calling FocusUpdate.
            if (focusVodget != null)
            {
                // Set the cursor and call FocusUpdate on focusVodget.

                SetCursor();
                focusVodget.FocusUpdate(this);

            }
        }
    }
}