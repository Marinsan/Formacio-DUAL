using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class Empleats
    {

        public Beans.EmpleatBean toBean()
        {
            return new Beans.EmpleatBean(
                            this.id,
                            this.codi,
                            this.nom,
                            this.primer_cognom,
                            this.segon_cognom
                );
        }
   }
}