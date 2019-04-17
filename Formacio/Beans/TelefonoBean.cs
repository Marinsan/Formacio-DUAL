using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio.Beans
{
    public class TelefonoBean
    {
        private List<ImagesBean> list_images;
        private SoBean soBean;
        private ConnectivityBean connectivityBean;
        private HardwareBean hardwareBean;

        public int? id                          { get; set; }
        public String codigo                    { get; set; }
        public String ImageUrl                  { get; set; }
        public String name                      { get; set; }
        public String snippet                   { get; set; }
        public String description               { get; set; }
        public String storage_ram               { get; set; }
        public String storage_flash             { get; set; }
        public String size                      { get; set; }
        public String additionalFeatures        { get; set; }
        public String weight                    { get; set; }
        public String screenSize                { get; set; }
        public String screenResolution          { get; set; }
        public Boolean? touchScreen             { get; set; }
        public String camera_primary            { get; set; }
        public String camera_features           { get; set; }
        public String battery_type              { get; set; }
        public String battery_talkTime          { get; set; }
        public String battery_standbyTime       { get; set; }
        public String availability              { get; set; }


        public SoBean so { get; set; }

        public ConnectivityBean connectivity { get; set; }

        public HardwareBean hardware { get; set; }

        public List<ImagesBean> images { get; set; }

        public TelefonoBean() { }

        public TelefonoBean(int? id,
                            String codigo, 
                            String ImageUrl, 
                            String name,
                            String snippet,
                            String description,
                            String storage_ram,
                            String storage_flash,
                            String size,
                            String additionalFeatures,
                            String weight,
                            String screenSize,
                            String screenResolution,
                            Boolean? touchScreen,
                            String camera_primary,
                            String camera_features,
                            String battery_type,
                            String battery_talkTime,
                            String battery_standbyTime,
                            String availability,
                            SoBean so,
                            ConnectivityBean connectivity,
                            HardwareBean hardware,
                            List<ImagesBean> list_images)
        {
            this.id                         = id;
            this.codigo                     = codigo;
            this.ImageUrl                   = ImageUrl;
            this.name                       = name;
            this.snippet                    = snippet;
            this.description                = description;
            this.storage_ram                = storage_ram;
            this.storage_flash              = storage_flash;
            this.size                       = size;
            this.additionalFeatures         = additionalFeatures;
            this.weight                     = weight;
            this.screenSize                 = screenSize;
            this.screenResolution           = screenResolution;
            this.touchScreen                = touchScreen;
            this.camera_primary             = camera_primary;
            this.camera_features            = camera_features;
            this.battery_type               = battery_type;
            this.battery_talkTime           = battery_talkTime;
            this.battery_standbyTime        = battery_standbyTime;
            this.availability               = availability;
            this.so                         = so;
            this.connectivity               = connectivity;
            this.hardware                   = hardware;
            this.list_images                = list_images;
        }
    }
}