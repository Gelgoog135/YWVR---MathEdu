using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
namespace YWVR.RE
{
    public class TouchController : MonoBehaviour
    {
        
        private InputDevice[] device=new InputDevice[2]; 
        void GetDevicesL()
        {
            List<InputDevice> devicesL=new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devicesL);
            device[0] = devicesL.FirstOrDefault(); 
        }
        void GetDevicesR()
        {
            List<InputDevice> devicesR = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devicesR);
            device[1] = devicesR.FirstOrDefault();
        }
        private void OnEnable()
        {
            if (!device[0].isValid) GetDevicesL();
            if (!device[1].isValid) GetDevicesR();
        }
        
        bool Lbtnpressed()
        {
            if ((device[0].TryGetFeatureValue(CommonUsages.triggerButton, out bool t1) && t1) ||
                (device[0].TryGetFeatureValue(CommonUsages.gripButton, out bool t3) && t3))
                return true;
            return false;
        }
        bool Rbtnpressed()
        {
            if ((device[1].TryGetFeatureValue(CommonUsages.triggerButton, out bool t2) && t2) ||
                (device[1].TryGetFeatureValue(CommonUsages.gripButton, out bool t4) && t4))
                return true;
            return false;
        }
        public GameObject left, right;
        private void Update()
        {
            OnEnable();
            if (Lbtnpressed())
            {
                var obj = left.GetComponent<XRRayInteractor>();
                RaycastHit hit;
                if(obj.GetCurrentRaycastHit(out hit)) CallFunction(hit.transform.name);    
           }
            if (Rbtnpressed())
            {
                var obj = right.GetComponent<XRRayInteractor>();
                RaycastHit hit;
                if(obj.GetCurrentRaycastHit(out hit))CallFunction(hit.transform.name);
            }
        }
        
        public void CallFunction(string objname)
        {         
            var crateController = GetComponent<CrateController>();
            var inventoryController = GetComponent<InventoryController>();
            var keyController = GetComponent<KeyController>();
            Debug.Log("Shown");
            switch (objname)
            {
                case Items.Key:
                    inventoryController.AddItem(Items.Key);
                    keyController.Hide();
                    break;
                case Items.Crate:
                case Items.CrateBox:
                case Items.CrateTop:
                    if (inventoryController.HasItem(Items.Key) && !crateController.isOpened)
                    {
                        
                        crateController.UnlockCrate();
                        inventoryController.RemoveItem(Items.Key);
                    }break;
                default:break;
            }
        }
    }
}

