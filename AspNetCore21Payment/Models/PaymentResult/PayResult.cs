
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore21Payment.Models.PaymentResult
{
    public class PayResult
    {
        #region نمایش پیغام های نتیجه پرداخت ملی

        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string Melli(string resultId)
        {
            string result = string.Empty;
            switch (resultId)
            {
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "0":
                    result = "تراکنش موفق";
                    break;
                case "3":
                    result = "پذيرنده کارت فعال نیست لطفا با بخش امور پذيرندگان، تماس حاصل فرمائید";
                    break;
                case "23":
                    result = "پذيرنده کارت نامعتبر است لطفا با بخش امور پذيرندگان، تماس حاصل فرمائید";
                    break;
                case "58":
                    result = "انجام تراکنش مربوطه توسط پايانه ی انجام دهنده مجاز نمی باشد";
                    break;
                case "61":
                    result = "مبلغ تراکنش از حد مجاز بالاتر است";
                    break;
                case "1000":
                    result = "ترتیب پارامترهای ارسالی اشتباه می باشد، لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند";
                    break;
                case "1001":
                    result = "لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند،پارامترهای پرداخت اشتباه می باشد";
                    break;
                case "1002":
                    result = "خطا در سیستم- تراکنش ناموفق";
                    break;
                case "1003":
                    result = "آی پی پذیرنده اشتباه است.لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند";
                    break;
                case "1004":
                    result = "لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند،شماره پذيرنده اشتباه است";
                    break;
                case "1005":
                    result = "خطای دسترسی:لطفا بعدا تلاش فرمايید";
                    break;
                case "1006":
                    result = "خطا در سیستم";
                    break;
                case "1011":
                    result = "درخواست تکراری- شماره سفارش تکراری می باشد";
                    break;
                case "1012":
                    result = "اطلاعات پذيرنده صحیح نیست،يکی از موارد تاريخ،زمان يا کلید تراکنش اشتباه است.لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند";
                    break;
                case "1015":
                    result = "پاسخ خطای نامشخص از سمت مرکز";
                    break;
                case "1017":
                    result = "مبلغ درخواستی شما جهت پرداخت از حد مجاز تعريف شده برای اين پذيرنده بیشتر است";
                    break;
                case "1018":
                    result = "اشکال در تاريخ و زمان سیستم. لطفا تاريخ و زمان سرور خود را با بانک هماهنگ نمايید";
                    break;
                case "1019":
                    result = "امکان پرداخت از طريق سیستم شتاب برای اين پذيرنده امکان پذير نیست";
                    break;
                case "1020":
                    result = "پذيرنده غیرفعال شده است.لطفا جهت فعال سازی با بانک تماس بگیريد";
                    break;
                case "1023":
                    result = "آدرس بازگشت پذيرنده نامعتبر است";
                    break;
                case "1024":
                    result = "مهر زمانی پذيرنده نامعتبر است";
                    break;
                case "1025":
                    result = "امضا تراکنش نامعتبر است";
                    break;
                case "1026":
                    result = "شماره سفارش تراکنش نامعتبر است";
                    break;
                case "1027":
                    result = "شماره پذيرنده نامعتبر است";
                    break;
                case "1028":
                    result = "شماره ترمینال پذيرنده نامعتبر است";
                    break;
                case "1029":
                    result = "آدرس آی پی پرداخت در محدوده آدرس های معتبر اعلام شده توسط پذيرنده نیست.لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند";
                    break;
                case "1030":
                    result = "آدرس دامنه پرداخت در محدوده آدرس های معتبر اعلام شده توسط پذيرنده نیست .لطفا مسئول فنی پذيرنده با بانک تماس حاصل فرمايند";
                    break;
                case "1031":
                    result = "مهلت زمانی شما جهت پرداخت به پايان رسیده است.لطفا مجددا سعی بفرمايید";
                    break;
                case "1032":
                    result = "پرداخت با اين کارت . برای پذيرنده مورد نظر شما امکان پذير نیست.لطفا از کارتهای مجاز که توسط پذيرنده معرفی شده است . استفاده نمايید.";
                    break;
                case "1033":
                    result = "به علت مشکل در سايت پذيرنده. پرداخت برای اين پذيرنده غیرفعال شده است. لطفا مسوول فنی سايت پذيرنده با بانک تماس حاصل فرمايند.";
                    break;
                case "1036":
                    result = "اطلاعات اضافی ارسال نشده يا دارای اشکال است";
                    break;
                case "1037":
                    result = "شماره پذيرنده يا شماره ترمینال پذيرنده صحیح نمیباشد";
                    break;
                case "1053":
                    result = "خطا: درخواست معتبر، از سمت پذيرنده صورت نگرفته است لطفا اطلاعات پذيرنده خود را چک کنید.";
                    break;
                case "1055":
                    result = "مقدار غیرمجاز در ورود اطلاعات";
                    break;
                case "1056":
                    result = "سیستم موقتا قطع میباشد.لطفا بعدا تلاش فرمايید.";
                    break;
                case "1058":
                    result = "سرويس پرداخت اينترنتی خارج از سرويس می باشد.لطفا بعدا سعی بفرمايید.";
                    break;
                case "1061":
                    result = "اشکال در تولید کد يکتا. لطفا مرورگر خود را بسته و با اجرای مجدد مرورگر عملیات پرداخت را انجام دهید )احتمال استفاده از دکمه " + "«Back»" + "مرورگر(";
                    break;
                case "1064":
                    result = "لطفا مجددا سعی بفرمايید";
                    break;
                case "1065":
                    result = "ارتباط ناموفق .لطفا چند لحظه ديگر مجددا سعی کنید";
                    break;
                case "1066":
                    result = "سیستم سرويس دهی پرداخت موقتا غیر فعال شده است";
                    break;
                case "1068":
                    result = "با عرض پوزش به علت بروزرسانی . سیستم موقتا قطع میباشد.";
                    break;
                case "1072":// مرج کد خطا درگاه پرداخت
                    result = "خطا در پردازش پارامترهای اختیاری پذيرنده";
                    break;
                case "1101":// مرج کد خطا درگاه پرداخت
                    result = "مبلغ تراکنش نامعتبر است";
                    break;
                case "1103":// مرج کد خطا درگاه پرداخت
                    result = "توکن ارسالی نامعتبر است";
                    break;
                case "1104":// مرج کد خطا درگاه پرداخت
                    result = "اطلاعات تسهیم صحیح نیست";
                    break;

                /////////////////////////////////////
                //Verify
                case "-1":
                    result = "پارامترهای ارسالی صحیح نیست و يا تراکنش در سیستم وجود ندارد.";
                    break;
                case "101":
                    result = "مهلت ارسال تراکنش به پايان رسیده است";
                    break;


            }
            return result;
        }

        #endregion

        #region نمایش پیغام های نتیجه پرداخت زرین پال
        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string ZarinPal(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "NOK":
                    result = "پرداخت ناموفق بود";
                    break;
                case "-1":
                    result = "اطلاعات ارسال شده ناقص است";
                    break;
                case "-2":
                    result = "و يا مرچنت كد پذيرنده صحيح نيست IP";
                    break;
                case "-3":
                    result = "با توجه به محدوديت هاي شاپرك امكان پرداخت با رقم درخواست شده ميسر نمي باشد";
                    break;
                case "-4":
                    result = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است.";
                    break;
                case "-11":
                    result = "درخواست مورد نظر يافت نشد.";
                    break;
                case "-12":
                    result = "امكان ويرايش درخواست ميسر نمي باشد.";
                    break;
                case "-21":
                    result = "هيچ نوع عمليات مالي براي اين تراكنش يافت نشد";
                    break;
                case "-22":
                    result = "تراكنش نا موفق ميباشد.";
                    break;
                case "-33":
                    result = "رقم تراكنش با رقم پرداخت شده مطابقت ندارد.";
                    break;
                case "34":
                    result = "سقف تقسيم تراكنش از لحاظ تعداد يا رقم عبور نموده است";
                    break;
                case "40":
                    result = "اجازه دسترسي به متد مربوطه وجود ندارد.";
                    break;
                case "41":
                    result = "غيرمعتبر ميباشد. AdditionalData اطلاعات ارسال شده مربوط به";
                    break;
                case "42":
                    result = "مدت زمان معتبر طول عمر شناسه پرداخت بايد بين 30 دقيقه تا 45 روز مي باشد.";
                    break;
                case "54":
                    result = "درخواست مورد نظر آرشيو شده است.";
                    break;
                case "100":
                    result = "عمليات با موفقيت انجام گرديده است.";
                    break;
                case "101":
                    result = "تراكنش انجام شده است. PaymentVerification عمليات پرداخت موفق بوده و قبلا";
                    break;

                default:
                    result = string.Empty;
                    break;

            }
            return result;
        }

        #endregion

        #region نمایش پیغام های نتیجه پرداخت پی

        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string Pay(string resultId)
        {
            string result = string.Empty;
            switch (resultId)
            {
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "-1":
                    result = "ارسال api الزامی می باشد";
                    break;
                case "-2":
                    result = "ارسال amount ( مبلغ تراکنش ) الزامی می باشد";
                    break;
                case "-3":
                    result = "amount ( مبلغ تراکنش )باید به صورت عددی باشد";
                    break;
                case "-4":
                    result = "amount نباید کمتر از 1000 باشد";
                    break;
                case "-5":
                    result = "ارسال redirect الزامی می باشد";
                    break;
                case "-6":
                    result = "درگاه پرداختی با api ارسالی یافت نشد و یا غیر فعال می باشد";
                    break;
                case "-7":
                    result = "فروشنده غیر فعال می باشد";
                    break;
                case "-8":
                    result = "آدرس بازگشتی با آدرس درگاه پرداخت ثبت شده همخوانی ندارد";
                    break;
                case "failed":
                    result = "تراکنش با خطا مواجه شد";
                    break;


            }
            return result;
        }

        #endregion

        #region نمایش پیغام های نتیجه پرداخت پارسیان

        // این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        // 1: به جز کد وضعیت 1- ، همۀ وضعیت هایی که مقدار آنها منفی است، مربوط به درگاه پرداخت اینترنتی پارسیان میباشد.
        // 2: وضعیت 0 به معناي موفق بودن عملیات میباشد.
        // 3: تمامی کدهاي وضعیت مثبت (بزرگتر از صفر) خطاهاي صادره از سوئیچ پرداخت میباشد
        public static string Parsian(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "0":
                    result = "تراکنش با موفقیت انجام شد";
                    break;
                case "1":
                    result = "صادرکننده ي کارت از انجام تراکنش صرف نظر کرد";
                    break;
                case "2":
                    result = "عملیات تاییدیه این تراکنش قبلا باموفقیت صورت پذیرفته است";
                    break;
                case "3":
                    result = "پذیرنده ي فروشگاهی نامعتبر می باشد";
                    break;
                case "5":
                    result = "از انجام تراکنش صرف نظر شد";
                    break;
                case "6":
                    result = "بروز خطایی ناشناخته";
                    break;
                case "8":
                    result = "باتشخیص هویت دارنده ي کارت، تراکنش موفق می باشد";
                    break;
                case "9":
                    result = "درخواست رسیده در حال پی گیري و انجام است";
                    break;
                case "10":
                    result = "تراکنش با مبلغی پایین تر از مبلغ درخواستی یا کمبود حساب مشتري-- پذیرفته شده است";
                    break;
                case "12":
                    result = "تراکنش نامعتبر است";
                    break;
                case "13":
                    result = "مبلغ تراکنش نادرست است";
                    break;
                case "14":
                    result = "شماره کارت ارسالی نامعتبر است یاوجود ندارد";
                    break;
                case "15":
                    result = "صادرکننده ي کارت نامعتبراست یا وجود ندارد";
                    break;
                case "17":
                    result = "مشتري درخواست کننده حذف شده است";
                    break;
                case "20":
                    result = "در موقعیتی که سوئیچ جهت پذیرش تراکنش نیازمند پرس و جو از کارت است ممکن است درخواست از کارت ( ترمینال) بنماید این پیاممبین نامعتبر بودن جواب است";
                    break;
                case "21":
                    result = "در صورتی که پاسخ به در خواست ترمینا ل نیازمند هیچ پاسخ خاص یا عملکردي نباشیم این پیام را خواهیم داشت";
                    break;
                case "22":
                    result = "تراکنش مشکوك به بد عمل کردن ( کارت ، ترمینال ، دارنده کارت ) بوده است لذا پذیرفته نشده است";
                    break;
                case "30":
                    result = "قالب پیام داراي اشکال است";
                    break;
                case "31":
                    result = "پذیرنده توسط سوئی پشتیبانی نمی شود";
                    break;
                case "32":
                    result = "تراکنش به صورت غیر قطعی کامل شده است ( به عنوان مثال تراکنش سپرده گزاري که از دید مشتري کامل شده است ولی می بایست تکمیل گردد";
                    break;
                case "33":
                    result = "تاریخ انقضاي کارت سپري شده است";
                    break;
                case "38":
                    result = "تعداد دفعات ورود رمزغلط بیش از حدمجاز است. کارت توسط دستگاه ضبط شود";
                    break;
                case "39":
                    result = "کارت حساب اعتباري ندارد";
                    break;
                case "40":
                    result = "عملیات درخواستی پشتیبانی نمی گردد";
                    break;
                case "41":
                    result = "کارت مفقودي می باشد";
                    break;
                case "43":
                    result = "کارت مسروقه می باشد";
                    break;
                case "45":
                    result = "قبض قابل پرداخت نمی باشد";
                    break;
                case "51":
                    result = "موجودي کافی نمی باشد";
                    break;
                case "54":
                    result = "تاریخ انقضاي کارت سپري شده است";
                    break;
                case "55":
                    result = "رمز کارت نا معتبر است";
                    break;
                case "56":
                    result = "کارت نا معتبر است";
                    break;
                case "57":
                    result = "انجام تراکنش مربوطه توسط دارنده ي کارت مجاز نمی باشد";
                    break;
                case "58":
                    result = "انجام تراکنش مربوطه توسط پایانه ي انجام دهنده مجاز نمی باشد";
                    break;
                case "59":
                    result = "کارت مظنون به تقلب است";
                    break;
                case "61":
                    result = "مبلغ تراکنش بیش از حد مجاز می باشد";
                    break;
                case "62":
                    result = "کارت محدود شده است";
                    break;
                case "63":
                    result = "تمهیدات امنیتی نقضگردیده است";
                    break;
                case "65":
                    result = "تعداد درخواست تراکنش بیش از حد مجاز می باشد";
                    break;
                case "68":
                    result = "پاسخ لازم براي تکمیل یا انجام تراکنش خیلی دیر رسیده است";
                    break;
                case "69":
                    result = "تعداد دفعات تکرار رمز از حد مجاز گذشته است";
                    break;
                case "75":
                    result = "کارت فعال نیست است";
                    break;
                case "78":
                    result = "کارت فعال نیست";
                    break;
                case "79":
                    result = "حساب متصل به کارت نا معتبر است یا داراي اشکال است";
                    break;
                case "80":
                    result = "درخواست تراکنش رد شده است";
                    break;
                case "81":
                    result = "کارت پذیرفته نشد";
                    break;
                case "83":
                    result = "سرویس دهنده سوئیچ کارت تراکنش را نپذیرفته است";
                    break;
                case "84":
                    result = "در تراکنشهایی که انجام آن مستلزم ارتباط با صادر کننده است در صورت فعال نبودن صادر کننده این پیام در پاسخ ارسال خواهد شد";
                    break;
                case "91":
                    result = "سیستم صدور مجوز انجام تراکنش موقتا غیر فعال است و یا زمان تعیین شده براي صدور مجوز به پایان رسیده است";
                    break;
                case "92":
                    result = "مقصد تراکنش پیدا نشد";
                    break;
                case "93":
                    result = "امکان تکمیل تراکنش وجود ندارد";
                    break;
                ////////////////////////////////////////////////
                case "-1":
                    result = "خطاي سرور";
                    break;
                case "-100":
                    result = "پذیرنده غیرفعال می باشد";
                    break;
                case "-101":
                    result = "پذیرنده اهراز هویت نشد";
                    break;
                case "-102":
                    result = "تراکنش با موفقیت برگشت داده شد";
                    break;
                case "-103":
                    result = "قابلیت خرید براي پذیرنده غیر فعال می باشد";
                    break;
                case "-104":
                    result = "قابلیت پرداخت قبض براي پذیرنده غیر فعال";
                    break;
                case "-105":
                    result = "قابلیت تاپ آپ براي پذیرنده غیر فعال می باشد";
                    break;
                case "-106":
                    result = "قابلیت شارژ براي پذیرنده غیر فعال می باشد";
                    break;
                case "-107":
                    result = "قابلیت ارسال تاییده تراکنش براي پذیرنده غیر فعال می باشد";
                    break;
                case "-108":
                    result = "قابلیت برگشت تراکنش براي پذیرنده غیر فعال می باشد";
                    break;
                case "-111":
                    result = "مبلغ تراکنش بیش از حد مجاز پذیرنده می باشد";
                    break;
                case "-112":
                    result = "شماره سفارش تکراري است";
                    break;
                case "-113":
                    result = "پارامتر ورودي خالی می باشد";
                    break;
                case "-114":
                    result = "شناسه قبض نامعتبر می باشد";
                    break;
                case "-115":
                    result = "شناسه پرداخت نامعتبر می باشد";
                    break;
                case "-116":
                    result = "طول رشته بیش از حد مجاز می باشد";
                    break;
                case "-117":
                    result = "طول رشته کم تر از حد مجاز می باشد";
                    break;
                case "-118":
                    result = "مقدار ارسال شده عدد نمی باشد";
                    break;
                case "-119":
                    result = "سازمان نامعتبر می باشد";
                    break;
                case "-120":
                    result = "طول داده ورودي معتبر نمی باشد";
                    break;
                case "-121":
                    result = "رشته داده شده بطور کامل عددي نمی باشد";
                    break;
                case "-126":
                    result = "کد شناسایی پذیرنده معتبر نمی باشد";
                    break;
                case "-127":
                    result = "آدرس اینترنتی معتبر نمی باشد";
                    break;
                case "-128":
                    result = "معتبر نمی باشد IP قالب آدرس";
                    break;
                case "-130":
                    result = "زمان منقضی شده است Token";
                    break;
                case "-131":
                    result = "امعتبر می باشد Token";
                    break;
                case "-132":
                    result = "مبلغ تراکنش کمتر از حداقل مجاز میباشد";
                    break;
                case "-138":
                    result = "عملیات پرداخت توسط کاربر لغو شد";
                    break;
                case "-1505":
                    result = "تایید تراکنش توسط پذیرنده انجام شد";
                    break;
                case "-1507":
                    result = "تراکنش برگشت به سوئیچ ارسال شد";
                    break;
                case "-1527":
                    result = "انجام عملیات درخواست پرداخت تراکنش خرید ناموفق بود";
                    break;
                case "-1528":
                    result = "اطلاعات پرداخت یافت نشد";
                    break;
                case "-1530":
                    result = "پذیرنده مجاز به تایید این تراکنش نمی باشد";
                    break;
                case "-1531":
                    result = "تایید تراکنش ناموفق امکان پذیر نمی باشد";
                    break;
                case "-1532":
                    result = "تراکنش از سوي پذیرنده تایید شد";
                    break;
                case "-1533":
                    result = "تراکنش قبلاً تایید شده است";
                    break;
                case "-1536":
                    result = "فراخوانی سرویس درخواست شارژ تاپ آپ ناموفق بود";
                    break;
                case "-1540":
                    result = "تایید تراکنش ناموفق می باشد";
                    break;
                case "-1548":
                    result = "فراخوانی سرویس درخواست پرداخت قبض ناموفق بود";
                    break;
                case "-1549":
                    result = "زمان مجاز براي درخواست برگشت تراکنش به اتمام رسیده است";
                    break;
                case "-1550":
                    result = "برگشت تراکنش در وضعیت جاري امکان پذیر نمی باشد";
                    break;
                case "-1551":
                    result = "برگشت تراکنش قبلا اًنجام شده است";
                    break;
                case "-1552":
                    result = "برگشت تراکنش مجاز نمی باشد";
                    break;
                case "-32768":
                    result = "خطاي ناشناخته رخ داده است";
                    break;


                /*********************************************************/
                default:
                    result = "خطای نامشخص";
                    break;

            }
            return result;
        }

        #endregion

        #region نمایش پیغام های نتیجه پرداخت سامان

        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string Saman(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "OK":
                    result = "پرداخت با موفقیت انجام شده";
                    break;
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "CanceledByUser":
                case "":
                    result = "تراكنش توسط خريدار كنسل شد";
                    break;
                case "InvalidAmount":
                    result = "مبلغ سند برگشتی، از مبلغ تراکنش اصلی بیشتر است";
                    break;
                case "InvalidTransaction":
                    result = "درخواست برگشت یک تراکنش رسیده است ، درحالی که تراکنش اصلی پیدا نمی شود";
                    break;
                case "InvalidCardNumber":
                    result = "شماره کارت اشتباه است";
                    break;
                case "NoSuchIssuer":
                    result = "چنین صادر کننده کارتی وجود ندارد";
                    break;
                case "ExpiredCardPickUp":
                    result = "از تاریخ انقضای کارت گذشته است و کارت دیگر معتبر نیست";
                    break;
                case "AllowablePINTriesExceededPickUp":
                    result = "رمز کارت 3 مرتبه اشتباه وارد شده است و در نتیجه کارت غیر فعال خواهد شد";
                    break;
                case "IncorrectPIN":
                    result = "خریدار رمز کارت را اشتباه وارد کرده است";
                    break;
                case "ExceedsWithdrawalAmountLimit":
                    result = "مبلغ بیش از سقف برداشت می باشد";
                    break;
                case "TransactionCannotBeCompleted":
                    result = "تراکنش Authorize شده است (شماره PIN و PAN درست هست) ولی امکان سند خوردن وجود ندارد";
                    break;
                case "ResponseReceivedTooLate":
                    result = "تراکنش در شبکه بانکی Timeout خورده است";
                    break;
                case "Suspected Fraud Pick Up":
                    result = "خریدار یا فیلد CVV2 و یا فیلد ExpDate را اشتباه وارد کرده است (یا اصلا وارد نکرده است)";
                    break;
                case "NoSufficientFunds":
                    result = "موجودی حساب خریدار، کافی نیست";
                    break;
                case "IssuerDownSlm":
                    result = "سیستم بانک صادر کننده کارت خریدار، در وضعیت عملیاتی نیست";
                    break;
                case "TMEError":
                    result = "کلیه خطاهای دیگر بانک باعث ایجاد چنین خطایی می گردد";
                    break;
                /*********************************************************/
                case "-1":
                    result = "خطای در پردازش اطلاعات ارسالی - مشکل در یکی از ورودی ها و ناموفق بودنفراخوانی متد برگشت تراکنش";
                    break;
                case "-3":
                    result = "ورودی ها حاوی کارکترهای غیرمجاز می باشند.";
                    break;
                case "-4":
                    result = "Merchant Authentication Failed  کلمه عبور یا کد فروشنده اشتباه است";
                    break;
                case "-6":
                    result = "سند قبلا برگشت کامل یافته است";
                    break;
                case "-7":
                    result = "رسید دیجیتالی تهی است";
                    break;
                case "-8":
                    result = "طو ورودی ها بیشتر از حد مجاز است";
                    break;
                case "-9":
                    result = "وجود کارکترهای غیرمجاز در مبلخ برگشتی";
                    break;
                case "-10":
                    result = "رسید دیجیتالی به صورت Base64 نیست)حاوی کاراکترهای غیرمجاز است)";
                    break;
                case "-11":
                    result = "طو ورودی ها کمتر از حد مجاز است";
                    break;
                case "-12":
                    result = "مبلخ برگشتی منفی است";
                    break;
                case "-13":
                    result = "مبلخ برگشتی برای برگشت جزئی بیش از مبلخ برگشت نخورده ی رسید دیجیتالی است";
                    break;
                case "-14":
                    result = "چنین تراکنشی تعریف نشده است";
                    break;
                case "-15":
                    result = "مبلخ برگشتی به صورت اعشاری داده شده است";
                    break;
                case "-16":
                    result = "خطای داخلی سیستم";
                    break;
                case "-17":
                    result = "برگشت زدن جزیی تراکنش مجاز نمی باشد";
                    break;
                case "-18":
                    result = "IP Address  فروشنده نا معتبر است";
                    break;
                default:
                    result = string.Empty;
                    break;

            }
            return result;
        }

        #endregion

        #region نمایش پیغام های نتیجه پرداخت بانک ملت

        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string Mellat(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "0":
                    result = "تراكنش با موفقيت انجام شد";
                    break;

                case "11":
                    result = "شماره كارت نامعتبر است ";
                    break;
                case "12":
                    result = "موجودي كافي نيست ";
                    break;
                case "13":
                    result = "رمز نادرست است ";
                    break;
                case "14":
                    result = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است ";
                    break;
                case "15":
                    result = "كارت نامعتبر است ";
                    break;
                case "16":
                    result = "دفعات برداشت وجه بيش از حد مجاز است ";
                    break;
                case "17":
                    result = "كاربر از انجام تراكنش منصرف شده است ";
                    break;
                case "18":
                    result = "تاريخ انقضاي كارت گذشته است ";
                    break;
                case "19":
                    result = "مبلغ برداشت وجه بيش از حد مجاز است ";
                    break;
                case "111":
                    result = "صادر كننده كارت نامعتبر است ";
                    break;
                case "112":
                    result = "خطاي سوييچ صادر كننده كارت ";
                    break;
                case "113":
                    result = "پاسخي از صادر كننده كارت دريافت نشد ";
                    break;
                case "114":
                    result = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                case "21":
                    result = "پذيرنده نامعتبر است ";
                    break;
                case "23":
                    result = "خطاي امنيتي رخ داده است ";
                    break;
                case "24":
                    result = "اطلاعات كاربري پذيرنده نامعتبر است ";
                    break;
                case "25":
                    result = "مبلغ نامعتبر است ";
                    break;
                case "31":
                    result = "پاسخ نامعتبر است ";
                    break;

                case "32":
                    result = "فرمت اطلاعات وارد شده صحيح نمي باشد ";
                    break;
                case "33":
                    result = "حساب نامعتبر است ";
                    break;
                case "34":
                    result = "خطاي سيستمي ";
                    break;
                case "35":
                    result = "تاريخ نامعتبر است ";
                    break;
                case "41":
                    result = "شماره درخواست تكراري است ، دوباره تلاش کنید";
                    break;
                case "42":
                    result = "يافت نشد  Sale تراكنش";
                    break;
                case "43":
                    result = "داده شده است  Verify قبلا درخواست";
                    break;
                case "44":
                    result = "يافت نشد  Verfiy درخواست";
                    break;
                case "45":
                    result = "شده است  Settle تراكنش";
                    break;
                case "46":
                    result = "نشده است  Settle تراكنش";
                    break;
                case "47":
                    result = "يافت نشد  Settle تراكنش";
                    break;
                case "48":
                    result = "شده است  Reverse تراكنش";
                    break;
                case "49":
                    result = "يافت نشد  Refund تراكنش";
                    break;
                case "412":
                    result = "شناسه قبض نادرست است ";
                    break;
                case "413":
                    result = "شناسه پرداخت نادرست است ";
                    break;
                case "414":
                    result = "سازمان صادر كننده قبض نامعتبر است ";
                    break;
                case "415":
                    result = "زمان جلسه كاري به پايان رسيده است ";
                    break;
                case "416":
                    result = "خطا در ثبت اطلاعات ";
                    break;
                case "417":
                    result = "شناسه پرداخت كننده نامعتبر است ";
                    break;

                case "418":
                    result = "اشكال در تعريف اطلاعات مشتري ";
                    break;
                case "419":
                    result = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است ";
                    break;
                case "421":
                    result = "نامعتبر است  IP";
                    break;
                case "51":
                    result = "تراكنش تكراري است ";
                    break;
                case "54":
                    result = "تراكنش مرجع موجود نيست ";
                    break;
                case "55":
                    result = "تراكنش نامعتبر است ";
                    break;
                case "61":
                    result = "خطا در واريز ";
                    break;

                default:
                    result = string.Empty;
                    break;

            }
            return result;
        }

        #endregion       

    }
}
