using UnityEngine;

namespace Core.Interfaces
{
    public abstract class AbstractDeviceFeature : MonoBehaviour
    {
        public abstract void XRStart();

        public abstract void XRUpdate();

        public abstract void SetModeVariables();
    }
}