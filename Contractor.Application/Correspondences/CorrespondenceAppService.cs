using AutoMapper;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.Identities;
using Contractor.Tools;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class CorrespondenceAppService : ICorrespondenceAppService
    {
        private readonly ICorrespondenceRepository _correspondenceRepository;
        private readonly IMapper _mapper;
        private readonly IBlobManager _blobManager;

        public CorrespondenceAppService(ICorrespondenceRepository correspondenceRepository,
            IMapper mapper,
            IBlobManager blobManager)
        {
            _correspondenceRepository = correspondenceRepository;
            _mapper = mapper;
            _blobManager = blobManager;
        }


        public async Task<CorrespondenceDto> CreateNewAsync(CorrespondenceDto correspondenceDto, string userId, List<string> toRecipients, List<string> cCRecipients, List<IFormFile>? correspondenceFiles)
        {
            var nextId = await _correspondenceRepository.GetNextIdAsync();

            var thread = CorrespondenceThread.Create();

            var correspondence = Correspondence.CreateNew(nextId, correspondenceDto.ReferenceNumber, correspondenceDto.ProjectId, correspondenceDto.Subject, correspondenceDto.Content);

            if (!toRecipients.Any())
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            foreach (var item in toRecipients)
            {
                correspondence.AddToRecipient(item);
            }

            if(cCRecipients != null)
            {
                foreach (var item in cCRecipients)
                {
                    correspondence.AddCCRecipient(item);
                }
            }

            correspondence.AddSender(userId);

            thread.AddCorrespondence(correspondence);
            
            if(correspondenceFiles != null)
            {
                List<AccessDefinition> accessDefinitions = await _blobManager.UploadCorrespondenceFiles(correspondence.ProjectId, userId, correspondenceFiles);

                foreach (var accessDefinition in accessDefinitions)
                {
                    CorrespondenceAccessDefinition correspondenceAccessDefinition = correspondence.AddCorrespondenceAccessDefinitions();

                    accessDefinition.AddCorrespondenceAccessDefinition(correspondenceAccessDefinition);
                }
            }

            await _correspondenceRepository.CreateAsync(correspondence);

            await _correspondenceRepository.CreateThreadAsync(thread);

            await _correspondenceRepository.SaveChangesAsync();

            return _mapper.Map<CorrespondenceDto>(correspondence);
        }

        public async Task<CorrespondenceDto> CreateReplayAsync(string? referenceNumber,
            string subject,
            string content,
            int originalId,
            string userId,
            List<IFormFile>? correspondenceFiles)
        {
            var nextId = await _correspondenceRepository.GetNextIdAsync();

            var original = await _correspondenceRepository.GetAsync(originalId);

            var replay = Correspondence.CreateReplay(nextId, referenceNumber, original.ProjectId, subject, content, original.ThreadId);

            var originalSender = original.GetSender();
            var originalCc = original.GetCCRecipient().Where(x => x.RecipientId != userId);
            var originalTo = original.GetToRecipient().Where(x => x.RecipientId != userId);


            replay.AddToRecipient(originalSender.RecipientId);

            replay.AddSender(userId);

            foreach (var recipient in originalTo)
            {
                replay.AddCCRecipient(recipient.RecipientId);
            }

            foreach (var recipient in originalCc)
            {
                replay.AddCCRecipient(recipient.RecipientId);
            }

            if (correspondenceFiles != null)
            {
                List<AccessDefinition> accessDefinitions = await _blobManager.UploadCorrespondenceFiles(replay.ProjectId, userId, correspondenceFiles);

                foreach (var accessDefinition in accessDefinitions)
                {
                    CorrespondenceAccessDefinition correspondenceAccessDefinition = replay.AddCorrespondenceAccessDefinitions();

                    accessDefinition.AddCorrespondenceAccessDefinition(correspondenceAccessDefinition);
                }
            }

            await _correspondenceRepository.CreateAsync(replay);

            await _correspondenceRepository.SaveChangesAsync();

            return _mapper.Map<CorrespondenceDto>(replay);
        }

        public async Task<CorrespondenceDto> GetAsync(int id)
        {
            var list = await _correspondenceRepository.GetAsync(id);

            return _mapper.Map<CorrespondenceDto>(list);
        }

        public async Task<List<CorrespondenceDto>> GetByThreadIdAsync(int threadId)
        {
            var list = await _correspondenceRepository.GetByThreadIdAsync(threadId);

            return _mapper.Map<List<CorrespondenceDto>>(list);
        }

        public async Task<ListServiceModel<GetCorrespondenceForListDto>> GetIncomingAsync(string userId, string? search, int pageNumber, int pageSize)
        {
            var list = await _correspondenceRepository.GetIncomingAsync(userId, search, pageNumber, pageSize);

            return new ListServiceModel<GetCorrespondenceForListDto>(list.TotalCount, _mapper.Map<List<GetCorrespondenceForListDto>>(list.List));
        }

        public async Task<ListServiceModel<GetCorrespondenceForListDto>> GetOutgoingAsync(string userId, string? search, int pageNumber, int pageSize)
        {
            var list = await _correspondenceRepository.GetOutgoingAsync(userId, search, pageNumber, pageSize);

            return new ListServiceModel<GetCorrespondenceForListDto>(list.TotalCount, _mapper.Map<List<GetCorrespondenceForListDto>>(list.List));
        }
    }
}
