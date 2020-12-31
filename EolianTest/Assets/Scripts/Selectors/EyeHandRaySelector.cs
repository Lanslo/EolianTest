using UnityEngine;
using UnityEngine.XR;


namespace Vodgets
{
    public class EyeHandRaySelector : Selector
    {

        // The cursor_ball_obj, local to each hand, that marks the world point where the ray hits.
        public Transform cursor_ball_obj = null;

        // Declare any class variables here. 
        // Note: You will want to save the raylength at the moment focus is grabbed.

        float Dhit = 0;
        float Dhand = 0;
        RaycastHit targetHit;

        [SerializeField]
        GameObject REye, LEye;

        GameObject MainEye;

        private Vodget DoRaycast()
        {
            Vodget hitVodget = null;

            LEye.transform.LookAt(this.transform.position);

            if (Physics.Raycast(LEye.transform.position, LEye.transform.forward, out targetHit))
            {
                hitVodget = targetHit.collider.gameObject.GetComponent<Vodget>();
            }

            return hitVodget;
        }

        protected override void SetCursor()
        {
            if (focus_grabbed)
            {
                float curHandEye = Vector3.Distance(LEye.transform.position, this.transform.position);

                float ratio = curHandEye / Dhand;

                LEye.transform.LookAt(this.transform.position);
                cursor.localPosition = LEye.transform.forward * (ratio * Dhit) + (LEye.transform.position - Vector3.zero);

                cursor.localRotation = this.transform.rotation;
                cursor.localScale = Vector3.one;
            }
            else if (focusVodget != null)
            {
                Cursor.Set(targetHit.point, transform.rotation, Vector3.one);
            }
            else
            {
                Cursor.Set(transform.localPosition, transform.rotation, Vector3.one);
            }

            cursor_ball_obj.position = Cursor.localPosition;

        }

        // With this selector the base class implementation of GrabFocus needs to be overridden in order to save
        // the eye-hand and eye to hitPoint distances at that moment in time.
        public override void GrabFocus(bool val)
        {
            base.GrabFocus(val);
            if (val == true)
            {

                Dhand = Vector3.Distance(LEye.transform.position, this.transform.position);
                Dhit  = Vector3.Distance(LEye.transform.position, targetHit.point);

                // DO some math here.... 
                Vector3 reconstructedHitPoint = (((LEye.transform.position - this.transform.position).normalized) * Dhit) + (LEye.transform.position);

                // Debug.Log("hitPoint" + targetHit.point);
                // Debug.Log("reconstructed" + reconstructedHitPoint);                   

            }
        }

        private void Update()
        {
            SetCursor();

            // Stop looking for vodgets in the scene when your selectors focus is grabbed.
            if (!focus_grabbed)
            {

                // Use a physics raycast to find vodgets in the scene.
                Vodget hitVodget = DoRaycast();

                // If a vodgets is found, give it focus and begin forwarding any events while it still has focus.
                if (hitVodget != focusVodget)
                {

                    // Set your Cursor to the hit point
                    //SetCursor();

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

                focusVodget.FocusUpdate(this);

            }
        }
    }
}