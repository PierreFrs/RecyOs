// TypeDocumentPdfService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service
{
    public class TypeDocumentPdfService<TTypeDocumentPdf, TTypeDocumentPdfDto> : BaseService, ITypeDocumentPdfService<TTypeDocumentPdfDto>  
        where TTypeDocumentPdf : TypeDocumentPdf, new()
        where TTypeDocumentPdfDto : TypeDocumentPdfDto, new()
    {
        protected readonly ITypeDocumentPdfRepository<TTypeDocumentPdf> _typeDocumentPdfRepository;
        private readonly IMapper _mapper;
        private readonly ITokenInfoService _tokenInfoService;

        public TypeDocumentPdfService(ICurrentContextProvider contextProvider,
            ITypeDocumentPdfRepository<TTypeDocumentPdf> typeDocumentPdfRepository,
            IMapper mapper,
            ITokenInfoService tokenInfoService) : base(contextProvider)
        {
            _typeDocumentPdfRepository = typeDocumentPdfRepository;
            _mapper = mapper;
            _tokenInfoService = tokenInfoService;
        }

        public async Task<TTypeDocumentPdfDto> CreateTypeAsync(string label)
        {
            var currentUser = _tokenInfoService.GetCurrentUserName();

            var typeDocumentPdf = new TTypeDocumentPdf()
            {
                Label = label,
                CreateDate = DateTime.Now,
                CreatedBy = currentUser
            };
            
            var createdType = await _typeDocumentPdfRepository.CreateTypeAsync(typeDocumentPdf);
            var typeDocumentPdfDto = _mapper.Map<TTypeDocumentPdfDto>(createdType);
            return typeDocumentPdfDto;
        }

        public async Task<List<TTypeDocumentPdfDto>> GetAllAsync()
        {
            var types = await _typeDocumentPdfRepository.GetListAsync(Session);
            return _mapper.Map<List<TTypeDocumentPdfDto>>(types);
        }

        public async Task<TTypeDocumentPdfDto> GetByIdAsync(int id)
        {
            var type = await _typeDocumentPdfRepository.GetByIdAsync(id, Session);
            return _mapper.Map<TTypeDocumentPdfDto>(type);
        }

        public async Task<TTypeDocumentPdfDto> GetByLabelAsync(string label)
        {
            var type = await _typeDocumentPdfRepository.GetByLabelAsync(label, Session);
            return _mapper.Map<TTypeDocumentPdfDto>(type);
        }

        public async Task<TTypeDocumentPdfDto> UpdateAsync(int id, string label)
        {
            var currentUser = _tokenInfoService.GetCurrentUserName();
            
            var existingTypeDocumentPdf = await _typeDocumentPdfRepository.GetByIdAsync(id, Session);
            if (existingTypeDocumentPdf == null)
            {
                return null;
            }

            existingTypeDocumentPdf.Label = label;
            existingTypeDocumentPdf.UpdatedAt = DateTime.Now;
            existingTypeDocumentPdf.UpdatedBy = currentUser;

            var updateTypeDocumentPdf = await _typeDocumentPdfRepository.UpdateAsync(existingTypeDocumentPdf, Session);
            var updateTypeDocumentPdfDto = _mapper.Map<TTypeDocumentPdfDto>(updateTypeDocumentPdf);

            return updateTypeDocumentPdfDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _typeDocumentPdfRepository.GetByIdAsync(id, Session);
            if (entity == null)
            {
                return false;
            }
            await _typeDocumentPdfRepository.DeleteAsync(id, Session);
            return true;
        }
    }
}