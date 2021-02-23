using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class Treasure : MonoBehaviour
    {
        public double coins = 0;
        private Button save;
        private Button load;
        private Button reset;

        public string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public T DeserializeObject<T>(string xml)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml))
            {
                T result = (T)serializer.Deserialize(reader);
                return result;
            }
        }


        private void Load()
        {
            List<CurrencyDto> currencyDtos = new List<CurrencyDto>();

            string xml = PlayerPrefs.GetString("Curencies");
            currencyDtos = DeserializeObject<List<CurrencyDto>>(xml);

            foreach (var currencyDto in currencyDtos)
            {
               var currency = GameObject.Find(currencyDto.Name).GetComponent<Currency>();
               currency.Amount = currencyDto.Amount;
                currency.ShowAmount();
            }
        }

        private void Save()
        {
            List<CurrencyDto> currencyDtos = new List<CurrencyDto>();

            GameObject[] gameObjectCurrencies = GameObject.FindGameObjectsWithTag("Curency");

            foreach (var gameObjectCurrency in gameObjectCurrencies)
            {
                var curency = gameObjectCurrency.GetComponent<Currency>();
                CurrencyDto dto = new CurrencyDto();
                dto.Name = curency.name;
                dto.Amount = curency.Amount;

                currencyDtos.Add(dto);
            }

            string xml = SerializeObject<List<CurrencyDto>>(currencyDtos);
            PlayerPrefs.SetString("Curencies", xml);
            PlayerPrefs.Save();
        }

        private void Start()
        {
            save = GameObject.Find("ButtonSave").GetComponent<Button>();
            save.onClick.AddListener(() => Save());

            load = GameObject.Find("ButtonLoad").GetComponent<Button>();
            load.onClick.AddListener(() => Load());

            reset = GameObject.Find("ButtonReset").GetComponent<Button>();
            reset.onClick.AddListener(() => Resets());


        }

        private void Resets()
        {
            GameObject[] gameObjectCurrencies = GameObject.FindGameObjectsWithTag("Curency");
            foreach (var gameObjectCurrency in gameObjectCurrencies)
            {
                var currency = gameObjectCurrency.GetComponent<Currency>();
                currency.Amount = 0;
                currency.ShowAmount();
            }
        }
    }


}

