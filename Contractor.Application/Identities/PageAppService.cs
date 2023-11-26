using AutoMapper;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class PageAppService : IPageAppService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;

        public PageAppService(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }

        public async Task<ListServiceModel<PageDto>> GetListAsync(string? name, int pageNumber, int pageSize)
        {
            var pages = await _pageRepository.GetListAsync(name, pageNumber, pageSize);
            var pagesDto = _mapper.Map<List<PageDto>>(pages.List);
            return new ListServiceModel<PageDto>(pages.TotalCount, pagesDto);
        }


    }
}