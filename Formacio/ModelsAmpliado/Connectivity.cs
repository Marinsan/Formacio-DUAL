using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Connectivity
    {

        public Beans.ConnectivityBean toBean()
        {
            return new Beans.ConnectivityBean(
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