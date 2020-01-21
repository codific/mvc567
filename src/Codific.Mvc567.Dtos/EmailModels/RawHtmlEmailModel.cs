using System;
using Codific.Mvc567.Dtos.EmailModels.Abstraction;

namespace Codific.Mvc567.Dtos.EmailModels
{
    public class RawHtmlEmailModel : EmailModel
    {
        public string HtmlBody { get; set; }
    }
}
