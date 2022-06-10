using UnityEngine;

namespace Core.Interfaces
{
    /**
     * This abstract class should be used to implement future device features
     */
    public abstract class AbstractDeviceFeature : MonoBehaviour
    {
        public abstract void XRStart();

        public abstract void XRUpdate();

        public abstract void SetModeVariables();
    }
}