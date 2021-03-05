using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YWVR.RE
{
    public class CrateController : MonoBehaviour
    {
        public GameObject crate, pwdSheet;
        [System.NonSerialized]
        public bool isOpened = false;
        void Start()
        {
            crate.GetComponent<Animator>().Play("Closed");
            pwdSheet.SetActive(false);          
        }
        public void UnlockCrate()
        {
            crate.GetComponent<Animator>().Play("Open");
            pwdSheet.SetActive(true);
            isOpened = true;
        }
    }

}
