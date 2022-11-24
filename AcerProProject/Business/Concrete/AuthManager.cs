using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Core.Utilities.Security.Jwt;
using Entity.Entities;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto UserForRegister, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = UserForRegister.Email,
                FirstName = UserForRegister.FirstName,
                LastName = UserForRegister.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.RegisterIsSuccess);
        }

        public IDataResult<User> Login(UserForLoginDto UserForLogin)
        {
            var userToCheck = _userService.GetByMail(UserForLogin.Email);
            if (userToCheck == null)
                return new ErrorDataResult<User>( Messages.UserNotFind);

            if (!HashingHelper.VerifyPasswordHash(UserForLogin.Password,
                userToCheck.PasswordHash, userToCheck.PasswordSalt))
                return new ErrorDataResult<User>(Messages.PasswordError);

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessLogin );
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.MailIsUsed);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            //var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.TokenCreated);
        }
    }
}


