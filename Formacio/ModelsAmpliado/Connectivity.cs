using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Connectivity
    {

        public Beans.Connectivity toBean()
        {
            return new Beans.Connectivity(
                       this.id,
                       this.codigo,
                       this.bluetooth,
                       this.cell,
                       this.gps,
                       this.infrared,
                       this.wifi
           );
        }
      
    }
}