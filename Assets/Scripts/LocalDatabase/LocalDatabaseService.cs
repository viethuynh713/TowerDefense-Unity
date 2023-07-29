using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using Newtonsoft.Json;
using UnityEngine;

namespace MythicEmpire.LocalDatabase
{
    public class LocalDatabaseService: IUserDataLocal,ISettingLocalData
    {
        public string GetOldUserId()
        {
            if (PlayerPrefs.HasKey("id"))
            {
                return PlayerPrefs.GetString("id");
            }

            return null;
        }

        public void UpdateUserId(string newId)
        {
            PlayerPrefs.SetString("id",newId);
        }

        public void DeleteUserId()
        {
            PlayerPrefs.DeleteKey("id");
            PlayerPrefs.DeleteKey("Setting-Config");
        }

        public SettingDataModel GetSettingData()
        {
            SettingDataModel modelSetting;
            if (PlayerPrefs.HasKey("Setting-Config"))
            {
                modelSetting = JsonConvert.DeserializeObject<SettingDataModel>(PlayerPrefs.GetString("Setting-Config"));
                
            }
            else
            {
                modelSetting = new SettingDataModel();
                var data = JsonConvert.SerializeObject(modelSetting);
                PlayerPrefs.SetString("Setting-Config",data);
                
            }

            return modelSetting;
        }

        public void UpdateSettingModel(SettingDataModel newModel)
        {
            var data = JsonConvert.SerializeObject(newModel);
            PlayerPrefs.SetString("Setting-Config",data);
            EventManager.Instance.PostEvent(EventID.ChangeSoundSetting);
        }
    }

    public interface IUserDataLocal
    {
        string GetOldUserId();
        void UpdateUserId(string newId);
        void DeleteUserId();
    }

    public interface ISettingLocalData
    {
        SettingDataModel GetSettingData();
        void UpdateSettingModel(SettingDataModel newModel);
    }
    public class SettingDataModel
    {
        public float EffectSoundVolume;
        public float MusicSoundVolume;
        public bool IsFullScreen;

        public SettingDataModel()
        {
            EffectSoundVolume = 1;
            MusicSoundVolume = 1;
            IsFullScreen = true;
        }
    }
}
