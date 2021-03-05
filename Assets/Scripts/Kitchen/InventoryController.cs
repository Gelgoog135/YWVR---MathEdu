using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace YWVR.RE
{
    public class InventoryController : MonoBehaviour
    {
        private List<string> items = new List<string>();
        public void AddItem(string item) => items.Add(item);
        public void RemoveItem(string item) => items.Remove(item);
        public bool HasItem(string item) => items.Any(x => x == item);
    }
}

