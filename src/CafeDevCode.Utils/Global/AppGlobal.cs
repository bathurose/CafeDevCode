using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Ultils.Global
{
    public class AppGlobal
    {
        public static DateTime SysDateTime => DateTime.Now;
        public static string InvalidUserName => "Ten dang nhap khong hop le";
        public static string InvalidPassword => "Mat khau dang nhap khong hop le";
        public static string DefaultSuccessMessage => "Thao tác thành công";

    }
}
