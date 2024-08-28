using System;
using UnityEngine;

namespace Saving.View
{
    public class CheckPointFlag : MonoBehaviour
    {
        [SerializeField] private CheckPoint checkPoint;
        [SerializeField] private GameObject flag;
        private void Start()
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            flag.SetActive(false);
            checkPoint.CheckPointSet += () => flag.SetActive(true);
        }
    }
}
