using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNameSpace
{
    public class TestClass : MonoBehaviour
    {
        public static TestClass Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void TestMethod()
        {
            Debug.Log("sadfa skhdobgsudhgfb");
        }
    }
}