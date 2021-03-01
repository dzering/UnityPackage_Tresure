using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [System.Serializable]
    public class CurrencyDto
    {
        public string Name
        {
            get;
            set;
        }
        public int Amount 
        { get; set; }
    }
}
