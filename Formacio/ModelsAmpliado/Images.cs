using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Images
    {

        public Beans.Images toBean()
        {
            return new Beans.Images(
                       this.id,
                       this.codigo,
                       this.image1,
                       this.image2,
                       this.image3,
                       this.image4,
                       this.image5
           );
        }

    }
}