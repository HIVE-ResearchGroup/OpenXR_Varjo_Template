using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VarjoExample
{
    public class Hand : MonoBehaviour
    {
        public Transform xrRig;

        Controller controller;

        public List<Interactable> contactedInteractables = new List<Interactable>();
        private bool triggerDown;
        private FixedJoint fixedJoint = null;
        private Interactable currentInteractable;
        private Rigidbody heldObjectBody;

        void Awake()
        {
            controller = GetComponent<Controller>();
            fixedJoint = GetComponent<FixedJoint>();
        }

        // Update is called once per frame
        void Update()
        {
            if (controller.triggerButton)
            {
                if (!triggerDown)
                {
                    triggerDown = true;
                    Pick();
                }
            }
            else if (!controller.primary2DAxisClick && triggerDown)
            {
                triggerDown = false;
                Drop();
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Pickable") || other.gameObject.CompareTag("Fracture"))
            {
                Debug.Log("Touched and added interactable on " + other.gameObject.name);
                contactedInteractables.Add(other.gameObject.GetComponent<Interactable>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Pickable") || other.gameObject.CompareTag("Fracture"))
            {
                Debug.Log("Let go and removed interactable on " + other.gameObject.name);

                contactedInteractables.Remove(other.gameObject.GetComponent<Interactable>());
            }
        }

        public void Pick()
        {
            currentInteractable = GetNearestInteractable();


            if (!currentInteractable)
            {
                return;
            }

            // Axel: Commeted this code because it disabled the functionality - not quite sure why to have this one anyway 
            /*Drop interactable if already held
            if (currentInteractable.activeHand)
            {
                currentInteractable.activeHand.Drop();
            }*/

            // Axel: In order to have less calls to fixedJoint, check if already has a rigidbody
            if (!fixedJoint.connectedBody)
            {
                // Attach
                heldObjectBody = currentInteractable.GetComponent<Rigidbody>();
                fixedJoint.connectedBody = heldObjectBody;
            }


            // Axel: Same as active hand code above
            // Set active hand
            //currentInteractable.activeHand = this;
        }

        public void Drop()
        {
            if (!currentInteractable)
                return;

            // Detach
            fixedJoint.connectedBody = null;

            // Apply velocity
            heldObjectBody = currentInteractable.GetComponent<Rigidbody>();
            heldObjectBody.velocity = xrRig.TransformVector(controller.DeviceVelocity);
            heldObjectBody.angularVelocity = xrRig.TransformDirection(controller.DeviceAngularVelocity);

            // Axel: Same as active hand code above
            // Clear
            //currentInteractable.activeHand = null;
            //currentInteractable = null;
        }

        private Interactable GetNearestInteractable()
        {
            Interactable nearest = null;
            float minDistance = float.MaxValue;
            float distance = 0.0f;

            foreach (Interactable interactable in contactedInteractables)
            {
                if (interactable)
                {
                    distance = (interactable.transform.position - transform.position).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = interactable;
                    }
                }
            }
            return nearest;
        }
    }
}