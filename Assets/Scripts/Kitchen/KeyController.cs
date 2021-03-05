using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace YWVR.RE
{
    public class KeyController : MonoBehaviour
    {
        public GameObject keys1, keys2, keys3;
        private GameObject key;
        // Start is called before the first frame update
        void Start()
        {
            var allKeys = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Key").ToList();
            IEnumerable<GameObject> keys;
            switch (Global.Difficulty)
            {
                case 3:
                    keys = keys3.transform.GetComponentsInChildren<Transform>().Where(x => x.name == "Key").Select(x => x.gameObject);
                    break;
                case 2:
                    keys = keys2.transform.GetComponentsInChildren<Transform>().Where(x => x.name == "Key").Select(x => x.gameObject);
                    break;
                case 1:
                default:
                    keys = keys1.transform.GetComponentsInChildren<Transform>().Where(x => x.name == "Key").Select(x => x.gameObject);
                    break;
            }
            var exceptIdx = Random.Range(0, keys.Count());
            key = keys.ToList()[exceptIdx];
            for (int i = 0; i < allKeys.Count(); i++)
            {
                allKeys[i].SetActive(false);
            }
            key.SetActive(true);
        }
        public void Hide()
        {
            key.SetActive(false);
        }
    }
}