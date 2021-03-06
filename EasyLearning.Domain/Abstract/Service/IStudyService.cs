﻿using EasyLearning.Domain.Entity;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IStudyService : IEntityService<Study>
    {
        Task<Study> GetByIdAsync(long Id);
    }
}
