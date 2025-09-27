using UnityEngine;
using UnityEngine.Serialization;
using ZebombaTest.Scripts;
using Zenject;

namespace ZebombaTest.Scripts
{
    public class ScreenBounds2D : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float thickness = 1f;
        [SerializeField] private float insetViewport;

        [Inject]
        private void Inject(CameraView cameraView)
        {
            camera = cameraView.Camera;
        }
    
        private void Start()
        {
            if (!camera) camera = Camera.main;

            var z = Mathf.Abs(camera.transform.position.z);

            var bl = camera.ViewportToWorldPoint(new Vector3(0f + insetViewport, 0f + insetViewport, z));
            var br = camera.ViewportToWorldPoint(new Vector3(1f - insetViewport, 0f + insetViewport, z));
            var tl = camera.ViewportToWorldPoint(new Vector3(0f + insetViewport, 1f - insetViewport, z));
            var tr = camera.ViewportToWorldPoint(new Vector3(1f - insetViewport, 1f - insetViewport, z));

            var width  = Vector3.Distance(bl, br);
            var height = Vector3.Distance(bl, tl);

            CreateWall(new Vector2((bl.x + br.x) * 0.5f, bl.y - thickness * 0.5f), new Vector2(width, thickness));
            CreateWall(new Vector2((tl.x + tr.x) * 0.5f, tl.y + thickness * 0.5f), new Vector2(width, thickness));
            CreateWall(new Vector2(bl.x - thickness * 0.5f, (bl.y + tl.y) * 0.5f), new Vector2(thickness, height));
            CreateWall(new Vector2(br.x + thickness * 0.5f, (br.y + tr.y) * 0.5f), new Vector2(thickness, height));
        }

        private void CreateWall(Vector2 center, Vector2 size)
        {
            var go = new GameObject("Wall");
            go.transform.SetParent(transform);
            go.transform.position = center;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            var col = go.AddComponent<BoxCollider2D>();
            col.size = size;
        }
    }
}