using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Formacio.Beans
{
    public class SoBean
    {

        public int? id { get; set; }
        public String codigo { get; set; }
        public String tipo { get; set; }
        public String versionOs { get; set; }
        public String versionUi { get; set; }

        public SoBean () { }

        public SoBean(int? id,
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
}