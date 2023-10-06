using System.Collections.Generic;

namespace CoreBot.Model
{
    public enum TourCategory
    {
        List = 0,
        Query = 1
    }

    public class Tours
    {
        public List<Tour> tour { get; set; }
    }

    public class Tour
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
}
