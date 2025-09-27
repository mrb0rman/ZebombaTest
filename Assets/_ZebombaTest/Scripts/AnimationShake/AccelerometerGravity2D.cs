using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace AnimationShake
    {
        public class AccelerometerGravity2D : MonoBehaviour
        {
            [SerializeField] private float gravityStrength = 9.81f;
                [SerializeField] private float smooth = 0.15f;        
                [SerializeField] private float shakeThreshold = 2f;
                [SerializeField] private float shakeForce = 5f;
                [SerializeField] private Rigidbody2D[] balls;
                
                private Vector3 lastAccel;
                private Vector3 filtered;
                private Quaternion calibration = Quaternion.identity;
                
                private void Start()
                {
                    filtered = Vector3.zero;
                    lastAccel = Input.acceleration;
            
                    Calibrate();
                }
            
                private void Update()
                {
                    var a = Input.acceleration;
                    var adjusted = calibration * a;
                    var desired = new Vector2(adjusted.x, adjusted.y);
            
                    filtered = Vector2.Lerp(filtered, desired,
                        1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.0001f, smooth)));
            
                    Physics2D.gravity = filtered.normalized * gravityStrength;
            
                    var delta = a - lastAccel;
                    lastAccel = a;
            
                    if (delta.magnitude > shakeThreshold)
                    {
                        foreach (var rb in balls)
                        {
                            if (rb == null) continue;
                            var randomDir = Random.insideUnitCircle.normalized;
                            rb.AddForce(randomDir * shakeForce, ForceMode2D.Impulse);
                        }
                    }
            
                    if (Input.GetMouseButtonDown(0))
                    {
                        Calibrate();
                    }
                }
            
                private void Calibrate()
                {
                    var g = Input.acceleration;
                    calibration = Quaternion.FromToRotation(g, Vector3.down);
                }
        }
    }
}

