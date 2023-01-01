using SPA_TravelTour_Db_Application.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPA_TravelTour_Db_Application.ViewModels
{
    public class TouristEditModel
    {
        public int TouristId { get; set; }
        [Required, StringLength(50), Display(Name = "Tourist Name")]
        public string TouristName { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Booking Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BookingDate { get; set; }
        [Required, StringLength(50), Display(Name = "Tourist Occupation")]
        public string TouristOccupation { get; set; } = default!;
        public IFormFile? TouristPicture { get; set; } = default!;
        [ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }
        public virtual TourPackage? TourPackage { get; set; } = default!;
    }
}
