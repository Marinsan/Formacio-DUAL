using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio.Beans
{
    public class TelefonoBean
    {

        public int? id                  { get; set; }
        public String codigo            { get; set; }
        public String ImageUrl          { get; set; }
        public String name              { get; set; }
        public String snippet           { get; set; }
        public String description       { get; set; }
        public String storage_ram       { get; set; }
        public String storage_flash     { get; set; }
        public String size              { get; set; }
        public String weight            { get; set; }

        public virtual So so { get; set; }

        public TelefonoBean(int? id,
                            String codigo, 
                            String ImageUrl, 
                            String name,
                            String snippet,
                            String description,
                            String storage_ram,
                            String storage_flash,
                            String size,
                            String weight,
                            So so)
        {
            this.id                 = id;
            this.codigo             = codigo;
            this.ImageUrl           = ImageUrl;
            this.name               = name;
            this.snippet            = snippet;
            this.description        = description;
            this.storage_ram        = storage_ram;
            this.storage_flash      = storage_flash;
            this.size               = size;
            this.weight             = weight;
            this.so                 = so;

        }
            


    }

    public class So
    {

        public int? id              { get; set; }
        public String codigo        { get; set; }
        public String tipo          { get; set; }
        public String versionOs      { get; set; }
        public String versionUi      { get; set; }

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
}