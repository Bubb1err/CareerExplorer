using CareerExplorer.Infrastructure.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class VacancyService : IVacancyService
    {
        public int[]? GetIdsFromString(string ids)
        {
            if(ids.IsNullOrEmpty())
                return null;
            string[] tagIdsStr = ids.Split(',');
            int[] tagIdsArray = new int[tagIdsStr.Length];
            for (int i = 0; i < tagIdsStr.Length; i++)
            {
                if (char.IsDigit(char.Parse(tagIdsStr[i])))
                {
                    tagIdsArray[i] = int.Parse(tagIdsStr[i]);
                }
                else
                {
                    break;
                    throw new ArgumentException();   
                }
            }
            if (tagIdsArray.Length == 0)
                return null;
            return tagIdsArray;
        }
    }
}
