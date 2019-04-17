using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Formacio.Beans
{
    public class HardwareBean
    {

        public int? id { get; set; }
        public String codigo { get; set; }
        public String cpu { get; set; }
        public String usb { get; set; }
        public String audioJack { get; set; }
        public Boolean? fmRadio { get; set; }
        public Boolean? accelerometer { get; set; }

        public HardwareBean() { }

        public HardwareBean(
                             int? id,
                             String codigo,
                             String cpu,
                             String usb,
                             String audioJack,
                             Boolean? fmRadio,
                             Boolean? accelerometer
                        )
        {
            this.id = id;
            this.codigo = codigo;
            this.cpu = cpu;
            this.usb = usb;
            this.audioJack = audioJack;
            this.fmRadio = fmRadio;
            this.accelerometer = accelerometer;
        }
    }
}