using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formacio
{
    public partial class So
    {

        public Beans.So toBean()
        {
            return new Beans.So(
                            this.id,
                            this.codigo,
                            this.tipo,
                            this.versionOs,
                            this.versionUi
                );
        }
   }
}