using SPA_TravelTour_Db_Application.Models;

namespace SPA_TravelTour_Db_Application.ViewModels
{
    public class DataViewModel
    {
        public int SelectedAgentId { get; set; }
        public IEnumerable<TravelAgent> TravelAgents { get; set; } = default!;
        public IEnumerable<TourPackage> TourPackages { get; set; } = default!;
        public IEnumerable<PackageFeature> PackageFeatures { get; set; } = default!;
        public IEnumerable<Tourist> Tourists { get; set; } = default!;
        public IEnumerable<AgentTourPackage> AgentTourPackages { get; set; } = default!;

    }
}
