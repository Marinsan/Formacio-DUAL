using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio.Beans
{
    public class TelefonoBean
    {

        public int? id              { get; set; }
        public String codigo        { get; set; }
        public String ImageUrl      { get; set; }
        public String name          { get; set; }
        public String snippet       { get; set; }

        public virtual So So { get; set; }

        public TelefonoBean(int? id,
                            String codigo, 
                            String ImageUrl, 
                            String name,
                            String snippet,
                            So so)
        {
            this.id         = id;
            this.codigo     = codigo;
            this.ImageUrl   = ImageUrl;
            this.name       = name;
            this.snippet    = snippet;
        }
            


    }

    public class So
    {

        public int? id              { get; set; }
        public String codigo        { get; set; }
        public String tipo          { get; set; }
        public String versioOs      { get; set; }
        public String versioUi      { get; set; }

        public So() { }

        public So(int? id,
                  String codigo,
                  String tipo,
                  String versionOs,
                  String versioUi)
        {
            this.id = id;
            this.codigo = codigo;
            this.tipo = tipo;
            this.versioOs = versioOs;
            this.versioUi = versioUi;
        }
    }
}