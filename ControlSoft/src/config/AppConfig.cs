using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using ControlSoft.src.bean;

namespace ControlSoft.src.config

{
    class AppConfig
    {
        public static AppConfig appConfig = new AppConfig();
        private AppConfig() { }

        public void setKeyValue(String key, String value) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (null != config) {
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                else {
                    config.AppSettings.Settings[key].Value = value;
                }
            }
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void setTempName(String index,String name) {
            setKeyValue(index + "temp", name);
        }

        public String getTempName(String index) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if(null != config)
            {
                if(config.AppSettings.Settings[index+"temp"] == null)
                {
                    return "温度";
                }
                else
                {
                    return config.AppSettings.Settings[index + "temp"].Value;
                }
            }

            return  "温度";

        }

        public void saveSoftMonitoring(String json)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (null != config)
            {
                if (config.AppSettings.Settings["soft"] == null)
                {
                    config.AppSettings.Settings.Add("soft", json);
                }
                else
                {
                    config.AppSettings.Settings["soft"].Value = json;
                }
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public SoftList getSoftMonitoring()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if(config.AppSettings.Settings[ "soft"] == null)
            {
                return new SoftList();
            }

            string json = config.AppSettings.Settings["soft"].Value;
            SoftList softList = JsonConvert.DeserializeObject<SoftList>(json);

            return softList;
        }
    }
}
