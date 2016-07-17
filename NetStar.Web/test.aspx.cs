using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetStar.Web
{
    using NetStar.Tools;

    public partial class test : NetStar.WebPage.ClientPage
    {
        public override void Page_Load(object sender, EventArgs e)
        {
            Response.Write(BeiJinTime);
            base.Page_Load(sender, e);
        }

        //...

    }
}