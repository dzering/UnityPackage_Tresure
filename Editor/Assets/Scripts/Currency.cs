using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Currency : MonoBehaviour
    {
        [SerializeField] private GameObject buttonMore;
        [SerializeField] private GameObject buttonLess;
        public int Amount;
        public Text textAmount;

        public void ShowAmount()
        {
            textAmount.text = Amount.ToString();
        }
        private void Start()
        {
            textAmount = GetComponentInChildren<Text>();
            Button btnLess = buttonLess.GetComponent<Button>();
            btnLess.onClick.AddListener(() => Less());


            Button btnMore = buttonMore.GetComponent<Button>();
            btnMore.onClick.AddListener(() => More());
        }

        private void More()
        {
            Amount++;
            ShowAmount();

        }

        private void Less()
        {
            if(Amount == 0)
            {
                return;
            }
            Amount--;
            ShowAmount();
        }
    }

    
}

