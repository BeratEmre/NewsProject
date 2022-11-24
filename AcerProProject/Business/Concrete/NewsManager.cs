using Business.Abstract;
using Business.Constants;
using Core.Utilities.Enums;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class NewsManager : INewsService
    {
        INewsDal _newsDal;
        private ITokenHelper _tokenHelper;
        private readonly ILogger<NewsManager> _logger;


        public NewsManager(INewsDal newsDal, ITokenHelper tokenHelper, ILogger<NewsManager> logger)
        {
            _newsDal = newsDal;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        public IDataResult<List<News>> ActiveNews()
        {
            var newsList = _newsDal.GetAll(n => n.Status == (byte)Enums.NewsStatus.Active);
            return new SuccessDataResult<List<News>>(newsList, Messages.GetActiveNews);
        }

        public IResult Add(News news, string token)
        {
            bool tokenIsActive = _tokenHelper.IsTokenValid(token);
            if (!tokenIsActive)
                return new DataResult<News>(null, false, Messages.PleaseLoginErr);
            var userId=GetUserId(token);
            news.UserId = userId;

            _logger.LogInformation(news.Id + " id owner " + news.Title + " titled before News adding");
            try
            {
                _newsDal.Add(news);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, news.ToString() + " Add-metod-err");
                throw;
            }

            return new SuccessResult("Haber" + Messages.Added);
        }

        public IResult Delete(int id, string token)
        {
            var control = UserControll(token, id);
            if (!control.Success)
                return new ErrorResult(control.Message);           

            _logger.LogInformation(control.Data.Id + " id owner" + control.Data.Title + " titled before News Deleting");
            try
            {
                _newsDal.Delete(control.Data);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, control.Data.ToString() + " Delete-metod-err");
                throw;
            }
            return new SuccessResult("Haber" + Messages.Deleted);
        }

        public IDataResult<List<News>> GetAll()
        {
            List<News> result;
            try
            {
                result = _newsDal.GetAll();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, " GetAll-metod-err");
                throw;
            }
            return new SuccessDataResult<List<News>>(result, "Haberler" + Messages.Get);
        }

        public IDataResult<News> GetById(int Id)
        {
            return new SuccessDataResult<News>(_newsDal.Get(c => c.Id == Id), "Haber" + Messages.Get);
        }

        public IResult MakeActive(int id, string token)
        {
            var control = UserControll(token, id);
            if (!control.Success)
                return new ErrorResult(control.Message);

            control.Data.Status = (byte)Enums.NewsStatus.Active;
            _logger.LogInformation(control.Data.Id + " id owner" + control.Data.Title + " titled before News MakeActive");
            try
            {
                _newsDal.Update(control.Data);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, control.Data.ToString() + " MakeActive-metod-err");
                throw;
            }
            return new SuccessResult(Messages.NewsActived);
        }

        public IResult MakePassive(int id, string token)
        {
            var control = UserControll(token, id);
            if (!control.Success)
                return new ErrorResult(control.Message);

            control.Data.Status = (byte)Enums.NewsStatus.Passive;
            _logger.LogInformation(control.Data.Id + " id owner" + control.Data.Title + " titled before News MakePassive");
            try
            {
                _newsDal.Update(control.Data);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, control.Data.ToString() + " MakePassive-metod-err");
                throw;
            }
            return new SuccessResult(Messages.NewsPassived);
        }

        public IDataResult<List<News>> PassiveNews()
        {
            List<News> newsList;
            try
            {
                newsList = _newsDal.GetAll(n => n.Status == (byte)Enums.NewsStatus.Active);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, " PassiveNews-metod-err");
                throw;
            }
            return new SuccessDataResult<List<News>>(newsList, Messages.GetPassiveNews);
        }

        public IResult Update(News news, string token)
        {
            var control = UserControll(token, news.Id);
            if (!control.Success)
                return new ErrorResult(control.Message);

            _logger.LogInformation(news.Id + " id owner" + news.Title + " titled before News Update");
            try
            {
                _newsDal.Update(news);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, " Update-metod-err");
                throw;
            }
            return new SuccessResult("Haber" + Messages.Updated);
        }

        private int GetUserId(string token)
        {
            int userId = 0;
            string userIdStr = _tokenHelper.GetJWTTokenClaim(token, "id");

            Int32.TryParse(userIdStr, out userId);
            return userId;
        }

        private DataResult<News> UserControll(string token, int newsId)
        {
            bool tokenIsActive = _tokenHelper.IsTokenValid(token);
            if (!tokenIsActive)
                return new DataResult<News>(null,false, Messages.PleaseLoginErr);
            string userIdStr = _tokenHelper.GetJWTTokenClaim(token, "id");
            int userId = 0;
            var controllingNews = _newsDal.Get(n => n.Id == newsId);

            Int32.TryParse(userIdStr, out userId);
            if (controllingNews.UserId != userId)
                return new DataResult<News>(null, false, Messages.OnlyFounder);

            return new DataResult<News>(controllingNews, true);

        }
    }
}
