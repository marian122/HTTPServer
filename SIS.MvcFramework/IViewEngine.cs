using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{

    public interface IViewEngine
    {
        string GetHtml(string template, object model);

    }
}
