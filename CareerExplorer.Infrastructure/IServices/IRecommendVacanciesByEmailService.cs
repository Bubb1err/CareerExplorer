﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.IServices
{
    public interface IRecommendVacanciesByEmailService
    {
        Task SendVacanciesToUsersByEmail(TimeSpan checkingPeriod);
    }
}
