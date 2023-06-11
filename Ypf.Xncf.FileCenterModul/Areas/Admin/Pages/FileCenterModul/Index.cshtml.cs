using Senparc.Ncf.Service;
using System;

namespace Ypf.Xncf.FileCenterModul.Areas.FileCenterModul.Pages
{
    public class Index : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        public Index(Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
        {

        }

        public void OnGet()
        {
        }
    }
}
