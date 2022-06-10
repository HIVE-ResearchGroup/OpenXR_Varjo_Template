using UnityEngine;

namespace Core.Interfaces
{
    /**
     * This abstract class should be used to implement future device features
     */
    public abstract class AbstractDeviceFeature : MonoBehaviour
    {
        /**
         * All the necessary variables which need the be initialized should be done here
         */
        public abstract void XRStart();

        /**
         * Since your variables are changed in the SetModeVariables method, you should check for a state change here
         */
        public abstract void XRUpdate();

        /**
         * This function will be called when theres an environment switch between AR and VR
         * You probably want to change your variables regarding which mode is called here
         */
        public abstract void SetModeVariables();
    }
}