using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IPageAppService
    {
        Task<ListServiceModel<PageDto>> GetListAsync(string name, int pageNumber, int pageSize);
    }
}
