using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio.Beans
{
    public class TelefonoBean
    {

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
        public String images                    { get; set; }
        public String availability              { get; set; }


        public virtual So so { get; set; }

        public virtual Connectivity connectivity { get; set; }

        public virtual Hardware hardware { get; set; }

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
                            String images,
                            String availability,
                            So so,
                            Connectivity connectivity,
                            Hardware hardware)
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
            this.images                     = images;
            this.availability               = availability;
            this.so                         = so;
            this.connectivity               = connectivity;
            this.hardware                   = hardware;

        }
            


    }

    public class So
    {

        public int? id              { get; set; }
        public String codigo        { get; set; }
        public String tipo          { get; set; }
        public String versionOs     { get; set; }
        public String versionUi     { get; set; }

        public So() { }

        public So(int? id,
                  String codigo,
                  String tipo,
                  String versionOs,
                  String versionUi)
        {
            this.id = id;
            this.codigo = codigo;
            this.tipo = tipo;
            this.versionOs = versionOs;
            this.versionUi = versionUi;
        }
    }

    public class Connectivity
    {

        public int? id { get; set; }
        public String codigo { get; set; }
        public String bluetooth { get; set; }
        public String cell { get; set; }
        public Boolean? gps { get; set; }
        public Boolean? infrared { get; set; }
        public String wifi { get; set; }

        public Connectivity() { }

        public Connectivity(int? id,
                  String codigo,
                  String bluetooth,
                  String cell,
                  Boolean? gps,
                  Boolean? infrared,
                  String wifi)
        {
            this.id             = id;
            this.codigo         = codigo;
            this.bluetooth      = bluetooth;
            this.cell           = cell;
            this.gps            = gps;
            this.infrared       = infrared;
            this.wifi           = wifi;
        }
    }

    public class Hardware
    {

        public int? id                      { get; set; }
        public String codigo                { get; set; }
        public String cpu                   { get; set; }
        public String usb                   { get; set; }
        public String audioJack             { get; set; }
        public Boolean? fmRadio               { get; set; }
        public Boolean? accelerometer       { get; set; }

        public Hardware() { }

        public Hardware(
                             int? id,
                             String codigo,
                             String cpu,
                             String usb,
                             String audioJack,
                             Boolean? fmRadio,
                             Boolean? accelerometer
                        )
        {
            this.id                 = id;
            this.codigo             = codigo;
            this.cpu                = cpu;
            this.usb                = usb;
            this.audioJack          = audioJack;
            this.fmRadio            = fmRadio;
            this.accelerometer      = accelerometer;
        }
    }
}