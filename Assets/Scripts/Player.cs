using System;
using UnityEngine;
using SoftFloat;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class Player : MonoBehaviour
    {
        public Guid id;
        public (sfloat x, sfloat y) position = ((sfloat)1, (sfloat)1);
        public TestInput input;

        private void Update()
        {
            transform.position = new Vector3((float)position.x, (float)position.y);
        }
    }
}