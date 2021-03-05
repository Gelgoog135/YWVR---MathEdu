using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//#define DEBUG
namespace YWVR.RE
{
    public class ComputerController : MonoBehaviour
    {
        public GameObject computerGUI,timeleftpanel,keyboard;
        public TMP_InputField txtPassword;
        // Start is called before the first frame update
        void Start()
        {
            Hide();
        }
        public void Show()
        {
            computerGUI.SetActive(true);
            keyboard.SetActive(true);
            timeleftpanel.SetActive(false);
        }

        public void Hide()
        {
            computerGUI.SetActive(false);
            keyboard.SetActive(false);
            timeleftpanel.SetActive(true);
            
        }

        public bool IsShown()
        {
            return computerGUI.activeSelf;
        }
        public void AggregrateNum(int t)
        {
            txtPassword.text += t.ToString();
            
        }
        public void bkspace() { 
            if(txtPassword.text.Length>0)
            txtPassword.text = txtPassword.text.Remove(txtPassword.text.Length - 1); 
        }
        public void EnterPassword()
        {
            var gameController = GetComponent<GameController>();
            if (txtPassword.text.ToLower() == "000000")
            {
                new SceneController(SceneController.Game.Kitchen).ChangeToGameLobby();
                return;
            }
            int resultV;
            var isValid = int.TryParse(txtPassword.text, out resultV);
            if (!isValid)
            {
                Toast.Instance.Show("Incorrect password!\n�K�X���~!", 5.0f);
                return;
            }

            string result = txtPassword.text.Trim();
            if (!gameController.CheckPassword(result))
            {
                Toast.Instance.Show("Incorrect password!\n�K�X���~!", 5.0f);
                return;
            }

            new SceneController(SceneController.Game.Kitchen).ChangeToGameLobby("You are correct!");
        }
    }
}

