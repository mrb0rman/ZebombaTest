using UnityEngine;

namespace ZebombaTest.Scripts
{
    public class CameraView : MonoBehaviour
    {
        public Camera Camera => camera;
        [SerializeField] private Camera camera;
    }
}

