using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Images
    {

        public Beans.ImagesBean toBean()
        {
            return new Beans.ImagesBean(
                       this.id,
                       this.idTelefono,
                       this.image
           );
        }

    }
}