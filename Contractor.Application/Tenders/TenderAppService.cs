using AutoMapper;
using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Files;
using Contractor.Tools;
using Contractor.Tools.Email;
using Microsoft.AspNetCore.Http;
using Contractor.Projects;

namespace Contractor.Tenders
{
    public class TenderAppService : ITenderAppService
    {
        private readonly ITenderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBlobManager _blobManager;
        private readonly IUserRepository _userRepository; 
        private readonly IEmailManager _emailManager;
        private readonly IProjectRepository _projectRepository;

        public TenderAppService(ITenderRepository repository, IMapper mapper, IBlobManager blobManager, IUserRepository userRepository
            , IEmailManager emailManager, IProjectRepository projectRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _blobManager = blobManager;
            _userRepository = userRepository;
            _emailManager = emailManager;
            _projectRepository = projectRepository;
        }

        public async Task<TenderDto> CreateAsync(TenderDto dto, string userId, List<IFormFile> tenderFiles)
        {
            var nextId = await _repository.GetNextIdAsync();

            if(tenderFiles == null || tenderFiles.Count == 0)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var tender = Tender.Create(nextId, dto.Title, dto.OpenDate, dto.CloseDate, dto.Status, dto.ProjectId, dto.Note);

            List<AccessDefinition> accessDefinitions = await _blobManager.UploadTenderFiles(tender.ProjectId, userId, tenderFiles);

            foreach (var accessDefinition in accessDefinitions)
            {
                var tenderAccessDefinition = tender.AddTenderAccessDefinition();

                accessDefinition.AddTenderAccessDefinition(tenderAccessDefinition);
            }


            await _repository.CreateAsync(tender);

            await _repository.SaveChangesAsync();

            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<TenderDto> UpdateAsync(TenderDto dto, string userId, List<IFormFile> tenderFiles)
        {
            var tender = await _repository.GetAsync(dto.Id);

            tender.Update(dto.Title, dto.OpenDate, dto.CloseDate, dto.Status, dto.Note);

            if (tenderFiles != null && tenderFiles.Any())
            {

                List<AccessDefinition> accessDefinitions = await _blobManager.UploadTenderFiles(tender.ProjectId, userId, tenderFiles);

                foreach (var accessDefinition in accessDefinitions)
                {
                    var tenderAccessDefinition = tender.AddTenderAccessDefinition();

                    accessDefinition.AddTenderAccessDefinition(tenderAccessDefinition);
                }
            }

            await _repository.SaveChangesAsync();

            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<ListServiceModel<TendersListDto>> GetAllAsync(string userId,
            string? tenderNumber,
            DateTime? tenderDate,
            DateTime? openDate,
            DateTime? endDate,
            string? projectNumber,
            string? tenderTitle,
            string? winner,
            TenderStatus? status,
            int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var list = await _repository.GetAllAsync(userId,
                user.UserType,
                tenderNumber,
                tenderDate,
                openDate,
                endDate,
                projectNumber,
                tenderTitle,
                winner,
                status,
                pageNumber, pageSize);

            var mappedList = list.List.Select(x => new TendersListDto()
            {
                Id = x.Id,
                TenderNumber = x.Number,
                ProjectNumber = x.Project.ProjectNumber,
                TenderDate = x.IssueDate,
                OpenDate = x.OpenDate,
                EndDate = x.CloseDate,
                Status = x.Status,
                TenderTitle = x.Title,
                ContractorInvitationStatus = user.UserType == UserTypes.Contractor ? x.TenderInvitations.FirstOrDefault(y => y.ContractorId == userId)!.Status : null,
                Winner = x.TenderInvitations
                  .FirstOrDefault(a => a.Status == TenderInvitationStatus.Winner) == null ? "" : x.TenderInvitations.FirstOrDefault(a => a.Status == TenderInvitationStatus.Winner)!.Contractor.DisplayName
            }).ToList();

            return new ListServiceModel<TendersListDto>(list.TotalCount, mappedList);
        }

        public async Task<TenderDto> GetAsync(int id)
        {
            var tender = await _repository.GetAsync(id);

            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<TenderDto> GetByProjectIdAsync(int projectId)
        {
            var tender = await _repository.GetByProjectIdAsync(projectId);

            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<ContractorTenderDto> GetForContractorAsync(int id, string userId)
        {
            var tender = await _repository.GetAsync(id);

            if (!tender.TenderInvitations.Any(x=>x.ContractorId == userId))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var tenderDto = _mapper.Map<ContractorTenderDto>(tender);

            tenderDto.TenderInvitation = _mapper.Map<TenderInvitationDto>(tender.TenderInvitations.FirstOrDefault(x => x.ContractorId == userId));

            var project = await _projectRepository.GetAsync(tender.ProjectId);

            tenderDto.ProjectNumber = project.ProjectNumber;
            tenderDto.ProjectTitle = project.ProjectTitle;

            return tenderDto;
        }

        public async Task DeleteTenderAccessDefinitionAsync(int tenderId, int accessDefinitionId, string userId)
        {
            var tender = await _repository.GetAsync(tenderId);

            tender.RemoveTenderAccessDefinition(accessDefinitionId);

            await _blobManager.Delete(accessDefinitionId, userId, true);

            
        }

        public async Task AcceptTenderInvitationAsync(int id, string userId)
        {
            var tender = await _repository.GetAsync(id);

            tender.AcceptInvitation(userId);

            await _repository.SaveChangesAsync();
        }

        public async Task DeclineTenderInvitationAsync(int id, string userId)
        {
            var tender = await _repository.GetAsync(id);

            tender.DeclineInvitation(userId);

            await _repository.SaveChangesAsync();
        }

        public async Task SubmitTenderProposalAsync(int tenderId, string note, List<IFormFile> files, string userId)
        {

            if(files  == null || files.Count == 0)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var tender = await _repository.GetAsync(tenderId);

            var user = await _userRepository.GetByIdAsync(userId);

            var invitation = tender.SubmitInvitation(note, userId);

            List<AccessDefinition> accessDefinitions = await _blobManager.UploadTenderSubmissionProposalFiles(tender.ProjectId, userId, user.DisplayName, files);

            foreach (var accessDefinition in accessDefinitions)
            {
                var invitationAccessDefinition = invitation.AddInvitationAccessDefinition();
                
                accessDefinition.AddInvitationAccessDefinition(invitationAccessDefinition);
            }

            await _repository.SaveChangesAsync();

        }

        public async Task InviteContractors(int tenderId, List<string> userIdList)
        {
            var tender = await _repository.GetAsync(tenderId);
            userIdList = userIdList.ToHashSet().ToList();

            tender.AddTenderInvitation(userIdList);

            var users = await _userRepository.GetByIdAsync(userIdList);
            await _repository.SaveChangesAsync();
            foreach (var user in users)
            {
                var header = string.Format("Tender invitation");
                var emailBody = string.Format("Hi {0}, <br /> You received a Tender invitation. Please check Tenders Page", user.Email);
                var message = Message.Create(new string[] { user.Email }, header, emailBody);
                await _emailManager.SendEmail(message);
            } 
       
        }

        public async Task<List<TenderUserInvitationsDto>> GetInvitationListAsync(int tenderId, string userId)
        {
            var tender = await _repository.GetInvitationListAsync(tenderId);

            var user = await _userRepository.GetByIdAsync(userId);

            if (!user.Projects.Any(x=>x.ProjectId == tender.ProjectId) || (user.UserType != UserTypes.Consultant && user.UserType != UserTypes.SubConsultant))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var model = _mapper.Map<List<TenderUserInvitationsDto>>(tender.TenderInvitations);
            return model;
        }

        public async Task SelectTenderProposalAsync(int tenderId, int invitationId, string userId)
        {
            var tender = await _repository.GetInvitationListAsync(tenderId);

            var user = await _userRepository.GetByIdAsync(userId);

            if (!user.Projects.Any(x => x.ProjectId == tender.ProjectId) || (user.UserType != UserTypes.Consultant && user.UserType != UserTypes.SubConsultant))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var invitation = tender.SelectWinner(invitationId);

            var project = await _projectRepository.GetAsync(tender.ProjectId);

            project.AddContractor(userId, new List<string> { invitation.ContractorId }, new List<string>());

            await _projectRepository.SaveChangesAsync();

        }
    }
}
