using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Hardware
    {

        public Beans.Hardware toBean()
        {
            return new Beans.Hardware(
                       this.id,
                       this.codigo,
                       this.cpu,
                       this.usb,
                       this.audioJack,
                       this.fmRadio,
                       this.accelerometer
           );
        }

    }
}