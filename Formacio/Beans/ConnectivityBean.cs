using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Formacio.Beans
{

    public class ConnectivityBean
    {

        public int? id { get; set; }
        public String codigo { get; set; }
        public String bluetooth { get; set; }
        public String cell { get; set; }
        public Boolean? gps { get; set; }
        public Boolean? infrared { get; set; }
        public String wifi { get; set; }

        public ConnectivityBean() { }

        public ConnectivityBean(int? id,
                  String codigo,
                  String bluetooth,
                  String cell,
                  Boolean? gps,
                  Boolean? infrared,
                  String wifi)
        {
            this.id = id;
            this.codigo = codigo;
            this.bluetooth = bluetooth;
            this.cell = cell;
            this.gps = gps;
            this.infrared = infrared;
            this.wifi = wifi;
        }
    }
}