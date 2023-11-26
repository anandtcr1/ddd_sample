using AutoMapper;
using Contractor.Identities;
using Contractor.Tools;
using Contractor.Tools.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class DraftProjectAppService : IDraftProjectAppService
    {
        private readonly IDraftProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEmailManager _emailManager;

        public DraftProjectAppService(IDraftProjectRepository repository, IMapper mapper, IUserRepository userRepository, IEmailManager emailManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _emailManager = emailManager;
        }

        public async Task<DraftProjectDto> CreateAsync(DraftProjectDto draftProjectDto)
        {
            var draftProject = DraftProject.Create(draftProjectDto.OwnerId, draftProjectDto.ConsultantId, draftProjectDto.CreatedDate);

            await _repository.CreateAsync(draftProject);

            await _repository.SaveChangesAsync();

            var consultant = await _userRepository.GetByIdAsync(draftProjectDto.ConsultantId);

            var owner = await _userRepository.GetByIdAsync(draftProjectDto.OwnerId);

            var header = string.Format("Project invitation from {0}", owner.DisplayName);

            var emailBody = string.Format("Hi {0}, <br /> You received a project invitation from {1}. Please login and check", consultant.DisplayName, owner.DisplayName);

            var message = Message.Create(new string[] { consultant.Email }, header, emailBody);

            await _emailManager.SendEmail(message);


            return _mapper.Map<DraftProjectDto>(draftProject);
        }

        public async Task<ListServiceModel<DraftProjectListDto>> GetAllAsync(string consultantId, string? ownerName, DraftProjectStatus? statusId, DateTime? createdDate, int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(consultantId, ownerName, statusId, createdDate, pageNumber, pageSize);

            return new ListServiceModel<DraftProjectListDto>(list.TotalCount, _mapper.Map<List<DraftProjectListDto>>(list.List));
        }

        public async Task<DraftProjectDto> GetAsync(int id)
        {
            var model = await _repository.GetAsync(id);

            return _mapper.Map<DraftProjectDto>(model);
        }

        public async Task<DraftProjectDto> ChangeProjectStatusAsync(int id, DraftProjectStatus statusId)
        {
            var model = await _repository.GetAsync(id);

            switch(statusId)
            {
                case DraftProjectStatus.Accept:
                    model.UpdateAcceptStatus();
                    break;
                case DraftProjectStatus.Reject:
                    model.UpdateRejectStatus(); 
                    break;
            }

            await _repository.SaveChangesAsync();

            return _mapper.Map<DraftProjectDto>(model);
        }
    }
}
