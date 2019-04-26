using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Formacio.Beans
{
    public class EmpleatBean
    {

        public int? id { get; set; }
        public String codi { get; set; }
        public String nom { get; set; }
        public String primer_cognom { get; set; }
        public String segon_cognom { get; set; }

        public EmpleatBean () { }

        public EmpleatBean(int? id,
                  String codi,
                  String nom,
                  String primer_cognom,
                  String segon_cognom)
        {
            this.id             = id;
            this.codi           = codi;
            this.nom            = nom;
            this.primer_cognom  = primer_cognom;
            this.segon_cognom   = segon_cognom;
        }
    }
}