using System.Collections.Generic;
using System.Threading.Tasks;
using Level.Views;

namespace Level.Services
{
    public interface IPathfindingService
    {
        public Task<List<INode>> FindPath(IFieldService fieldService);
    }
}