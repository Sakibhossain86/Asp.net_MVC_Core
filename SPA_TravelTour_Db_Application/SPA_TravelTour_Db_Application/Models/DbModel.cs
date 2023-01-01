using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPA_TravelTour_Db_Application.Models
{
    public enum TransPortMode { Bus = 1, PrivateCar, CruiseShip, Plane, PrivateJet }
    public enum PakageCategory { Gold = 1, Silver, Platinum, Express, Economy }
    public class TravelAgent
    {
        public int TravelAgentId { get; set; }
        [Required, StringLength(50), Display(Name = "Agent Name")]
        public string AgentName { get; set; } = default!;
        [Required, StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required,StringLength(20)]
        public string PhoneNumber { get; set; } = default!;
        [Required, StringLength(70), Display(Name = "Agent Address")]
        public string AgentAddress { get; set; } = default!;
        public ICollection<AgentTourPackage> AgentTourPackages { get; set; } = new List<AgentTourPackage>();
    }
    public class TourPackage
    {
        public int TourPackageId { get; set; }
        [EnumDataType(typeof(PakageCategory)), Display(Name = "Pakage Category")]
        public PakageCategory PackageCategory { get; set; }
        [Required, StringLength(50), Display(Name = "Package Name")]
        public string PackageName { get; set; } = default!;
        [Required, Display(Name = "Cost Per Person"), DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostPerPerson { get; set; }
        [Required, Display(Name = "Tour Time")]
        public int TourTime { get; set; }
        public virtual ICollection<PackageFeature> PackageFeatures { get; set; } = new List<PackageFeature>();
        public virtual ICollection<Tourist> Tourists { get; set; } = new List<Tourist>();
        public virtual ICollection<AgentTourPackage> AgentTourPackages { get; set; } = new List<AgentTourPackage>();
    }
    public class PackageFeature
    {
        public int PackageFeatureId { get; set; }
        [EnumDataType(typeof(TransPortMode)), Display(Name = "TransPort Mode")]
        public TransPortMode TransportMode { get; set; }
        [Required, StringLength(50), Display(Name = "Hotel Booking")]
        public string HotelBooking { get; set; } = default!;
        public bool Status { get; set; }
        [ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }
        public virtual TourPackage? TourPackage { get; set; } = default!;
    }
    public class Tourist
    {
        public int TouristId { get; set; }
        [Required, StringLength(50), Display(Name = "Tourist Name")]
        public string TouristName { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Booking Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BookingDate { get; set; }
        [Required, StringLength(50), Display(Name = "Tourist Occupation")]
        public string TouristOccupation { get; set; } = default!;
        [Required, StringLength(150)]
        public string TouristPicture { get; set; } = default!;
        [ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }
        public virtual TourPackage? TourPackage { get; set; } = default!;

    }
    public class AgentTourPackage
    {
        [Key,ForeignKey("TravelAgent")]
        public int TravelAgentId { get; set; }
        [Key,ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }
        public virtual TravelAgent? TravelAgent { get; set; } = default!;
        public virtual TourPackage? TourPackage { get; set; } = default!;
    }
    public class TravelTourDbContext : DbContext
    {
        public TravelTourDbContext(DbContextOptions<TravelTourDbContext> options) : base(options) { }
        public DbSet<TravelAgent> TravelAgents { get; set; } = default!;
        public DbSet<TourPackage> TourPackages { get; set; } = default!;
        public DbSet<PackageFeature> PackageFeatures { get; set; } = default!;
        public DbSet<Tourist> Tourists { get; set; } = default!;
        public DbSet<AgentTourPackage> AgentTourPackages { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentTourPackage>().HasKey(o => new { o.TravelAgentId, o.TourPackageId });
        }
    }
}
