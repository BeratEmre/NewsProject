using Core.Entities.Concrete;
using Entities.Concrete;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class Messages
    {
        public static string Get = " başarıyla getirildi";

        public static string Deleted = " başarıyla silindi";

        public static string Added = " başarıyla eklendi";

        public static string Updated = " başarıyla güncellendi";

        public static string UnexpectedError = "Beklenmedik bir hata ile karşılaşıldı";

        public static string CantEditNews = "Haberleri sadece haberi yayınlayan kişi düzenliyebilir";

        public static string NewsActived = "Haber yayına alındı";

        public static string NewsPassived = "Haber yayından kaldırıldı";

        public static string GetPassiveNews = "Yayından kaldırılan haberler başarıyla getirildi";

        public static string GetActiveNews = "Yayındaki haberler başarıyla getirildi";

        public static string PasswordError = "Parola hatası";

        public static string UserNotFind = "Kullanıcı bulunamadı";

        public static string SuccessLogin = "Başarılı giriş";

        public static string MailIsUsed = "Bu mail adresi zaten kullanılıyor";

        public static string TokenCreated = "Token oluşturuldu";

        public static string RegisterIsSuccess = "Kayıt oldu";

        public static string PleaseLoginErr = "Lütfen giriş yapınız!";

        public static string OnlyFounder = "Sadece haber sahibi bu işlemi yapabilir!";
    }
}
