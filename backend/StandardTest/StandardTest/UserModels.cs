using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StandardTest
{
    [Table("Agents")]
    public class Agent
    {
        [Key]
        [Column("AgentId")]
        public int AgentId { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("phoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("AgentName")]
        public string AgentName { get; set; }
    }

    [Table("Community")]
    public class Community
    {
        [Key]
        [Column("CommunityId")]
        public int CommunityId { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("phoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("CommunityName")]
        public string CommunityName { get; set; }
    }

    public class DistInfo
    {
        public int ResultCode { get; set; } = 1;
        public double SchoolDist { get; set; } = 0;
        public double ParkDist { get; set; } = 0;
        public double HospitalDist { get; set; } = 0;
        public double FactoryDist { get; set; } = 0;
    }

    [Table("favorites")]
    public class Favorite
    {
        [Key]
        [Column("favorite_id")]
        public int FavoriteId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("distance0101")]
        public double Distance0101 { get; set; }

        [Column("distance0201")]
        public double Distance0201 { get; set; }

        [Column("distance0202")]
        public double Distance0202 { get; set; }

        [Column("distance0301")]
        public double Distance0301 { get; set; }

        [Column("distance0401")]
        public double Distance0401 { get; set; }

        [Column("distance0402")]
        public double Distance0402 { get; set; }

        [Column("distance0403")]
        public double Distance0403 { get; set; }

        [Column("distance0501")]
        public double Distance0501 { get; set; }

        [Column("distance0502")]
        public double Distance0502 { get; set; }

        [Column("distance0503")]
        public double Distance0503 { get; set; }

        [Column("distance0504")]
        public double Distance0504 { get; set; }

        [Column("distance0505")]
        public double Distance0505 { get; set; }

        [Column("note")]
        public string Note { get; set; }
    }

    public class UserFavorite
    {
        public List<Favorite> Value { get; set; }
        public int ResultCode { get; set; }
    }

    [Table("LandUse")]
    public class LandUse
    {
        [Key]
        [Column("UUID")]
        public int UUID { get; set; }

        [Column("Lon")]
        public double Lon { get; set; }

        [Column("Lat")]
        public double Lat { get; set; }

        [Column("F_AREA")]
        public double F_AREA { get; set; }

        [Column("Level1")]
        public int Level1 { get; set; }

        [Column("Level2")]
        public int Level2 { get; set; }
    }

    [Table("News")]
    public class NewsItem
    {
        [Key]
        [Column("newsName")]
        public string NewsName { get; set; }

        [Column("newstTime")]
        public DateTime NewsTime { get; set; }

        [Column("newsLink")]
        public string NewsLink { get; set; }
    }

    [Table("street_view")]
    public class StreetView
    {
        [Column("road_name")]
        public string RoadName { get; set; } = null;

        [Column("lng")]
        public double Lng { get; set; } = 0;

        [Column("lat")]
        public double Lat { get; set; } = 0;

        [Column("english")]
        public string English { get; set; } = null;

        [Column("vegetationPortion")]
        public double VegetationPortion { get; set; } = 0;

        [Column("skyPortion")]
        public double SkyPortion { get; set; } = 0;

    }

    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }


        [Column("password")]
        public string Password { get; set; }
    }
}