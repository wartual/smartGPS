using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.PrometInfo
{
    public class Event
    {
        [JsonProperty("dovoljenjeDatKon")]
        public String DovoljenjeDatKon { get; set; }

        [JsonProperty("y_wgs")]
        public double Y_WGS { get; set; }

        [JsonProperty("kategorija")]
        public String Kategorija { get; set; }

        [JsonProperty("zbrisano")]
        public String Zbrisano { get; set; }

        [JsonProperty("isMejniPrehod")]
        public Boolean MejniPrehod { get; set; }

        [JsonProperty("opis")]
        public String Opis { get; set; }

        [JsonProperty("vir")]
        public String Vir { get; set; }

        [JsonProperty("operater_izbris")]
        public String OperaterIzbris { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("veljavnostDo")]
        public long VeljavnostDo { get; set; }

        [JsonProperty("prioriteta")]
        public int Prioriteta { get; set; }

        [JsonProperty("operater_sprememba")]
        public String OperaterSpememba { get; set; }

        [JsonProperty("prioritetaCeste")]
        public int PrioritetaCeste{ get; set; }

        [JsonProperty("veljavnostOd")]
        public long VeljavnostOd { get; set; }

        [JsonProperty("cesta")]
        public String Cesta { get; set; }

        [JsonProperty("vneseno")]
        public double Vneseno { get; set; }
        
        [JsonProperty("updated")]
        public double Updated { get; set; }
        
        [JsonProperty("x_wgs")]
        public double X_WGS { get; set; }
        
        [JsonProperty("dovoljenjeSt")]
        public String DovoljenjeSt { get; set; }
        
        [JsonProperty("opisEn")]
        public String OpisEn { get; set; }
        
        [JsonProperty("stacionaza")]
        public String Stacionaza { get; set; }
        
        [JsonProperty("operater_vnos")]
        public String OperaterVnos { get; set; }
   
        [JsonProperty("odsek")]
        public String Odsek { get; set; }
   
        [JsonProperty("vzrokEn")]
        public String VzrokEn { get; set; }
        
        [JsonProperty("icon")]
        public String Icon { get; set; }
    
        [JsonProperty("spremenjeno")]
        public String Spremenjeno { get; set; }
        
        [JsonProperty("vzrok")]
        public String Vzrok { get; set; }
        
        [JsonProperty("dovoljenjeDatZac")]
        public String DovoljenjeDatZac { get; set; }
        
        [JsonProperty("y")]
        public double Y { get; set; }
        
        [JsonProperty("X")]
        public double X { get; set; }

        public DateTime VeljavnostDoDateTime
        {
            get
            {
                return Utilities.ToDateTimeFromEpoch(VeljavnostDo);
            }
        }

         public DateTime VeljavnostOdDateTime
        {
            get
            {
                return Utilities.ToDateTimeFromEpoch(VeljavnostOd);
            }
        }
    }
}