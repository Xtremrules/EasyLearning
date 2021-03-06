﻿using EasyLearning.Domain.Abstract.Service;
using EasyLearning.Domain.Concrete;
using EasyLearning.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Service
{
    public class CollegeService : EntityService<College>, ICollegeService
    {
        public CollegeService(EasyLearningDB context)
            : base(context) { }
        public async Task<College> GetByIdAsync(int Id)
        {
            return await _dbset.FindAsync(Id);
        }

        public override IEnumerable<College> GetAll()
        {
            return _context.Colleges
                .Include(x => x.Departments)
                .ToList();
        }
    }
}
