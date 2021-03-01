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
        private string pathForSaveGame;


        private void BinarySerializeObject<T>(T _currencies, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Create(path))
            {
                bf.Serialize(file, _currencies);
            }
        }

        private T BinaryDeserializeObject<T>(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                T result = (T)bf.Deserialize(file);
                return result;
            }
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

        private List<CurrencyDto> CreateCollectionCurrencyDto()
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

            return currencyDtos;
        }



        private void Save()
        {
            var currencyDtos = CreateCollectionCurrencyDto();
            string xml = SerializeObject<List<CurrencyDto>>(currencyDtos);
            PlayerPrefs.SetString("Curencies", xml);
            PlayerPrefs.Save();
        }

        private void SaveInFile()
        {
            var currencyDtos = CreateCollectionCurrencyDto();
            BinarySerializeObject<List<CurrencyDto>>(currencyDtos, pathForSaveGame);
        }

        private void ShowAmountCurrency(List<CurrencyDto> currencyDtos)
        {
            foreach (var currencyDto in currencyDtos)
            {
                var currency = GameObject.Find(currencyDto.Name).GetComponent<Currency>();
                currency.Amount = currencyDto.Amount;
                currency.ShowAmount();
            } 
        }

        private void Load()
        {
           string xml = PlayerPrefs.GetString("Curencies");
           List<CurrencyDto> currencyDtos = DeserializeObject<List<CurrencyDto>>(xml);
           ShowAmountCurrency(currencyDtos);

        }

        private void LoadFromFile()
        {
            List<CurrencyDto> currencyDtos = BinaryDeserializeObject<List<CurrencyDto>>(pathForSaveGame);
            ShowAmountCurrency(currencyDtos);
        }

       
        private void Start()
        {
            pathForSaveGame = Path.Combine(Application.persistentDataPath, "savegame.txt");

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
