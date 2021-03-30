using System.Collections.Generic;

namespace TurboVax
{
    public class ListAvailableAppointmentsRequest
    {
        public ISet<string> PortalNames { get; set; }
    }
}
