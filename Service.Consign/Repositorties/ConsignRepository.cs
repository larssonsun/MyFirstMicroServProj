using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Service.Consign
{
    public interface IConsignRepository
    {
        Task<bool> CreateConsign(Consign consign);
        Task<List<Consign>> GetConsigns(QueryFilterDTO queryFilter);
    }

    public class ConsignRepository : IConsignRepository
    {
        private readonly ConsignContext _consignContext;
        private readonly ICapPublisher _capPublisher;

        public ConsignRepository(ConsignContext consignContext, ICapPublisher capPublisher)
        {
            _consignContext = consignContext;
            _capPublisher = capPublisher;
        }

        public async Task<bool> CreateConsign(Consign consign)
        {
            using (_consignContext.Database.BeginTransaction(_capPublisher, autoCommit: true))
            {
                consign.ConsignNo = await ConsignNoGerneratorAsync();
                _consignContext.Add(consign);
                var r = await _consignContext.SaveChangesAsync() > 0;
                if (r)
                {
                    await _capPublisher.PublishAsync("consign.main", new { ConsignNo = consign.ConsignNo });
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Consign>> GetConsigns(QueryFilterDTO queryFilter)
        {
            return await _consignContext.Consigns
                .OrderBy(x => x.ConsignNo)
                .Skip(queryFilter.PageSize * --queryFilter.PageNo).Take(queryFilter.PageSize)
                .AsNoTracking().ToListAsync();
        }

        private async Task<string> ConsignNoGerneratorAsync()
        {
            var year = DateTime.Now.Year.ToString();
            var currentConsignNo = await _consignContext.Consigns.Where(x => x.ConsignNo.IndexOf(year) == 0).MaxAsync(x => x.ConsignNo)
                ?? year + "000000";
            if (currentConsignNo.IndexOf(year) != 0)
            {
                currentConsignNo = year + "000000";
            }

            return (int.Parse(currentConsignNo) + 1).ToString();
        }
    }
}