using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface INewsService
    {
        IDataResult<List<News>> GetAll();
        IDataResult<List<News>> ActiveNews();
        IDataResult<List<News>> PassiveNews();
        IDataResult<News> GetById(int Id);
        IResult Add(News news, string token);
        IResult Update(News news, string token);
        IResult Delete(int id, string token);
        IResult MakeActive(int id, string token);
        IResult MakePassive(int id, string token);
    }
}
