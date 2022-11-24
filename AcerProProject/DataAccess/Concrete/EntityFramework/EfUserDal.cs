using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using Core.Entities.Concrete;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal: EntityRepositoryBase<User, AcerProContexts>,IUserDal
    {
    }
}
