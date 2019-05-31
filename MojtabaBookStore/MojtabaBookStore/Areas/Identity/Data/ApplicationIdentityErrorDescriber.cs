using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Areas.Identity.Data
{
    public class ApplicationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = nameof(DuplicateUserName), Description = $"نام کاربری '{userName}' قبلا توسط کاربر دیگری انتخاب شده است" };
        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "پسورد باید حداقل دارای یک کاراکتر غیر عددی و غیر حرفی باشد یعنی شامل حداقل یک کاراکتر علامت باشد" };
        public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "کلمه عبور باید حداقل داری یک کاراکتر عددی بین صفر تا نه باشد" };
        public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "کلمه عبور باید حداقل دارای یک حرف کوچک انگلیسی باشد" };
        public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "کلمه عبور باید حداقل دارای یک حرف بزرگ انگلیسی باشد" };
        public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = nameof(PasswordTooShort), Description = $"کلمه عبور باید حداقل دارای {length} کاراکتر باشد" };

        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = nameof(InvalidUserName), Description = "نام کاربری فقط میتواند شامل حروف انگلیسی و اعداد باشد" };

        public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = nameof(DuplicateEmail), Description = $"شما قبلا با ایمیل  {email}  ثبت نام کرده اید" };
        






    }
}
