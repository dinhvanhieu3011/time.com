using BMBSOFT.GIS.Data.Interfaces;
using BMBSOFT.GIS.Data.Repository;
using BMBSOFT.GIS.Entity.Planing;

namespace BMBSOFT.GIS.Data.Implements
{
    public class PlanningDistrictRepository : BaseRepository<PlanningDistrict>, IPlanningDistrictRepository
    {
        public PlanningDistrictRepository(AppDbContext context) : base(context)
        {
        }
    }
}
