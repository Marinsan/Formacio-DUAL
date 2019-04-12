using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Telefonos
    {     

        public Beans.TelefonoBean toBean()
        {
           
            return new Beans.TelefonoBean(
                        this.id,
                        this.codigo,
                        this.imageURL,
                        this.name,
                        this.snippet,
                        this.description,
                        this.storage_ram,
                        this.storage_flash,
                        this.size,
                        this.weight,
                        this.screenSize,
                        this.screenResolution,
                        this.touchScreen,
                        this.camera_primary,
                        this.camera_features,
                        this.battery_type,
                        this.battery_talkTime,
                        this.battery_standbyTime,
                        this.So!=null?this.So.toBean():null,
                        this.Connectivity!=null?this.Connectivity.toBean():null,
                        this.Hardware != null ? this.Hardware.toBean() : null);
        }

    }
}

