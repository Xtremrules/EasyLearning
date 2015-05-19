using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace EasyLearning.Domain.Concrete
{
    public class DbInitializer : CreateDatabaseIfNotExists<EasyLearningDB>
    {
        protected override void Seed(EasyLearningDB context)
        {
            base.Seed(context);
        }
    }
}
