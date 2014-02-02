using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class Video
    {
        [JsonProperty("movie")]
        public MovieDetails MovieDetails { get; set; }

        [JsonProperty("tv_show")]
        public SeriesDeatails SeriesDetails { get; set; }

        public Boolean isMovie
        {
            get
            {
                if (MovieDetails == null)
                    return false;
                else
                    return true;
            }
        }
    }
}