using System;
using System.Collections.Generic;
using System.Text;

namespace Hubspot_API_Service.Model
{
    class hubSpotEntityModel
    {
        public List<hubSpotEntityDataModel> properties { get; set; }
    }

    class hubSpotEntityDataModel
    {
        public object property { get; set; }
        public object value { get; set; }
    }
}
