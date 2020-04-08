using System.Collections.Generic;

namespace Codific.Mvc567.Dtos.ServiceResults
{
    public class InlineEditResult
    {
        public InlineEditResult(bool succeeded, string error = null)
        {
            this.Succeeded = succeeded;
            this.Error = error;
        }

        public bool Succeeded { get; set; }

        public string Error { get; set; }
    }
}