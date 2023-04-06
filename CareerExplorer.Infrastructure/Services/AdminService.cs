using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class AdminService : IAdminService
    {
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
        }
        public async Task AcceptVacancy(int id)
        {
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            vacancy.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
    }
}
