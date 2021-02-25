using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;


namespace Assets.Scripts
{
    public class Treasure : MonoBehaviour
    {
        public double coins = 0;
        private Button save;
        private Button saveInFile;
        private Button load;
        private Button loadFromFile;
        private Button reset;



        private void BinarySerializeObject<T>(T _currencies)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.txt");
            
            bf.Serialize(file, _currencies);
            file.Close();
        }

        private T BinaryDeserializeObject<T>()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.txt", FileMode.Open);
            T result = (T)bf.Deserialize(file);
            file.Close();
            return result;
        }

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

        private void SaveInFile()
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
            BinarySerializeObject<List<CurrencyDto>>(currencyDtos);
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

        private void LoadFromFile()
        {
            List<CurrencyDto> currencyDtos = new List<CurrencyDto>();
            currencyDtos = BinaryDeserializeObject<List<CurrencyDto>>();
            foreach (var currencyDto in currencyDtos)
            {
                var currency = GameObject.Find(currencyDto.Name).GetComponent<Currency>();
                currency.Amount = currencyDto.Amount;
                currency.ShowAmount();
            }
            currencyDtos = BinaryDeserializeObject<List<CurrencyDto>>();
        }

       


        private void Start()
        {
            save = GameObject.Find("ButtonSave").GetComponent<Button>();
            save.onClick.AddListener(() => Save());

            load = GameObject.Find("ButtonLoad").GetComponent<Button>();
            load.onClick.AddListener(() => Load());

            reset = GameObject.Find("ButtonReset").GetComponent<Button>();
            reset.onClick.AddListener(() => Resets());

            saveInFile = GameObject.Find("ButtonSaveInFile").GetComponent<Button>();
            saveInFile.onClick.AddListener(() => SaveInFile());

            loadFromFile = GameObject.Find("ButtonLoadFromFile").GetComponent<Button>();
            loadFromFile.onClick.AddListener(() => LoadFromFile());

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

